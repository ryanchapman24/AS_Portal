using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class InboundMessage
    {
        public int Id { get; set; }
        public DateTimeOffset Sent { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public string ReceiverId { get; set; }
        public bool Out { get; set; }
        public bool Read { get; set; }
        public bool Urgent { get; set; }
        public bool Active { get; set; }
        public bool Ghost { get; set; }
        public string File { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}