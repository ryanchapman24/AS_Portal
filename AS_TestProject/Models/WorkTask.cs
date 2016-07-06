using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class WorkTask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int TaskPriorityId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public bool Complete { get; set; }
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual TaskPriority TaskPriority { get; set; }
    }
}