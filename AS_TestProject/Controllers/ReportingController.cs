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

        public class ActiveDomain
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public int GCalls { get; set; }
            public int WCalls { get; set; }
        }

        // GET: Reporting
        [Authorize(Roles = "Admin, IT, HR, Quality, Operations")]
        public ActionResult Index()
        {
            var todayYear = System.DateTime.Now.Year;
            var todayMonth = System.DateTime.Now.Month;
            var todayDay = System.DateTime.Now.Day;

            var GEmployees = mb.Employees.Where(e => e.SiteID == 1 && e.IsActive == true).ToList();
            var WEmployees = mb.Employees.Where(e => e.SiteID == 2 && e.IsActive == true).ToList();

            var GAgentIds = new List<EmployeeFive9Agent>();
            var WAgentIds = new List<EmployeeFive9Agent>();
            foreach (var employee in GEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        GAgentIds.Add(agent);
                    }
                }
            }
            foreach (var employee in WEmployees)
            {
                foreach (var agent in mb.EmployeeFive9Agent)
                {
                    if (agent.EmployeeID == employee.EmployeeID)
                    {
                        WAgentIds.Add(agent);
                    }
                }
            }

            var GCalls = new List<CallLogRealTime>();
            var WCalls = new List<CallLogRealTime>();
            var calls = mb.CallLogRealTimes.Where(c => c.RecordDate.Year == todayYear && c.RecordDate.Month == todayMonth && c.RecordDate.Day == todayDay);

            foreach (var call in calls)
            {
                foreach (var agent in GAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        GCalls.Add(call);
                    }
                }
                foreach (var agent in WAgentIds)
                {
                    if (call.AgentID == agent.AgentID)
                    {
                        WCalls.Add(call);
                    }
                }
            }

            var actDoms = new List<ActiveDomain>();
            int processed = 0;
            foreach (var actDom in mb.DomainMasters.Where(d => d.IsActive == true && d.DomainMasterID != 21 && d.DomainMasterID != 28).OrderBy(d => d.DomainName))
            {
                var item = new ActiveDomain();
                item.Number = processed++;
                item.Name = actDom.DomainName;
                item.GCalls = GCalls.Where(c => c.DomainMasterID == actDom.DomainMasterID).Count();
                item.WCalls = WCalls.Where(c => c.DomainMasterID == actDom.DomainMasterID).Count();
                actDoms.Add(item);
            }

            ViewBag.ActiveDomains = actDoms;
            return View();
        }

        // GET: Reporting/TransfersByDomain
        [Authorize(Roles = "Admin, IT, Operations")]
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
        [Authorize(Roles = "Admin, IT, Operations")]
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

        // GET: HR
        [Authorize(Roles = "Admin, HR, Operations")]
        public ActionResult HireTerm()
        {
            var thisMonth = DateTime.Now.Month;
            var lastMonth = DateTime.Now.AddMonths(-1).Month;
            var twoMonthsAgo = DateTime.Now.AddMonths(-2).Month;
            var threeMonthsAgo = DateTime.Now.AddMonths(-3).Month;
            var fourMonthsAgo = DateTime.Now.AddMonths(-4).Month;
            var fiveMonthsAgo = DateTime.Now.AddMonths(-5).Month;

            var thisMonthYear = DateTime.Now.Year;
            var lastMonthYear = DateTime.Now.AddMonths(-1).Year;
            var twoMonthsAgoYear = DateTime.Now.AddMonths(-2).Year;
            var threeMonthsAgoYear = DateTime.Now.AddMonths(-3).Year;
            var fourMonthsAgoYear = DateTime.Now.AddMonths(-4).Year;
            var fiveMonthsAgoYear = DateTime.Now.AddMonths(-5).Year;

            ViewBag.ThisMonth = DateTime.Now.ToString("MMMM");
            ViewBag.LastMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");
            ViewBag.TwoMonthsAgo = DateTime.Now.AddMonths(-2).ToString("MMMM");
            ViewBag.ThreeMonthsAgo = DateTime.Now.AddMonths(-3).ToString("MMMM");
            ViewBag.FourMonthsAgo = DateTime.Now.AddMonths(-4).ToString("MMMM");
            ViewBag.FiveMonthsAgo = DateTime.Now.AddMonths(-5).ToString("MMMM");

            ViewBag.OverallHires = mb.Employees.Where(e => ((e.HireDate.Month == thisMonth && e.HireDate.Year == thisMonthYear) || (e.HireDate.Month == lastMonth && e.HireDate.Year == lastMonthYear) || (e.HireDate.Month == twoMonthsAgo && e.HireDate.Year == twoMonthsAgoYear) || (e.HireDate.Month == threeMonthsAgo && e.HireDate.Year == threeMonthsAgoYear) || (e.HireDate.Month == fourMonthsAgo && e.HireDate.Year == fourMonthsAgoYear) || (e.HireDate.Month == fiveMonthsAgo && e.HireDate.Year == fiveMonthsAgoYear)) && (e.SiteID == 1 || e.SiteID == 2)).OrderByDescending(e => e.HireDate).ToList();
            ViewBag.OverallTerms = mb.Employees.Where(e => ((e.TerminationDate.Month == thisMonth && e.TerminationDate.Year == thisMonthYear) || (e.TerminationDate.Month == lastMonth && e.TerminationDate.Year == lastMonthYear) || (e.TerminationDate.Month == twoMonthsAgo && e.TerminationDate.Year == twoMonthsAgoYear) || (e.TerminationDate.Month == threeMonthsAgo && e.TerminationDate.Year == threeMonthsAgoYear) || (e.TerminationDate.Month == fourMonthsAgo && e.TerminationDate.Year == fourMonthsAgoYear) || (e.TerminationDate.Month == fiveMonthsAgo && e.TerminationDate.Year == fiveMonthsAgoYear)) && (e.SiteID == 1 || e.SiteID == 2)).OrderByDescending(e => e.TerminationDate).ToList();
            ViewBag.WichitaHires = mb.Employees.Where(e => ((e.HireDate.Month == thisMonth && e.HireDate.Year == thisMonthYear) || (e.HireDate.Month == lastMonth && e.HireDate.Year == lastMonthYear) || (e.HireDate.Month == twoMonthsAgo && e.HireDate.Year == twoMonthsAgoYear) || (e.HireDate.Month == threeMonthsAgo && e.HireDate.Year == threeMonthsAgoYear) || (e.HireDate.Month == fourMonthsAgo && e.HireDate.Year == fourMonthsAgoYear) || (e.HireDate.Month == fiveMonthsAgo && e.HireDate.Year == fiveMonthsAgoYear)) && e.SiteID == 2).OrderByDescending(e => e.HireDate).ToList();
            ViewBag.WichitaTerms = mb.Employees.Where(e => ((e.TerminationDate.Month == thisMonth && e.TerminationDate.Year == thisMonthYear) || (e.TerminationDate.Month == lastMonth && e.TerminationDate.Year == lastMonthYear) || (e.TerminationDate.Month == twoMonthsAgo && e.TerminationDate.Year == twoMonthsAgoYear) || (e.TerminationDate.Month == threeMonthsAgo && e.TerminationDate.Year == threeMonthsAgoYear) || (e.TerminationDate.Month == fourMonthsAgo && e.TerminationDate.Year == fourMonthsAgoYear) || (e.TerminationDate.Month == fiveMonthsAgo && e.TerminationDate.Year == fiveMonthsAgoYear)) && e.SiteID == 2).OrderByDescending(e => e.TerminationDate).ToList();
            ViewBag.GreensboroHires = mb.Employees.Where(e => ((e.HireDate.Month == thisMonth && e.HireDate.Year == thisMonthYear) || (e.HireDate.Month == lastMonth && e.HireDate.Year == lastMonthYear) || (e.HireDate.Month == twoMonthsAgo && e.HireDate.Year == twoMonthsAgoYear) || (e.HireDate.Month == threeMonthsAgo && e.HireDate.Year == threeMonthsAgoYear) || (e.HireDate.Month == fourMonthsAgo && e.HireDate.Year == fourMonthsAgoYear) || (e.HireDate.Month == fiveMonthsAgo && e.HireDate.Year == fiveMonthsAgoYear)) && e.SiteID == 1).OrderByDescending(e => e.HireDate).ToList();
            ViewBag.GreensboroTerms = mb.Employees.Where(e => ((e.TerminationDate.Month == thisMonth && e.TerminationDate.Year == thisMonthYear) || (e.TerminationDate.Month == lastMonth && e.TerminationDate.Year == lastMonthYear) || (e.TerminationDate.Month == twoMonthsAgo && e.TerminationDate.Year == twoMonthsAgoYear) || (e.TerminationDate.Month == threeMonthsAgo && e.TerminationDate.Year == threeMonthsAgoYear) || (e.TerminationDate.Month == fourMonthsAgo && e.TerminationDate.Year == fourMonthsAgoYear) || (e.TerminationDate.Month == fiveMonthsAgo && e.TerminationDate.Year == fiveMonthsAgoYear)) && e.SiteID == 1).OrderByDescending(e => e.TerminationDate).ToList();

            ViewBag.ThisMonthOvrH = mb.Employees.Where(e => e.HireDate.Month == thisMonth && e.HireDate.Year == thisMonthYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.LastMonthOvrH = mb.Employees.Where(e => e.HireDate.Month == lastMonth && e.HireDate.Year == lastMonthYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.TwoMonthsAgoOvrH = mb.Employees.Where(e => e.HireDate.Month == twoMonthsAgo && e.HireDate.Year == twoMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.ThreeMonthsAgoOvrH = mb.Employees.Where(e => e.HireDate.Month == threeMonthsAgo && e.HireDate.Year == threeMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.FourMonthsAgoOvrH = mb.Employees.Where(e => e.HireDate.Month == fourMonthsAgo && e.HireDate.Year == fourMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.FiveMonthsAgoOvrH = mb.Employees.Where(e => e.HireDate.Month == fiveMonthsAgo && e.HireDate.Year == fiveMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.ThisMonthOvrT = mb.Employees.Where(e => e.TerminationDate.Month == thisMonth && e.TerminationDate.Year == thisMonthYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.LastMonthOvrT = mb.Employees.Where(e => e.TerminationDate.Month == lastMonth && e.TerminationDate.Year == lastMonthYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.TwoMonthsAgoOvrT = mb.Employees.Where(e => e.TerminationDate.Month == twoMonthsAgo && e.TerminationDate.Year == twoMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.ThreeMonthsAgoOvrT = mb.Employees.Where(e => e.TerminationDate.Month == threeMonthsAgo && e.TerminationDate.Year == threeMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.FourMonthsAgoOvrT = mb.Employees.Where(e => e.TerminationDate.Month == fourMonthsAgo && e.TerminationDate.Year == fourMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.FiveMonthsAgoOvrT = mb.Employees.Where(e => e.TerminationDate.Month == fiveMonthsAgo && e.TerminationDate.Year == fiveMonthsAgoYear && (e.SiteID == 1 || e.SiteID == 2)).Count();

            ViewBag.ThisMonthWH = mb.Employees.Where(e => e.HireDate.Month == thisMonth && e.HireDate.Year == thisMonthYear && e.SiteID == 2).Count();
            ViewBag.LastMonthWH = mb.Employees.Where(e => e.HireDate.Month == lastMonth && e.HireDate.Year == lastMonthYear && e.SiteID == 2).Count();
            ViewBag.TwoMonthsAgoWH = mb.Employees.Where(e => e.HireDate.Month == twoMonthsAgo && e.HireDate.Year == twoMonthsAgoYear && e.SiteID == 2).Count();
            ViewBag.ThreeMonthsAgoWH = mb.Employees.Where(e => e.HireDate.Month == threeMonthsAgo && e.HireDate.Year == threeMonthsAgoYear && e.SiteID == 2).Count();
            ViewBag.FourMonthsAgoWH = mb.Employees.Where(e => e.HireDate.Month == fourMonthsAgo && e.HireDate.Year == fourMonthsAgoYear && e.SiteID == 2).Count();
            ViewBag.FiveMonthsAgoWH = mb.Employees.Where(e => e.HireDate.Month == fiveMonthsAgo && e.HireDate.Year == fiveMonthsAgoYear && e.SiteID == 2).Count();
            ViewBag.ThisMonthWT = mb.Employees.Where(e => e.TerminationDate.Month == thisMonth && e.TerminationDate.Year == thisMonthYear && e.SiteID == 2).Count();
            ViewBag.LastMonthWT = mb.Employees.Where(e => e.TerminationDate.Month == lastMonth && e.TerminationDate.Year == lastMonthYear && e.SiteID == 2).Count();
            ViewBag.TwoMonthsAgoWT = mb.Employees.Where(e => e.TerminationDate.Month == twoMonthsAgo && e.TerminationDate.Year == twoMonthsAgoYear && e.SiteID == 2).Count();
            ViewBag.ThreeMonthsAgoWT = mb.Employees.Where(e => e.TerminationDate.Month == threeMonthsAgo && e.TerminationDate.Year == threeMonthsAgoYear && e.SiteID == 2).Count();
            ViewBag.FourMonthsAgoWT = mb.Employees.Where(e => e.TerminationDate.Month == fourMonthsAgo && e.TerminationDate.Year == fourMonthsAgoYear && e.SiteID == 2).Count();
            ViewBag.FiveMonthsAgoWT = mb.Employees.Where(e => e.TerminationDate.Month == fiveMonthsAgo && e.TerminationDate.Year == fiveMonthsAgoYear && e.SiteID == 2).Count();

            ViewBag.ThisMonthGH = mb.Employees.Where(e => e.HireDate.Month == thisMonth && e.HireDate.Year == thisMonthYear && e.SiteID == 1).Count();
            ViewBag.LastMonthGH = mb.Employees.Where(e => e.HireDate.Month == lastMonth && e.HireDate.Year == lastMonthYear && e.SiteID == 1).Count();
            ViewBag.TwoMonthsAgoGH = mb.Employees.Where(e => e.HireDate.Month == twoMonthsAgo && e.HireDate.Year == twoMonthsAgoYear && e.SiteID == 1).Count();
            ViewBag.ThreeMonthsAgoGH = mb.Employees.Where(e => e.HireDate.Month == threeMonthsAgo && e.HireDate.Year == threeMonthsAgoYear && e.SiteID == 1).Count();
            ViewBag.FourMonthsAgoGH = mb.Employees.Where(e => e.HireDate.Month == fourMonthsAgo && e.HireDate.Year == fourMonthsAgoYear && e.SiteID == 1).Count();
            ViewBag.FiveMonthsAgoGH = mb.Employees.Where(e => e.HireDate.Month == fiveMonthsAgo && e.HireDate.Year == fiveMonthsAgoYear && e.SiteID == 1).Count();
            ViewBag.ThisMonthGT = mb.Employees.Where(e => e.TerminationDate.Month == thisMonth && e.TerminationDate.Year == thisMonthYear && e.SiteID == 1).Count();
            ViewBag.LastMonthGT = mb.Employees.Where(e => e.TerminationDate.Month == lastMonth && e.TerminationDate.Year == lastMonthYear && e.SiteID == 1).Count();
            ViewBag.TwoMonthsAgoGT = mb.Employees.Where(e => e.TerminationDate.Month == twoMonthsAgo && e.TerminationDate.Year == twoMonthsAgoYear && e.SiteID == 1).Count();
            ViewBag.ThreeMonthsAgoGT = mb.Employees.Where(e => e.TerminationDate.Month == threeMonthsAgo && e.TerminationDate.Year == threeMonthsAgoYear && e.SiteID == 1).Count();
            ViewBag.FourMonthsAgoGT = mb.Employees.Where(e => e.TerminationDate.Month == fourMonthsAgo && e.TerminationDate.Year == fourMonthsAgoYear && e.SiteID == 1).Count();
            ViewBag.FiveMonthsAgoGT = mb.Employees.Where(e => e.TerminationDate.Month == fiveMonthsAgo && e.TerminationDate.Year == fiveMonthsAgoYear && e.SiteID == 1).Count();

            ViewBag.ThisMonthOvrDiff = ViewBag.ThisMonthOvrH - ViewBag.ThisMonthOvrT;
            ViewBag.LastMonthOvrDiff = ViewBag.LastMonthOvrH - ViewBag.LastMonthOvrT;
            ViewBag.TwoMonthsAgoOvrDiff = ViewBag.TwoMonthsAgoOvrH - ViewBag.TwoMonthsAgoOvrT;
            ViewBag.ThreeMonthsAgoOvrDiff = ViewBag.ThreeMonthsAgoOvrH - ViewBag.ThreeMonthsAgoOvrT;
            ViewBag.FourMonthsAgoOvrDiff = ViewBag.FourMonthsAgoOvrH - ViewBag.FourMonthsAgoOvrT;
            ViewBag.FiveMonthsAgoOvrDiff = ViewBag.FiveMonthsAgoOvrH - ViewBag.FiveMonthsAgoOvrT;
            ViewBag.OverallDiff = ViewBag.ThisMonthOvrDiff + ViewBag.LastMonthOvrDiff + ViewBag.TwoMonthsAgoOvrDiff + ViewBag.ThreeMonthsAgoOvrDiff + ViewBag.FourMonthsAgoOvrDiff + ViewBag.FiveMonthsAgoOvrDiff;

            ViewBag.ThisMonthWDiff = ViewBag.ThisMonthWH - ViewBag.ThisMonthWT;
            ViewBag.LastMonthWDiff = ViewBag.LastMonthWH - ViewBag.LastMonthWT;
            ViewBag.TwoMonthsAgoWDiff = ViewBag.TwoMonthsAgoWH - ViewBag.TwoMonthsAgoWT;
            ViewBag.ThreeMonthsAgoWDiff = ViewBag.ThreeMonthsAgoWH - ViewBag.ThreeMonthsAgoWT;
            ViewBag.FourMonthsAgoWDiff = ViewBag.FourMonthsAgoWH - ViewBag.FourMonthsAgoWT;
            ViewBag.FiveMonthsAgoWDiff = ViewBag.FiveMonthsAgoWH - ViewBag.FiveMonthsAgoWT;
            ViewBag.WichitaDiff = ViewBag.ThisMonthWDiff + ViewBag.LastMonthWDiff + ViewBag.TwoMonthsAgoWDiff + ViewBag.ThreeMonthsAgoWDiff + ViewBag.FourMonthsAgoWDiff + ViewBag.FiveMonthsAgoWDiff;

            ViewBag.ThisMonthGDiff = ViewBag.ThisMonthGH - ViewBag.ThisMonthGT;
            ViewBag.LastMonthGDiff = ViewBag.LastMonthGH - ViewBag.LastMonthGT;
            ViewBag.TwoMonthsAgoGDiff = ViewBag.TwoMonthsAgoGH - ViewBag.TwoMonthsAgoGT;
            ViewBag.ThreeMonthsAgoGDiff = ViewBag.ThreeMonthsAgoGH - ViewBag.ThreeMonthsAgoGT;
            ViewBag.FourMonthsAgoGDiff = ViewBag.FourMonthsAgoGH - ViewBag.FourMonthsAgoGT;
            ViewBag.FiveMonthsAgoGDiff = ViewBag.FiveMonthsAgoGH - ViewBag.FiveMonthsAgoGT;
            ViewBag.GreensboroDiff = ViewBag.ThisMonthGDiff + ViewBag.LastMonthGDiff + ViewBag.TwoMonthsAgoGDiff + ViewBag.ThreeMonthsAgoGDiff + ViewBag.FourMonthsAgoGDiff + ViewBag.FiveMonthsAgoGDiff;

            ViewBag.OvrCurrentEmpCount = mb.Employees.Where(e => e.IsActive == true && (e.SiteID == 1 || e.SiteID == 2)).Count();
            ViewBag.OvrSixMonthsAgoEmpCount = ViewBag.OvrCurrentEmpCount - ViewBag.OverallDiff;
            double overallDiff = ViewBag.OverallDiff;
            double ovrSixMonthsAgoEmpCount = ViewBag.OvrSixMonthsAgoEmpCount;
            double ovrGrowthPercent = Math.Abs(overallDiff) / ovrSixMonthsAgoEmpCount;
            ViewBag.OvrGrowthPercent = ovrGrowthPercent.ToString("P2");

            ViewBag.WCurrentEmpCount = mb.Employees.Where(e => e.IsActive == true && e.SiteID == 2).Count();
            ViewBag.WSixMonthsAgoEmpCount = ViewBag.WCurrentEmpCount - ViewBag.WichitaDiff;
            double wichitaDiff = ViewBag.WichitaDiff;
            double wSixMonthsAgoEmpCount = ViewBag.wSixMonthsAgoEmpCount;
            double wGrowthPercent = Math.Abs(wichitaDiff) / wSixMonthsAgoEmpCount;
            ViewBag.WGrowthPercent = wGrowthPercent.ToString("P2");

            ViewBag.GCurrentEmpCount = mb.Employees.Where(e => e.IsActive == true && e.SiteID == 1).Count();
            ViewBag.GSixMonthsAgoEmpCount = ViewBag.GCurrentEmpCount - ViewBag.GreensboroDiff;
            double greensboroDiff = ViewBag.GreensboroDiff;
            double gSixMonthsAgoEmpCount = ViewBag.gSixMonthsAgoEmpCount;
            double gGrowthPercent = Math.Abs(greensboroDiff) / gSixMonthsAgoEmpCount;
            ViewBag.GGrowthPercent = gGrowthPercent.ToString("P2");

            return View();
        }

        // GET: HR
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Rehires()
        {
            var thisYr = System.DateTime.Now.Year;
            var lastYr = System.DateTime.Now.AddYears(-1).Year;
            ViewBag.ThisYear = thisYr;
            ViewBag.LastYear = lastYr;

            ViewBag.ThisYearsRehires = mb.Employees.Where(e => e.RehireDate.Value.Year == thisYr).OrderByDescending(e => e.RehireDate).ToList();
            ViewBag.ThisYearTotal = mb.Employees.Where(e => e.RehireDate.Value.Year == thisYr).Count();
            ViewBag.LastYearsRehires = mb.Employees.Where(e => e.RehireDate.Value.Year == lastYr).OrderByDescending(e => e.RehireDate).ToList();
            ViewBag.LastYearTotal = mb.Employees.Where(e => e.RehireDate.Value.Year == lastYr).Count();
            return View();
        }

        // GET: Quality/Index
        [Authorize(Roles = "Admin, Quality, Operations")]
        public ActionResult CFRStats()
        {
            decimal mtgCFR = mb.CFRMortgages.Count();
            decimal insCFR = mb.CFRInsurances.Count();
            decimal prCFR = mb.CFRPatientRecruitments.Count();

            ViewBag.mtgCFR = mtgCFR;
            ViewBag.insCFR = insCFR;
            ViewBag.prCFR = prCFR;

            ////////////////////// MORTGAGE //////////////////////////////////////////////////////////
            decimal mTE1yes = mb.CFRMortgages.Where(c => c.mTEQ1 == 1).Count();
            decimal mTE1no = mb.CFRMortgages.Where(c => c.mTEQ1 == 2).Count();
            decimal mTE1na = mb.CFRMortgages.Where(c => c.mTEQ1 == 3).Count();

            ViewBag.mTE1yes = Math.Round((mTE1yes * 10000) / mtgCFR) / 100;
            ViewBag.mTE1no = Math.Round((mTE1no * 10000) / mtgCFR) / 100;
            ViewBag.mTE1na = Math.Round((mTE1na * 10000) / mtgCFR) / 100;

            decimal mTE2yes = mb.CFRMortgages.Where(c => c.mTEQ2 == 1).Count();
            decimal mTE2no = mb.CFRMortgages.Where(c => c.mTEQ2 == 2).Count();
            decimal mTE2na = mb.CFRMortgages.Where(c => c.mTEQ2 == 3).Count();

            ViewBag.mTE2yes = Math.Round((mTE2yes * 10000) / mtgCFR) / 100;
            ViewBag.mTE2no = Math.Round((mTE2no * 10000) / mtgCFR) / 100;
            ViewBag.mTE2na = Math.Round((mTE2na * 10000) / mtgCFR) / 100;

            decimal mTE3yes = mb.CFRMortgages.Where(c => c.mTEQ3 == 1).Count();
            decimal mTE3no = mb.CFRMortgages.Where(c => c.mTEQ3 == 2).Count();
            decimal mTE3na = mb.CFRMortgages.Where(c => c.mTEQ3 == 3).Count();

            ViewBag.mTE3yes = Math.Round((mTE3yes * 10000) / mtgCFR) / 100;
            ViewBag.mTE3no = Math.Round((mTE3no * 10000) / mtgCFR) / 100;
            ViewBag.mTE3na = Math.Round((mTE3na * 10000) / mtgCFR) / 100;

            decimal mTE4yes = mb.CFRMortgages.Where(c => c.mTEQ4 == 1).Count();
            decimal mTE4no = mb.CFRMortgages.Where(c => c.mTEQ4 == 2).Count();
            decimal mTE4na = mb.CFRMortgages.Where(c => c.mTEQ4 == 3).Count();

            ViewBag.mTE4yes = Math.Round((mTE4yes * 10000) / mtgCFR) / 100;
            ViewBag.mTE4no = Math.Round((mTE4no * 10000) / mtgCFR) / 100;
            ViewBag.mTE4na = Math.Round((mTE4na * 10000) / mtgCFR) / 100;

            decimal mTE5yes = mb.CFRMortgages.Where(c => c.mTEQ5 == 1).Count();
            decimal mTE5no = mb.CFRMortgages.Where(c => c.mTEQ5 == 2).Count();
            decimal mTE5na = mb.CFRMortgages.Where(c => c.mTEQ5 == 3).Count();

            ViewBag.mTE5yes = Math.Round((mTE5yes * 10000) / mtgCFR) / 100;
            ViewBag.mTE5no = Math.Round((mTE5no * 10000) / mtgCFR) / 100;
            ViewBag.mTE5na = Math.Round((mTE5na * 10000) / mtgCFR) / 100;

            decimal mP1yes = mb.CFRMortgages.Where(c => c.mPQ1 == 1).Count();
            decimal mP1no = mb.CFRMortgages.Where(c => c.mPQ1 == 2).Count();
            decimal mP1na = mb.CFRMortgages.Where(c => c.mPQ1 == 3).Count();

            ViewBag.mP1yes = Math.Round((mP1yes * 10000) / mtgCFR) / 100;
            ViewBag.mP1no = Math.Round((mP1no * 10000) / mtgCFR) / 100;
            ViewBag.mP1na = Math.Round((mP1na * 10000) / mtgCFR) / 100;

            decimal mP2yes = mb.CFRMortgages.Where(c => c.mPQ2 == 1).Count();
            decimal mP2no = mb.CFRMortgages.Where(c => c.mPQ2 == 2).Count();
            decimal mP2na = mb.CFRMortgages.Where(c => c.mPQ2 == 3).Count();

            ViewBag.mP2yes = Math.Round((mP2yes * 10000) / mtgCFR) / 100;
            ViewBag.mP2no = Math.Round((mP2no * 10000) / mtgCFR) / 100;
            ViewBag.mP2na = Math.Round((mP2na * 10000) / mtgCFR) / 100;

            decimal mC1yes = mb.CFRMortgages.Where(c => c.mCQ1 == 1).Count();
            decimal mC1no = mb.CFRMortgages.Where(c => c.mCQ1 == 2).Count();
            decimal mC1na = mb.CFRMortgages.Where(c => c.mCQ1 == 3).Count();

            ViewBag.mC1yes = Math.Round((mC1yes * 10000) / mtgCFR) / 100;
            ViewBag.mC1no = Math.Round((mC1no * 10000) / mtgCFR) / 100;
            ViewBag.mC1na = Math.Round((mC1na * 10000) / mtgCFR) / 100;

            decimal mC2yes = mb.CFRMortgages.Where(c => c.mCQ2 == 1).Count();
            decimal mC2no = mb.CFRMortgages.Where(c => c.mCQ2 == 2).Count();
            decimal mC2na = mb.CFRMortgages.Where(c => c.mCQ2 == 3).Count();

            ViewBag.mC2yes = Math.Round((mC2yes * 10000) / mtgCFR) / 100;
            ViewBag.mC2no = Math.Round((mC2no * 10000) / mtgCFR) / 100;
            ViewBag.mC2na = Math.Round((mC2na * 10000) / mtgCFR) / 100;

            decimal mC3yes = mb.CFRMortgages.Where(c => c.mCQ3 == 1).Count();
            decimal mC3no = mb.CFRMortgages.Where(c => c.mCQ3 == 2).Count();
            decimal mC3na = mb.CFRMortgages.Where(c => c.mCQ3 == 3).Count();

            ViewBag.mC3yes = Math.Round((mC3yes * 10000) / mtgCFR) / 100;
            ViewBag.mC3no = Math.Round((mC3no * 10000) / mtgCFR) / 100;
            ViewBag.mC3na = Math.Round((mC3na * 10000) / mtgCFR) / 100;

            decimal mA1yes = mb.CFRMortgages.Where(c => c.mAQ1 == 1).Count();
            decimal mA1no = mb.CFRMortgages.Where(c => c.mAQ1 == 2).Count();
            decimal mA1na = mb.CFRMortgages.Where(c => c.mAQ1 == 3).Count();

            ViewBag.mA1yes = Math.Round((mA1yes * 10000) / mtgCFR) / 100;
            ViewBag.mA1no = Math.Round((mA1no * 10000) / mtgCFR) / 100;
            ViewBag.mA1na = Math.Round((mA1na * 10000) / mtgCFR) / 100;

            decimal mA2yes = mb.CFRMortgages.Where(c => c.mAQ2 == 1).Count();
            decimal mA2no = mb.CFRMortgages.Where(c => c.mAQ2 == 2).Count();
            decimal mA2na = mb.CFRMortgages.Where(c => c.mAQ2 == 3).Count();

            ViewBag.mA2yes = Math.Round((mA2yes * 10000) / mtgCFR) / 100;
            ViewBag.mA2no = Math.Round((mA2no * 10000) / mtgCFR) / 100;
            ViewBag.mA2na = Math.Round((mA2na * 10000) / mtgCFR) / 100;

            decimal mA3yes = mb.CFRMortgages.Where(c => c.mAQ3 == 1).Count();
            decimal mA3no = mb.CFRMortgages.Where(c => c.mAQ3 == 2).Count();
            decimal mA3na = mb.CFRMortgages.Where(c => c.mAQ3 == 3).Count();

            ViewBag.mA3yes = Math.Round((mA3yes * 10000) / mtgCFR) / 100;
            ViewBag.mA3no = Math.Round((mA3no * 10000) / mtgCFR) / 100;
            ViewBag.mA3na = Math.Round((mA3na * 10000) / mtgCFR) / 100;

            decimal mA4yes = mb.CFRMortgages.Where(c => c.mAQ4 == 1).Count();
            decimal mA4no = mb.CFRMortgages.Where(c => c.mAQ4 == 2).Count();
            decimal mA4na = mb.CFRMortgages.Where(c => c.mAQ4 == 3).Count();

            ViewBag.mA4yes = Math.Round((mA4yes * 10000) / mtgCFR) / 100;
            ViewBag.mA4no = Math.Round((mA4no * 10000) / mtgCFR) / 100;
            ViewBag.mA4na = Math.Round((mA4na * 10000) / mtgCFR) / 100;

            decimal mA5yes = mb.CFRMortgages.Where(c => c.mAQ5 == 1).Count();
            decimal mA5no = mb.CFRMortgages.Where(c => c.mAQ5 == 2).Count();
            decimal mA5na = mb.CFRMortgages.Where(c => c.mAQ5 == 3).Count();

            ViewBag.mA5yes = Math.Round((mA5yes * 10000) / mtgCFR) / 100;
            ViewBag.mA5no = Math.Round((mA5no * 10000) / mtgCFR) / 100;
            ViewBag.mA5na = Math.Round((mA5na * 10000) / mtgCFR) / 100;

            decimal mAOI1yes = mb.CFRMortgages.Where(c => c.mAOIQ1 == 1).Count();
            decimal mAOI1no = mb.CFRMortgages.Where(c => c.mAOIQ1 == 2).Count();
            decimal mAOI1na = mb.CFRMortgages.Where(c => c.mAOIQ1 == 3).Count();

            ViewBag.mAOI1yes = Math.Round((mAOI1yes * 10000) / mtgCFR) / 100;
            ViewBag.mAOI1no = Math.Round((mAOI1no * 10000) / mtgCFR) / 100;
            ViewBag.mAOI1na = Math.Round((mAOI1na * 10000) / mtgCFR) / 100;

            decimal mAOI2yes = mb.CFRMortgages.Where(c => c.mAOIQ2 == 1).Count();
            decimal mAOI2no = mb.CFRMortgages.Where(c => c.mAOIQ2 == 2).Count();
            decimal mAOI2na = mb.CFRMortgages.Where(c => c.mAOIQ2 == 3).Count();

            ViewBag.mAOI2yes = Math.Round((mAOI2yes * 10000) / mtgCFR) / 100;
            ViewBag.mAOI2no = Math.Round((mAOI2no * 10000) / mtgCFR) / 100;
            ViewBag.mAOI2na = Math.Round((mAOI2na * 10000) / mtgCFR) / 100;

            decimal mAOI3yes = mb.CFRMortgages.Where(c => c.mAOIQ3 == 1).Count();
            decimal mAOI3no = mb.CFRMortgages.Where(c => c.mAOIQ3 == 2).Count();
            decimal mAOI3na = mb.CFRMortgages.Where(c => c.mAOIQ3 == 3).Count();

            ViewBag.mAOI3yes = Math.Round((mAOI3yes * 10000) / mtgCFR) / 100;
            ViewBag.mAOI3no = Math.Round((mAOI3no * 10000) / mtgCFR) / 100;
            ViewBag.mAOI3na = Math.Round((mAOI3na * 10000) / mtgCFR) / 100;

            decimal mAOI4yes = mb.CFRMortgages.Where(c => c.mAOIQ4 == 1).Count();
            decimal mAOI4no = mb.CFRMortgages.Where(c => c.mAOIQ4 == 2).Count();
            decimal mAOI4na = mb.CFRMortgages.Where(c => c.mAOIQ4 == 3).Count();

            ViewBag.mAOI4yes = Math.Round((mAOI4yes * 10000) / mtgCFR) / 100;
            ViewBag.mAOI4no = Math.Round((mAOI4no * 10000) / mtgCFR) / 100;
            ViewBag.mAOI4na = Math.Round((mAOI4na * 10000) / mtgCFR) / 100;

            ////////////////////// INSURANCE //////////////////////////////////////////////////////////
            decimal iTE1yes = mb.CFRInsurances.Where(c => c.iTEQ1 == 1).Count();
            decimal iTE1no = mb.CFRInsurances.Where(c => c.iTEQ1 == 2).Count();
            decimal iTE1na = mb.CFRInsurances.Where(c => c.iTEQ1 == 3).Count();

            ViewBag.iTE1yes = Math.Round((iTE1yes * 10000) / insCFR) / 100;
            ViewBag.iTE1no = Math.Round((iTE1no * 10000) / insCFR) / 100;
            ViewBag.iTE1na = Math.Round((iTE1na * 10000) / insCFR) / 100;

            decimal iTE2yes = mb.CFRInsurances.Where(c => c.iTEQ2 == 1).Count();
            decimal iTE2no = mb.CFRInsurances.Where(c => c.iTEQ2 == 2).Count();
            decimal iTE2na = mb.CFRInsurances.Where(c => c.iTEQ2 == 3).Count();

            ViewBag.iTE2yes = Math.Round((iTE2yes * 10000) / insCFR) / 100;
            ViewBag.iTE2no = Math.Round((iTE2no * 10000) / insCFR) / 100;
            ViewBag.iTE2na = Math.Round((iTE2na * 10000) / insCFR) / 100;

            decimal iTE3yes = mb.CFRInsurances.Where(c => c.iTEQ3 == 1).Count();
            decimal iTE3no = mb.CFRInsurances.Where(c => c.iTEQ3 == 2).Count();
            decimal iTE3na = mb.CFRInsurances.Where(c => c.iTEQ3 == 3).Count();

            ViewBag.iTE3yes = Math.Round((iTE3yes * 10000) / insCFR) / 100;
            ViewBag.iTE3no = Math.Round((iTE3no * 10000) / insCFR) / 100;
            ViewBag.iTE3na = Math.Round((iTE3na * 10000) / insCFR) / 100;

            decimal iTE4yes = mb.CFRInsurances.Where(c => c.iTEQ4 == 1).Count();
            decimal iTE4no = mb.CFRInsurances.Where(c => c.iTEQ4 == 2).Count();
            decimal iTE4na = mb.CFRInsurances.Where(c => c.iTEQ4 == 3).Count();

            ViewBag.iTE4yes = Math.Round((iTE4yes * 10000) / insCFR) / 100;
            ViewBag.iTE4no = Math.Round((iTE4no * 10000) / insCFR) / 100;
            ViewBag.iTE4na = Math.Round((iTE4na * 10000) / insCFR) / 100;

            decimal iTE5yes = mb.CFRInsurances.Where(c => c.iTEQ5 == 1).Count();
            decimal iTE5no = mb.CFRInsurances.Where(c => c.iTEQ5 == 2).Count();
            decimal iTE5na = mb.CFRInsurances.Where(c => c.iTEQ5 == 3).Count();

            ViewBag.iTE5yes = Math.Round((iTE5yes * 10000) / insCFR) / 100;
            ViewBag.iTE5no = Math.Round((iTE5no * 10000) / insCFR) / 100;
            ViewBag.iTE5na = Math.Round((iTE5na * 10000) / insCFR) / 100;

            decimal iP1yes = mb.CFRInsurances.Where(c => c.iPQ1 == 1).Count();
            decimal iP1no = mb.CFRInsurances.Where(c => c.iPQ1 == 2).Count();
            decimal iP1na = mb.CFRInsurances.Where(c => c.iPQ1 == 3).Count();

            ViewBag.iP1yes = Math.Round((iP1yes * 10000) / insCFR) / 100;
            ViewBag.iP1no = Math.Round((iP1no * 10000) / insCFR) / 100;
            ViewBag.iP1na = Math.Round((iP1na * 10000) / insCFR) / 100;

            decimal iP2yes = mb.CFRInsurances.Where(c => c.iPQ2 == 1).Count();
            decimal iP2no = mb.CFRInsurances.Where(c => c.iPQ2 == 2).Count();
            decimal iP2na = mb.CFRInsurances.Where(c => c.iPQ2 == 3).Count();

            ViewBag.iP2yes = Math.Round((iP2yes * 10000) / insCFR) / 100;
            ViewBag.iP2no = Math.Round((iP2no * 10000) / insCFR) / 100;
            ViewBag.iP2na = Math.Round((iP2na * 10000) / insCFR) / 100;

            decimal iC1yes = mb.CFRInsurances.Where(c => c.iCQ1 == 1).Count();
            decimal iC1no = mb.CFRInsurances.Where(c => c.iCQ1 == 2).Count();
            decimal iC1na = mb.CFRInsurances.Where(c => c.iCQ1 == 3).Count();

            ViewBag.iC1yes = Math.Round((iC1yes * 10000) / insCFR) / 100;
            ViewBag.iC1no = Math.Round((iC1no * 10000) / insCFR) / 100;
            ViewBag.iC1na = Math.Round((iC1na * 10000) / insCFR) / 100;

            decimal iC2yes = mb.CFRInsurances.Where(c => c.iCQ2 == 1).Count();
            decimal iC2no = mb.CFRInsurances.Where(c => c.iCQ2 == 2).Count();
            decimal iC2na = mb.CFRInsurances.Where(c => c.iCQ2 == 3).Count();

            ViewBag.iC2yes = Math.Round((iC2yes * 10000) / insCFR) / 100;
            ViewBag.iC2no = Math.Round((iC2no * 10000) / insCFR) / 100;
            ViewBag.iC2na = Math.Round((iC2na * 10000) / insCFR) / 100;

            decimal iC3yes = mb.CFRInsurances.Where(c => c.iCQ3 == 1).Count();
            decimal iC3no = mb.CFRInsurances.Where(c => c.iCQ3 == 2).Count();
            decimal iC3na = mb.CFRInsurances.Where(c => c.iCQ3 == 3).Count();

            ViewBag.iC3yes = Math.Round((iC3yes * 10000) / insCFR) / 100;
            ViewBag.iC3no = Math.Round((iC3no * 10000) / insCFR) / 100;
            ViewBag.iC3na = Math.Round((iC3na * 10000) / insCFR) / 100;

            decimal iA1yes = mb.CFRInsurances.Where(c => c.iAQ1 == 1).Count();
            decimal iA1no = mb.CFRInsurances.Where(c => c.iAQ1 == 2).Count();
            decimal iA1na = mb.CFRInsurances.Where(c => c.iAQ1 == 3).Count();

            ViewBag.iA1yes = Math.Round((iA1yes * 10000) / insCFR) / 100;
            ViewBag.iA1no = Math.Round((iA1no * 10000) / insCFR) / 100;
            ViewBag.iA1na = Math.Round((iA1na * 10000) / insCFR) / 100;

            decimal iA2yes = mb.CFRInsurances.Where(c => c.iAQ2 == 1).Count();
            decimal iA2no = mb.CFRInsurances.Where(c => c.iAQ2 == 2).Count();
            decimal iA2na = mb.CFRInsurances.Where(c => c.iAQ2 == 3).Count();

            ViewBag.iA2yes = Math.Round((iA2yes * 10000) / insCFR) / 100;
            ViewBag.iA2no = Math.Round((iA2no * 10000) / insCFR) / 100;
            ViewBag.iA2na = Math.Round((iA2na * 10000) / insCFR) / 100;

            decimal iA3yes = mb.CFRInsurances.Where(c => c.iAQ3 == 1).Count();
            decimal iA3no = mb.CFRInsurances.Where(c => c.iAQ3 == 2).Count();
            decimal iA3na = mb.CFRInsurances.Where(c => c.iAQ3 == 3).Count();

            ViewBag.iA3yes = Math.Round((iA3yes * 10000) / insCFR) / 100;
            ViewBag.iA3no = Math.Round((iA3no * 10000) / insCFR) / 100;
            ViewBag.iA3na = Math.Round((iA3na * 10000) / insCFR) / 100;

            decimal iA4yes = mb.CFRInsurances.Where(c => c.iAQ4 == 1).Count();
            decimal iA4no = mb.CFRInsurances.Where(c => c.iAQ4 == 2).Count();
            decimal iA4na = mb.CFRInsurances.Where(c => c.iAQ4 == 3).Count();

            ViewBag.iA4yes = Math.Round((iA4yes * 10000) / insCFR) / 100;
            ViewBag.iA4no = Math.Round((iA4no * 10000) / insCFR) / 100;
            ViewBag.iA4na = Math.Round((iA4na * 10000) / insCFR) / 100;

            decimal iA5yes = mb.CFRInsurances.Where(c => c.iAQ5 == 1).Count();
            decimal iA5no = mb.CFRInsurances.Where(c => c.iAQ5 == 2).Count();
            decimal iA5na = mb.CFRInsurances.Where(c => c.iAQ5 == 3).Count();

            ViewBag.iA5yes = Math.Round((iA5yes * 10000) / insCFR) / 100;
            ViewBag.iA5no = Math.Round((iA5no * 10000) / insCFR) / 100;
            ViewBag.iA5na = Math.Round((iA5na * 10000) / insCFR) / 100;

            decimal iAOI1yes = mb.CFRInsurances.Where(c => c.iAOIQ1 == 1).Count();
            decimal iAOI1no = mb.CFRInsurances.Where(c => c.iAOIQ1 == 2).Count();
            decimal iAOI1na = mb.CFRInsurances.Where(c => c.iAOIQ1 == 3).Count();

            ViewBag.iAOI1yes = Math.Round((iAOI1yes * 10000) / insCFR) / 100;
            ViewBag.iAOI1no = Math.Round((iAOI1no * 10000) / insCFR) / 100;
            ViewBag.iAOI1na = Math.Round((iAOI1na * 10000) / insCFR) / 100;

            decimal iAOI2yes = mb.CFRInsurances.Where(c => c.iAOIQ2 == 1).Count();
            decimal iAOI2no = mb.CFRInsurances.Where(c => c.iAOIQ2 == 2).Count();
            decimal iAOI2na = mb.CFRInsurances.Where(c => c.iAOIQ2 == 3).Count();

            ViewBag.iAOI2yes = Math.Round((iAOI2yes * 10000) / insCFR) / 100;
            ViewBag.iAOI2no = Math.Round((iAOI2no * 10000) / insCFR) / 100;
            ViewBag.iAOI2na = Math.Round((iAOI2na * 10000) / insCFR) / 100;

            decimal iAOI3yes = mb.CFRInsurances.Where(c => c.iAOIQ3 == 1).Count();
            decimal iAOI3no = mb.CFRInsurances.Where(c => c.iAOIQ3 == 2).Count();
            decimal iAOI3na = mb.CFRInsurances.Where(c => c.iAOIQ3 == 3).Count();

            ViewBag.iAOI3yes = Math.Round((iAOI3yes * 10000) / insCFR) / 100;
            ViewBag.iAOI3no = Math.Round((iAOI3no * 10000) / insCFR) / 100;
            ViewBag.iAOI3na = Math.Round((iAOI3na * 10000) / insCFR) / 100;

            decimal iAOI4yes = mb.CFRInsurances.Where(c => c.iAOIQ4 == 1).Count();
            decimal iAOI4no = mb.CFRInsurances.Where(c => c.iAOIQ4 == 2).Count();
            decimal iAOI4na = mb.CFRInsurances.Where(c => c.iAOIQ4 == 3).Count();

            ViewBag.iAOI4yes = Math.Round((iAOI4yes * 10000) / insCFR) / 100;
            ViewBag.iAOI4no = Math.Round((iAOI4no * 10000) / insCFR) / 100;
            ViewBag.iAOI4na = Math.Round((iAOI4na * 10000) / insCFR) / 100;

            decimal iAOI5yes = mb.CFRInsurances.Where(c => c.iAOIQ5 == 1).Count();
            decimal iAOI5no = mb.CFRInsurances.Where(c => c.iAOIQ5 == 2).Count();
            decimal iAOI5na = mb.CFRInsurances.Where(c => c.iAOIQ5 == 3).Count();

            ViewBag.iAOI5yes = Math.Round((iAOI5yes * 10000) / insCFR) / 100;
            ViewBag.iAOI5no = Math.Round((iAOI5no * 10000) / insCFR) / 100;
            ViewBag.iAOI5na = Math.Round((iAOI5na * 10000) / insCFR) / 100;

            ////////////////////// PATIENT RECRUITMENT //////////////////////////////////////////////
            decimal pTE1yes = mb.CFRPatientRecruitments.Where(c => c.pTEQ1 == 1).Count();
            decimal pTE1no = mb.CFRPatientRecruitments.Where(c => c.pTEQ1 == 2).Count();
            decimal pTE1na = mb.CFRPatientRecruitments.Where(c => c.pTEQ1 == 3).Count();

            ViewBag.pTE1yes = Math.Round((pTE1yes * 10000) / prCFR) / 100;
            ViewBag.pTE1no = Math.Round((pTE1no * 10000) / prCFR) / 100;
            ViewBag.pTE1na = Math.Round((pTE1na * 10000) / prCFR) / 100;

            decimal pTE2yes = mb.CFRPatientRecruitments.Where(c => c.pTEQ2 == 1).Count();
            decimal pTE2no = mb.CFRPatientRecruitments.Where(c => c.pTEQ2 == 2).Count();
            decimal pTE2na = mb.CFRPatientRecruitments.Where(c => c.pTEQ2 == 3).Count();

            ViewBag.pTE2yes = Math.Round((pTE2yes * 10000) / prCFR) / 100;
            ViewBag.pTE2no = Math.Round((pTE2no * 10000) / prCFR) / 100;
            ViewBag.pTE2na = Math.Round((pTE2na * 10000) / prCFR) / 100;

            decimal pTE3yes = mb.CFRPatientRecruitments.Where(c => c.pTEQ3 == 1).Count();
            decimal pTE3no = mb.CFRPatientRecruitments.Where(c => c.pTEQ3 == 2).Count();
            decimal pTE3na = mb.CFRPatientRecruitments.Where(c => c.pTEQ3 == 3).Count();

            ViewBag.pTE3yes = Math.Round((pTE3yes * 10000) / prCFR) / 100;
            ViewBag.pTE3no = Math.Round((pTE3no * 10000) / prCFR) / 100;
            ViewBag.pTE3na = Math.Round((pTE3na * 10000) / prCFR) / 100;

            decimal pP1yes = mb.CFRPatientRecruitments.Where(c => c.pPQ1 == 1).Count();
            decimal pP1no = mb.CFRPatientRecruitments.Where(c => c.pPQ1 == 2).Count();
            decimal pP1na = mb.CFRPatientRecruitments.Where(c => c.pPQ1 == 3).Count();

            ViewBag.pP1yes = Math.Round((pP1yes * 10000) / prCFR) / 100;
            ViewBag.pP1no = Math.Round((pP1no * 10000) / prCFR) / 100;
            ViewBag.pP1na = Math.Round((pP1na * 10000) / prCFR) / 100;

            decimal pP2yes = mb.CFRPatientRecruitments.Where(c => c.pPQ2 == 1).Count();
            decimal pP2no = mb.CFRPatientRecruitments.Where(c => c.pPQ2 == 2).Count();
            decimal pP2na = mb.CFRPatientRecruitments.Where(c => c.pPQ2 == 3).Count();

            ViewBag.pP2yes = Math.Round((pP2yes * 10000) / prCFR) / 100;
            ViewBag.pP2no = Math.Round((pP2no * 10000) / prCFR) / 100;
            ViewBag.pP2na = Math.Round((pP2na * 10000) / prCFR) / 100;

            decimal pC1yes = mb.CFRPatientRecruitments.Where(c => c.pCQ1 == 1).Count();
            decimal pC1no = mb.CFRPatientRecruitments.Where(c => c.pCQ1 == 2).Count();
            decimal pC1na = mb.CFRPatientRecruitments.Where(c => c.pCQ1 == 3).Count();

            ViewBag.pC1yes = Math.Round((pC1yes * 10000) / prCFR) / 100;
            ViewBag.pC1no = Math.Round((pC1no * 10000) / prCFR) / 100;
            ViewBag.pC1na = Math.Round((pC1na * 10000) / prCFR) / 100;

            decimal pC2yes = mb.CFRPatientRecruitments.Where(c => c.pCQ2 == 1).Count();
            decimal pC2no = mb.CFRPatientRecruitments.Where(c => c.pCQ2 == 2).Count();
            decimal pC2na = mb.CFRPatientRecruitments.Where(c => c.pCQ2 == 3).Count();

            ViewBag.pC2yes = Math.Round((pC2yes * 10000) / prCFR) / 100;
            ViewBag.pC2no = Math.Round((pC2no * 10000) / prCFR) / 100;
            ViewBag.pC2na = Math.Round((pC2na * 10000) / prCFR) / 100;

            decimal pC3yes = mb.CFRPatientRecruitments.Where(c => c.pCQ3 == 1).Count();
            decimal pC3no = mb.CFRPatientRecruitments.Where(c => c.pCQ3 == 2).Count();
            decimal pC3na = mb.CFRPatientRecruitments.Where(c => c.pCQ3 == 3).Count();

            ViewBag.pC3yes = Math.Round((pC3yes * 10000) / prCFR) / 100;
            ViewBag.pC3no = Math.Round((pC3no * 10000) / prCFR) / 100;
            ViewBag.pC3na = Math.Round((pC3na * 10000) / prCFR) / 100;

            decimal pC4yes = mb.CFRPatientRecruitments.Where(c => c.pCQ4 == 1).Count();
            decimal pC4no = mb.CFRPatientRecruitments.Where(c => c.pCQ4 == 2).Count();
            decimal pC4na = mb.CFRPatientRecruitments.Where(c => c.pCQ4 == 3).Count();

            ViewBag.pC4yes = Math.Round((pC4yes * 10000) / prCFR) / 100;
            ViewBag.pC4no = Math.Round((pC4no * 10000) / prCFR) / 100;
            ViewBag.pC4na = Math.Round((pC4na * 10000) / prCFR) / 100;

            decimal pC5yes = mb.CFRPatientRecruitments.Where(c => c.pCQ5 == 1).Count();
            decimal pC5no = mb.CFRPatientRecruitments.Where(c => c.pCQ5 == 2).Count();
            decimal pC5na = mb.CFRPatientRecruitments.Where(c => c.pCQ5 == 3).Count();

            ViewBag.pC5yes = Math.Round((pC5yes * 10000) / prCFR) / 100;
            ViewBag.pC5no = Math.Round((pC5no * 10000) / prCFR) / 100;
            ViewBag.pC5na = Math.Round((pC5na * 10000) / prCFR) / 100;

            decimal pA1yes = mb.CFRPatientRecruitments.Where(c => c.pAQ1 == 1).Count();
            decimal pA1no = mb.CFRPatientRecruitments.Where(c => c.pAQ1 == 2).Count();
            decimal pA1na = mb.CFRPatientRecruitments.Where(c => c.pAQ1 == 3).Count();

            ViewBag.pA1yes = Math.Round((pA1yes * 10000) / prCFR) / 100;
            ViewBag.pA1no = Math.Round((pA1no * 10000) / prCFR) / 100;
            ViewBag.pA1na = Math.Round((pA1na * 10000) / prCFR) / 100;

            decimal pA2yes = mb.CFRPatientRecruitments.Where(c => c.pAQ2 == 1).Count();
            decimal pA2no = mb.CFRPatientRecruitments.Where(c => c.pAQ2 == 2).Count();
            decimal pA2na = mb.CFRPatientRecruitments.Where(c => c.pAQ2 == 3).Count();

            ViewBag.pA2yes = Math.Round((pA2yes * 10000) / prCFR) / 100;
            ViewBag.pA2no = Math.Round((pA2no * 10000) / prCFR) / 100;
            ViewBag.pA2na = Math.Round((pA2na * 10000) / prCFR) / 100;

            decimal pA3yes = mb.CFRPatientRecruitments.Where(c => c.pAQ3 == 1).Count();
            decimal pA3no = mb.CFRPatientRecruitments.Where(c => c.pAQ3 == 2).Count();
            decimal pA3na = mb.CFRPatientRecruitments.Where(c => c.pAQ3 == 3).Count();

            ViewBag.pA3yes = Math.Round((pA3yes * 10000) / prCFR) / 100;
            ViewBag.pA3no = Math.Round((pA3no * 10000) / prCFR) / 100;
            ViewBag.pA3na = Math.Round((pA3na * 10000) / prCFR) / 100;

            decimal pA4yes = mb.CFRPatientRecruitments.Where(c => c.pAQ4 == 1).Count();
            decimal pA4no = mb.CFRPatientRecruitments.Where(c => c.pAQ4 == 2).Count();
            decimal pA4na = mb.CFRPatientRecruitments.Where(c => c.pAQ4 == 3).Count();

            ViewBag.pA4yes = Math.Round((pA4yes * 10000) / prCFR) / 100;
            ViewBag.pA4no = Math.Round((pA4no * 10000) / prCFR) / 100;
            ViewBag.pA4na = Math.Round((pA4na * 10000) / prCFR) / 100;

            decimal pA5yes = mb.CFRPatientRecruitments.Where(c => c.pAQ5 == 1).Count();
            decimal pA5no = mb.CFRPatientRecruitments.Where(c => c.pAQ5 == 2).Count();
            decimal pA5na = mb.CFRPatientRecruitments.Where(c => c.pAQ5 == 3).Count();

            ViewBag.pA5yes = Math.Round((pA5yes * 10000) / prCFR) / 100;
            ViewBag.pA5no = Math.Round((pA5no * 10000) / prCFR) / 100;
            ViewBag.pA5na = Math.Round((pA5na * 10000) / prCFR) / 100;

            decimal pAOI1yes = mb.CFRPatientRecruitments.Where(c => c.pAOIQ1 == 1).Count();
            decimal pAOI1no = mb.CFRPatientRecruitments.Where(c => c.pAOIQ1 == 2).Count();
            decimal pAOI1na = mb.CFRPatientRecruitments.Where(c => c.pAOIQ1 == 3).Count();

            ViewBag.pAOI1yes = Math.Round((pAOI1yes * 10000) / prCFR) / 100;
            ViewBag.pAOI1no = Math.Round((pAOI1no * 10000) / prCFR) / 100;
            ViewBag.pAOI1na = Math.Round((pAOI1na * 10000) / prCFR) / 100;

            decimal pAOI2yes = mb.CFRPatientRecruitments.Where(c => c.pAOIQ2 == 1).Count();
            decimal pAOI2no = mb.CFRPatientRecruitments.Where(c => c.pAOIQ2 == 2).Count();
            decimal pAOI2na = mb.CFRPatientRecruitments.Where(c => c.pAOIQ2 == 3).Count();

            ViewBag.pAOI2yes = Math.Round((pAOI2yes * 10000) / prCFR) / 100;
            ViewBag.pAOI2no = Math.Round((pAOI2no * 10000) / prCFR) / 100;
            ViewBag.pAOI2na = Math.Round((pAOI2na * 10000) / prCFR) / 100;

            return View();
        }
    }
}