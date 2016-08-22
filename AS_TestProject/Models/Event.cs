using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AllDay { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}