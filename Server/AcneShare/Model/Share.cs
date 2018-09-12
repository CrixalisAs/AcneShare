using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcneShare.Model
{
    class Share
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }

        public Share(int id, int userId, string content)
        {
            Id = id;
            UserId = userId;
            Content = content;
        }
    }
}
