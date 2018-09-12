using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcneShare.Model
{
    class Task
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int State { get; set; }

        public Task(int id, int userId, string content, int state)
        {
            Id = id;
            UserId = userId;
            Content = content;
            State = state;
        }
    }
}
