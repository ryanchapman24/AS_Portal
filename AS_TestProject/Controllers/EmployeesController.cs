using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_TestProject.Entities;
using AS_TestProject.Models;
using Microsoft.AspNet.Identity;

namespace AS_TestProject.Controllers
{
    public class EmployeesController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,EmailAddress,HomePhone,CellPhone,BirthDate,HireDate,TerminationDate,IsManager,ManagerEmployeeID,PositionID,SiteID,IsActive,AddDate,AddByEmployeeID,RehireDate,FileNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                employee.AddByEmployeeID = user.EmployeeID;
                employee.IsActive = true;
                if (employee.AddressLine2 == null)
                {
                    employee.AddressLine2 = "";
                }
                if (employee.HomePhone == null)
                {
                    employee.HomePhone = "";
                }
                if (employee.CellPhone == null)
                {
                    employee.CellPhone = "";
                }
                mb.Employees.Add(employee);
                mb.SaveChanges();
                return RedirectToAction("Directory", "Home");
            }

            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName", employee.SiteID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = mb.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName", employee.SiteID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,EmailAddress,HomePhone,CellPhone,BirthDate,HireDate,TerminationDate,IsManager,ManagerEmployeeID,PositionID,SiteID,IsActive,AddDate,AddByEmployeeID,RehireDate,FileNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                mb.Employees.Attach(employee);
                if (employee.AddressLine2 == null)
                {
                    employee.AddressLine2 = "";
                }
                if (employee.HomePhone == null)
                {
                    employee.HomePhone = "";
                }
                if (employee.CellPhone == null)
                {
                    employee.CellPhone = "";
                }
                mb.Entry(employee).Property("FirstName").IsModified = true;
                mb.Entry(employee).Property("LastName").IsModified = true;
                mb.Entry(employee).Property("AddressLine1").IsModified = true;
                mb.Entry(employee).Property("AddressLine2").IsModified = true;
                mb.Entry(employee).Property("City").IsModified = true;
                mb.Entry(employee).Property("State").IsModified = true;
                mb.Entry(employee).Property("ZipCode").IsModified = true;
                mb.Entry(employee).Property("EmailAddress").IsModified = true;
                mb.Entry(employee).Property("HomePhone").IsModified = true;
                mb.Entry(employee).Property("CellPhone").IsModified = true;
                mb.Entry(employee).Property("BirthDate").IsModified = true;
                mb.Entry(employee).Property("HireDate").IsModified = true;
                mb.Entry(employee).Property("TerminationDate").IsModified = true;
                mb.Entry(employee).Property("IsManager").IsModified = true;
                mb.Entry(employee).Property("ManagerEmployeeID").IsModified = true;
                mb.Entry(employee).Property("PositionID").IsModified = true;
                mb.Entry(employee).Property("SiteID").IsModified = true;
                mb.Entry(employee).Property("IsActive").IsModified = true;
                mb.Entry(employee).Property("RehireDate").IsModified = true;
                mb.Entry(employee).Property("FileNumber").IsModified = true;
                mb.SaveChanges();
                return RedirectToAction("Directory", "Home");
            }
            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName", employee.SiteID);
            return View(employee);
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee= mb.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            Employee manager = mb.Employees.Find(employee.ManagerEmployeeID);
            Employee creator = mb.Employees.Find(employee.AddByEmployeeID);
            ViewBag.Manager = manager.FirstName + " " + manager.LastName;
            ViewBag.Creator = creator.FirstName + " " + creator.LastName;

            ViewBag.DisciplinaryActions = mb.DisciplinaryActions.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(d => d.Date).ToList();

            return View(employee);
        }

        //// GET: Employees/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employee employee = mb.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employee);
        //}

        //// POST: Employees/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Employee employee = mb.Employees.Find(id);
        //    mb.Employees.Remove(employee);
        //    mb.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDA([Bind(Include = "DisciplinaryActionID,EmployeeID,FirstName,LastName,Date,Reason,Explanation,EditByEmployeeID,EditTimeStamp")] DisciplinaryAction DiscAct)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                
                DiscAct.EditByEmployeeID = user.EmployeeID;
                DiscAct.EditTimeStamp = System.DateTime.Now;
                mb.DisciplinaryActions.Add(DiscAct);
                mb.SaveChanges();

                return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
            }
            return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
        }

        //// GET: Positions/Edit/5
        //[Authorize(Roles = "Admin, HR")]
        //public ActionResult EditDA(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Entities.Position position = mb.Positions.Find(id);
        //    if (position == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(position);
        //}

        //// POST: Positions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[Authorize(Roles = "Admin, HR")]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditDA([Bind(Include = "PositionID,PositionName,PositionDescription")] Entities.Position position)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        mb.Positions.Attach(position);
        //        mb.Entry(position).Property("PositionName").IsModified = true;
        //        mb.Entry(position).Property("PositionDescription").IsModified = true;
        //        mb.SaveChanges();

        //        var myPosition = db.Positions.Find(position.PositionID);
        //        myPosition.PositionName = position.PositionName;
        //        myPosition.PositionDescription = position.PositionDescription;
        //        db.SaveChanges();

        //        return RedirectToAction("Index", "Admin");
        //    }
        //    return View(position);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
