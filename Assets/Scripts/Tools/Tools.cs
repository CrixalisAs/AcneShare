using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Tools {

    public static byte[] ReadPNG(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);

        fileStream.Seek(0, SeekOrigin.Begin);

        byte[] binary = new byte[fileStream.Length]; //创建文件长度的buffer 
        fileStream.Read(binary, 0, (int)fileStream.Length);

        fileStream.Close();

        fileStream.Dispose();

        fileStream = null;

        return binary;
    }

    public static string PackBytes(byte[] bytes)
    {
        StringBuilder sb=new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b);
            sb.Append('|');
        }
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }
    public static byte[] ParseBytes(string data)
    {
        string[] bs = data.Split('|');
        return Array.ConvertAll<string, byte>(bs, byte.Parse);
    }
}
