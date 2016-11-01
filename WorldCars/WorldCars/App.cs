using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCars
{
    class App
    {
        public UserClass user;
        public DBManager db;

        public App()
        {
            db = new DBManager();
            db.Connect();
            user = new UserClass();
        }

        public bool login(string _login, string _password)
        {
            user.setLoginPassword(_login,_password);

            // если все прошло успешно, то возвращается значение true
            if (refreshUserData())
                return true;
            else
                return false;
        }

        public bool refreshUserData()
        {
            bool allFine = false;

            UserClass b = db.ReturnUserByLoginAndPassword(user.login,user.password);
            if (b!=null)
            {
                user = b;
                allFine = true;
            }
            
            return allFine;
        }
        public bool addUser(string login, string password,string name)
        {

            return false;
        }

        static public string returnDriveType(int drive_type)
        {
            string result;
            switch (drive_type)
            {
                case 0: result = "полный"; break;
                case 1: result = "задний"; break;
                case 2: result = "передний"; break;
                default:
                    result = "неверное значение!";
                    break;
            }
            return result;
        }

        public string RatingToString(int rating)
        {
            string result="";
            for (int i = 0; i < rating; i++)
            {
                result += ("★");
            }
            for (int i = rating; i < 10; i++)
            {
                result += ("☆");
            }

            return result;
        }

        static public string ReturnTransmission(int drive_type)
        {
            string result;
            switch (drive_type)
            {
                case 0: result = "автоматическая"; break;
                case 1: result = "ручная"; break;
               
                default:
                    result = "неверное значение!";
                    break;
            }
            return result;
        }
        static public string ReturnRole(int role)
        {
            string result;
            switch (role)
            {
                case 0: result = "гость"; break;
                case 1: result = "пользоаватель"; break;
                case 2: result = "модератор"; break;
                case 3: result = "администратор"; break;

                default:
                    result = "неверное значение!";
                    break;
            }
            return result;
        }
    }
    
}
