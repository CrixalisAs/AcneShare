using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AcneShare.DAO;
using AcneShare.Model;
using AcneShare.Servers;
using AcneShare.Tool;
using Common;

namespace AcneShare.Controller
{
    class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.VerifyUser(client.MySQLConn, strs[0], strs[1]);
            if (user == null)
            {
                return ((int)ReturnCode.NotFind).ToString();
            }
            else if (user.Online == 1)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                //Result result = resultDAO.GetResultByUserId(client.MySQLConn, user.Id);
                client.SetUserData(user);
                userDAO.UpdateOnline(client.MySQLConn, user.Username, true);
                return string.Format("{0},{1},{2},{3},{4},{5},{6}", ((int)ReturnCode.Success), user.Id, user.Username,user.Name,user.Age,user.Sex?1:0,Tools.PackBytes(user.Image));
            }
        }
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];
            string name = strs[2];
            int age = int.Parse(strs[3]);
            string sex = strs[4];

            if (userDAO.HasUser(client.MySQLConn, username))
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                userDAO.AddUser(client.MySQLConn, username, password, name, age, sex, Tools.ParseBytes(strs[5]));
                return ((int)ReturnCode.Success).ToString();
            }
        }
        public string QuitLogin(string data, Client client, Server server)
        {
            userDAO.UpdateOnline(client.MySQLConn, data, false);
            return null;
        }


    }
}
