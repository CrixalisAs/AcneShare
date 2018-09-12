using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Model;
using MySql.Data.MySqlClient;

namespace AcneShare.DAO
{
    class ShareDAO
    {
        public void AddShare(MySqlConnection conn, int userId,string content)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into share set userId=@userId , content=@content", conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("content", content);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddShare时出现异常：" + e);
            }
        }
        public List<Share> GetShares(MySqlConnection conn)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from share",
                    conn);
                reader = cmd.ExecuteReader();
                List<Share> shares=new List<Share>();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int userId = reader.GetInt32("userId");
                    string content = reader.GetString("content");
                    shares.Add(new Share(id,userId,content));
                }
                return shares;
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
    }
}
