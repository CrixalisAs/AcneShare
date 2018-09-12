using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcneShare.Model
{
    class User
    {
        public byte Online { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Sex { get; set; }//0男1女
        public byte[] Image { get; set; }

        public User(int id, string username, string password, byte online,string name,int age,bool sex,byte[] image)
        {
            Id = id;
            Username = username;
            Password = password;
            Online = online;
            Name = name;
            Age = age;
            Sex = sex;
            Image = image;
        }
    }
}
