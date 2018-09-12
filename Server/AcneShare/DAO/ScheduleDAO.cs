using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Model;
using MySql.Data.MySqlClient;

namespace AcneShare.DAO
{
    class ScheduleDAO
    {
        public int AddSchedule(MySqlConnection conn, int userId, string time,byte isNotice,string content)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into schedule set userId=@userId ,time=@time,isNotice=@isNotice,content=@content", conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("time", time);
                cmd.Parameters.AddWithValue("isNotice", isNotice);
                cmd.Parameters.AddWithValue("content", content);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("SELECT * FROM schedule where userId=@userId ORDER BY id DESC LIMIT 1", conn);
                cmd.Parameters.AddWithValue("userId", userId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt32("id");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddSchedule时出现异常：" + e);
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
        public List<Schedule> GetSchedules(MySqlConnection conn,int userId)
        {
            MySqlDataReader reader = null;
            List<int> idToDelete=new List<int>();
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from schedule where userId=@userId",
                    conn);
                cmd.Parameters.AddWithValue("userId", userId);
                reader = cmd.ExecuteReader();
                List<Schedule> schedules = new List<Schedule>();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string time = reader.GetString("time");
                    DateTime dateTime=new DateTime();
                    dateTime = DateTime.Now;
                    string[] dates = time.Split('|');
                    string[] date = dates[0].Split('/');
                    int month = int.Parse(date[0]);
                    int day = int.Parse(date[1]);
                    int year= int.Parse(date[2]);
                    int h = int.Parse(dates[1]);
                    int m = int.Parse(dates[2]);
                    if (year < dateTime.Year)
                    {
                        idToDelete.Add(id);
                        continue;
                    }
                    else if (year == dateTime.Year)
                    {
                        if (month < dateTime.Month)
                        {
                            idToDelete.Add(id);
                            continue;
                        }
                        else if (month == dateTime.Month)
                        {
                            if (day < dateTime.Day)
                            {
                                idToDelete.Add(id);
                                continue;
                            }
                            else if (day == dateTime.Day)
                            {
                                if (h < dateTime.Hour)
                                {
                                    idToDelete.Add(id);
                                    continue;
                                }
                                else if (h == dateTime.Hour)
                                {
                                    if (m < dateTime.Minute)
                                    {
                                        idToDelete.Add(id);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    byte isNotice = reader.GetByte("isNotice");
                    string content = reader.GetString("content");
                    schedules.Add(new Schedule(id,userId,time, isNotice, content));
                }
                return schedules;
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetSchedules时出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                foreach (int id in idToDelete)
                {
                    DeleteSchedule(conn,id);
                }
            }
            return null;
        }
        public void DeleteSchedule(MySqlConnection conn, int id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("delete from schedule where id=@id", conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("在DeleteSchedule时出现异常：" + e);
            }
        }
    }
}
