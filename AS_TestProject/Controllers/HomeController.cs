using AS_TestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_TestProject.Entities;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Net;

namespace AS_TestProject.Controllers
{
    [Authorize]
    public class HomeController : UserNames
    {
        public ActionResult Index()
        {
            var mb = new ReportEntities();
            var user = db.Users.Find(User.Identity.GetUserId());
            var todayYear = System.DateTime.Now.Year;
            var todayMonth = System.DateTime.Now.Month;
            var todayDay = System.DateTime.Now.Day;

            ViewBag.birthdayList = mb.Employees.Where(e => e.BirthDate.Month == todayMonth && e.BirthDate.Day == todayDay && e.IsActive == true).ToList();
            ViewBag.MonthlyTasks = user.Tasks.Where(t => t.Complete == true && t.Completed.Value.Month == todayMonth && t.Completed.Value.Year == todayYear).ToList();
            ViewBag.DailyMessages = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Out == true && m.Sent.Year == todayYear && m.Sent.Month == todayMonth && m.Sent.Day == todayDay).ToList();

            var siteId = user.SiteID;
            //var ourEmployees = new List<Employee>();
            //var theirEmployees = new List<Employee>();

            var ourEmployees = mb.Employees.Where(e => e.SiteID == siteId && e.IsActive == true).ToList();
            var theirEmployees = mb.Employees.Where(e => e.SiteID != siteId && e.IsActive == true).ToList();
            //foreach (var employee in mb.Employees)
            //{
            //    if (employee.SiteID == siteId)
            //    {
            //        ourEmployees.Add(employee);
            //    }
            //    if (employee.SiteID != siteId)
            //    {
            //        theirEmployees.Add(employee);
            //    }
            //}

