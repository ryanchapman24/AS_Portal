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

namespace AS_TestProject.Controllers
{
    public class PayrollController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        // GET: Payroll
        public ActionResult Index(int id)
        {
            var now = System.DateTime.Now;
            var payPeriod = mb.PayPeriods.First(p => p.StartDate <= now && System.Data.Entity.DbFunctions.AddDays(p.EndDate, 1) > now);
            var payPeriodId = payPeriod.PayPeriodID;

            var agent = mb.Employees.Find(id);

            var agentDailyHours = mb.AgentDailyHours.Where(a => a.EmployeeID == id && a.PayPeriodID == payPeriodId).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();
            ViewBag.AgentName = agent.FirstName + ' ' + agent.LastName;
            ViewBag.PayPeriodStart = payPeriod.StartDate;
            ViewBag.PayPeriodEnd = payPeriod.EndDate;

            ViewBag.Date = now.ToShortDateString();
            ViewBag.EmployeeID = id;
            ViewBag.PayPeriodID = payPeriodId;
            ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName");
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason");

            return View(agentDailyHours);
        }

        // POST: Payroll/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentDailyHoursID,EmployeeID,DomainMasterID,LoginTimeStamp,LogoutTimeStamp,LoginDuration,AgentTimeAdjustmentReasonID,PayPeriodID")] AgentDailyHour agentDailyHour, int empId, short ppId)
        {
            if (ModelState.IsValid)
            {
                agentDailyHour.PayPeriodID = ppId;
                agentDailyHour.EmployeeID = empId;
                mb.AgentDailyHours.Add(agentDailyHour);
                mb.SaveChanges();

                // STORED PROCEDURES
                mb.uspOverTimeHoursCalculation();
                mb.SaveChanges();

                return RedirectToAction("Index", new { id = empId });
            }

            ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName", agentDailyHour.DomainMasterID);
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason");

            return RedirectToAction("Index", new { id = empId });            
        }

        // GET: Payroll/Edit/5
        public ActionResult Edit(int? id)
        {
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentDailyHoursID,EmployeeID,DomainMasterID,LoginTimeStamp,LogoutTimeStamp,LoginDuration,AgentTimeAdjustmentReasonID,PayPeriodID")] AgentDailyHour agentDailyHour)
        {
            if (ModelState.IsValid)
            {
                mb.AgentDailyHours.Attach(agentDailyHour);
                mb.Entry(agentDailyHour).Property("LoginTimeStamp").IsModified = true;
                mb.Entry(agentDailyHour).Property("LogoutTimeStamp").IsModified = true;
                mb.Entry(agentDailyHour).Property("LoginDuration").IsModified = true;
                mb.Entry(agentDailyHour).Property("AgentTimeAdjustmentReasonID").IsModified = true;
                mb.SaveChanges();

                // STORED PROCEDURES
                mb.uspOverTimeHoursCalculation();
                mb.SaveChanges();

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
