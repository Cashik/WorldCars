using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WorldCars
{
    public partial class MinCarInfo : UserControl
    {
        MainForm parent;
        int car_id;
        public MinCarInfo(CarInfoClass carInfo,MainForm mainForm)
        {
            InitializeComponent();
            parent = mainForm;

            
            if (carInfo.image == null)
            {
                pictureBox.Image = Image.FromFile("transport.png");
            }
            else
            {
                MemoryStream ms = new MemoryStream(carInfo.image);
                pictureBox.Image = Image.FromStream(ms);
            }

            maxSpeedLbl.Text = carInfo.max_speed.ToString() + " км/ч";
            carNameLbl.Text = carInfo.name.ToString();
            transmissionLbl.Text = App.ReturnTransmission(carInfo.transmission);
            driveTypeLbl.Text = App.returnDriveType(carInfo.drive_type);
            ratingLbl.Text = carInfo.rating.ToString();
            car_id = carInfo.id;

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void takeMoreInfoBtn_Click(object sender, EventArgs e)
        {
            parent.OpenCarInfo(car_id);
        }
    }
}
