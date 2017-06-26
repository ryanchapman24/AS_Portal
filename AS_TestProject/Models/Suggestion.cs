using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class Suggestion
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string AuthorId { get; set; }
        public int EmployeeID { get; set; }
        public int SuggestionTypeId { get; set; }
        public string Body { get; set; }
        public bool New { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual SuggestionType SuggestionType { get; set; }
    }
}