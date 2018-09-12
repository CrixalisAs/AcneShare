using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Servers;
using Common;

namespace AcneShare.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }

        private void InitController()
        {
            controllerDict.Add(RequestCode.None, new DefaultController());
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Share, new ShareController());
            controllerDict.Add(RequestCode.Info, new InfoController());
            controllerDict.Add(RequestCode.Knowledge, new KnowledgeController());
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller;
            if (controllerDict.TryGetValue(requestCode, out controller) == false)
            {
                Console.WriteLine("无法得到" + requestCode + "所对应的Controller,无法处理请求");
                return;
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[警告]在Controller[" + controller.GetType() + "]中没有对应的处理方法:[" + methodName + "]");
                return;
            }
            object[] parameters = { data, client, server };
            object o = mi.Invoke(controller, parameters);
            Console.WriteLine(requestCode.ToString() + " " + actionCode.ToString() );
            if (o == null )
            {
                return;
            }
            server.SendResponse(client, actionCode, o as string);
        }
    }
}
