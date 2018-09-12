using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcneShare.Tool
{
    public class Tools
    {
        public static byte[] ParseBytes(string data)
        {
            string[] bs = data.Split('|');
            return Array.ConvertAll<string, byte>(bs, byte.Parse);
        }
        public static string PackBytes(byte[] bytes)
        {
            if (bytes == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(((short)b).ToString());
                sb.Append('|');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        public static bool checkDir(string url)
        {
            try
            {
                if (!Directory.Exists(url)) //如果不存在就创建file文件夹　　             　　              
                    Directory.CreateDirectory(url); //创建该文件夹　　            
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
