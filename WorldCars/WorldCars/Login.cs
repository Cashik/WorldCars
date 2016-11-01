using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WorldCars
{
    public partial class loginForm : Form
    {

        public loginForm()
        {
            InitializeComponent();
        }


        private void loginBtn_Click(object sender, EventArgs e)
        {
            //
            string login = loginTxt.Text;
            string password = passwordTxt.Text;




            Program.app.user.setLoginPassword(login, password);

            if (Program.app.login(login,password))
            {
                // если пользователь в базе, то создаем окно и передаем туда логин, пароль
                Form main = new MainForm(login, password);
                main.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин/пароль");
            }
            
        }

        private void showRegistrationFormBtn_Click(object sender, EventArgs e)
        {
            Form reg = new Registration(this);
            Hide();
            reg.ShowDialog();
            Show();
        }

        private void info_Click(object sender, EventArgs e)
        {
            Form info = new Info();
            info.Show();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
