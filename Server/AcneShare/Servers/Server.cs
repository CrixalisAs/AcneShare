using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AcneShare.Controller;
using Common;

namespace AcneShare.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private List<Room> roomList = new List<Room>();
        private ControllerManager controllerManage;
        public Server()
        {
            controllerManage = new ControllerManager(this);
        }

        public Server(string ipStr, int port)
        {
            controllerManage = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
        }

        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSocket.Bind(ipEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
            serverSocket.Listen(0);
            Console.WriteLine("BeginAccept");
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Console.WriteLine("ClientCallBack");
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);
            Console.WriteLine("一个客户端接入了");
            serverSocket.BeginAccept(AcceptCallBack, null);
        }
        
        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }

        public bool IsSocketConnected(Socket clientSocket)
        {
            return !((clientSocket.Poll(1000, SelectMode.SelectRead) && (clientSocket.Available == 0)) || !clientSocket.Connected);
        }
        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {

            foreach (Client client in clientList)
            {
                if (client != excludeClient)
                {
                    SendResponse(client, actionCode, data);
                }
            }
        }
        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            client.Send(actionCode, data);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManage.HandleRequest(requestCode, actionCode, data, client);
        }

        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.Create(client);
            roomList.Add(room);
        }

        public List<Room> GetRoomList()
        {
            return roomList;
        }

        public void RemoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }
        
    }
}
