using AS_TestProject.Entities;
using AS_TestProject.Models;
using AS_TestProject.Models.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AS_TestProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : UserNames
    {
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var loggedUser = db.Users.Find(User.Identity.GetUserId());

            var mb = new ReportEntities();            
            //Now, using the mb object you have access to you data
            //You can select, add, delete according to your permission level
            //This is where you could use LINQ to SQL
            ViewBag.Customers = mb.Customers.OrderBy(c => c.CustomerName).ToList();

            ViewBag.Employees = db.Users.Where(e => e.SiteID == loggedUser.SiteID).ToList().OrderBy(e => e.LastName);

            List<AdminUserListModels> users = new List<AdminUserListModels>();
            UserRolesHelper helper = new UserRolesHelper(db);
            foreach (var user in db.Users.ToList())
            {
                var eachUser = new AdminUserListModels();
                eachUser.roles = new List<string>();
                eachUser.user = user;
                eachUser.roles = helper.ListUserRoles(user.Id).ToList();

                users.Add(eachUser);
            }
            return View(users.OrderBy(u => u.user.LastName).ToList());
        }

        // Get: Admin/EditUserRoles
        [Authorize(Roles = "Admin")]
        public ActionResult EditUserRoles(string id)
        {
            var user = db.Users.Find(id);
            UserRolesHelper helper = new UserRolesHelper(db);
            var model = new AdminUserViewModels();
            model.Name = user.DisplayName;
            model.Id = user.Id;
            model.SelectedRoles = helper.ListUserRoles(id).ToArray();
            model.Roles = new MultiSelectList(db.Roles, "Name", "Name", model.SelectedRoles);

            return View(model);
        }

        // Post: Admin/EditUserRoles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUserRoles(AdminUserViewModels model)
        {
            var user = db.Users.Find(model.Id);
            UserRolesHelper helper = new UserRolesHelper(db);

            foreach (var role in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(user.Id, role);
            }

            if (model.SelectedRoles != null)
            {
                foreach (var role in model.SelectedRoles)
                {
                    helper.AddUserToRole(user.Id, role);
                }

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
    }
}