using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.Servers;
using Common;

namespace AcneShare.Controller
{
    class DefaultController : BaseController
    {
        public string None(string data, Client client, Server server)
        {
            Console.WriteLine(data);
            return null;
        }
    }
}
