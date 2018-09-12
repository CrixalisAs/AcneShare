using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcneShare.DAO;
using AcneShare.Model;
using AcneShare.Servers;
using AcneShare.Tool;
using Common;
using Task = AcneShare.Model.Task;

namespace AcneShare.Controller
{
    class InfoController:BaseController
    {
        ScheduleDAO scheduleDAO=new ScheduleDAO();
        TaskDAO taskDAO=new TaskDAO();
        HistoryDAO historyDAO=new HistoryDAO();
        UserDAO userDAO = new UserDAO();
        public InfoController()
        {
            requestCode = RequestCode.Info;
        }
        public string AddSchedule(string data, Client client, Server server)
        {
            string[] strs = data.Split('$');
            string time = strs[0];
            byte isNotice = byte.Parse(strs[1]);
            string content = strs[2];
            int id = scheduleDAO.AddSchedule(client.MySQLConn, client.GetUserId(), time,isNotice,content);
            return id+"$"+UpdateSchedule("", client, server);
        }
        public string DeleteSchedule(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            scheduleDAO.DeleteSchedule(client.MySQLConn,id);
            return UpdateSchedule("", client, server);
        }
        public string UpdateSchedule(string data, Client client, Server server)
        {
            List<Schedule> schedules = scheduleDAO.GetSchedules(client.MySQLConn,client.GetUserId());
            if (schedules != null&&schedules.Count!=0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Schedule schedule in schedules)
                {
                    sb.Append(schedule.Id);
                    sb.Append("&");
                    sb.Append(schedule.Date);
                    sb.Append("&");
                    sb.Append(schedule.IsNotice);
                    sb.Append("&");
                    sb.Append(schedule.Content);
                    sb.Append("$");
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            else
            {
                Console.WriteLine("无schedule");
            }
            return "";
        }
        public string ScheduleAmount(string data, Client client, Server server)
        {
            List<Schedule> schedules = scheduleDAO.GetSchedules(client.MySQLConn, client.GetUserId());
            if (schedules != null)
            {
                return schedules.Count.ToString();
            }
            return "0";
        }
        public string TaskInfo(string data, Client client, Server server)
        {
            return UpdateTask("", client, server);
        }
        public string AddTask(string data, Client client, Server server)
        {
            string content = data;
            taskDAO.AddTask(client.MySQLConn, client.GetUserId(), content);
            return UpdateTask("", client, server);
        }

        public string ConfirmTask(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            int state = int.Parse(strs[1]);
            Console.WriteLine("id:" + id);
            Console.WriteLine("state:"+state);
            taskDAO.ConfirmTask(client.MySQLConn,id,state);
            return UpdateTask("", client, server);
        }
        public string DeleteTask(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            taskDAO.DeleteTask(client.MySQLConn, id);
            return UpdateTask("", client, server);
        }
        public string UpdateTask(string data, Client client, Server server)
        {
            List<Task> tasks = taskDAO.GetTasks(client.MySQLConn, client.GetUserId());
            if (tasks != null && tasks.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Task task in tasks)
                {
                    sb.Append(task.Id);
                    sb.Append("&");
                    sb.Append(task.State);
                    sb.Append("&");
                    sb.Append(task.Content);
                    sb.Append("$");
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            else
            {
                Console.WriteLine("无task");
            }
            return "";
        }
        public string AddHistoryPhoto(string data, Client client, Server server)
        {
            historyDAO.UpdateHistory(client.MySQLConn, client.GetUserId(), Tools.ParseBytes(data));
            return null;
        }
        public string AddHistoryContent(string data, Client client, Server server)
        {
            historyDAO.UpdateHistory(client.MySQLConn, client.GetUserId(), data);
            return null;
        }
        public string UpdateHistory(string data, Client client, Server server)
        {
            List<History> histories = historyDAO.GetHistories(client.MySQLConn, client.GetUserId());
            if (histories != null && histories.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (History history in histories)
                {
                    sb.Append(history.Id);
                    sb.Append("&");
                    sb.Append(history.Date.ToString("D"));
                    sb.Append("&");
                    Console.WriteLine(Tools.PackBytes(history.Photo).Length);
                    sb.Append(Tools.PackBytes(history.Photo));
                    Console.WriteLine(Tools.PackBytes(history.Photo).Length);
                    sb.Append("&");
                    sb.Append(history.Content);
                    sb.Append("$");
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            else
            {
                Console.WriteLine("无history");
            }
            return "";
        }
    }
}
