using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class UserData
{

    public UserData(string userData)
    {
        string[] strs = userData.Split(',');
        this.Id = int.Parse(strs[0]);
        this.Username = strs[1];
    }

    //public UserData(string username)
    //{
    //    Username = username;
    //}
    public UserData(int id, string username,string name,int age,bool sex,byte[] image)
    {
        Id = id;
        Username = username;
        Name = name;
        Age = age;
        Sex = sex;
        Image = image;

    }

    public int Id { get; private set; }
    public string Username { get; private set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public bool Sex { get; set; }
    public byte[] Image { get; set; }
}
