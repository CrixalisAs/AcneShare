using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Servers;
using AcneShare.Tool;

namespace AcneShare
{
    class Program
    {
        //public static string user;
        //public static string pwd;
        static void Main(string[] args)
        {

            //Tools.checkDir("C:/UserData/5/HistoryPhoto");
            //Server server = new Server("172.16.252.14", 6688);
            Server server = new Server("127.0.0.1", 6688);
            //Console.Write("输入服务器内网IP：");
            //string IP = Console.ReadLine();
            //Console.Write("输入服务器TCP端口：");
            //string PORT = Console.ReadLine();
            //Console.Write("输入数据库用户名：");
            //user = Console.ReadLine();
            //Console.Write("输入数据库密码：");
            //pwd = Console.ReadLine();
            //Server server = new Server(IP, int.Parse(PORT));
            server.Start();
            //Console.WriteLine(IP+":"+PORT);
            Console.WriteLine("服务器开启");
            Console.ReadKey();
        }
    }
}
