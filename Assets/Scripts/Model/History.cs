using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{
    public class History
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public byte[] Photo { get; set; }
        public string Content { get; set; }

        public History(int id, DateTime date,  string content,byte[] photo)
        {
            Id = id;
            Date = date;
            Photo = photo;
            Content = content;
        }
    }
}
