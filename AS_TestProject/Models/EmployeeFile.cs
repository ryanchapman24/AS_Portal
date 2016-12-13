using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class EmployeeFile
    {
        public int Id { get; set; }
        public string File { get; set; }
        public string AuthorId { get; set; }
        public int EmployeeID { get; set; }
        public DateTimeOffset Created { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}