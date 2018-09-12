using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Model;
using Common;
using MySql.Data.MySqlClient;

namespace AcneShare.DAO
{
    class KnowledgeDAO
    {
        public void AddKnowledges(MySqlConnection conn, List<Knowledge> knowledges)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update knowledge set isNew=@isNew", conn);
                cmd.Parameters.AddWithValue("isNew", 0);
                cmd.ExecuteNonQuery();
                DateTime date=DateTime.Now;
                foreach (Knowledge knowledge in knowledges)
                {
                    cmd = new MySqlCommand("insert into knowledge set date=@date,title=@title , content=@content,isNew=@isNew,type=@type", conn);
                    cmd.Parameters.AddWithValue("title", knowledge.Title);
                    cmd.Parameters.AddWithValue("content", knowledge.Content);
                    cmd.Parameters.AddWithValue("isNew", 1);
                    cmd.Parameters.AddWithValue("date", date.ToString());
                    cmd.Parameters.AddWithValue("type", (int)knowledge.Type);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddShare时出现异常：" + e);
            }
        }
        public List<Knowledge> GetKnowledges(MySqlConnection conn)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from knowledge",
                    conn);
                reader = cmd.ExecuteReader();
                List<Knowledge> knowledges = new List<Knowledge>();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string title = reader.GetString("title");
                    string date = reader.GetString("date");
                    string content = reader.GetString("content");
                    byte isNew = reader.GetByte("isNew");
                    KnowledgeType type = (KnowledgeType) reader.GetInt32("type");
                    knowledges.Add(new Knowledge(id, title,DateTime.Parse(date), content,isNew==1, type));
                }
                knowledges.Reverse();
                return knowledges;
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
