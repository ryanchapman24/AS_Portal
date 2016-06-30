using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class Position
    {
        public Position()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }
      
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}