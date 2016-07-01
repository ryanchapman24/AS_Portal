using AS_TestProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                //ViewBag.ProfilePic = user.ProfilePic;
                base.OnActionExecuting(filterContext);
            }
        }
    }
}