            var ourAgentIds = new List<EmployeeFive9Agent>();
            var theirAgentIds = new List<EmployeeFive9Agent>();
            foreach (var employee in ourEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        ourAgentIds.Add(agent);
                    }
                }

            }
            foreach (var employee in theirEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        theirAgentIds.Add(agent);
                    }
                }
            }

            var ourTransfers = new List<CallLogRealTime>();
            var theirTransfers = new List<CallLogRealTime>();
            var calls = mb.CallLogRealTimes.Where(c => c.AgentID != "" && c.Disposition.Contains("Transfer") && !(c.Disposition.Contains("Not Int")) && c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth && c.RecordDate.Day == todayDay);

            foreach (var call in calls)
            {
                foreach (var agent in ourAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        ourTransfers.Add(call);
                    }
                }
                foreach (var agent in theirAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        theirTransfers.Add(call);
                    }
                }
            }
            ViewBag.OurTransfersToday = ourTransfers.Count();
            ViewBag.TheirTransfersToday = theirTransfers.Count();
            ViewBag.AnomalyTransfersToday = ourTransfers.Count() + theirTransfers.Count();
            var userSite = mb.Sites.First(s => s.SiteID == user.SiteID);
            var otherSite = mb.Sites.First(s => s.SiteID != user.SiteID);
            ViewBag.UserSite = userSite.SiteName;
            ViewBag.OtherSite = otherSite.SiteName;

            //var empId = user.EmployeeID;
            //var agents = mb.EmployeeFive9Agent.Where(a => a.EmployeeID == empId).ToList();
            //var totalCalls = new List<CallLogRealTime>();
            //foreach(var log in mb.CallLogRealTimes)
            //{
            //    foreach(var agent in agents)
            //    {
            //        if(log.AgentID == agent.AgentID)
            //        {
            //            totalCalls.Add(log);
            //        }
            //    }
            //}

            //var allContacts = totalCalls.Where(c => c.Disposition != "" && c.Disposition != "" && c.Disposition != "" && c.Disposition != "" && c.Disposition != "");
            //var allTransfers = totalCalls.Where(c => c.Disposition.Contains("Transfer") && !(c.Disposition.Contains("Not Int")));
            //decimal allTimeCallCount = allContacts.Count();
            //decimal allTimeTransferCount = allTransfers.Count();
            //decimal yearlyCallCount = allContacts.Where(c => c.RecordDate.Year == todayYear).Count();
            //decimal yearlyTransferCount = allTransfers.Where(c => c.RecordDate.Year == todayYear).Count();
            //decimal monthlyCallCount = allContacts.Where(c => c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth).Count();
            //decimal monthlyTransferCount = allTransfers.Where(c => c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth).Count();
            //decimal dailyCallCount = allContacts.Where(c => c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth && c.RecordDate.Day == todayDay).Count();
            //decimal dailyTransferCount = allTransfers.Where(c => c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth && c.RecordDate.Day == todayDay).Count();
            //var allTimeContactToTransfer = "";
            //var yearlyContactToTransfer = "";
            //var monthlyContactToTransfer = "";
            //var dailyContactToTransfer = "";

            //if (allTimeCallCount == 0)
            //{
            //   allTimeContactToTransfer = "N/A";
            //}
            //else
            //{
            //    var ratio = allTimeTransferCount/allTimeCallCount;
            //    var percentage = ratio * 100;
            //    allTimeContactToTransfer = percentage.ToString("0.00") + "%";
            //}
            //if (yearlyCallCount == 0)
            //{
            //    yearlyContactToTransfer = "N/A";
            //}
            //else
            //{
            //    var ratio = yearlyTransferCount/yearlyCallCount;
            //    var percentage = ratio * 100;
            //    yearlyContactToTransfer = percentage.ToString("0.00") + "%";
            //}
            //if (monthlyCallCount == 0)
            //{
            //    monthlyContactToTransfer = "N/A";
            //}
            //else
            //{
            //    var ratio = monthlyTransferCount/monthlyCallCount;
            //    var percentage = ratio * 100;
            //    monthlyContactToTransfer = percentage.ToString("0.00") + "%";
            //}
            //if (dailyCallCount == 0)
            //{
            //    dailyContactToTransfer = "N/A";
            //}
            //else
            //{
            //    var ratio = dailyTransferCount/dailyCallCount;
            //    var percentage = ratio * 100;
            //    dailyContactToTransfer = percentage.ToString("0.00") + "%";
            //}

            //ViewBag.AllTimeCTT = allTimeContactToTransfer;
            //ViewBag.YearlyCTT = yearlyContactToTransfer;
            //ViewBag.MonthlyCTT = monthlyContactToTransfer;
            //ViewBag.DailyCTT = dailyContactToTransfer;

            return View();
        }

        public ActionResult Tools()
        {
            return View();
        }

        public ActionResult Directory()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var mb = new ReportEntities();
            if (user.EmployeeID == 1000250 || user.EmployeeID == 1001811 || user.EmployeeID == 1000070 || user.EmployeeID == 1000082 || user.EmployeeID == 1000098 || user.EmployeeID == 1000135 || user.EmployeeID == 1000229 || user.EmployeeID == 1000184 || user.EmployeeID == 1000303 || user.EmployeeID == 1000160)
            {
                ViewBag.ActiveEmps = mb.Employees.Where(d => d.IsActive == true).OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToList();
                ViewBag.InactiveEmps = mb.Employees.Where(d => d.IsActive == false).OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToList();
            }
            else
            {
                ViewBag.ActiveEmps = mb.Employees.Where(d => d.SiteID == user.SiteID && d.IsActive == true).OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToList();
                ViewBag.InactiveEmps = mb.Employees.Where(d => d.SiteID == user.SiteID && d.IsActive == false).OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToList();

            }

            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName");
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName");

            return View();
        }

        public ActionResult ProfilePage(string id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var mb = new ReportEntities();
            var now = System.DateTime.Now;
            var yesterDay = now.AddDays(-1).Day;
            var yesterMonth = now.AddDays(-1).Month;
            var yesterYear = now.AddDays(-1).Year;
            var payPeriod = mb.PayPeriods.First(p => p.StartDate <= now && System.Data.Entity.DbFunctions.AddDays(p.EndDate, 1) > now);


            if (!string.IsNullOrWhiteSpace(id))
            {
                var userCheck = db.Users.Find(id);
                if (userCheck != null)
                {
                    var userA = userCheck.Id;


                    var mCFRyesterday = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Day == yesterDay && c.DateOfFeedback.Month == yesterMonth && c.DateOfFeedback.Year == yesterYear).Count();
                    var mCFRtoday = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Day == now.Day && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
                    var mCFRmonth = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
                    var mCFRyear = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Year == now.Year).Count();

                    var iCFRyesterday = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Day == yesterDay && c.DateOfFeedback.Month == yesterMonth && c.DateOfFeedback.Year == yesterYear).Count();
                    var iCFRtoday = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Day == now.Day && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
                    var iCFRmonth = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
                    var iCFRyear = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Year == now.Year).Count();

                    var pCFRyesterday = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Day == yesterDay && c.DateOfFeedback.Month == yesterMonth && c.DateOfFeedback.Year == yesterYear).Count();
                    var pCFRtoday = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Day == now.Day && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
                    var pCFRmonth = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
                    var pCFRyear = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == userCheck.EmployeeID && c.DateOfFeedback.Year == now.Year).Count();

                    ViewBag.CFRyesterday = mCFRyesterday + iCFRyesterday + pCFRyesterday;
                    ViewBag.CFRtoday = mCFRtoday + iCFRtoday + pCFRtoday;
                    ViewBag.CFRmonth = mCFRmonth + iCFRmonth + pCFRmonth;
                    ViewBag.CFRyear = mCFRyear + iCFRyear + pCFRyear;

                    ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "Id", "Priority");
                    ViewBag.Urgent = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
                    ViewBag.High = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
                    ViewBag.Medium = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
                    ViewBag.Low = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();
                    ViewBag.TaskCounter = userCheck.TaskTally;

                    var entriesThisPayPeriod = mb.AgentDailyHours.Where(h => h.PayPeriodID == payPeriod.PayPeriodID && h.EmployeeID == userCheck.EmployeeID).OrderByDescending(h => h.LoginTimeStamp).ToList();                
                    if (entriesThisPayPeriod.Count > 0)
                    {
                        decimal firstWeekHours = 0;
                        decimal secondWeekHours = 0;
                        var firstWeekEntries = entriesThisPayPeriod.Where(h => h.LoginTimeStamp >= payPeriod.StartDate && h.LoginTimeStamp < payPeriod.MidDate).OrderByDescending(h => h.LoginTimeStamp).ToList();
                        var secondWeekEntries = entriesThisPayPeriod.Where(h => h.LoginTimeStamp >= payPeriod.MidDate && h.LoginTimeStamp < payPeriod.EndDate).OrderByDescending(h => h.LoginTimeStamp).ToList();
                        if (firstWeekEntries.Count > 0)
                        {
                            firstWeekHours = firstWeekEntries.First().RegularHours.Value + firstWeekEntries.First().OverTimeHours.Value;
                        }
                        if (secondWeekEntries.Count > 0)
                        {
                            secondWeekHours = secondWeekEntries.First().RegularHours.Value + secondWeekEntries.First().OverTimeHours.Value;
                        }
                        ViewBag.Hours = firstWeekHours + secondWeekHours;
                    }
                    else
                    {
                        ViewBag.Hours = 0;
                    }
                    ViewBag.EmployeeID = userCheck.EmployeeID;
                    

                    // Computing Time With Company
                    var userFromEmpTable = mb.Employees.First(e => e.EmployeeID == userCheck.EmployeeID);
                    var hireDate = userFromEmpTable.HireDate;
                    var today = System.DateTime.Now;
                    DateTime date1 = hireDate;
                    DateTime date2 = today;
                    int oldMonth = date2.Month;
                    while (oldMonth == date2.Month)
                    {
                        date1 = date1.AddDays(-1);
                        date2 = date2.AddDays(-1);
                    }
                    int years = 0, months = 0, days = 0;
                    // getting number of years
                    while (date2.CompareTo(date1) >= 0)
                    {
                        years++;
                        date2 = date2.AddYears(-1);
                    }
                    date2 = date2.AddYears(1);
                    years--;
                    // getting number of months and days
                    oldMonth = date2.Month;
                    while (date2.CompareTo(date1) >= 0)
                    {
                        days++;
                        date2 = date2.AddDays(-1);
                        if ((date2.CompareTo(date1) >= 0) && (oldMonth != date2.Month))
                        {
                            months++;
                            days = 0;
                            oldMonth = date2.Month;
                        }
                    }
                    date2 = date2.AddDays(1);
                    days--;
                    // Formatting string possibilities
                    var y = "";
                    if (years == 1)
                    {
                        y = " year, ";
                    }
                    else if (years > 1)
                    {
                        y = " years, ";
                    }
                    var m = "";
                    if (months == 1)
                    {
                        m = " month, ";
                    }
                    else if (months > 1)
                    {
                        m = " months, ";
                    }
                    var d = "";
                    if (days == 1)
                    {
                        d = " day";
                    }
                    else if (days > 1)
                    {
                        d = " days";
                    }
                    if (years == 0 && months > 0 && days > 0)
                    {
                        ViewBag.TimeWithCompany = months.ToString() + m + days.ToString() + d;
                    }
                    else if (years > 0 && months == 0 && days > 0)
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y + days.ToString() + d;
                    }
                    else if (years > 0 && months > 0 && days == 0)
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y + months.ToString() + m.Substring(0, m.Length - 2);
                    }
                    else if (years == 0 && months == 0 && days > 0)
                    {
                        ViewBag.TimeWithCompany = days.ToString() + d;
                    }
                    else if (years == 0 && months > 0 && days == 0)
                    {
                        ViewBag.TimeWithCompany = months.ToString() + m.Substring(0, m.Length - 2);
                    }
                    else if (years > 0 && months == 0 && days == 0)
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y.Substring(0, y.Length - 2);
                    }
                    else
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y + months.ToString() + m + days.ToString() + d;
                    }               
                    return View(userCheck);
                }
            }

            var mCFRyesterdayME = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == user.EmployeeID && c.DateOfFeedback.Day == yesterDay && c.DateOfFeedback.Month == yesterMonth && c.DateOfFeedback.Year == yesterYear).Count();
            var mCFRtodayME = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == user.EmployeeID && c.DateOfFeedback.Day == now.Day && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
            var mCFRmonthME = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == user.EmployeeID && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
            var mCFRyearME = mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == user.EmployeeID && c.DateOfFeedback.Year == now.Year).Count();

            var iCFRyesterdayME = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == user.EmployeeID && c.DateOfFeedback.Day == yesterDay && c.DateOfFeedback.Month == yesterMonth && c.DateOfFeedback.Year == yesterYear).Count();
            var iCFRtodayME = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == user.EmployeeID && c.DateOfFeedback.Day == now.Day && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
            var iCFRmonthME = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == user.EmployeeID && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
            var iCFRyearME = mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == user.EmployeeID && c.DateOfFeedback.Year == now.Year).Count();

            var pCFRyesterdayME = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == user.EmployeeID && c.DateOfFeedback.Day == yesterDay && c.DateOfFeedback.Month == yesterMonth && c.DateOfFeedback.Year == yesterYear).Count();
            var pCFRtodayME = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == user.EmployeeID && c.DateOfFeedback.Day == now.Day && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
            var pCFRmonthME = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == user.EmployeeID && c.DateOfFeedback.Month == now.Month && c.DateOfFeedback.Year == now.Year).Count();
            var pCFRyearME = mb.CFRInsurances.Where(c => c.Employee.EmployeeID == user.EmployeeID && c.DateOfFeedback.Year == now.Year).Count();

            ViewBag.CFRyesterday = mCFRyesterdayME + iCFRyesterdayME + pCFRyesterdayME;
            ViewBag.CFRtoday = mCFRtodayME + iCFRtodayME + pCFRtodayME;
            ViewBag.CFRmonth = mCFRmonthME + iCFRmonthME + pCFRmonthME;
            ViewBag.CFRyear = mCFRyearME + iCFRyearME + pCFRyearME;

            ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "Id", "Priority");
            ViewBag.Urgent = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
            ViewBag.High = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
            ViewBag.Medium = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
            ViewBag.Low = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();
            ViewBag.TaskCounter = user.TaskTally;

            var selfEntriesThisPayPeriod = mb.AgentDailyHours.Where(h => h.PayPeriodID == payPeriod.PayPeriodID && h.EmployeeID == user.EmployeeID).OrderByDescending(h => h.LoginTimeStamp).ToList();
            if (selfEntriesThisPayPeriod.Count > 0)
            {
                decimal selfFirstWeekHours = 0;
                decimal selfSecondWeekHours = 0;
                var selfFirstWeekEntries = selfEntriesThisPayPeriod.Where(h => h.LoginTimeStamp >= payPeriod.StartDate && h.LoginTimeStamp < payPeriod.MidDate).OrderByDescending(h => h.LoginTimeStamp).ToList();
                var selfSecondWeekEntries = selfEntriesThisPayPeriod.Where(h => h.LoginTimeStamp >= payPeriod.MidDate && h.LoginTimeStamp < payPeriod.EndDate).OrderByDescending(h => h.LoginTimeStamp).ToList();
                if (selfFirstWeekEntries.Count > 0)
                {
                    selfFirstWeekHours = selfFirstWeekEntries.First().RegularHours.Value + selfFirstWeekEntries.First().OverTimeHours.Value;
                }
                if (selfSecondWeekEntries.Count > 0)
                {
                    selfSecondWeekHours = selfSecondWeekEntries.First().RegularHours.Value + selfSecondWeekEntries.First().OverTimeHours.Value;
                }
                ViewBag.Hours = selfFirstWeekHours + selfSecondWeekHours;
            }
            else
            {
                ViewBag.Hours = 0;
            }
            ViewBag.EmployeeID = user.EmployeeID;

            // Computing MY Time With Company
            var selfUserFromEmpTable = mb.Employees.First(e => e.EmployeeID == user.EmployeeID);
            var selfHireDate = selfUserFromEmpTable.HireDate;
            var selfToday = System.DateTime.Now;
            DateTime selfDate1 = selfHireDate;
            DateTime selfDate2 = selfToday;
            int selfOldMonth = selfDate2.Month;
            while (selfOldMonth == selfDate2.Month)
            {
                selfDate1 = selfDate1.AddDays(-1);
                selfDate2 = selfDate2.AddDays(-1);
            }
            int selfYears = 0, selfMonths = 0, selfDays = 0;
            // getting number of years
            while (selfDate2.CompareTo(selfDate1) >= 0)
            {
                selfYears++;
                selfDate2 = selfDate2.AddYears(-1);
            }
            selfDate2 = selfDate2.AddYears(1);
            selfYears--;
            // getting number of months and days
            selfOldMonth = selfDate2.Month;
            while (selfDate2.CompareTo(selfDate1) >= 0)
            {
                selfDays++;
                selfDate2 = selfDate2.AddDays(-1);
                if ((selfDate2.CompareTo(selfDate1) >= 0) && (selfOldMonth != selfDate2.Month))
                {
                    selfMonths++;
                    selfDays = 0;
                    selfOldMonth = selfDate2.Month;
                }
            }
            selfDate2 = selfDate2.AddDays(1);
            selfDays--;

            // Formatting string possibilities
            var selfY = "";
            if (selfYears == 1)
            {
                selfY = " year, ";
            }
            else if (selfYears > 1)
            {
                selfY = " years, ";
            }
            var selfM = "";
            if (selfMonths == 1)
            {
                selfM = " month, ";
            }
            else if (selfMonths > 1)
            {
                selfM = " months, ";
            }
            var selfD = "";
            if (selfDays == 1)
            {
                selfD = " day";
            }
            else if (selfDays > 1)
            {
                selfD = " days";
            }
            if (selfYears == 0 && selfMonths > 0 && selfDays > 0)
            {
                ViewBag.TimeWithCompany = selfMonths.ToString() + selfM + selfDays.ToString() + selfD;
            }
            else if (selfYears > 0 && selfMonths == 0 && selfDays > 0)
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY + selfDays.ToString() + selfD;
            }
            else if (selfYears > 0 && selfMonths > 0 && selfDays == 0)
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY + selfMonths.ToString() + selfM.Substring(0, selfM.Length - 2);
            }
            else if (selfYears == 0 && selfMonths == 0 && selfDays > 0)
            {
                ViewBag.TimeWithCompany = selfDays.ToString() + selfD;
            }
            else if (selfYears == 0 && selfMonths > 0 && selfDays == 0)
            {
                ViewBag.TimeWithCompany = selfMonths.ToString() + selfM.Substring(0, selfM.Length - 2);
            }
            else if (selfYears > 0 && selfMonths == 0 && selfDays == 0)
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY.Substring(0, selfY.Length - 2);
            }
            else
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY + selfMonths.ToString() + selfM + selfDays.ToString() + selfD;
            }
            return View(user);
        }

        // POST: Tasks/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AddTask(WorkTask task)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.IsValid)
            {
                task.AuthorId = User.Identity.GetUserId();
                task.Created = System.DateTime.Now;
                task.Complete = false;
                db.Tasks.Add(task);
                db.SaveChanges();               
            }
            ViewBag.TicketTypeId = new SelectList(db.TaskPriorities, "Id", "Priority", task.TaskPriorityId);
            ViewBag.Urgent = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
            ViewBag.High = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
            ViewBag.Medium = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
            ViewBag.Low = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();
            return RedirectToAction("ProfilePage");
        }

        //POST: Tasks/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult SubmitTask(List<int> Change)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            if (Change != null)
            {
                if (Request.Form["complete"] != null)
                {
                    int count = Change.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var task = db.Tasks.Find(Change[i]);
                        task.Complete = true;
                        task.Completed = System.DateTime.Now;
                        user.TaskTally = user.TaskTally + 1;
                        db.SaveChanges();
                    }
                }
                else if (Request.Form["abort"] != null)
                {
                    int count = Change.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var task = db.Tasks.Find(Change[i]);
                        db.Tasks.Remove(task);
                        db.SaveChanges();
                    }
                }              
            }

            return RedirectToAction("ProfilePage");
        }

        public ActionResult Forms()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var now = System.DateTime.Now;
            var nextMonth = now.AddMonths(1).Month;
            var nextYear = now.AddYears(1).Year;
            ViewBag.Date = now.ToShortDateString();
            ViewBag.UpcomingEvents = db.Events.Where(e => (e.AuthorId == user.Id || e.Universal == true) && ((e.StartDate.Day >= now.Day && e.StartDate.Month == now.Month && e.StartDate.Year == now.Year) || ((e.StartDate.Month == nextMonth && e.StartDate.Year == now.Year)) || (e.StartDate.Month == nextMonth && e.StartDate.Year == nextYear))).OrderBy(e => e.StartDate).ToList();
            ViewBag.MyEvents = db.Events.Where(e => e.AuthorId == user.Id || e.Universal == true).OrderByDescending(e => e.StartDate).ToList();

            return View();
        }

        public ActionResult GetEvents()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var allEvents = db.Events.Where(e => e.AuthorId == user.Id || e.Universal == true).ToList();
            var eventlist = new List<object>();
            foreach (var x in allEvents)
            {
                eventlist.Add(new { title = x.Title, start = x.StartDate, end = x.EndDate, allDay = x.AllDay });
            }
            return Content(JsonConvert.SerializeObject(eventlist), "application/json");
        }

        public ActionResult CreateEvent(Event calEvent)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            calEvent.AuthorId = user.Id;
            db.Events.Add(calEvent);
            db.SaveChanges();

            return RedirectToAction("Calendar");
        }
      
        public ActionResult Gallery()
        {
            var model = new PhotoJudgment();
            var unpubPhotos = db.GalleryPhotos.Where(p => p.Published == false).OrderByDescending(p => p.Id).ToList();

            foreach (var photo in unpubPhotos)
            {
                model.PhotoList.Add(photo);
            }
            db.SaveChanges();

            ViewBag.Unpublished = db.GalleryPhotos.Where(p => p.Published == false).OrderByDescending(p => p.Id).ToList();
            ViewBag.GalleryPhotos = db.GalleryPhotos.Where(p => p.Published == true).OrderByDescending(p => p.Id).ToList();
           
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhotoToGallery(GalleryPhoto galleryPhoto, HttpPostedFileBase image)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ImageUploadValidator.IsWebFriendlyImage(image))
            {
                //var fileName = Path.GetFileName(image.FileName);
                //image.SaveAs(Path.Combine(Server.MapPath("~/GalleryPhotos/"), fileName));
                //galleryPhoto.File = "/GalleryPhotos/" + fileName;

                //Counter
                var num = 0;
                //Gets Filename without the extension
                var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                var gPic = Path.Combine("/GalleryPhotos/", fileName + Path.GetExtension(image.FileName));
                //Checks if pPic matches any of the current attachments, 
                //if so it will loop and add a (number) to the end of the filename
                while (db.GalleryPhotos.Any(p => p.File == gPic))
                {
                    //Sets "filename" back to the default value
                    fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    //Add's parentheses after the name with a number ex. filename(4)
                    fileName = string.Format(fileName + "(" + ++num + ")");
                    //Makes sure pPic gets updated with the new filename so it could check
                    gPic = Path.Combine("/GalleryPhotos/", fileName + Path.GetExtension(image.FileName));
                }
                image.SaveAs(Path.Combine(Server.MapPath("~/GalleryPhotos/"), fileName + Path.GetExtension(image.FileName)));
                galleryPhoto.File = gPic;
                db.SaveChanges();
            }

            galleryPhoto.Created = System.DateTime.Now;
            galleryPhoto.AuthorId = user.Id;
            galleryPhoto.Published = false;
            db.GalleryPhotos.Add(galleryPhoto);
            db.SaveChanges();

            return RedirectToAction("Gallery", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Marketing")]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(List<int> Published, List<int> Delete)
        {
            if (Published != null)
            {
                int count = Published.Count();
                for (int i = 0; i < count; i++)
                {
                    var photo = db.GalleryPhotos.Find(Published[i]);
                    photo.Published = true;
                    db.SaveChanges();
                }
            }

            if (Delete != null)
            {
                int total = Delete.Count();
                for (int i = 0; i < total; i++)
                {
                    var photo = db.GalleryPhotos.Find(Delete[i]);
                    db.GalleryPhotos.Remove(photo);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Gallery", "Home");
        }
    }
}