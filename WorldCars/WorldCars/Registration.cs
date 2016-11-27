using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldCars
{
    public partial class Registration : Form
    {
        Form loginForm;
        public Registration(Form _loginForm)
        {
            InitializeComponent();
            loginForm = _loginForm;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void registrationBtn_Click(object sender, EventArgs e)
        {
            // прочитать данные из текстовых полей
            string login = loginTxt.Text;
            string password = passwordTxt.Text;
            string passwordConfim = passwordConfimTxt.Text;

            // Валидация:
            bool allFine = true;
            // 1. Проверка пароля(совпадение 2х полей с паролями)
            if (passwordConfim!=password)
            {
                allFine = false;
                MessageBox.Show("Пароли не совпадают!");
            }
            // 2. Длинна пароля
            if (password.Length<8)
            {
                allFine = false;
                MessageBox.Show("Пароль должен содержать 8 и более символов!");
            }
            // 3. Уникальность логина
            if (Program.app.db.UserExist(login))
            {
                allFine = false;
                MessageBox.Show("Логин занят!");
            }

            // если все  норм, то создаем запись в БД 
            if (allFine)
            {
                UserClass newUser = new UserClass();
                newUser.login = login;
                newUser.name = nameTxt.Text;
                newUser.password = password;
                if (Program.app.db.AddUser(newUser))
                {
                    MessageBox.Show("Аккаунт успешно создан!");
                }
                else
                {
                    MessageBox.Show("Ошибка создания аккаунта!");

                }
                // возвращаем в окно входа
                Close();
            }
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void infoBtn_Click(object sender, EventArgs e)
        {
            Form info = new InfoForm();
            info.ShowDialog();
        }
    }
}
