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
    class KnowledgeController:BaseController
    {
        KnowledgeDAO knowledgeDAO=new KnowledgeDAO();
        public KnowledgeController()
        {
            requestCode = RequestCode.Knowledge;
        }
        public string AddKnowledge(string data, Client client, Server server)
        {
            if (data == "") return null;
            string[] strs = data.Split('$');
            List<Knowledge> knowledges=new List<Knowledge>();
            foreach (var str in strs)
            {
                string[] s = str.Split('&');
                knowledges.Add(new Knowledge(s[0],s[1],(KnowledgeType)Enum.Parse(typeof(KnowledgeType),s[2])));
            }
            knowledgeDAO.AddKnowledges(client.MySQLConn, knowledges);
            return UpdateKnowledge("",client,server);
        }

        public string UpdateKnowledge(string data, Client client, Server server)
        {
            List<Knowledge> knowledges = knowledgeDAO.GetKnowledges(client.MySQLConn);
            if (knowledges != null && knowledges.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Knowledge knowledge in knowledges)
                {
                    sb.Append(knowledge.Id);
                    sb.Append("&");
                    sb.Append(knowledge.Title);
                    sb.Append("&");
                    sb.Append(knowledge.Date);
                    sb.Append("&");
                    sb.Append(knowledge.Content);
                    sb.Append("&");
                    sb.Append(knowledge.IsNew?1:0);
                    sb.Append("&");
                    sb.Append(knowledge.Type);
                    sb.Append("$");
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return "";
        }
    }
}
