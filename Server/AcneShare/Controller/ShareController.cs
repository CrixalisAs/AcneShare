using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.DAO;
using AcneShare.Model;
using AcneShare.Servers;
using AcneShare.Tool;
using Common;

namespace AcneShare.Controller
{
    class ShareController:BaseController
    {
        private ShareDAO shareDAO=new ShareDAO();
        private UserDAO userDAO=new UserDAO();
        public ShareController()
        {
            requestCode = RequestCode.Share;
        }

        public string Share(string data, Client client, Server server)
        {
            shareDAO.AddShare(client.MySQLConn,client.GetUserId(),data);
            return null;
        }

        public string UpdateShare(string data, Client client, Server server)
        {
            Console.WriteLine("UpdateShare");
            List<Share> shares = shareDAO.GetShares(client.MySQLConn);
            shares.Reverse();
            if (shares != null&&shares.Count!=0)
            {
                StringBuilder sb=new StringBuilder();
                foreach (Share share in shares)
                {
                    User user = userDAO.GetUserById(client.MySQLConn, share.UserId);
                    sb.Append(user.Name);
                    sb.Append("&");
                    sb.Append(Tools.PackBytes(user.Image));
                    sb.Append("&");
                    sb.Append(share.Content);
                    sb.Append("$");
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return "";
        }
    }
}
