using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Model;
using AcneShare.Tool;
using MySql.Data.MySqlClient;

namespace AcneShare.DAO
{
    class UserDAO
    {
        public User VerifyUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username and password=@password",
                    conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    byte online = reader.GetByte("online");
                    string name = reader.GetString("name");
                    int age = reader.GetInt32("age");
                    bool sex = reader.GetString("sex") == "女";
                    string path = reader.GetString("path");

                    FileStream fs = File.OpenRead(path); //OpenRead
                    int filelength = 0;
                    filelength = (int)fs.Length; //获得文件长度 
                    Byte[] image = new Byte[filelength]; //建立一个字节数组 
                    fs.Read(image, 0, filelength); //按字节流读取 
                    fs.Close();

                    User user = new User(id, username, password, online,name,age,sex,image);
                    reader.Close();
                    return user;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }

        public bool HasUser(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username",
                    conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return false;
        }
        public int GetIdByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username",
                    conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt32("id");
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return -1;
        }
        public User GetUserByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username",
                    conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    byte online = reader.GetByte("online");
                    string name = reader.GetString("name");
                    string password = reader.GetString("password");
                    int age = reader.GetInt32("age");
                    bool sex = reader.GetString("sex") == "女";
                    string path = reader.GetString("path");

                    FileStream fs = File.OpenRead(path); //OpenRead
                    int filelength = 0;
                    filelength = (int)fs.Length; //获得文件长度 
                    Byte[] image = new Byte[filelength]; //建立一个字节数组 
                    fs.Read(image, 0, filelength); //按字节流读取 
                    fs.Close();

                    User user = new User(id, username, password, online, name, age, sex, image);
                    return user;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }
        public User GetUserById(MySqlConnection conn, int id)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where id=@id",
                    conn);
                cmd.Parameters.AddWithValue("id", id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string username = reader.GetString("username");
                    byte online = reader.GetByte("online");
                    string name = reader.GetString("name");
                    string password = reader.GetString("password");
                    int age = reader.GetInt32("age");
                    bool sex = reader.GetString("sex") == "女";
                    string path = reader.GetString("path");

                    FileStream fs = File.OpenRead(path); //OpenRead
                    int filelength = 0;
                    filelength = (int)fs.Length; //获得文件长度 
                    Byte[] image = new Byte[filelength]; //建立一个字节数组 
                    fs.Read(image, 0, filelength); //按字节流读取 
                    fs.Close();

                    User user = new User(id, username, password, online, name, age, sex, image);
                    return user;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserById时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }
        public void AddUser(MySqlConnection conn, string username, string password, string name, int age, string sex,byte[] image)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username=@username , password=@password , name=@name , " +
                                                    "age=@age , sex=@sex , path=@path", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("age", age);
                cmd.Parameters.AddWithValue("sex", sex);
                cmd.Parameters.AddWithValue("path", "null");
                cmd.ExecuteNonQuery();
                int id = GetIdByUsername(conn, username);
                string path = "C:/UserData/" + id + "/HeadImage/" + id + ".png";
                Tools.checkDir("C:/UserData/" + id + "/HeadImage");
                File.WriteAllBytes(path, image);
                cmd = new MySqlCommand("update user set id=@id,path=@path where username=@username", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("path", path);
                cmd.ExecuteNonQuery();
                TaskDAO taskDAO=new TaskDAO();
                string tasks = "用清水与洗面奶洗脸,按医嘱服用口服药,按医嘱使用外用药膏,没有吃辛辣油腻食物,没有用手把痘痘挤破,做半小时以上运动,保持平静不生气不上火";
                string[] contents = tasks.Split(',');
                foreach (string content in contents)
                {
                    taskDAO.AddTask(conn, id, content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddUser时出现异常：" + e);
            }
        }
        public void UpdateOnline(MySqlConnection conn, string username, bool online)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update user set online=@online where username=@username", conn);
                cmd.Parameters.AddWithValue("online", Convert.ToByte(online));
                cmd.Parameters.AddWithValue("username", username);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdateOnline时出现异常：" + e);
            }
        }
    }
}
