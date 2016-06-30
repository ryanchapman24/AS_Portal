using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class Site
    {
        public Site()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string MainPhoneNumber { get; set; }
        public System.DateTime AddDate { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}