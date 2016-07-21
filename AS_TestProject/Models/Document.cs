using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string Department { get; set; }
        public DateTimeOffset Created { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}