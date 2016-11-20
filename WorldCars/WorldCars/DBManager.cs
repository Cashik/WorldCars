using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WorldCars
{
    class DBManager
    {
        SqlConnectionStringBuilder connectStringBuilder;
        public SqlConnection connection;

        public DBManager()
        {
            connectStringBuilder = new SqlConnectionStringBuilder();
            connectStringBuilder.DataSource = @"(localdb)\MSSQLLocalDB";
            connectStringBuilder.ConnectTimeout = 30;
            connectStringBuilder.IntegratedSecurity = true;
            connectStringBuilder.AttachDBFilename = @"|DataDirectory|WorldCarsDB.mdf";

            connection = new SqlConnection();
            connection.ConnectionString = connectStringBuilder.ConnectionString;
        }
        public bool Connect()
        {
            try
            {
                //Открыть подключение
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public void Close()
        {
            connection.Close();
        }
        public bool IsOpen()
        {
            return connection.State == System.Data.ConnectionState.Open;
        }
        public bool RefreshConnect()
        {
            Close();
            return Connect();
        }


        public List<CommentClass> ReturnAllCommentsByAtr(string atribute,int value)
        {
            List<CommentClass> result = new  List<CommentClass>();

            string command = string.Format("SELECT * FROM [Comment] WHERE {0}='{1}'", atribute, value.ToString());

            bool allFine = false;
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CommentClass buf = new CommentClass();
                    buf.id = (int)dr[dr.GetOrdinal("Id")];
                    buf.user_id = (int)dr[dr.GetOrdinal("user_id")];
                    buf.rating = (int)((float)dr[dr.GetOrdinal("rating")]);
                    buf.text = (string)dr[dr.GetOrdinal("text")];
                    buf.datetime = (DateTime)dr[dr.GetOrdinal("datetime")];

                    result.Add(buf);               
                }
                dr.Close();
                allFine = true;
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }

            
            if (allFine)
                return result;
            else
                return null;
            
        }
        //todo: ДОБАВИТЬретурн кар инфо по ид
        public List<CarInfoClass> ReturnAllCarInfo()
        {
            List<CarInfoClass> result = new List<CarInfoClass>();
            string command = string.Format("SELECT * FROM [CarInfo]");
            bool allFine = false;
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CarInfoClass buf = new CarInfoClass();
                    buf.id = (int)dr[dr.GetOrdinal("Id")];
                    buf.body_type = (string)dr[dr.GetOrdinal("body_type")];
                    buf.car_make = (string)dr[dr.GetOrdinal("car_make")];
                    buf.promoter_id = (int)dr[dr.GetOrdinal("promoter_id")];
                    if (dr.IsDBNull(dr.GetOrdinal("description"))) buf.description = "";
                    else  buf.description = (string)dr[dr.GetOrdinal("description")];

                    buf.max_speed = (int)dr[dr.GetOrdinal("max_speed")];

                    buf.cost = (int)dr[dr.GetOrdinal("cost")];
                    buf.datetime = (DateTime)dr[dr.GetOrdinal("datetime")];

                    if (dr.IsDBNull(dr.GetOrdinal("rating"))) buf.rating = 0;
                    else buf.rating = (float)dr[dr.GetOrdinal("rating")];

                    buf.drive_type = (int)dr[dr.GetOrdinal("drive_type")];

                    buf.transmission = (int)dr[dr.GetOrdinal("transmission")];

                    buf.name = (string)dr[dr.GetOrdinal("name")];

                    if (dr.IsDBNull(dr.GetOrdinal("image"))) buf.image = null;
                    else  buf.image = (byte[])dr[dr.GetOrdinal("image")];

                    result.Add(buf);
                }
                dr.Close();
                allFine = true;
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }


            if (allFine)
                return result;
            else
                return null;

        }
        public List<CarInfoClass> ReturnAllCarByPromoterId(int promoter_id)
        {
            List<CarInfoClass> result = new List<CarInfoClass>();
            string command = string.Format("SELECT * FROM [CarInfo] where promoter_id = '{0}'", promoter_id);
            bool allFine = false;
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CarInfoClass buf = new CarInfoClass();
                    buf.id = (int)dr[dr.GetOrdinal("Id")];
                    buf.body_type = (string)dr[dr.GetOrdinal("body_type")];
                    buf.car_make = (string)dr[dr.GetOrdinal("car_make")];
                    buf.promoter_id = (int)dr[dr.GetOrdinal("promoter_id")];
                    if (dr.IsDBNull(dr.GetOrdinal("description"))) buf.description = "";
                    else buf.description = (string)dr[dr.GetOrdinal("description")];

                    buf.max_speed = (int)dr[dr.GetOrdinal("max_speed")];

                    buf.cost = (int)dr[dr.GetOrdinal("cost")];
                    buf.datetime = (DateTime)dr[dr.GetOrdinal("datetime")];

                    if (dr.IsDBNull(dr.GetOrdinal("rating"))) buf.rating = 0;
                    else buf.rating = (float)dr[dr.GetOrdinal("rating")];

                    buf.drive_type = (int)dr[dr.GetOrdinal("drive_type")];

                    buf.transmission = (int)dr[dr.GetOrdinal("transmission")];

                    buf.name = (string)dr[dr.GetOrdinal("name")];

                    if (dr.IsDBNull(dr.GetOrdinal("image"))) buf.image = null;
                    else buf.image = (byte[])dr[dr.GetOrdinal("image")];

                    result.Add(buf);
                }
                dr.Close();
                allFine = true;
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }


            if (allFine)
                return result;
            else
                return null;

        }
        public List<string> ReturnAllMarkOrBodyType(string table,string atr)
        {
            List<string> result = new List<string>();
            string command = string.Format("select * from [{0}]",table);
            bool allFine = false;
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string buf;
                    buf = (string)dr[dr.GetOrdinal(atr)];

                    result.Add(buf);
                }
                dr.Close();
                allFine = true;
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }


            if (allFine)
                return result;
            else
                return null;

        }

        public UserClass ReturnUserById(int promoter_id)
        {
            UserClass result = new UserClass();
            string command = string.Format("SELECT * FROM [User] WHERE id ='{0}'", promoter_id);

            bool allFine = false;
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result.id = (int)dr[dr.GetOrdinal("Id")];
                    result.login = (string)dr[dr.GetOrdinal("login")];
                    result.password = (string)dr[dr.GetOrdinal("password")];
                    result.access_level = (int)dr[dr.GetOrdinal("access_level")];
                    result.name = (string)dr[dr.GetOrdinal("name")];
                    result.datetime = (DateTime)dr[dr.GetOrdinal("datetime")];
                    if (dr.IsDBNull(dr.GetOrdinal("image"))) result.image = null;
                    else result.image = (byte[])dr[dr.GetOrdinal("image")];

                    allFine = true;

                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }


            if (allFine)
                return result;
            else
                return null;
        }
        public UserClass ReturnUserByLoginAndPassword(string login,string password)
        {
            UserClass result = new UserClass();
            string command = string.Format("SELECT * FROM [User] WHERE login ='{0}' and password='{1}'", login,password);

            bool allFine = false;
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result.id = (int)dr[dr.GetOrdinal("Id")];
                    result.login = (string)dr[dr.GetOrdinal("login")];
                    result.password = (string)dr[dr.GetOrdinal("password")];
                    result.access_level = (int)dr[dr.GetOrdinal("access_level")];
                    result.name = (string)dr[dr.GetOrdinal("name")];
                    result.datetime = (DateTime)dr[dr.GetOrdinal("datetime")];
                    if (dr.IsDBNull(dr.GetOrdinal("image"))) result.image = null;
                    else result.image = (byte[])dr[dr.GetOrdinal("image")];
                    allFine = true;

                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }
            

            if (allFine)
                return result;
            else
                return null;
        }

        public bool UserExist(string login)
        {
            bool result = false;
            string command = string.Format("SELECT * FROM [User] WHERE login ='{0}'", login);

            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result = true;
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }


            return result;
           
        }
        public CarInfoClass ReturnCarInfoById(int carInfoId)
        {
            CarInfoClass result = new CarInfoClass();
            string command = string.Format("SELECT * FROM [CarInfo] WHERE id ='{0}'", carInfoId);

            bool allFine = false;
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result.id = (int)dr[dr.GetOrdinal("Id")];
                    result.body_type = (string)dr[dr.GetOrdinal("body_type")];
                    result.car_make = (string)dr[dr.GetOrdinal("car_make")];
                    result.promoter_id = (int)dr[dr.GetOrdinal("promoter_id")];

                    if (dr.IsDBNull(dr.GetOrdinal("description"))) result.description ="";  
                    else result.description = (string)dr[dr.GetOrdinal("description")];

                    result.max_speed = (int)dr[dr.GetOrdinal("max_speed")];
                    result.cost = (int)dr[dr.GetOrdinal("cost")];
                    result.datetime = (DateTime)dr[dr.GetOrdinal("datetime")];

                    if (dr.IsDBNull(dr.GetOrdinal("rating"))) result.rating = -1;
                    else result.rating = (float)dr[dr.GetOrdinal("rating")];

                    result.drive_type = (int)dr[dr.GetOrdinal("drive_type")];
                    result.transmission = (int)dr[dr.GetOrdinal("transmission")];

                    result.name = (string)dr[dr.GetOrdinal("name")];

                    if (dr.IsDBNull(dr.GetOrdinal("image"))) result.image = null;
                    else result.image = (byte[])dr[dr.GetOrdinal("image")];
                    allFine = true;

                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);
            }


            if (allFine)
                return result;
            else
                return null;
        }
      

        public bool AddComment(CommentClass comment)
        {
            //add comment
            bool allFine = false;
            string Command = "INSERT INTO [Comment](user_id,car_info_id, rating,text) Values(@user_id,@car_info_id, @rating,@text)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(Command, connection))
                {
                    // Добавить параметры
                    cmd.Parameters.AddWithValue("@user_id", comment.user_id);
                    cmd.Parameters.AddWithValue("@car_info_id", comment.car_info_id);
                    cmd.Parameters.AddWithValue("@rating", comment.rating);
                    cmd.Parameters.AddWithValue("@text", comment.text);

                    cmd.ExecuteNonQuery();
                    allFine = true;
                }

            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);

            }

            return allFine;
        }
        // возвращает id созданной таблицы
        public int AddCarInfo(CarInfoClass carInfo)
        {
            //add car info
            string Command = "INSERT INTO [CarInfo](body_type,car_make, promoter_id,description,max_speed,cost,rating,drive_type,name,transmission,image) Values(@body_type,@car_make, @promoter_id,@description,@max_speed,@cost,@rating,@drive_type,@name,@transmission,@image)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(Command, connection))
                {
                    // Добавить параметры
                    cmd.Parameters.AddWithValue("@body_type", carInfo.body_type);
                    cmd.Parameters.AddWithValue("@car_make", carInfo.car_make);
                    cmd.Parameters.AddWithValue("@promoter_id", carInfo.promoter_id);
                    cmd.Parameters.AddWithValue("@description", carInfo.description);
                    cmd.Parameters.AddWithValue("@max_speed", carInfo.max_speed);
                    cmd.Parameters.AddWithValue("@cost", carInfo.cost);
                    cmd.Parameters.AddWithValue("@rating", carInfo.rating);
                    cmd.Parameters.AddWithValue("@drive_type", carInfo.drive_type);
                    cmd.Parameters.AddWithValue("@name", carInfo.name);
                    cmd.Parameters.AddWithValue("@transmission", carInfo.transmission);
                    cmd.Parameters.AddWithValue("@image", carInfo.image);

                    //TODO:image cmd.Parameters.AddWithValue("@image", carInfo.image);

                    int a = cmd.ExecuteNonQuery();
                }

            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);

            }

            int addedCarId=-1;
            Command = "select @@IDENTITY";

            try
            {
                using (SqlCommand cmd = new SqlCommand(Command, connection))
                {
                    addedCarId = Int32.Parse(cmd.ExecuteScalar().ToString());
                }

            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);

            }

            return addedCarId;
        }
        public bool AddUser(UserClass user)
        {
            //add comment
            bool allFine = false;
            string Command = "INSERT INTO [User](login,password, access_level,name,image) Values(@login,@password, @access_level,@name,@image)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(Command, connection))
                {
                    // Добавить параметры
                    cmd.Parameters.AddWithValue("@login", user.login);
                    cmd.Parameters.AddWithValue("@password", user.password);
                    cmd.Parameters.AddWithValue("@access_level", user.access_level);
                    cmd.Parameters.AddWithValue("@name", user.name);
                    cmd.Parameters.AddWithValue("@image", user.image);

                    cmd.ExecuteNonQuery();
                    allFine = true;
                }

            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);

            }

            return allFine;
        }
        public bool AddStringNameToTable(string table,string name)
        {
            return false;
        }
     


        public bool DeleteTableById(string tableName,int tableExpIndex)
        {
            bool result = false;

            string Command = string.Format("Delete from [{0}] where id = '{1}'",tableName,tableExpIndex);
            using (SqlCommand cmd = new SqlCommand(Command, connection))
            {
                try
                {
                    int a = cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (SqlException ex)
                {
                    // Протоколировать исключение
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        public bool ChangeCarInfo(CarInfoClass carInfo)
        { 

            //add car info
            bool allFine = false;
            string Command = "UPDATE [CarInfo] set body_type=@body_type,car_make=@car_make, promoter_id=@promoter_id,description=@description,max_speed=@max_speed,cost=@cost,rating=@rating,drive_type=@drive_type,name=@name,transmission=@transmission,image=@image where id='" + carInfo.id.ToString() + "'";
            try
            {
                using (SqlCommand cmd = new SqlCommand(Command, connection))
                {
                    // Добавить параметры
                    cmd.Parameters.AddWithValue("@body_type", carInfo.body_type);
                    cmd.Parameters.AddWithValue("@car_make", carInfo.car_make);
                    cmd.Parameters.AddWithValue("@promoter_id", carInfo.promoter_id);
                    cmd.Parameters.AddWithValue("@description", carInfo.description);
                    cmd.Parameters.AddWithValue("@max_speed", carInfo.max_speed);
                    cmd.Parameters.AddWithValue("@cost", carInfo.cost);
                    cmd.Parameters.AddWithValue("@rating", carInfo.rating);
                    cmd.Parameters.AddWithValue("@drive_type", carInfo.drive_type);
                    cmd.Parameters.AddWithValue("@name", carInfo.name);
                    cmd.Parameters.AddWithValue("@transmission", carInfo.transmission);
                    cmd.Parameters.AddWithValue("@image", carInfo.image);

                    int a = cmd.ExecuteNonQuery();
                    allFine = true;
                }

            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);

            }

            return allFine;
        }

        public bool ChangeUserInfo(UserClass newUserData)
        {
            //add car info
            bool allFine = false;
            string Command = "UPDATE [User] set name=@name,access_level=@access_level,image=@image where id='" + newUserData.id.ToString() + "'";
            try
            {
                using (SqlCommand cmd = new SqlCommand(Command, connection))
                {
                    // Добавить параметры
                    cmd.Parameters.AddWithValue("@name", newUserData.name);
                    cmd.Parameters.AddWithValue("@image", newUserData.image);
                    if (newUserData.access_level!=-1)
                        cmd.Parameters.AddWithValue("@access_level", newUserData.access_level);

                    int a = cmd.ExecuteNonQuery();
                    allFine = true;
                }

            }
            catch (SqlException ex)
            {
                // Протоколировать исключение
                Console.WriteLine(ex.Message);

            }

            return allFine;
        }

        public bool DeleteCarInfo(int carInfoId)
        {
            bool result = false;

            // сначала нужно удалить все комментарии
            // принадлежащие этой записи
            string Command = string.Format("Delete from [Comment] where car_info_id = '{0}'",  carInfoId);
            using (SqlCommand cmd = new SqlCommand(Command, connection))
            {
                try
                {
                    int a = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Протоколировать исключение
                    Console.WriteLine(ex.Message);
                }
            }


            Command = string.Format("Delete from [CarInfo] where Id = '{0}'", carInfoId);
            using (SqlCommand cmd = new SqlCommand(Command, connection))
            {
                try
                {
                    int a = cmd.ExecuteNonQuery();
                    if (a>0) result = true;
                }
                catch (SqlException ex)
                {
                    // Протоколировать исключение
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        internal bool DeleteUserById(int userId)
        {
            bool result = false;

            // сначала нужно удалить все комментарии
            // принадлежащие этому пользователю 
            string Command = string.Format("Delete from [Comment] where user_id = '{0}'", userId);
            using (SqlCommand cmd = new SqlCommand(Command, connection))
            {
                try
                {
                    int a = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Протоколировать исключение
                    Console.WriteLine(ex.Message);
                }
            }

            // затем удаляем всю его выложенную информацию
            // о мащинах
            Command = string.Format("Delete from [CarInfo] where promoter_id = '{0}'", userId);
            using (SqlCommand cmd = new SqlCommand(Command, connection))
            {
                try
                {
                    int a = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Протоколировать исключение
                    Console.WriteLine(ex.Message);
                }
            }

            Command = string.Format("Delete from [User] where Id = '{0}'", userId);
            using (SqlCommand cmd = new SqlCommand(Command, connection))
            {
                try
                {
                    int a = cmd.ExecuteNonQuery();
                    if (a > 0) result = true;
                }
                catch (SqlException ex)
                {
                    // Протоколировать исключение
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }
    }
}
