using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AcneShare.Model
{
    class Knowledge
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public bool IsNew { get; set; }
        public KnowledgeType Type { get; set; }

        public Knowledge(int id, string title, DateTime date, string content, bool isNew,KnowledgeType type)
        {
            Id = id;
            Title = title;
            Date = date;
            Content = content;
            IsNew = isNew;
            Type = type;
        }
        public Knowledge(string title, string content, KnowledgeType type)
        {
            Title = title;
            Content = content;
            Type = type;
        }
    }
}
