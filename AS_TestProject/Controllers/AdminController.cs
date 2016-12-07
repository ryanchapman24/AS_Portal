using AS_TestProject.Entities;
using AS_TestProject.Models;
using AS_TestProject.Models.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AS_TestProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var loggedUser = db.Users.Find(User.Identity.GetUserId());

            var mb = new ReportEntities();            
            ViewBag.Employees = db.Users.Where(e => e.SiteID == loggedUser.SiteID).ToList().OrderBy(e => e.LastName);
            ViewBag.Customers = mb.Customers.OrderBy(c => c.CustomerName).ToList();
            ViewBag.Domains = mb.DomainMasters.OrderBy(d => d.FileMask).Include(d => d.Customer).Include(d => d.DomainType).ToList();
            ViewBag.Positions = mb.Positions.OrderBy(p => p.PositionName).ToList();

            var customers = mb.Customers.Where(c => c.IsActive == true).OrderBy(c => c.CustomerName);
            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CustomerName");
            ViewBag.DomainTypeID = new SelectList(mb.DomainTypes, "DomainTypeID", "DomainTypeName");

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

        // GET: Customers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult CustomerDetails(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = mb.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer([Bind(Include = "CustomerID,CustomerName,AddressLine1,AddressLine2,City,State,ZipCode,ImplementationDate,TerminationDate,AddDate,AddByEmployeeID,IsActive")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                customer.IsActive = true;
                customer.AddByEmployeeID = user.EmployeeID;
                if (customer.AddressLine1 == null)
                {
                    customer.AddressLine1 = "N/A";
                }
                if (customer.AddressLine2 == null)
                {
                    customer.AddressLine2 = "N/A";
                }
                if (customer.City == null)
                {
                    customer.City = "N/A";
                }
                if (customer.State == null)
                {
                    customer.State = "Q";
                }
                if (customer.ZipCode == null)
                {
                    customer.ZipCode = "N/A";
                }
                mb.Customers.Add(customer);
                mb.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult EditCustomer(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = mb.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "CustomerID,CustomerName,AddressLine1,AddressLine2,City,State,ZipCode,ImplementationDate,TerminationDate,AddDate,AddByEmployeeID,IsActive")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                mb.Customers.Attach(customer);
                if (customer.AddressLine1 == null)
                {
                    customer.AddressLine1 = "N/A";
                }
                if (customer.AddressLine2 == null)
                {
                    customer.AddressLine2 = "N/A";
                }
                if (customer.City == null)
                {
                    customer.City = "N/A";
                }
                if (customer.State == null)
                {
                    customer.State = "Q";
                }
                if (customer.ZipCode == null)
                {
                    customer.ZipCode = "N/A";
                }
                mb.Entry(customer).Property("CustomerName").IsModified = true;
                mb.Entry(customer).Property("AddressLine1").IsModified = true;
                mb.Entry(customer).Property("AddressLine2").IsModified = true;
                mb.Entry(customer).Property("City").IsModified = true;
                mb.Entry(customer).Property("State").IsModified = true;
                mb.Entry(customer).Property("ZipCode").IsModified = true;
                mb.Entry(customer).Property("TerminationDate").IsModified = true;
                mb.Entry(customer).Property("IsActive").IsModified = true;
                mb.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View(customer);
        }

        // GET: DomainMasters/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult DomainDetails(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainMaster domainMaster = mb.DomainMasters.Find(id);
            if (domainMaster == null)
            {
                return HttpNotFound();
            }
            Employee employee = mb.Employees.Find(domainMaster.AddByEmployeeID);
            ViewBag.EmployeeName = employee.FirstName + " " + employee.LastName;
            return View(domainMaster);
        }

        // POST: DomainMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDomain([Bind(Include = "DomainMasterID,DomainName,CustomerID,DomainTypeID,CostCode,FileMask,IsActive,DeactiveDate,AddDate,AddTime,AddByEmployeeID")] DomainMaster domain)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                domain.IsActive = true;
                domain.AddByEmployeeID = user.EmployeeID;
                mb.DomainMasters.Add(domain);
                mb.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            var customers = mb.Customers.Where(c => c.IsActive == true);
            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CustomerName");
            ViewBag.DomainTypeID = new SelectList(mb.DomainTypes, "DomainTypeID", "DomainTypeName");
            return RedirectToAction("Index", "Admin");
        }

        // GET: DomainMasters/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult EditDomain(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainMaster domain = mb.DomainMasters.Find(id);
            if (domain == null)
            {
                return HttpNotFound();
            }
            var customers = mb.Customers.Where(c => c.IsActive == true);
            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CustomerName", domain.CustomerID);
            ViewBag.DomainTypeID = new SelectList(mb.DomainTypes, "DomainTypeID", "DomainTypeName", domain.DomainTypeID);
            return View(domain);
        }

        // POST: DomainMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult EditDomain([Bind(Include = "DomainMasterID,DomainName,CustomerID,DomainTypeID,CostCode,FileMask,IsActive,DeactiveDate,AddDate,AddTime,AddByEmployeeID")] DomainMaster domain)
        {
            if (ModelState.IsValid)
            {
                mb.DomainMasters.Attach(domain);
                mb.Entry(domain).Property("DomainName").IsModified = true;
                mb.Entry(domain).Property("CustomerID").IsModified = true;
                mb.Entry(domain).Property("DomainTypeID").IsModified = true;
                mb.Entry(domain).Property("CostCode").IsModified = true;
                mb.Entry(domain).Property("FileMask").IsModified = true;
                mb.Entry(domain).Property("IsActive").IsModified = true;
                mb.Entry(domain).Property("DeactiveDate").IsModified = true;
                mb.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            var customers = mb.Customers.Where(c => c.IsActive == true);
            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CustomerName", domain.CustomerID);
            ViewBag.DomainTypeID = new SelectList(mb.DomainTypes, "DomainTypeID", "DomainTypeName", domain.DomainTypeID);
            return View(domain);
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePosition([Bind(Include = "PositionID,PositionName,PositionDescription")] Entities.Position position)
        {
            if (ModelState.IsValid)
            {
                mb.Positions.Add(position);
                mb.SaveChanges();

                Models.Position myPosition = new Models.Position();
                myPosition.PositionName = position.PositionName;
                myPosition.PositionDescription = position.PositionDescription;
                db.Positions.Add(myPosition);
                db.SaveChanges();

                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }

        // GET: Positions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult EditPosition(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities.Position position = mb.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPosition([Bind(Include = "PositionID,PositionName,PositionDescription")] Entities.Position position)
        {
            if (ModelState.IsValid)
            {
                mb.Positions.Attach(position);
                mb.Entry(position).Property("PositionName").IsModified = true;
                mb.Entry(position).Property("PositionDescription").IsModified = true;
                mb.SaveChanges();

                var myPosition = db.Positions.Find(position.PositionID);
                myPosition.PositionName = position.PositionName;
                myPosition.PositionDescription = position.PositionDescription;
                db.SaveChanges();

                return RedirectToAction("Index", "Admin");
            }
            return View(position);
        }
    }
}