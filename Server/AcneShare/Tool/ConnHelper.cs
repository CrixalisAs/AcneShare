using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AcneShare.Tool
{
    class ConnHelper
    {
        public const string CONNECTIONSTRING = "datasource=127.0.0.1;port=3306;database=acne;user=root ;pwd=root";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据库时出现异常" + e);
                return null;
            }
        }

        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }
            else
            {
                Console.WriteLine("MySqlConnection不能为空");
            }
        }
    }
}
