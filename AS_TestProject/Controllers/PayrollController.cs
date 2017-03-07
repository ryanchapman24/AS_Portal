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

        public class Skill
        {
            public int Id { get; set; }
            public string FileMaskPlusName { get; set; }
        }

        // GET: Payroll
        [Authorize]
        public ActionResult Index(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (id != user.EmployeeID  && !(User.IsInRole("Payroll")))
            {
                return RedirectToAction("Directory", "Home");
            }

            var now = System.DateTime.Now;;
            var payPeriod = mb.PayPeriods.First(p => p.StartDate <= now && System.Data.Entity.DbFunctions.AddDays(p.EndDate, 1) > now);
            var payPeriodId = payPeriod.PayPeriodID;
            var prevPayPeriodId = payPeriodId - 1;
            var prevPayPeriod = mb.PayPeriods.First(p => p.PayPeriodID == prevPayPeriodId);

            var agent = mb.Employees.Find(id);

            var agentDailyHours = mb.AgentDailyHours.Where(a => a.EmployeeID == id && a.PayPeriodID == payPeriodId).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();
            ViewBag.PrevAgentDailyHours = mb.AgentDailyHours.Where(a => a.EmployeeID == id && a.PayPeriodID == prevPayPeriodId).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();

            ViewBag.AgentName = agent.FirstName + ' ' + agent.LastName;
            ViewBag.PayPeriodStart = payPeriod.StartDate;
            ViewBag.PayPeriodEnd = payPeriod.EndDate;
            ViewBag.PrevPayPeriodStart = prevPayPeriod.StartDate;
            ViewBag.PrevPayPeriodEnd = prevPayPeriod.EndDate;

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
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason");

            //Prep for Payroll Changes
            ViewBag.Domains = new MultiSelectList(domains, "Id", "FileMaskPlusName");
            var allAgentHours = mb.AgentDailyHours.Where(a => a.EmployeeID == id && (a.PayPeriodID == payPeriodId || a.PayPeriodID == prevPayPeriodId)).Include(a => a.DomainMaster).Include(a => a.Employee).Include(a => a.AgentTimeAdjustmentReason).Include(a => a.PayPeriod).OrderByDescending(a => a.LoginTimeStamp).ToList();
            var agentHoursToDomainMasters = mb.AgentDailyHoursToDomainMasters.ToList();
            var relatedSkills = agentHoursToDomainMasters.Where(a => allAgentHours.Any(h => h.AgentDailyHoursID == a.AgentDailyHoursID)).ToList();
            var skills = new List<Skill>();
            foreach (var skill in relatedSkills)
            {
                var newSkill = new Skill();
                newSkill.Id = skill.AgentDailyHoursID;
                var fileMaskPlusName = mb.DomainMasters.FirstOrDefault(d => d.DomainMasterID == skill.DomainMasterID).FileMask + " - " + mb.DomainMasters.FirstOrDefault(d => d.DomainMasterID == skill.DomainMasterID).DomainName;
                newSkill.FileMaskPlusName = fileMaskPlusName;

                skills.Add(newSkill);
            }
            ViewBag.Skills = skills;
            //End Prep

            return View(agentDailyHours);
        }

        // POST: Payroll/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Payroll")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentDailyHoursID,EmployeeID,DomainMasterID,LoginTimeStamp,LogoutTimeStamp,LoginDuration,AgentTimeAdjustmentReasonID,PayPeriodID,EditByEmployeeID,EditTimeStamp")] AgentDailyHour agentDailyHour, int empId, short ppId, List<byte> Domains)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                if (agentDailyHour.LoginTimeStamp != agentDailyHour.LogoutTimeStamp)
                {
                    agentDailyHour.DomainMasterID = 21;
                    agentDailyHour.PayPeriodID = ppId;
                    agentDailyHour.EmployeeID = empId;
                    agentDailyHour.EditByEmployeeID = user.EmployeeID;
                    agentDailyHour.EditTimeStamp = System.DateTime.Now;
                    mb.AgentDailyHours.Add(agentDailyHour);
                    mb.SaveChanges();

                    if (Domains != null)
                    {
                        int count = Domains.Count();
                        for (byte i = 0; i < count; i++)
                        {
                            var d = new AgentDailyHoursToDomainMaster();
                            d.AgentDailyHoursID = agentDailyHour.AgentDailyHoursID;
                            d.DomainMasterID = Domains[i];
                            d.IsDeleted = false;
                            mb.AgentDailyHoursToDomainMasters.Add(d);
                            mb.SaveChanges();
                        }
                        mb.SaveChanges();
                    }

                    // STORED PROCEDURES
                    mb.uspHoursCalculation(empId, ppId);
                    mb.SaveChanges();
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
            ViewBag.AgentTimeAdjustmentReasonID = new SelectList(mb.AgentTimeAdjustmentReasons, "AgentTimeAdjustmentReasonID", "Reason");

            //Prep for Payroll Changes
            ViewBag.Domains = new MultiSelectList(domains, "Id", "FileMaskPlusName");
            //End Prep

            return RedirectToAction("Index", new { id = empId });            
        }

        // GET: Payroll/Edit/5
        [Authorize(Roles = "Admin, Payroll")]
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

            //Prep for Payroll Changes
            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            var domains2 = mb.DomainMasters.Where(d => d.DomainMasterID == agentDailyHour.DomainMasterID).ToArray();
            ViewBag.Domains = new MultiSelectList(domains, "Id", "FileMaskPlusName", domains2);
            //End Prep

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

                if (agentDailyHour.LoginTimeStamp == agentDailyHour.LogoutTimeStamp)
                {
                    mb.AgentDailyHours.Attach(agentDailyHour);
                    mb.AgentDailyHours.Remove(agentDailyHour);
                    mb.SaveChanges();
                }
                else
                {
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

            //Prep for Payroll Changes
            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            var domains2 = mb.DomainMasters.Where(d => d.DomainMasterID == agentDailyHour.DomainMasterID).ToArray();
            ViewBag.Domains = new MultiSelectList(domains, "Id", "FileMaskPlusName", domains2);
            //End Prep

            return View(agentDailyHour);
        }
    }
}
