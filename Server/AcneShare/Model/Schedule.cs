using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcneShare.Model
{
    class Schedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public byte IsNotice { get; set; }
        public string Content { get; set; }

        public Schedule(int id, int userId, string date, byte isNotice, string content)
        {
            Id = id;
            UserId = userId;
            Date = date;
            IsNotice = isNotice;
            Content = content;
        }
    }
}
