using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{
    public enum TaskState
    {
        UnConfrimed,
        Finished,
        UnFinished
    }
    public class Task
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public TaskState TaskState { get; set; }
        public Task(int id,string content,TaskState taskState)
        {
            Id = id;
            Content = content;
            TaskState = taskState;
        }
    }
}
