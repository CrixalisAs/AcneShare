using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            sb.Append(((short)b).ToString());
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
    public static string[] Search(string param, string[] datas)
    {
        if (string.IsNullOrEmpty(param))
            return null;

        string[] words = param.Split(new char[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            int maxDist = (word.Length - 1) / 2;

            var q = from str in datas
                where word.Length <= str.Length
                      && Enumerable.Range(0, maxDist + 1)
                          .Any(dist =>
                          {
                              return Enumerable.Range(0, Math.Max(str.Length - word.Length - dist + 1, 0))
                                  .Any(f =>
                                  {
                                      return Distance(word, str.Substring(f, word.Length + dist)) <= maxDist;
                                  });
                          })
                orderby str
                select str;
            datas = q.ToArray();
        }

        return datas;
    }

    static int Distance(string str1, string str2)
    {
        int n = str1.Length;
        int m = str2.Length;
        int[,] C = new int[n + 1, m + 1];
        int i, j, x, y, z;
        for (i = 0; i <= n; i++)
            C[i, 0] = i;
        for (i = 1; i <= m; i++)
            C[0, i] = i;
        for (i = 0; i < n; i++)
        for (j = 0; j < m; j++)
        {
            x = C[i, j + 1] + 1;
            y = C[i + 1, j] + 1;
            if (str1[i] == str2[j])
                z = C[i, j];
            else
                z = C[i, j] + 1;
            C[i + 1, j + 1] = Math.Min(Math.Min(x, y), z);
        }
        return C[n, m];
    }

}
