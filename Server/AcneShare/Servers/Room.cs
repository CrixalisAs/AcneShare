using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace AcneShare.Servers
{
    class Room
    {
        private Server server;
        internal List<Client> ClientRoom { get; set; } = new List<Client>();
        private Client owner;
        public const int PLAYER_NUM = 5;
        public bool IsGameStarted = false;
        private bool isTimerStarted = false;

        public Room(Server server)
        {
            this.server = server;
        }

        public void Create(Client client)
        {
            AddClient(client);
            owner = client;
        }
        
        public void AddClient(Client client)
        {
            ClientRoom.Add(client);
            client.Room = this;
        }
        
        public string GetRoomData(bool readyData = false)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Client client in ClientRoom)
            {
                sb.Append( "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public void BroadcastMessage(ActionCode actionCode, string data, Client excludeClient = null)
        {
            lock (ClientRoom)
            {
                for (int i = 0; i < ClientRoom.Count; i++)
                {
                    if (ClientRoom[i] != null)
                    {
                        if (ClientRoom[i] != excludeClient)
                        {
                            server.SendResponse(ClientRoom[i], actionCode, data);
                        }
                    }
                }
            }
        }
        public void BroadcastMessage(ActionCode actionCode, string data, List<Client> clients)
        {
            lock (ClientRoom)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i] != null)
                    {
                        server.SendResponse(clients[i], actionCode, data);
                    }
                }
            }
        }

        public void Close()
        {
            foreach (Client client in ClientRoom)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
            Console.WriteLine("Remove a room");
        }

        public void QuitClient(Client client)
        {
            ClientRoom.Remove(client);
        }
    }
}
