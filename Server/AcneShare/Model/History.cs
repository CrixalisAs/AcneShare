using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcneShare.Model
{
    class History
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public byte[] Photo { get; set; }
        public string Content { get; set; }

        public History(int id,int userId, DateTime date, string content, byte[] photo)
        {
            Id = id;
            UserId = userId;
            Date = date;
            Photo = photo;
            Content = content;
        }
    }
}
