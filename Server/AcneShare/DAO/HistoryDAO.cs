using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Model;
using AcneShare.Tool;
using MySql.Data.MySqlClient;
using Task = AcneShare.Model.Task;

namespace AcneShare.DAO
{
    class HistoryDAO
    {
        public void UpdateHistory(MySqlConnection conn, int userId,byte[] image)
        {
            MySqlDataReader reader = null;
            try
            {
                Tools.checkDir("C:/UserData/"+userId+"/HistoryPhoto");
                string path = "C:/UserData/" + userId + "/HistoryPhoto/" + DateTime.Now.ToString("D") + ".png";
                File.WriteAllBytes(path, image);
                MySqlCommand cmd = new MySqlCommand("select * from history where date=@date ", conn);
                cmd.Parameters.AddWithValue("date", DateTime.Now.ToString("D"));
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string date = reader.GetString("date");
                    reader.Close();
                    cmd = new MySqlCommand("update history set path=@path where date=@date", conn);
                    cmd.Parameters.AddWithValue("path", path);
                    cmd.Parameters.AddWithValue("date", date);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    cmd = new MySqlCommand("insert into history set userId=@userId ,path=@path,date=@date,content=@content", conn);
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("path", path);
                    cmd.Parameters.AddWithValue("date", DateTime.Now.ToString("D"));
                    cmd.Parameters.AddWithValue("content", "null");
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdateHistory时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        public void UpdateHistory(MySqlConnection conn, int userId, string content)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from history where date=@date ", conn);
                cmd.Parameters.AddWithValue("date", DateTime.Now.ToString("D"));
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string date = reader.GetString("date");
                    reader.Close();
                    cmd = new MySqlCommand("update history set content=@content where date=@date", conn);
                    cmd.Parameters.AddWithValue("content", content);
                    cmd.Parameters.AddWithValue("date", date);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    cmd = new MySqlCommand("insert into history set userId=@userId ,path=@path,content=@content,date=@date", conn);
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("content", content);
                    cmd.Parameters.AddWithValue("date", DateTime.Now.ToString("D"));
                    cmd.Parameters.AddWithValue("path", "null");

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdateHistory时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        public List<History> GetHistories(MySqlConnection conn, int userId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from history where userId=@userId",
                    conn);
                cmd.Parameters.AddWithValue("userId", userId);
                reader = cmd.ExecuteReader();
                List<History> histories = new List<History>();
                while (reader.Read())
                {
                    string date = reader.GetString("date");
                    int id = reader.GetInt32("id");
                    string path = reader.IsDBNull(reader.GetOrdinal("path")) ? "" : reader.GetString("path");
                    string content = reader.IsDBNull(reader.GetOrdinal("content")) ? "" : reader.GetString("content");
                    if (content == "null") content = "";
                    if (path=="null")
                    {
                        histories.Add(new History(id, userId, DateTime.Parse(date), content, null));
                    }
                    else
                    {
                        FileStream fs = File.OpenRead(path); //OpenRead
                        int filelength = 0;
                        filelength = (int)fs.Length; //获得文件长度 
                        Byte[] image = new Byte[filelength]; //建立一个字节数组 
                        fs.Read(image, 0, filelength); //按字节流读取 
                        fs.Close();
                        histories.Add(new History(id, userId, DateTime.Parse(date), content, image));
                    }
                }
                return histories;
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetHistories时出现异常：" + e);
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
    }
}
