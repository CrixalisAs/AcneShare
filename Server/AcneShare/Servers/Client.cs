using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AcneShare.DAO;
using AcneShare.Model;
using AcneShare.Tool;
using Common;
using MySql.Data.MySqlClient;

namespace AcneShare.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
        private MySqlConnection mySqlConn;
        private User user;
        private Room room;
        private UserDAO userDAO = new UserDAO();
        
        
        public void SetUserData(User user)
        {
            this.user = user;
        }

        public Socket GetClientSocket()
        {
            return clientSocket;
        }
        public MySqlConnection MySQLConn => mySqlConn;

        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mySqlConn = ConnHelper.Connect();
        }

        public Room Room
        {
            set => room = value;
            get => room;
        }

        public int GetUserId()
        {
            return user.Id;
        }
        public void Start()
        {
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {

            if (clientSocket == null || clientSocket.Connected == false) return;
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                    return;
                }
                msg.ReadMessage(count, OnProcessMessage);
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode, actionCode, data, this);
        }
        
        
        public void Close()
        {
            if (user != null)//未登入时user为空
                userDAO.UpdateOnline(mySqlConn, user.Username, false);
            if (room != null)
            {
                room.QuitClient(this);
            }
            ConnHelper.CloseConnection(mySqlConn);
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }

        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                clientSocket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息：" + e);
            }
        }
        
        public string QuitLogin(string data)
        {
            userDAO.UpdateOnline(MySQLConn, data, false);
            return null;
        }
    }
}
