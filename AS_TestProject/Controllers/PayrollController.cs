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
            var payPeriod = mb.PayPeriods.First(p => p.StartDate <= now && p.EndDate >= now);
            var payPeriodId = payPeriod.PayPeriodID;

            var agent = mb.Employees.Find(id);

            var agentDailyHours = mb.AgentDailyHours.Where(a => a.EmployeeID == id && a.PayPeriodID == payPeriodId).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();
            ViewBag.AgentName = agent.FirstName + ' ' + agent.LastName;
            ViewBag.PayPeriodStart = payPeriod.StartDate.Date;
            ViewBag.PayPeriodEnd = payPeriod.EndDate.Date;
                     
            return View(agentDailyHours);
        }

        // GET: Payroll/Create
        public ActionResult Create()
        {
            ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName");
            ViewBag.EmployeeID = new SelectList(mb.Employees, "EmployeeID", "FirstName");
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason");
            ViewBag.PayPeriodID = new SelectList(mb.PayPeriods, "PayPeriodID", "PayPeriodID");
            return View();
        }

        // POST: Payroll/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentDailyHoursID,EmployeeID,DomainMasterID,LoginTimeStamp,LogoutTimeStamp,LoginDuration,AgentTimeAdjustmentReasonID,PayPeriodID")] AgentDailyHour agentDailyHour)
        {
            if (ModelState.IsValid)
            {
                mb.AgentDailyHours.Add(agentDailyHour);
                mb.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName", agentDailyHour.DomainMasterID);
            ViewBag.EmployeeID = new SelectList(mb.Employees, "EmployeeID", "FirstName", agentDailyHour.EmployeeID);
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason", agentDailyHour.AgentTimeAdjustmentReasonID);
            ViewBag.PayPeriodID = new SelectList(mb.PayPeriods, "PayPeriodID", "PayPeriodID", agentDailyHour.PayPeriodID);
            return View(agentDailyHour);
        }

        // GET: Payroll/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentDailyHour agentDailyHour = mb.AgentDailyHours.Find(id);
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
                mb.Entry(agentDailyHour).State = EntityState.Modified;
                mb.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DomainMasterID = new SelectList(mb.DomainMasters, "DomainMasterID", "DomainName", agentDailyHour.DomainMasterID);
            ViewBag.EmployeeID = new SelectList(mb.Employees, "EmployeeID", "FirstName", agentDailyHour.EmployeeID);
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason", agentDailyHour.AgentTimeAdjustmentReasonID);
            ViewBag.PayPeriodID = new SelectList(mb.PayPeriods, "PayPeriodID", "PayPeriodID", agentDailyHour.PayPeriodID);
            return View(agentDailyHour);
        }
    }
}
