using AS_TestProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AS_TestProject.Models
{
    public class UserNames : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                ViewBag.DisplayName = user.DisplayName;
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Position = user.Position.PositionName;
                ViewBag.PositionDescr = user.Position.PositionDescription;
                ViewBag.Site = user.Site.SiteName;
                ViewBag.Email = user.Email;
                ViewBag.PhoneNumber = user.PhoneNumber;
                ViewBag.ProfilePic = user.ProfilePic;

                ViewBag.TaskTally = user.TaskTally;
                ViewBag.UrgentTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
                ViewBag.HighTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
                ViewBag.MediumTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
                ViewBag.LowTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();

                ViewBag.Messages = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Read == false && m.Out == true && m.Active == true && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
                ViewBag.Team = db.Users.Where(t => t.SiteID == user.SiteID && t.Id != user.Id).OrderBy(t => t.FirstName);

                base.OnActionExecuting(filterContext);
            }
        }
    }
}