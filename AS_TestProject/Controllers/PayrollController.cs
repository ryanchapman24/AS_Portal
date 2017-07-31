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
    [Authorize]
    public class PayrollController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        public class Domain
        {
            public byte Id { get; set; }
            public string FileMaskPlusName { get; set; }
        }

        // GET: Payroll
        [Authorize]
        public ActionResult Index(int id/*, List<AgentDailyHour> conflicted*/)
        {
            var conflicted = TempData["list"] as List<AgentDailyHour>;
            var user = db.Users.Find(User.Identity.GetUserId());

            if (id != user.EmployeeID && !(User.IsInRole("Payroll")))
            {
                return RedirectToAction("Directory", "Home");
            }

            var now = System.DateTime.Now;
            var payPeriod = mb.PayPeriods.First(p => p.StartDate <= now && System.Data.Entity.DbFunctions.AddDays(p.EndDate, 1) > now);
            var payPeriodId = payPeriod.PayPeriodID;
            var prevPayPeriodId = payPeriodId - 1;
            var prevPayPeriod = mb.PayPeriods.First(p => p.PayPeriodID == prevPayPeriodId);

            var agent = mb.Employees.Find(id);

            var agentDailyHours = mb.AgentDailyHours.Where(a => a.EmployeeID == id && a.PayPeriodID == payPeriodId).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();
            ViewBag.PrevAgentDailyHours = mb.AgentDailyHours.Where(a => a.EmployeeID == id && a.PayPeriodID == prevPayPeriodId).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();

            ViewBag.AgentName = agent.FirstName + ' ' + agent.LastName;
            ViewBag.PayPeriodStart = payPeriod.StartDate.ToShortDateString();
            ViewBag.PayPeriodEnd = payPeriod.EndDate.ToShortDateString();
            ViewBag.PrevPayPeriodStart = prevPayPeriod.StartDate.ToShortDateString();
            ViewBag.PrevPayPeriodEnd = prevPayPeriod.EndDate.ToShortDateString();
            ViewBag.Today = System.DateTime.Now.ToShortDateString();

            ViewBag.Date = now.ToShortDateString();
            ViewBag.PrevDate = prevPayPeriod.MidDate.ToShortDateString();
            ViewBag.EmployeeID = id;
            ViewBag.PayPeriodID = payPeriodId;
            ViewBag.PrevPayPeriodID = prevPayPeriodId;

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }

            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName");
            //ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName");
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason");
            if (conflicted != null)
            {
                ViewBag.ConflictBool = 1;
                ViewBag.Conflicted = conflicted;
            }
            else
            {
                ViewBag.ConflictBool = 0;
            }

            return View(agentDailyHours);
        }

        // POST: Payroll/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Payroll")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentDailyHoursID,EmployeeID,DomainMasterID,LoginTimeStamp,LogoutTimeStamp,LoginDuration,AgentTimeAdjustmentReasonID,PayPeriodID,EditByEmployeeID,EditTimeStamp")] AgentDailyHour agentDailyHour, int empId, short ppId)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                var now = System.DateTime.Now;
                var payPeriod = mb.PayPeriods.First(p => p.StartDate <= now && System.Data.Entity.DbFunctions.AddDays(p.EndDate, 1) > now);
                var payPeriodId = payPeriod.PayPeriodID;
                var prevPayPeriodId = payPeriodId - 1;
                var prevPayPeriod = mb.PayPeriods.First(p => p.PayPeriodID == prevPayPeriodId);

                if (agentDailyHour.LoginTimeStamp != agentDailyHour.LogoutTimeStamp)
                {
                    var agent = mb.Employees.Find(empId);
                    var agentDailyHours = mb.AgentDailyHours.Where(a => a.EmployeeID == empId && (a.PayPeriodID == payPeriodId || a.PayPeriodID == prevPayPeriodId)).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();
                    if (agentDailyHours.Any(a => (a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp && a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp) || (a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp && a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp)))
                    {
                        var conflictedHours = agentDailyHours.Where(a => (a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp && a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp) || (a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp && a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp)).ToList();
                        TempData["list"] = conflictedHours.ToList();
                        return RedirectToAction("Index", new { id = empId });
                    }
                    else
                    {
                        agentDailyHour.PayPeriodID = ppId;
                        agentDailyHour.EmployeeID = empId;
                        agentDailyHour.EditByEmployeeID = user.EmployeeID;
                        agentDailyHour.EditTimeStamp = System.DateTime.Now;
                        agentDailyHour.IsNew = true;
                        mb.AgentDailyHours.Add(agentDailyHour);
                        mb.SaveChanges();

                        // STORED PROCEDURES
                        mb.uspHoursCalculation(empId, ppId);
                        mb.SaveChanges();
                    }
                }

                return RedirectToAction("Index", new { id = empId });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }

            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName");
            //ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName", agentDailyHour.DomainMasterID);
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason");

            return RedirectToAction("Index", new { id = empId });
        }

        // GET: Payroll/Edit/5
        [Authorize(Roles = "Admin, Payroll")]
        public ActionResult Edit(int? id)
        {
            var conflicted = TempData["list"] as List<AgentDailyHour>;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentDailyHour agentDailyHour = mb.AgentDailyHours.Find(id);
            var date = agentDailyHour.LoginTimeStamp.ToString("d");
            ViewBag.Date = date + " ";
            if (agentDailyHour == null)
            {
                return HttpNotFound();
            }
            if (conflicted != null)
            {
                ViewBag.ConflictBool = 1;
                ViewBag.Conflicted = conflicted;
            }
            else
            {
                ViewBag.ConflictBool = 0;
            }
            ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName", agentDailyHour.DomainMasterID);
            ViewBag.EmployeeID = new SelectList(mb.Employees, "EmployeeID", "FirstName", agentDailyHour.EmployeeID);
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason", agentDailyHour.AgentTimeAdjustmentReasonID);
            ViewBag.PayPeriodID = new SelectList(mb.PayPeriods, "PayPeriodID", "PayPeriodID", agentDailyHour.PayPeriodID);
            return View(agentDailyHour);
        }

        // POST: Payroll/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Payroll")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentDailyHoursID,EmployeeID,DomainMasterID,LoginTimeStamp,LogoutTimeStamp,LoginDuration,AgentTimeAdjustmentReasonID,PayPeriodID")] AgentDailyHour agentDailyHour)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var empId = agentDailyHour.EmployeeID;
                var now = System.DateTime.Now;
                var payPeriod = mb.PayPeriods.First(p => p.StartDate <= now && System.Data.Entity.DbFunctions.AddDays(p.EndDate, 1) > now);
                var payPeriodId = payPeriod.PayPeriodID;
                var prevPayPeriodId = payPeriodId - 1;
                var prevPayPeriod = mb.PayPeriods.First(p => p.PayPeriodID == prevPayPeriodId);

                var agent = mb.Employees.Find(empId);
                var agentDailyHours = mb.AgentDailyHours.AsNoTracking().Where(a => a.AgentDailyHoursID != agentDailyHour.AgentDailyHoursID).Where(a => a.EmployeeID == empId && (a.PayPeriodID == payPeriodId || a.PayPeriodID == prevPayPeriodId)).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();
                
                if (agentDailyHour.LoginTimeStamp == agentDailyHour.LogoutTimeStamp)
                {
                    var ppId = agentDailyHour.PayPeriodID;

                    mb.AgentDailyHours.Attach(agentDailyHour);
                    mb.AgentDailyHours.Remove(agentDailyHour);
                    mb.SaveChanges();

                    // STORED PROCEDURES
                    mb.uspHoursCalculation(empId, ppId);
                    mb.SaveChanges();
                }
                else
                {
                    if (agentDailyHours.Any(a => (a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp && a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp) || (a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp && a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp)))
                    {
                        var conflictedHours = agentDailyHours.Where(a => (a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp && a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp) || (a.LoginTimeStamp < agentDailyHour.LogoutTimeStamp && a.LogoutTimeStamp > agentDailyHour.LoginTimeStamp)).ToList();
                        TempData["list"] = conflictedHours.ToList();
                        return RedirectToAction("Edit", new { id = agentDailyHour.AgentDailyHoursID });
                    }

                    mb.AgentDailyHours.Attach(agentDailyHour);
                    agentDailyHour.EditByEmployeeID = user.EmployeeID;
                    agentDailyHour.EditTimeStamp = System.DateTime.Now;
                    mb.Entry(agentDailyHour).Property("LoginTimeStamp").IsModified = true;
                    mb.Entry(agentDailyHour).Property("LogoutTimeStamp").IsModified = true;
                    mb.Entry(agentDailyHour).Property("LoginDuration").IsModified = true;
                    mb.Entry(agentDailyHour).Property("AgentTimeAdjustmentReasonID").IsModified = true;
                    mb.SaveChanges();

                    // STORED PROCEDURES
                    mb.uspHoursCalculation(agentDailyHour.EmployeeID, agentDailyHour.PayPeriodID);
                    mb.SaveChanges();
                }

                return RedirectToAction("Index", new { id = agentDailyHour.EmployeeID });
            }
            ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName", agentDailyHour.DomainMasterID);
            ViewBag.EmployeeID = new SelectList(mb.Employees, "EmployeeID", "FirstName", agentDailyHour.EmployeeID);
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason", agentDailyHour.AgentTimeAdjustmentReasonID);
            ViewBag.PayPeriodID = new SelectList(mb.PayPeriods, "PayPeriodID", "PayPeriodID", agentDailyHour.PayPeriodID);
            return View(agentDailyHour);
        }
    }
}

