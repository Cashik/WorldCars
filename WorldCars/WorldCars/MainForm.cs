using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WorldCars
{
    public partial class MainForm : Form
    {
        int bestAndNewestCarsShowCount = 5;
        int carInfoCommentsCount = 5;
        int userCarInfoCount = 5;
        int userCommentsCount = 5;
        Point PanelMouseDownLocation;
        // id просматриваемых данных в окнах Пользователь и О машине
        int currentCarInfoId = -1;
        int currentUserId;

        // статус редактирования
        bool editOn = false;
        //App app = new App();
        public MainForm(string login,string pswd)
        {
            InitializeComponent();

            currentUserId = Program.app.user.id;

            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            refreshMainForm();
        }

        public void OpenUserInfo(int userId)
        {
            currentUserId = userId;
            editOn = false;
            if (tabControl1.SelectedTab != tabPage3) tabControl1.SelectTab(tabPage3);
            else refreshMainForm();
        }

        void RefreshNewestCarInfo()
        {
            // remove all controls
            newestCarsInfoPanel.Controls.Clear();
            List<CarInfoClass> carInfos = Program.app.db.ReturnAllCarInfo();

            // 
            carInfos.Sort((CarInfoClass x, CarInfoClass y) => x.datetime.CompareTo(y.datetime));

            int max = bestAndNewestCarsShowCount;
            foreach (CarInfoClass carInfo in carInfos)
            {
                
                MinCarInfo minCarInfoForm = new MinCarInfo(carInfo,this);
                minCarInfoForm.Dock = DockStyle.Top;
                newestCarsInfoPanel.Controls.Add(minCarInfoForm);

                --max;
                if (max <= 0) break;
            }
        }
        void RefreshBestCarInfo()
        {
            // remove all controls
            bestCarsInfoPanel.Controls.Clear();

            List<CarInfoClass> carInfos = Program.app.db.ReturnAllCarInfo();
            //
            carInfos.Sort(delegate (CarInfoClass x, CarInfoClass y) { return x.rating.CompareTo(y.rating); });

            int max = bestAndNewestCarsShowCount;
            foreach (CarInfoClass carInfo in carInfos)
            {
                MinCarInfo minCarInfoForm = new MinCarInfo(carInfo, this);
                minCarInfoForm.Dock = DockStyle.Top;
                bestCarsInfoPanel.Controls.Add(minCarInfoForm);

                --max;
                if (max <= 0) break;
            }

        }

        public void loadBestCars()
        {
            ;
        }
        public void refreshMainForm()
        {
            nameLabel.Text = Program.app.user.name;
            RefreshSearchPanelElements();

            if (Program.app.user.image== null)
            {
                userAvaterPictureBox.Image = Image.FromFile("people.png");
            }
            else
            {
                MemoryStream ms = new MemoryStream(Program.app.user.image);
                userAvaterPictureBox.Image = Image.FromStream(ms);
                ms.Close();
            }


            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    RefreshHomePage();
                    break;
                case 1:
                    RefreshCarInfo();
                    break;
                case 2:
                    RefreshUserInfo();
                    break;
            }

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            editOn = false;
            ReInit();
            refreshMainForm();
        }

        private void ReInit()
        {
             bestAndNewestCarsShowCount = 10;
             carInfoCommentsCount = 5;
             userCarInfoCount = 5;
             userCommentsCount = 5;
        }

        private void RefreshHomePage()
        {
            RefreshBestCarInfo();
            RefreshNewestCarInfo();
        }
        private void RefreshSearchPanelElements()
        {
            markSearchComboBox.Items.Clear();
            markSearchComboBox.Items.Add("не важно...");
            List<string> list = Program.app.db.ReturnAllMarkOrBodyType("CarMake", "car_make");
            if (list == null)
            {
                MessageBox.Show("В базе нет ни одной марки автомобиля!");
            }
            else
            {
                foreach (string i in list)
                    markSearchComboBox.Items.Add(i);
            }
            markSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            markSearchComboBox.SelectedIndex = 0;

            bodyTypeSearchComboBox.Items.Clear();
            bodyTypeSearchComboBox.Items.Add("не важно...");
            list = Program.app.db.ReturnAllMarkOrBodyType("CarBodyType", "body_type");
            if (list == null)
            {
                MessageBox.Show("В базе нет ни одного типа кузова!");
            }
            else
            {
                foreach (string i in list)
                    bodyTypeSearchComboBox.Items.Add(i);
            }
            bodyTypeSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            bodyTypeSearchComboBox.SelectedIndex = 0;

            transmissionSearchComboBox.Items.Clear();
            transmissionSearchComboBox.Items.Add("не важно...");
            transmissionSearchComboBox.Items.Add("автоматическая");
            transmissionSearchComboBox.Items.Add("ручная");
            transmissionSearchComboBox.SelectedIndex = 0;

            driveTypeSearchComboBox.Items.Clear();
            driveTypeSearchComboBox.Items.Add("не важно...");
            driveTypeSearchComboBox.Items.Add("полный");
            driveTypeSearchComboBox.Items.Add("задний");
            driveTypeSearchComboBox.Items.Add("передний");
            driveTypeSearchComboBox.SelectedIndex = 0;

        }
        public void RefreshCarInfo()
        {

            CarInfoClass carInfo;
            UserClass promoter;

            if (currentCarInfoId != -1)
            {
                carInfo = Program.app.db.ReturnCarInfoById(currentCarInfoId);
                promoter = Program.app.db.ReturnUserById(carInfo.promoter_id);

                
            }
            else
            {
                carInfo = new CarInfoClass();
                promoter = Program.app.user;
                carInfo.promoter_id = promoter.id;
                editOn = true;
            }



            // стираем все элементы с нужных панелей

            markPanelCI.Controls.Clear();
            bodyTypePanelCI.Controls.Clear();
            costPanelCI.Controls.Clear();
            transmissionPanelCI.Controls.Clear();
            speedPanelCI.Controls.Clear();
            driveTypePanelCI.Controls.Clear();
            namePanelCI.Controls.Clear();
            creatorPanelCI.Controls.Clear();
            descriptionPanelCI.Controls.Clear();
            commentAddRRTBPanel.Controls.Clear();
            allCommentsPanel.Controls.Clear();
            userComentPanel.Controls.Clear();
            saveCarInfoBtn.Visible = editPanel.Visible= editCarInfoBtn.Visible= false;

            if (currentCarInfoId == -1)
            {
                userCommentMainPanel.Visible = false;
                addNewCommentPanel.Visible = false;
                panel1234.Visible = false;
            }
            else
            {
                userCommentMainPanel.Visible = false;
                addNewCommentPanel.Visible = true;
                panel1234.Visible =true;

            }




            //
            bool itIsUserCarInfo = ((carInfo.promoter_id == Program.app.user.id) ? true : false);

            // пользователь может редактировать поля формы, если это его запись и нажат режим редактирования
            bool userCanEditCarInfo = (itIsUserCarInfo && editOn);
            bool readOnly = !userCanEditCarInfo;

            // добавляем элементы учитывая доступ пользователя к ним
            ratingLbl.Text = Program.app.RatingToString((int)carInfo.rating)+"("+ carInfo.rating.ToString("F")+")";

            if (carInfo.image == null)
            {
                imageCI.Image = Image.FromFile("transport.png");
            }
            else
            {
                MemoryStream ms = new MemoryStream(carInfo.image);
                imageCI.Image = Image.FromStream(ms);
                ms.Close();
            }


            if (readOnly)
            {
                changeCarInfoImage.Visible = false;
            }
            else
            {
                changeCarInfoImage.Visible = true;
            }


            // марка машины
            if (readOnly)
            {
                markPanelCI.Controls.Add((new RRTB(this,"markTextUC", markPanelCI.Width, readOnly, carInfo.car_make)));
            }
            else
            {
                ComboBox carMakeComboBox = new ComboBox();
                List<string> list = Program.app.db.ReturnAllMarkOrBodyType("CarMake", "car_make");
                if (list==null)
                {
                    MessageBox.Show("В базе нет ни одной марки автомобиля!");
                }else
                {
                    int k = 0;
                    foreach (string i in list)
                    {
                        carMakeComboBox.Items.Add(i);
                        if (carInfo.car_make == i) carMakeComboBox.SelectedIndex = k;
                        k++;
                    }
                    if (currentCarInfoId == -1) carMakeComboBox.SelectedIndex = 0;

                }
                carMakeComboBox.Name = "carMakeComboBox";
                carMakeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                carMakeComboBox.Dock = DockStyle.Fill;
                markPanelCI.Controls.Add(carMakeComboBox);
            }

            // кузов
            if (readOnly)
            {
                bodyTypePanelCI.Controls.Add((new RRTB(this, "bodyTypeTextUC", bodyTypePanelCI.Width, readOnly, carInfo.body_type)));
            }
            else
            {
                ComboBox bodyTypeComboBox = new ComboBox();
                List<string> list = Program.app.db.ReturnAllMarkOrBodyType("CarBodyType", "body_type");
                if (list == null)
                {
                    MessageBox.Show("В базе нет ни одного типа кузова для выбора!");
                }
                else
                {
                    int k = 0;
                    foreach (string i in list)
                    {
                        bodyTypeComboBox.Items.Add(i);
                        if (carInfo.body_type == i) bodyTypeComboBox.SelectedIndex = k;
                        k++;
                    }
                    if (currentCarInfoId == -1) bodyTypeComboBox.SelectedIndex = 0;
                }

                bodyTypeComboBox.Name = "bodyTypeComboBox";
                bodyTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                bodyTypeComboBox.Dock = DockStyle.Fill;
                bodyTypePanelCI.Controls.Add(bodyTypeComboBox);
            }

            string textForUC = "0";
            if (currentCarInfoId != -1) textForUC = carInfo.cost.ToString();
            costPanelCI.Controls.Add((new RRTB(this, "costTextUC", costPanelCI.Width, readOnly, textForUC)));

            // каробка передач
            if (readOnly)
            {
                transmissionPanelCI.Controls.Add((new RRTB(this, "TextUC", transmissionPanelCI.Width, readOnly, App.ReturnTransmission(carInfo.transmission))));
            }
            else
            {
                ComboBox transmissionTypeComboBox = new ComboBox();
                transmissionTypeComboBox.Items.Add("автоматическая");
                transmissionTypeComboBox.Items.Add("ручная");
                if (currentCarInfoId == -1) transmissionTypeComboBox.SelectedIndex = 0;
                else transmissionTypeComboBox.SelectedIndex = carInfo.transmission;
                transmissionTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                transmissionTypeComboBox.Dock = DockStyle.Fill;
                transmissionTypeComboBox.Name = "userRoleComboBox";
                transmissionPanelCI.Controls.Add(transmissionTypeComboBox);
            }

            // привод
            if (readOnly)
            {
                driveTypePanelCI.Controls.Add((new RRTB(this, "driveTypeTextUC", driveTypePanelCI.Width, readOnly, App.returnDriveType(carInfo.drive_type))));
            }
            else
            {
                ComboBox driveTypeComboBox = new ComboBox();
                driveTypeComboBox.Items.Add("полный");
                driveTypeComboBox.Items.Add("задний");
                driveTypeComboBox.Items.Add("передний");
                if (currentCarInfoId == -1) driveTypeComboBox.SelectedIndex = 0;
                else driveTypeComboBox.SelectedIndex = carInfo.transmission;
                driveTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                driveTypeComboBox.Dock = DockStyle.Fill;
                driveTypeComboBox.Name = "driveTypeComboBox";
                driveTypePanelCI.Controls.Add(driveTypeComboBox);
            }

            textForUC = "0";
            if (currentCarInfoId != -1) textForUC = carInfo.max_speed.ToString();
            speedPanelCI.Controls.Add((new RRTB(this, "speedTextUC", speedPanelCI.Width, readOnly, textForUC)));

            textForUC = "Car name";
            if (currentCarInfoId != -1) textForUC = carInfo.name;
            RRTB b = (new RRTB(this, "nameTextUC", namePanelCI.Width, readOnly, textForUC));
            b.Dock = DockStyle.Fill;
            namePanelCI.Controls.Add(b);

            textForUC = promoter.name;
            creatorPanelCI.Controls.Add((new RRTB(this, "creatorTextUC", creatorPanelCI.Width, true, textForUC, promoter.id)));

            textForUC = "";
            if (currentCarInfoId != -1) textForUC = carInfo.description;
            b = (new RRTB(this, "descriptionTextUC", descriptionPanelCI.Width, readOnly, textForUC));
            b.Dock = DockStyle.Bottom;
            descriptionPanelCI.Controls.Add(b);

            // добавляем свой richtextbox в панель комментирования
            b = new RRTB(this, "commentAddTextUC", commentAddRRTBPanel.Width - commentAddRRTBPanel.Padding.Left - commentAddRRTBPanel.Padding.Right, false);
            b.Dock = DockStyle.Top;
            commentAddRRTBPanel.Controls.Add(b);

            ratingCB.SelectedIndex = 9;

            // включаем панель редактирования
            if (itIsUserCarInfo || Program.app.user.access_level >= 3)
            {
                editPanel.Visible = true;
                if (itIsUserCarInfo)
                {
                     editCarInfoBtn.Visible = true;
                    if (editOn) saveCarInfoBtn.Visible = true;
                }
            }

            // загрузка комментариев 
            List<CommentClass> commentsList = Program.app.db.ReturnAllCommentsByAtr("car_info_id", carInfo.id);
            if (commentsList != null)
            {
                int max = carInfoCommentsCount;
                foreach (CommentClass comment in commentsList)
                {
                    bool deletable = false;
                    if (comment.user_id == Program.app.user.id || Program.app.user.access_level >= 3)
                        deletable = true;
                    CommentForm cf = new CommentForm(this, comment, true);
                    cf.Dock = DockStyle.Bottom;
                    allCommentsPanel.Controls.Add(cf);

                    if (comment.user_id == Program.app.user.id)
                    {
                        // создаем еще 1 идентичный экземпляр для помещения в панель комментария пользователя
                        CommentForm ucf = new CommentForm(this, comment, true);
                        ucf.Dock = DockStyle.Bottom;
                        userComentPanel.Controls.Add(ucf);
                        userCommentMainPanel.Visible = true;
                        addNewCommentPanel.Visible = false;
                    }
                    --max;
                    if (max <= 0) break;
                }
            }
                



        }

        public void OpenCarInfo(int newCarInfoId)
        {
            currentCarInfoId = newCarInfoId;
            editOn = false;
            if (tabControl1.SelectedTab!=tabPage2) tabControl1.SelectTab(tabPage2);
            else refreshMainForm();

        }

        private void closePictureBox_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void logoutPictureBox_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }

        private void homePagePictureBox_Click(object sender, EventArgs e)
        {
            currentUserId = Program.app.user.id;
            tabControl1.SelectedTab = tabPage1;
        }

        private void nameLabel_Click(object sender, EventArgs e)
        {

        }

        private void addCarInfo_Click(object sender, EventArgs e)
        {
            if (Program.app.user.access_level >= 2)
            {
                OpenCarInfo(-1);
            }
            else
            {
                MessageBox.Show("Недостаточно прав доступа!");
            }
        }

        private void refreshPictureBox_Click(object sender, EventArgs e)
        {
            refreshMainForm();
        }

        private void userPagePictureBox_Click(object sender, EventArgs e)
        {

            tabControl1.SelectedTab = tabPage3;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (editOn) editOn = false;
            else editOn = true;
            RefreshCarInfo();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            CarInfoClass newCarInfo = new CarInfoClass();

            newCarInfo.id = currentCarInfoId;

            Control[]  b = Controls.Find("nameTextUC", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем nameTextUC!");
            else newCarInfo.name = ((RRTB)b[0]).getText();

            b = Controls.Find("carMakeComboBox", true);
            if (b[0]==null) MessageBox.Show("Ошибка! Нет элемента с именем carMakeComboBox!");
            else newCarInfo.car_make = b[0].Text;


            b = Controls.Find("bodyTypeComboBox", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем bodyTypeComboBox!");
            else newCarInfo.body_type = b[0].Text;

            b = Controls.Find("userRoleComboBox", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем userRoleComboBox!");
            else newCarInfo.transmission = ((ComboBox)b[0]).SelectedIndex ;

            b = Controls.Find("costTextUC", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем costTextUC!");
            else newCarInfo.cost = Int32.Parse(((RRTB)b[0]).getText());

            b = Controls.Find("speedTextUC", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем speedTextUC!");
            else newCarInfo.max_speed = Int32.Parse(((RRTB)b[0]).getText());

            b = Controls.Find("descriptionTextUC", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем descriptionTextUC!");
            else newCarInfo.description = ((RRTB)b[0]).getText();

            b = Controls.Find("driveTypeComboBox", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем driveTypeComboBox!");
            else newCarInfo.drive_type = ((ComboBox)b[0]).SelectedIndex;

            newCarInfo.promoter_id = Program.app.user.id;

            MemoryStream ms = new MemoryStream();
            imageCI.Image.Save(ms, imageCI.Image.RawFormat);
            newCarInfo.image = ms.GetBuffer();
            ms.Close();

            if (currentCarInfoId == -1)
            {
                //TODO: create new CarInfo
                int cid = Program.app.db.AddCarInfo(newCarInfo);
                if (cid == -1) MessageBox.Show("Ошибка саздания!");
                else OpenCarInfo(cid);
            }
            else
            {
                if (Program.app.db.ChangeCarInfo(newCarInfo)) OpenCarInfo(currentCarInfoId);
                else MessageBox.Show("Ошибка изменения!");
                
            }


        }

        private void addCommentBtn_Click(object sender, EventArgs e)
        {
            CommentClass comment = new CommentClass();

            comment.car_info_id = currentCarInfoId;
            comment.rating = ratingCB.SelectedIndex+1;

            Control[] b = Controls.Find("commentAddTextUC", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем commentAddTextUC!");
            else comment.text = ((RRTB)b[0]).getText();

            comment.user_id = Program.app.user.id;

            Program.app.db.AddComment(comment);

            RefreshCarInfo();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            // считываем из элементов управления и переводим в числа
            string mark = markSearchComboBox.GetItemText(markSearchComboBox.SelectedItem);
            string bodyType = bodyTypeSearchComboBox.GetItemText(bodyTypeSearchComboBox.SelectedItem);
            int driveType = driveTypeSearchComboBox.SelectedIndex;
            int transmission = transmissionSearchComboBox.SelectedIndex;


            int minCost = ((costMinSearchTB.Text == "") ? 0: Int32.Parse(costMinSearchTB.Text));
            int maxCost = ((costMaxSearchTB.Text == "") ? -1 : Int32.Parse(costMaxSearchTB.Text));
            int minSped = ((speedMinSearchTB.Text == "") ? 0 : Int32.Parse(speedMinSearchTB.Text));
            int maxSpeed = ((speedMaxSearchTB.Text == "") ? -1 : Int32.Parse(speedMaxSearchTB.Text));

            // считываем все машины и отбрасываем ненужные
            List<CarInfoClass> carsInfo = Program.app.db.ReturnAllCarInfo();


            foreach (CarInfoClass carInfo in carsInfo)
            {
                bool result = true;
                if (mark!="не важно...")
                {
                    if (mark != carInfo.car_make) result = false;
                }
                if (bodyType != "не важно...")
                {
                    if (bodyType != carInfo.body_type) result = false;
                }
                if (transmission != 0)
                {
                    if (transmission-1 != carInfo.transmission) result = false;
                }
                if (driveType != 0)
                {
                    if (driveType-1 != carInfo.drive_type) result = false;
                }
                if (carInfo.cost < minCost) result = false;
                if (carInfo.max_speed < minSped) result = false;
                if (maxCost != -1)
                {
                    if (carInfo.cost > maxCost) result = false;
                }
                if (maxSpeed != -1)
                {
                    if (carInfo.max_speed > maxSpeed) result = false;
                }

                if (result)
                {
                    MinCarInfo ci = new MinCarInfo(carInfo, this);
                    ci.Dock = DockStyle.Bottom;
                    searchResultsPanel.Controls.Add(ci);
                }

                string searchPhrase = phraseSearchTB.Text;
                if (searchPhrase != "")
                {
                    if (carInfo.description.IndexOf(searchPhrase) == -1 && carInfo.name.IndexOf(searchPhrase) == -1)
                        result = false;
                }
            }

            OpenSearchResults();

        }

        private void OpenSearchResults()
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void clearSearchResult_Click(object sender, EventArgs e)
        {
            searchResultsPanel.Controls.Clear();

        }

        private void changeCarInfoImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "PNG|*.png|JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif";
            f.FilterIndex = 1;
            if (f.ShowDialog() == DialogResult.OK)
            {
                imageCI.Image = Image.FromFile(f.FileName);
            }
        }


        public void RefreshUserInfo()
        {

            UserClass user = Program.app.db.ReturnUserById(currentUserId);

            // очистка перед обновлением
            allUserCommentsPanel.Controls.Clear();
            allUserCarsInfoPanel.Controls.Clear();
            userNameTextUCPanel.Controls.Clear();
            userRoleTextUCPanel.Controls.Clear();
            userCreateAcLablePanel.Controls.Clear();
            allUserCommentsMainPanel.Visible = false;
            editUserPanel.Visible = saveUserBtn.Visible = editUserBtn.Visible = false;



            // доступ
            bool isUserAccount;
            if (currentUserId == Program.app.user.id) isUserAccount = true;
            else isUserAccount = false;
            bool readOnly = !(isUserAccount && editOn);

            // включаем панель редактирования
            if (isUserAccount || Program.app.user.access_level >= 3)
            {
                editUserPanel.Visible = true;
                saveUserBtn.Visible = editUserBtn.Visible = true;
                    
            }

            // имя
            userNameTextUCPanel.Controls.Add((new RRTB(this, "userNameTextUC", userNameTextUCPanel.Width, readOnly, user.name)));
            // роль
            if (Program.app.user.access_level >= 3 && editOn)
            {
                ComboBox userRoleComboBox = new ComboBox();
                userRoleComboBox.Items.Add("пользователь");
                userRoleComboBox.Items.Add("модератор");
                userRoleComboBox.Items.Add("администраротр");
                userRoleComboBox.SelectedIndex = user.access_level - 1;
                userRoleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                userRoleComboBox.Dock = DockStyle.Fill;
                userRoleComboBox.Name = "userRoleComboBox";
                userRoleTextUCPanel.Controls.Add(userRoleComboBox);
            }
            else
            {
                userRoleTextUCPanel.Controls.Add((new RRTB(this, "userRoleTextUC", userRoleTextUCPanel.Width, readOnly, App.ReturnRole(user.access_level))));
            }
            // дата
            userCreateAcLablePanel.Controls.Add((new RRTB(this, "userDateTextUC", userCreateAcLablePanel.Width, true, user.datetime.ToString())));

            if (!readOnly)
            {
                changeUserImageBtn.Visible = true;
            }
            else
            {
                changeUserImageBtn.Visible = false;

            }

            if (user.image == null)
            {
                userAvatarPB.Image = Image.FromFile("people.png");
            }
            else
            {
                MemoryStream ms = new MemoryStream(user.image);
                userAvatarPB.Image = Image.FromStream(ms);
                ms.Close();
            }

            // комментарии чугого пользователя можно смотреть только администраторам
            if (Program.app.user.access_level >= 3 || isUserAccount)
            {
                allUserCommentsMainPanel.Visible = true;
                int k = userCommentsCount;
                // загрузка комментариев пользователя
                List<CommentClass> commentsList = Program.app.db.ReturnAllCommentsByAtr("user_id", user.id);
                if (commentsList != null)
                    foreach (CommentClass comment in commentsList)
                    {
                        bool deletable = false;
                        if (comment.user_id == Program.app.user.id)
                            deletable = true;
                        CommentForm cf = new CommentForm(this, comment, deletable);
                        cf.Dock = DockStyle.Bottom;
                        allUserCommentsPanel.Controls.Add(cf);
                        --k;
                        if (k <= 0) break;
                    }

            }



            // загрузка записей пользователя. Их видят все.
            
            List<CarInfoClass> carInfoList = Program.app.db.ReturnAllCarByPromoterId(user.id);
            if (carInfoList != null)
            {
                int k = userCommentsCount;
                foreach (CarInfoClass carInfo in carInfoList)
                {

                    MinCarInfo cf = new MinCarInfo(carInfo, this);
                    cf.Dock = DockStyle.Bottom;
                    allUserCarsInfoPanel.Controls.Add(cf);
                    --k;
                    if (k <= 0) break;
                }

            }

        }
        private void changeUserImageBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "PNG|*.png|JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif";
            f.FilterIndex = 1;
            if (f.ShowDialog() == DialogResult.OK)
            {
                userAvatarPB.Image = Image.FromFile(f.FileName);
            }
        }

        private void editUserBtn_Click(object sender, EventArgs e)
        {
            if (editOn) editOn = false;
            else editOn = true;
            RefreshUserInfo();
        }

        private void saveUserBtn_Click(object sender, EventArgs e)
        {
            UserClass newUserData = new UserClass();

            newUserData.id = currentUserId;

            Control[] b = Controls.Find("userNameTextUC", true);
            if (b[0] == null) MessageBox.Show("Ошибка! Нет элемента с именем nameTextUC!");
            else newUserData.name = ((RRTB)b[0]).getText();

            b = Controls.Find("userRoleComboBox", true);
            if (b.Length > 0) newUserData.access_level = ((ComboBox)b[0]).SelectedIndex + 1;
            else  newUserData.access_level = -1;

            MemoryStream ms = new MemoryStream();
            userAvatarPB.Image.Save(ms, userAvatarPB.Image.RawFormat);
            newUserData.image = ms.GetBuffer();
            ms.Close();

            editOn = false;
            if (Program.app.db.ChangeUserInfo(newUserData))
            {
                Program.app.refreshUserData();
                refreshMainForm();
            }
            else MessageBox.Show("Ошибка изменения!");

        }

        private void AddBestAndNewestCarInfoBtn_Click(object sender, EventArgs e)
        {
            bestAndNewestCarsShowCount += 5;
            refreshMainForm();
        }

        private void AddCommentsToCarInfo_Click(object sender, EventArgs e)
        {
            carInfoCommentsCount += 5;
            refreshMainForm();

        }

        private void AddUserCommentsBtn_Click(object sender, EventArgs e)
        {
            userCommentsCount += 5;
            refreshMainForm();

        }

        private void addUserCarInfoBtn_Click(object sender, EventArgs e)
        {
            userCarInfoCount += 5;
            refreshMainForm();

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PanelMouseDownLocation = e.Location;

        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X + -PanelMouseDownLocation.X;
                this.Top += e.Y - PanelMouseDownLocation.Y;
            }

        }

        private void deleteCarInfoBtn_Click(object sender, EventArgs e)
        {
            Program.app.db.DeleteCarInfo(currentCarInfoId);
            currentCarInfoId = -1;
            tabControl1.SelectedTab = tabPage1;

        }

        private void deleteUserBtn_Click(object sender, EventArgs e)
        {
            if (Program.app.db.DeleteUserById(currentUserId))
            {
                Form loginForm = new loginForm();
                loginForm.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка удаления пользователя");
            }
        }

        private void aboutPictureBox_Click(object sender, EventArgs e)
        {
            Form info = new Info();
            info.ShowDialog();
        }

        private void label36_Click(object sender, EventArgs e)
        {

        }
    }
}
