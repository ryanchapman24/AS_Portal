using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_TestProject.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public string Additional { get; set; }
        public int NotificationTypeId { get; set; }
        public string NotifyUserId { get; set; }
        public bool New { get; set; }

        public virtual ApplicationUser NotifyUser { get; set; }
        public virtual NotificationType NotificationType { get; set; }
    }
}