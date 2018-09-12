using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Model;
using MySql.Data.MySqlClient;
using Task = AcneShare.Model.Task;

namespace AcneShare.DAO
{
    class TaskDAO
    {
        public int AddTask(MySqlConnection conn, int userId, string content)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into task set userId=@userId ,content=@content,date=@date,state=0", conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("content", content);
                cmd.Parameters.AddWithValue("date", DateTime.Now.ToString("D"));
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("SELECT * FROM task where userId=@userId ORDER BY id DESC LIMIT 1", conn);
                cmd.Parameters.AddWithValue("userId", userId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt32("id");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddTask时出现异常：" + e);
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
        public List<Task> GetTasks(MySqlConnection conn, int userId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from task where userId=@userId",
                    conn);
                cmd.Parameters.AddWithValue("userId", userId);
                reader = cmd.ExecuteReader();
                List<Task> tasks = new List<Task>();
                bool isUpdate = false;
                while (reader.Read())
                {
                    string date = reader.GetString("date");
                    if (!string.Equals(date, DateTime.Now.ToString("D")))
                    {
                        isUpdate = true;
                    }
                    int id = reader.GetInt32("id");
                    int state = reader.GetInt32("state");
                    string content = reader.GetString("content");
                    if (isUpdate)
                    {
                        state = 0;
                    }
                    tasks.Add(new Task(id,userId,content,state));
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (isUpdate)
                {
                    cmd= new MySqlCommand("update task set state=0 , date=@date where userId=@userId", conn);
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("date", DateTime.Now.ToString("D"));
                    cmd.ExecuteNonQuery();
                }
                return tasks;
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetTasks时出现异常：" + e);
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
        public void DeleteTask(MySqlConnection conn, int id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("delete from task where id=@id", conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("在DeleteTask时出现异常：" + e);
            }
        }

        public void ConfirmTask(MySqlConnection conn, int id,int state)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update task set state=@state where id=@id", conn);
                cmd.Parameters.AddWithValue("state", state);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("在ConfirmTask时出现异常：" + e);
            }
        }
    }
}
