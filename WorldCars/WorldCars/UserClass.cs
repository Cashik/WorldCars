
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WorldCars
{
    class UserClass
    {
        public string login="";
        public string password ="";
        public int id;
        public string name;
        public int access_level;
        public DateTime datetime;
        public byte[] image;

        public UserClass(){}

        public UserClass(string _login,string _password)
        {
            setLoginPassword(_login, _password);
        }

        public void setLoginPassword(string _login, string _password)
        {
            login = _login;
            password = _password;
        }

        

    
    }
}
