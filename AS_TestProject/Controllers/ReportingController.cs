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
    [Authorize(Roles = "Admin, IT")]
    public class ReportingController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        public class Domain
        {
            public byte Id { get; set; }
            public string FileMaskPlusName { get; set; }
            public int GreensTransfersToday { get; set; }
            public int WichTransfersToday { get; set; }
            public int TotalTransfersToday { get; set; }
            public int GreensTransfersMonth { get; set; }
            public int WichTransfersMonth { get; set; }
            public int TotalTransfersMonth { get; set; }
            public int GreensTransfersYear { get; set; }
            public int WichTransfersYear { get; set; }
            public int TotalTransfersYear { get; set; }
        }

        public class Emp
        {
            public int EmployeeID { get; set; }
            public string FullName { get; set; }
            public int SiteID { get; set; }
            public int TotalTransfersToday { get; set; }
            public int TotalTransfersMonth { get; set; }
            public int TotalTransfersYear { get; set; }
        }

        // GET: Reporting
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reporting/TransfersByDomain
        public ActionResult TransfersByDomain()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var todayYear = System.DateTime.Now.Year;
            var todayMonth = System.DateTime.Now.Month;
            var todayDay = System.DateTime.Now.Day;

            var GreensEmployees = mb.Employees.Where(e => e.SiteID == 1 && e.IsActive == true).ToList();
            var WichEmployees = mb.Employees.Where(e => e.SiteID == 2 && e.IsActive == true).ToList();

            var GreensAgentIds = new List<EmployeeFive9Agent>();
            var WichAgentIds = new List<EmployeeFive9Agent>();
            foreach (var employee in GreensEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        GreensAgentIds.Add(agent);
                    }
                }
            }
            foreach (var employee in WichEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        WichAgentIds.Add(agent);
                    }
                }
            }

            var GreensTransfersToday = new List<CallLogRealTime>();
            var WichTransfersToday = new List<CallLogRealTime>();
            var GreensTransfersMonth = new List<CallLogRealTime>();
            var WichTransfersMonth = new List<CallLogRealTime>();
            var GreensTransfersYear = new List<CallLogRealTime>();
            var WichTransfersYear = new List<CallLogRealTime>();
            var callsToday = mb.CallLogRealTimes.Where(c => c.AgentID != "" && c.LeadID != "" && (c.Disposition.Contains("Transfer") || c.Disposition.Contains("LO Not Available") || c.Disposition.Contains("LA Not Available")) && !(c.Disposition.Contains("Not Int")) && c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth && c.RecordDate.Day == todayDay);
            var callsMonth = mb.CallLogRealTimes.Where(c => c.AgentID != "" && c.LeadID != "" && (c.Disposition.Contains("Transfer") || c.Disposition.Contains("LO Not Available") || c.Disposition.Contains("LA Not Available")) && !(c.Disposition.Contains("Not Int")) && c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth);
            var callsYear = mb.CallLogRealTimes.Where(c => c.AgentID != "" && c.LeadID != "" && (c.Disposition.Contains("Transfer") || c.Disposition.Contains("LO Not Available") || c.Disposition.Contains("LA Not Available")) && !(c.Disposition.Contains("Not Int")) && c.RecordDate.Year == todayYear);

            foreach (var call in callsToday)
            {
                //while (!(calls.Any(c => c.CampaignName == call.CampaignName && c.LeadID == call.LeadID)))
                //{
                foreach (var agent in GreensAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        GreensTransfersToday.Add(call);
                    }
                }
                foreach (var agent in WichAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        WichTransfersToday.Add(call);
                    }
                }
                //}
            }

            foreach (var call in callsMonth)
            {
                //while (!(calls.Any(c => c.CampaignName == call.CampaignName && c.LeadID == call.LeadID)))
                //{
                foreach (var agent in GreensAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        GreensTransfersMonth.Add(call);
                    }
                }
                foreach (var agent in WichAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        WichTransfersMonth.Add(call);
                    }
                }
                //}
            }

            foreach (var call in callsYear)
            {
                //while (!(calls.Any(c => c.CampaignName == call.CampaignName && c.LeadID == call.LeadID)))
                //{
                foreach (var agent in GreensAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        GreensTransfersYear.Add(call);
                    }
                }
                foreach (var agent in WichAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        WichTransfersYear.Add(call);
                    }
                }
                //}
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true && d.DomainMasterID != 21).OrderBy(d => d.FileMask))
            {
                var item = new Domain();
                item.Id = domain.DomainMasterID;
                item.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;
                item.GreensTransfersToday = GreensTransfersToday.Where(t => t.DomainMasterID == domain.DomainMasterID).Count();
                item.WichTransfersToday = WichTransfersToday.Where(t => t.DomainMasterID == domain.DomainMasterID).Count();
                item.TotalTransfersToday = item.GreensTransfersToday + item.WichTransfersToday;
                item.GreensTransfersMonth = GreensTransfersMonth.Where(t => t.DomainMasterID == domain.DomainMasterID).Count();
                item.WichTransfersMonth = WichTransfersMonth.Where(t => t.DomainMasterID == domain.DomainMasterID).Count();
                item.TotalTransfersMonth = item.GreensTransfersMonth + item.WichTransfersMonth;
                item.GreensTransfersYear = GreensTransfersYear.Where(t => t.DomainMasterID == domain.DomainMasterID).Count();
                item.WichTransfersYear = WichTransfersYear.Where(t => t.DomainMasterID == domain.DomainMasterID).Count();
                item.TotalTransfersYear = item.GreensTransfersYear + item.WichTransfersYear;

                domains.Add(item);
            }
            ViewBag.DomainsToday = domains.OrderByDescending(d => d.TotalTransfersToday);
            ViewBag.DomainsMonth = domains.OrderByDescending(d => d.TotalTransfersMonth);
            ViewBag.DomainsYear = domains.OrderByDescending(d => d.TotalTransfersYear);
            return View();
        }

        // GET: Reporting/TransfersByEmployee
        public ActionResult TransfersByEmployee()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var todayYear = System.DateTime.Now.Year;
            var todayMonth = System.DateTime.Now.Month;
            var todayDay = System.DateTime.Now.Day;

            var GreensEmployees = mb.Employees.Where(e => e.SiteID == 1 && e.IsActive == true).ToList();
            var WichEmployees = mb.Employees.Where(e => e.SiteID == 2 && e.IsActive == true).ToList();

            var GreensAgentIds = new List<EmployeeFive9Agent>();
            var WichAgentIds = new List<EmployeeFive9Agent>();
            foreach (var employee in GreensEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        GreensAgentIds.Add(agent);
                    }
                }
            }
            foreach (var employee in WichEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        WichAgentIds.Add(agent);
                    }
                }
            }

            var GreensTransfersToday = new List<CallLogRealTime>();
            var WichTransfersToday = new List<CallLogRealTime>();
            var GreensTransfersMonth = new List<CallLogRealTime>();
            var WichTransfersMonth = new List<CallLogRealTime>();
            var GreensTransfersYear = new List<CallLogRealTime>();
            var WichTransfersYear = new List<CallLogRealTime>();
            var callsToday = mb.CallLogRealTimes.Where(c => c.AgentID != "" && c.LeadID != "" && (c.Disposition.Contains("Transfer") || c.Disposition.Contains("LO Not Available") || c.Disposition.Contains("LA Not Available")) && !(c.Disposition.Contains("Not Int")) && c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth && c.RecordDate.Day == todayDay);
            var callsMonth = mb.CallLogRealTimes.Where(c => c.AgentID != "" && c.LeadID != "" && (c.Disposition.Contains("Transfer") || c.Disposition.Contains("LO Not Available") || c.Disposition.Contains("LA Not Available")) && !(c.Disposition.Contains("Not Int")) && c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth);
            var callsYear = mb.CallLogRealTimes.Where(c => c.AgentID != "" && c.LeadID != "" && (c.Disposition.Contains("Transfer") || c.Disposition.Contains("LO Not Available") || c.Disposition.Contains("LA Not Available")) && !(c.Disposition.Contains("Not Int")) && c.RecordDate.Year == todayYear);

            foreach (var call in callsToday)
            {
                //while (!(calls.Any(c => c.CampaignName == call.CampaignName && c.LeadID == call.LeadID)))
                //{
                foreach (var agent in GreensAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        GreensTransfersToday.Add(call);
                    }
                }
                foreach (var agent in WichAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        WichTransfersToday.Add(call);
                    }
                }
                //}
            }

            foreach (var call in callsMonth)
            {
                //while (!(calls.Any(c => c.CampaignName == call.CampaignName && c.LeadID == call.LeadID)))
                //{
                foreach (var agent in GreensAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        GreensTransfersMonth.Add(call);
                    }
                }
                foreach (var agent in WichAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        WichTransfersMonth.Add(call);
                    }
                }
                //}
            }

            foreach (var call in callsYear)
            {
                //while (!(calls.Any(c => c.CampaignName == call.CampaignName && c.LeadID == call.LeadID)))
                //{
                foreach (var agent in GreensAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        GreensTransfersYear.Add(call);
                    }
                }
                foreach (var agent in WichAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        WichTransfersYear.Add(call);
                    }
                }
                //}
            }

            var employees = new List<Emp>();
            foreach (var employee in mb.Employees.Where(e => e.IsActive == true && (e.PositionID == 3 || e.PositionID == 26)).OrderBy(e => e.FirstName))
            {
                var empLogins = mb.EmployeeFive9Agent.Where(e => e.EmployeeID == employee.EmployeeID).ToList();
                var item = new Emp();
                item.EmployeeID = employee.EmployeeID;
                item.FullName = employee.FirstName + " " + employee.LastName;
                item.SiteID = employee.SiteID;
                if (employee.SiteID == 1)
                {
                    item.TotalTransfersToday = GreensTransfersToday.Where(t => empLogins.Any(e => t.AgentID == e.AgentID)).Count();
                    item.TotalTransfersMonth = GreensTransfersMonth.Where(t => empLogins.Any(e => t.AgentID == e.AgentID)).Count();
                    item.TotalTransfersYear = GreensTransfersYear.Where(t => empLogins.Any(e => t.AgentID == e.AgentID)).Count();
                }
                else
                {
                    item.TotalTransfersToday = WichTransfersToday.Where(t => empLogins.Any(e => e.AgentID == t.AgentID)).Count();
                    item.TotalTransfersMonth = WichTransfersMonth.Where(t => empLogins.Any(e => e.AgentID == t.AgentID)).Count();
                    item.TotalTransfersYear = WichTransfersYear.Where(t => empLogins.Any(e => e.AgentID == t.AgentID)).Count();
                }

                employees.Add(item);
            }
            ViewBag.EmployeesToday = employees.Where(e => e.TotalTransfersToday > 0).OrderByDescending(e => e.TotalTransfersToday);
            ViewBag.WichitaToday = employees.Where(e => e.SiteID == 2 && e.TotalTransfersToday > 0).OrderByDescending(e => e.TotalTransfersToday);
            ViewBag.GreensboroToday = employees.Where(e => e.SiteID == 1 && e.TotalTransfersToday > 0).OrderByDescending(e => e.TotalTransfersToday);
            ViewBag.EmployeesMonth = employees.Where(e => e.TotalTransfersMonth > 0).OrderByDescending(e => e.TotalTransfersMonth);
            ViewBag.WichitaMonth = employees.Where(e => e.SiteID == 2 && e.TotalTransfersMonth > 0).OrderByDescending(e => e.TotalTransfersMonth);
            ViewBag.GreensboroMonth = employees.Where(e => e.SiteID == 1 && e.TotalTransfersMonth > 0).OrderByDescending(e => e.TotalTransfersMonth);
            ViewBag.EmployeesYear = employees.Where(e => e.TotalTransfersYear > 0).OrderByDescending(e => e.TotalTransfersYear);
            ViewBag.WichitaYear = employees.Where(e => e.SiteID == 2 && e.TotalTransfersYear > 0).OrderByDescending(e => e.TotalTransfersYear);
            ViewBag.GreensboroYear = employees.Where(e => e.SiteID == 1 && e.TotalTransfersYear > 0).OrderByDescending(e => e.TotalTransfersYear);
            return View();
        }
    }
}