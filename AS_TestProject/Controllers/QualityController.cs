﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AS_TestProject.Models;
using AS_TestProject.Entities;
using AS_TestProject.Models.Helpers;
using System.Net;

namespace AS_TestProject.Controllers
{
    [Authorize(Roles = "Admin, Quality")]
    public class QualityController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        public class Emp
        {
            public int EmployeeID { get; set; }
            public string FullName { get; set; }
            public int SiteID { get; set; }
            public int TotalCFRsMonth { get; set; }
            public int RemainingCFRsNeeded { get; set; }
            public int DaysHere { get; set; }
        }


        // GET: Quality/Index
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult Index()
        {
            ViewBag.Domains = mb.DomainMasters.Where(d => d.IsActive == true && d.FileMask != "D00").OrderBy(d => d.FileMask).ToList();
            return View();
        }

        // GET: Quality/Details
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult Details(int? id, DateTime? start, DateTime? end, int? SiteID)
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

            ViewBag.Id = id;
            ViewBag.Domain = domain.FileMask + " - " + domain.DomainName;

            if(domain.DomainMasterID == 1)
            {
                ViewBag.DomainID = 1;
                if (start != null && end != null && SiteID != null)
                {
                    var accCFR = domain.CFRAcurians.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID).Count();
                    decimal totCFR = accCFR;

                    var accCFRs = domain.CFRAcurians.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID);

                    var totCalls = 0;
                    if (totCFR > 0)
                    {
                        foreach (var call in accCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Introduction % calculation
                    var accI1 = accCFRs.Where(d => d.IntroductionRating == 1).Count();
                    var accI2 = accCFRs.Where(d => d.IntroductionRating == 2).Count();
                    var accI3 = accCFRs.Where(d => d.IntroductionRating == 3).Count();
                    var accI4 = accCFRs.Where(d => d.IntroductionRating == 4).Count();

                    decimal totalI1 = accI1;
                    decimal totalI2 = accI2;
                    decimal totalI3 = accI3;
                    decimal totalI4 = accI4;

                    decimal percentI1 = 0;
                    decimal percentI2 = 0;
                    decimal percentI3 = 0;
                    decimal percentI4 = 0;

                    if (totCFR > 0)
                    {
                        percentI1 = Math.Round((totalI1 * 10000) / totCFR) / 100;
                        percentI2 = Math.Round((totalI2 * 10000) / totCFR) / 100;
                        percentI3 = Math.Round((totalI3 * 10000) / totCFR) / 100;
                        percentI4 = Math.Round((totalI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentI1 = percentI1;
                    ViewBag.PercentI2 = percentI2;
                    ViewBag.PercentI3 = percentI3;
                    ViewBag.PercentI4 = percentI4;

                    //Communication Skills % calculation
                    var accCS1 = accCFRs.Where(d => d.CommunicationSkillsRating == 1).Count();
                    var accCS2 = accCFRs.Where(d => d.CommunicationSkillsRating == 2).Count();
                    var accCS3 = accCFRs.Where(d => d.CommunicationSkillsRating == 3).Count();
                    var accCS4 = accCFRs.Where(d => d.CommunicationSkillsRating == 4).Count();

                    decimal totalCS1 = accCS1;
                    decimal totalCS2 = accCS2;
                    decimal totalCS3 = accCS3;
                    decimal totalCS4 = accCS4;

                    decimal percentCS1 = 0;
                    decimal percentCS2 = 0;
                    decimal percentCS3 = 0;
                    decimal percentCS4 = 0;

                    if (totCFR > 0)
                    {
                        percentCS1 = Math.Round((totalCS1 * 10000) / totCFR) / 100;
                        percentCS2 = Math.Round((totalCS2 * 10000) / totCFR) / 100;
                        percentCS3 = Math.Round((totalCS3 * 10000) / totCFR) / 100;
                        percentCS4 = Math.Round((totalCS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCS1 = percentCS1;
                    ViewBag.PercentCS2 = percentCS2;
                    ViewBag.PercentCS3 = percentCS3;
                    ViewBag.PercentCS4 = percentCS4;

                    //Soft Skills % calculation
                    var accSS1 = accCFRs.Where(d => d.SoftSkillsRating == 1).Count();
                    var accSS2 = accCFRs.Where(d => d.SoftSkillsRating == 2).Count();
                    var accSS3 = accCFRs.Where(d => d.SoftSkillsRating == 3).Count();
                    var accSS4 = accCFRs.Where(d => d.SoftSkillsRating == 4).Count();

                    decimal totalSS1 = accSS1;
                    decimal totalSS2 = accSS2;
                    decimal totalSS3 = accSS3;
                    decimal totalSS4 = accSS4;

                    decimal percentSS1 = 0;
                    decimal percentSS2 = 0;
                    decimal percentSS3 = 0;
                    decimal percentSS4 = 0;

                    if (totCFR > 0)
                    {
                        percentSS1 = Math.Round((totalSS1 * 10000) / totCFR) / 100;
                        percentSS2 = Math.Round((totalSS2 * 10000) / totCFR) / 100;
                        percentSS3 = Math.Round((totalSS3 * 10000) / totCFR) / 100;
                        percentSS4 = Math.Round((totalSS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentSS1 = percentSS1;
                    ViewBag.PercentSS2 = percentSS2;
                    ViewBag.PercentSS3 = percentSS3;
                    ViewBag.PercentSS4 = percentSS4;

                    //Compliance % calculation
                    var accCO1 = accCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var accCO2 = accCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var accCO3 = accCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var accCO4 = accCFRs.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalCO1 = accCO1;
                    decimal totalCO2 = accCO2;
                    decimal totalCO3 = accCO3;
                    decimal totalCO4 = accCO4;

                    decimal percentCO1 = 0;
                    decimal percentCO2 = 0;
                    decimal percentCO3 = 0;
                    decimal percentCO4 = 0;

                    if (totCFR > 0)
                    {
                        percentCO1 = Math.Round((totalCO1 * 10000) / totCFR) / 100;
                        percentCO2 = Math.Round((totalCO2 * 10000) / totCFR) / 100;
                        percentCO3 = Math.Round((totalCO3 * 10000) / totCFR) / 100;
                        percentCO4 = Math.Round((totalCO4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCO1 = percentCO1;
                    ViewBag.PercentCO2 = percentCO2;
                    ViewBag.PercentCO3 = percentCO3;
                    ViewBag.PercentCO4 = percentCO4;

                    //Closing % calculation
                    var accCL1 = accCFRs.Where(d => d.ClosingRating == 1).Count();
                    var accCL2 = accCFRs.Where(d => d.ClosingRating == 2).Count();
                    var accCL3 = accCFRs.Where(d => d.ClosingRating == 3).Count();
                    var accCL4 = accCFRs.Where(d => d.ClosingRating == 4).Count();

                    decimal totalCL1 = accCL1;
                    decimal totalCL2 = accCL2;
                    decimal totalCL3 = accCL3;
                    decimal totalCL4 = accCL4;

                    decimal percentCL1 = 0;
                    decimal percentCL2 = 0;
                    decimal percentCL3 = 0;
                    decimal percentCL4 = 0;

                    if (totCFR > 0)
                    {
                        percentCL1 = Math.Round((totalCL1 * 10000) / totCFR) / 100;
                        percentCL2 = Math.Round((totalCL2 * 10000) / totCFR) / 100;
                        percentCL3 = Math.Round((totalCL3 * 10000) / totCFR) / 100;
                        percentCL4 = Math.Round((totalCL4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCL1 = percentCL1;
                    ViewBag.PercentCL2 = percentCL2;
                    ViewBag.PercentCL3 = percentCL3;
                    ViewBag.PercentCL4 = percentCL4;

                    ViewBag.StartDate = start.Value.ToString("d");
                    ViewBag.EndDate = end.Value.ToString("d");
                    if (SiteID == 1)
                    {
                        ViewBag.Site = "Greensboro";
                    }
                    if (SiteID == 2)
                    {
                        ViewBag.Site = "Wichita";
                    }
                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName", SiteID);
                    ViewBag.Phrase = 1;
                }

                else if ((start == null || end == null) && SiteID != null)
                {
                    var accCFR = domain.CFRAcurians.Where(d => d.Employee.SiteID == SiteID).Count();
                    decimal totCFR = accCFR;

                    var accCFRs = domain.CFRAcurians.Where(d => d.Employee.SiteID == SiteID);

                    var totCalls = 0;
                    if (totCFR > 0)
                    {
                        foreach (var call in accCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Introduction % calculation
                    var accI1 = accCFRs.Where(d => d.IntroductionRating == 1).Count();
                    var accI2 = accCFRs.Where(d => d.IntroductionRating == 2).Count();
                    var accI3 = accCFRs.Where(d => d.IntroductionRating == 3).Count();
                    var accI4 = accCFRs.Where(d => d.IntroductionRating == 4).Count();

                    decimal totalI1 = accI1;
                    decimal totalI2 = accI2;
                    decimal totalI3 = accI3;
                    decimal totalI4 = accI4;

                    decimal percentI1 = 0;
                    decimal percentI2 = 0;
                    decimal percentI3 = 0;
                    decimal percentI4 = 0;

                    if (totCFR > 0)
                    {
                        percentI1 = Math.Round((totalI1 * 10000) / totCFR) / 100;
                        percentI2 = Math.Round((totalI2 * 10000) / totCFR) / 100;
                        percentI3 = Math.Round((totalI3 * 10000) / totCFR) / 100;
                        percentI4 = Math.Round((totalI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentI1 = percentI1;
                    ViewBag.PercentI2 = percentI2;
                    ViewBag.PercentI3 = percentI3;
                    ViewBag.PercentI4 = percentI4;

                    //Communication Skills % calculation
                    var accCS1 = accCFRs.Where(d => d.CommunicationSkillsRating == 1).Count();
                    var accCS2 = accCFRs.Where(d => d.CommunicationSkillsRating == 2).Count();
                    var accCS3 = accCFRs.Where(d => d.CommunicationSkillsRating == 3).Count();
                    var accCS4 = accCFRs.Where(d => d.CommunicationSkillsRating == 4).Count();

                    decimal totalCS1 = accCS1;
                    decimal totalCS2 = accCS2;
                    decimal totalCS3 = accCS3;
                    decimal totalCS4 = accCS4;

                    decimal percentCS1 = 0;
                    decimal percentCS2 = 0;
                    decimal percentCS3 = 0;
                    decimal percentCS4 = 0;

                    if (totCFR > 0)
                    {
                        percentCS1 = Math.Round((totalCS1 * 10000) / totCFR) / 100;
                        percentCS2 = Math.Round((totalCS2 * 10000) / totCFR) / 100;
                        percentCS3 = Math.Round((totalCS3 * 10000) / totCFR) / 100;
                        percentCS4 = Math.Round((totalCS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCS1 = percentCS1;
                    ViewBag.PercentCS2 = percentCS2;
                    ViewBag.PercentCS3 = percentCS3;
                    ViewBag.PercentCS4 = percentCS4;

                    //Soft Skills % calculation
                    var accSS1 = accCFRs.Where(d => d.SoftSkillsRating == 1).Count();
                    var accSS2 = accCFRs.Where(d => d.SoftSkillsRating == 2).Count();
                    var accSS3 = accCFRs.Where(d => d.SoftSkillsRating == 3).Count();
                    var accSS4 = accCFRs.Where(d => d.SoftSkillsRating == 4).Count();

                    decimal totalSS1 = accSS1;
                    decimal totalSS2 = accSS2;
                    decimal totalSS3 = accSS3;
                    decimal totalSS4 = accSS4;

                    decimal percentSS1 = 0;
                    decimal percentSS2 = 0;
                    decimal percentSS3 = 0;
                    decimal percentSS4 = 0;

                    if (totCFR > 0)
                    {
                        percentSS1 = Math.Round((totalSS1 * 10000) / totCFR) / 100;
                        percentSS2 = Math.Round((totalSS2 * 10000) / totCFR) / 100;
                        percentSS3 = Math.Round((totalSS3 * 10000) / totCFR) / 100;
                        percentSS4 = Math.Round((totalSS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentSS1 = percentSS1;
                    ViewBag.PercentSS2 = percentSS2;
                    ViewBag.PercentSS3 = percentSS3;
                    ViewBag.PercentSS4 = percentSS4;

                    //Compliance % calculation
                    var accCO1 = accCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var accCO2 = accCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var accCO3 = accCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var accCO4 = accCFRs.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalCO1 = accCO1;
                    decimal totalCO2 = accCO2;
                    decimal totalCO3 = accCO3;
                    decimal totalCO4 = accCO4;

                    decimal percentCO1 = 0;
                    decimal percentCO2 = 0;
                    decimal percentCO3 = 0;
                    decimal percentCO4 = 0;

                    if (totCFR > 0)
                    {
                        percentCO1 = Math.Round((totalCO1 * 10000) / totCFR) / 100;
                        percentCO2 = Math.Round((totalCO2 * 10000) / totCFR) / 100;
                        percentCO3 = Math.Round((totalCO3 * 10000) / totCFR) / 100;
                        percentCO4 = Math.Round((totalCO4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCO1 = percentCO1;
                    ViewBag.PercentCO2 = percentCO2;
                    ViewBag.PercentCO3 = percentCO3;
                    ViewBag.PercentCO4 = percentCO4;

                    //Closing % calculation
                    var accCL1 = accCFRs.Where(d => d.ClosingRating == 1).Count();
                    var accCL2 = accCFRs.Where(d => d.ClosingRating == 2).Count();
                    var accCL3 = accCFRs.Where(d => d.ClosingRating == 3).Count();
                    var accCL4 = accCFRs.Where(d => d.ClosingRating == 4).Count();

                    decimal totalCL1 = accCL1;
                    decimal totalCL2 = accCL2;
                    decimal totalCL3 = accCL3;
                    decimal totalCL4 = accCL4;

                    decimal percentCL1 = 0;
                    decimal percentCL2 = 0;
                    decimal percentCL3 = 0;
                    decimal percentCL4 = 0;

                    if (totCFR > 0)
                    {
                        percentCL1 = Math.Round((totalCL1 * 10000) / totCFR) / 100;
                        percentCL2 = Math.Round((totalCL2 * 10000) / totCFR) / 100;
                        percentCL3 = Math.Round((totalCL3 * 10000) / totCFR) / 100;
                        percentCL4 = Math.Round((totalCL4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCL1 = percentCL1;
                    ViewBag.PercentCL2 = percentCL2;
                    ViewBag.PercentCL3 = percentCL3;
                    ViewBag.PercentCL4 = percentCL4;

                    if (SiteID == 1)
                    {
                        ViewBag.Site = "Greensboro";
                    }
                    if (SiteID == 2)
                    {
                        ViewBag.Site = "Wichita";
                    }
                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName", SiteID);
                    ViewBag.Phrase = 2;
                }

                else if (start != null && end != null && SiteID == null)
                {
                    var accCFR = domain.CFRAcurians.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1)).Count();
                    decimal totCFR = accCFR;

                    var accCFRs = domain.CFRAcurians.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));

                    var totCalls = 0;
                    if (totCFR > 0)
                    {
                        foreach (var call in accCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Introduction % calculation
                    var accI1 = accCFRs.Where(d => d.IntroductionRating == 1).Count();
                    var accI2 = accCFRs.Where(d => d.IntroductionRating == 2).Count();
                    var accI3 = accCFRs.Where(d => d.IntroductionRating == 3).Count();
                    var accI4 = accCFRs.Where(d => d.IntroductionRating == 4).Count();

                    decimal totalI1 = accI1;
                    decimal totalI2 = accI2;
                    decimal totalI3 = accI3;
                    decimal totalI4 = accI4;

                    decimal percentI1 = 0;
                    decimal percentI2 = 0;
                    decimal percentI3 = 0;
                    decimal percentI4 = 0;

                    if (totCFR > 0)
                    {
                        percentI1 = Math.Round((totalI1 * 10000) / totCFR) / 100;
                        percentI2 = Math.Round((totalI2 * 10000) / totCFR) / 100;
                        percentI3 = Math.Round((totalI3 * 10000) / totCFR) / 100;
                        percentI4 = Math.Round((totalI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentI1 = percentI1;
                    ViewBag.PercentI2 = percentI2;
                    ViewBag.PercentI3 = percentI3;
                    ViewBag.PercentI4 = percentI4;

                    //Communication Skills % calculation
                    var accCS1 = accCFRs.Where(d => d.CommunicationSkillsRating == 1).Count();
                    var accCS2 = accCFRs.Where(d => d.CommunicationSkillsRating == 2).Count();
                    var accCS3 = accCFRs.Where(d => d.CommunicationSkillsRating == 3).Count();
                    var accCS4 = accCFRs.Where(d => d.CommunicationSkillsRating == 4).Count();

                    decimal totalCS1 = accCS1;
                    decimal totalCS2 = accCS2;
                    decimal totalCS3 = accCS3;
                    decimal totalCS4 = accCS4;

                    decimal percentCS1 = 0;
                    decimal percentCS2 = 0;
                    decimal percentCS3 = 0;
                    decimal percentCS4 = 0;

                    if (totCFR > 0)
                    {
                        percentCS1 = Math.Round((totalCS1 * 10000) / totCFR) / 100;
                        percentCS2 = Math.Round((totalCS2 * 10000) / totCFR) / 100;
                        percentCS3 = Math.Round((totalCS3 * 10000) / totCFR) / 100;
                        percentCS4 = Math.Round((totalCS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCS1 = percentCS1;
                    ViewBag.PercentCS2 = percentCS2;
                    ViewBag.PercentCS3 = percentCS3;
                    ViewBag.PercentCS4 = percentCS4;

                    //Soft Skills % calculation
                    var accSS1 = accCFRs.Where(d => d.SoftSkillsRating == 1).Count();
                    var accSS2 = accCFRs.Where(d => d.SoftSkillsRating == 2).Count();
                    var accSS3 = accCFRs.Where(d => d.SoftSkillsRating == 3).Count();
                    var accSS4 = accCFRs.Where(d => d.SoftSkillsRating == 4).Count();

                    decimal totalSS1 = accSS1;
                    decimal totalSS2 = accSS2;
                    decimal totalSS3 = accSS3;
                    decimal totalSS4 = accSS4;

                    decimal percentSS1 = 0;
                    decimal percentSS2 = 0;
                    decimal percentSS3 = 0;
                    decimal percentSS4 = 0;

                    if (totCFR > 0)
                    {
                        percentSS1 = Math.Round((totalSS1 * 10000) / totCFR) / 100;
                        percentSS2 = Math.Round((totalSS2 * 10000) / totCFR) / 100;
                        percentSS3 = Math.Round((totalSS3 * 10000) / totCFR) / 100;
                        percentSS4 = Math.Round((totalSS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentSS1 = percentSS1;
                    ViewBag.PercentSS2 = percentSS2;
                    ViewBag.PercentSS3 = percentSS3;
                    ViewBag.PercentSS4 = percentSS4;

                    //Compliance % calculation
                    var accCO1 = accCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var accCO2 = accCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var accCO3 = accCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var accCO4 = accCFRs.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalCO1 = accCO1;
                    decimal totalCO2 = accCO2;
                    decimal totalCO3 = accCO3;
                    decimal totalCO4 = accCO4;

                    decimal percentCO1 = 0;
                    decimal percentCO2 = 0;
                    decimal percentCO3 = 0;
                    decimal percentCO4 = 0;

                    if (totCFR > 0)
                    {
                        percentCO1 = Math.Round((totalCO1 * 10000) / totCFR) / 100;
                        percentCO2 = Math.Round((totalCO2 * 10000) / totCFR) / 100;
                        percentCO3 = Math.Round((totalCO3 * 10000) / totCFR) / 100;
                        percentCO4 = Math.Round((totalCO4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCO1 = percentCO1;
                    ViewBag.PercentCO2 = percentCO2;
                    ViewBag.PercentCO3 = percentCO3;
                    ViewBag.PercentCO4 = percentCO4;

                    //Closing % calculation
                    var accCL1 = accCFRs.Where(d => d.ClosingRating == 1).Count();
                    var accCL2 = accCFRs.Where(d => d.ClosingRating == 2).Count();
                    var accCL3 = accCFRs.Where(d => d.ClosingRating == 3).Count();
                    var accCL4 = accCFRs.Where(d => d.ClosingRating == 4).Count();

                    decimal totalCL1 = accCL1;
                    decimal totalCL2 = accCL2;
                    decimal totalCL3 = accCL3;
                    decimal totalCL4 = accCL4;

                    decimal percentCL1 = 0;
                    decimal percentCL2 = 0;
                    decimal percentCL3 = 0;
                    decimal percentCL4 = 0;

                    if (totCFR > 0)
                    {
                        percentCL1 = Math.Round((totalCL1 * 10000) / totCFR) / 100;
                        percentCL2 = Math.Round((totalCL2 * 10000) / totCFR) / 100;
                        percentCL3 = Math.Round((totalCL3 * 10000) / totCFR) / 100;
                        percentCL4 = Math.Round((totalCL4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCL1 = percentCL1;
                    ViewBag.PercentCL2 = percentCL2;
                    ViewBag.PercentCL3 = percentCL3;
                    ViewBag.PercentCL4 = percentCL4;

                    ViewBag.StartDate = start.Value.ToString("d");
                    ViewBag.EndDate = end.Value.ToString("d");
                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName");
                    ViewBag.Phrase = 3;
                }

                else
                {
                    var accCFR = domain.CFRAcurians.Count;
                    decimal totCFR = accCFR;
                    var totCalls = 0;

                    if (totCFR > 0)
                    {
                        var accCFRs = domain.CFRAcurians;

                        foreach (var call in accCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Introduction % calculation
                    var accI1 = domain.CFRAcurians.Where(d => d.IntroductionRating == 1).Count();
                    var accI2 = domain.CFRAcurians.Where(d => d.IntroductionRating == 2).Count();
                    var accI3 = domain.CFRAcurians.Where(d => d.IntroductionRating == 3).Count();
                    var accI4 = domain.CFRAcurians.Where(d => d.IntroductionRating == 4).Count();

                    decimal totalI1 = accI1;
                    decimal totalI2 = accI2;
                    decimal totalI3 = accI3;
                    decimal totalI4 = accI4;

                    decimal percentI1 = 0;
                    decimal percentI2 = 0;
                    decimal percentI3 = 0;
                    decimal percentI4 = 0;

                    if (totCFR > 0)
                    {
                        percentI1 = Math.Round((totalI1 * 10000) / totCFR) / 100;
                        percentI2 = Math.Round((totalI2 * 10000) / totCFR) / 100;
                        percentI3 = Math.Round((totalI3 * 10000) / totCFR) / 100;
                        percentI4 = Math.Round((totalI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentI1 = percentI1;
                    ViewBag.PercentI2 = percentI2;
                    ViewBag.PercentI3 = percentI3;
                    ViewBag.PercentI4 = percentI4;

                    //Communication Skills % calculation
                    var accCS1 = domain.CFRAcurians.Where(d => d.CommunicationSkillsRating == 1).Count();
                    var accCS2 = domain.CFRAcurians.Where(d => d.CommunicationSkillsRating == 2).Count();
                    var accCS3 = domain.CFRAcurians.Where(d => d.CommunicationSkillsRating == 3).Count();
                    var accCS4 = domain.CFRAcurians.Where(d => d.CommunicationSkillsRating == 4).Count();

                    decimal totalCS1 = accCS1;
                    decimal totalCS2 = accCS2;
                    decimal totalCS3 = accCS3;
                    decimal totalCS4 = accCS4;

                    decimal percentCS1 = 0;
                    decimal percentCS2 = 0;
                    decimal percentCS3 = 0;
                    decimal percentCS4 = 0;

                    if (totCFR > 0)
                    {
                        percentCS1 = Math.Round((totalCS1 * 10000) / totCFR) / 100;
                        percentCS2 = Math.Round((totalCS2 * 10000) / totCFR) / 100;
                        percentCS3 = Math.Round((totalCS3 * 10000) / totCFR) / 100;
                        percentCS4 = Math.Round((totalCS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCS1 = percentCS1;
                    ViewBag.PercentCS2 = percentCS2;
                    ViewBag.PercentCS3 = percentCS3;
                    ViewBag.PercentCS4 = percentCS4;

                    //Soft Skills % calculation
                    var accSS1 = domain.CFRAcurians.Where(d => d.SoftSkillsRating == 1).Count();
                    var accSS2 = domain.CFRAcurians.Where(d => d.SoftSkillsRating == 2).Count();
                    var accSS3 = domain.CFRAcurians.Where(d => d.SoftSkillsRating == 3).Count();
                    var accSS4 = domain.CFRAcurians.Where(d => d.SoftSkillsRating == 4).Count();

                    decimal totalSS1 = accSS1;
                    decimal totalSS2 = accSS2;
                    decimal totalSS3 = accSS3;
                    decimal totalSS4 = accSS4;

                    decimal percentSS1 = 0;
                    decimal percentSS2 = 0;
                    decimal percentSS3 = 0;
                    decimal percentSS4 = 0;

                    if (totCFR > 0)
                    {
                        percentSS1 = Math.Round((totalSS1 * 10000) / totCFR) / 100;
                        percentSS2 = Math.Round((totalSS2 * 10000) / totCFR) / 100;
                        percentSS3 = Math.Round((totalSS3 * 10000) / totCFR) / 100;
                        percentSS4 = Math.Round((totalSS4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentSS1 = percentSS1;
                    ViewBag.PercentSS2 = percentSS2;
                    ViewBag.PercentSS3 = percentSS3;
                    ViewBag.PercentSS4 = percentSS4;

                    //Compliance % calculation
                    var accCO1 = domain.CFRAcurians.Where(d => d.ComplianceRating == 1).Count();
                    var accCO2 = domain.CFRAcurians.Where(d => d.ComplianceRating == 2).Count();
                    var accCO3 = domain.CFRAcurians.Where(d => d.ComplianceRating == 3).Count();
                    var accCO4 = domain.CFRAcurians.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalCO1 = accCO1;
                    decimal totalCO2 = accCO2;
                    decimal totalCO3 = accCO3;
                    decimal totalCO4 = accCO4;

                    decimal percentCO1 = 0;
                    decimal percentCO2 = 0;
                    decimal percentCO3 = 0;
                    decimal percentCO4 = 0;

                    if (totCFR > 0)
                    {
                        percentCO1 = Math.Round((totalCO1 * 10000) / totCFR) / 100;
                        percentCO2 = Math.Round((totalCO2 * 10000) / totCFR) / 100;
                        percentCO3 = Math.Round((totalCO3 * 10000) / totCFR) / 100;
                        percentCO4 = Math.Round((totalCO4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCO1 = percentCO1;
                    ViewBag.PercentCO2 = percentCO2;
                    ViewBag.PercentCO3 = percentCO3;
                    ViewBag.PercentCO4 = percentCO4;

                    //Closing % calculation
                    var accCL1 = domain.CFRAcurians.Where(d => d.ClosingRating == 1).Count();
                    var accCL2 = domain.CFRAcurians.Where(d => d.ClosingRating == 2).Count();
                    var accCL3 = domain.CFRAcurians.Where(d => d.ClosingRating == 3).Count();
                    var accCL4 = domain.CFRAcurians.Where(d => d.ClosingRating == 4).Count();

                    decimal totalCL1 = accCL1;
                    decimal totalCL2 = accCL2;
                    decimal totalCL3 = accCL3;
                    decimal totalCL4 = accCL4;

                    decimal percentCL1 = 0;
                    decimal percentCL2 = 0;
                    decimal percentCL3 = 0;
                    decimal percentCL4 = 0;

                    if (totCFR > 0)
                    {
                        percentCL1 = Math.Round((totalCL1 * 10000) / totCFR) / 100;
                        percentCL2 = Math.Round((totalCL2 * 10000) / totCFR) / 100;
                        percentCL3 = Math.Round((totalCL3 * 10000) / totCFR) / 100;
                        percentCL4 = Math.Round((totalCL4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentCL1 = percentCL1;
                    ViewBag.PercentCL2 = percentCL2;
                    ViewBag.PercentCL3 = percentCL3;
                    ViewBag.PercentCL4 = percentCL4;

                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName");
                }
            }
            else
            {
                ViewBag.DomainID = 2;
                if (start != null && end != null && SiteID != null)
                {
                    var mtgCFR = domain.CFRMortgages.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID).Count();
                    var insCFR = domain.CFRInsurances.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID).Count();
                    var prCFR = domain.CFRPatientRecruitments.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID).Count();
                    var slsCFR = domain.CFRSales.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID).Count();
                    decimal totCFR = mtgCFR + insCFR + prCFR + slsCFR;

                    var mtgCFRs = domain.CFRMortgages.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID);
                    var insCFRs = domain.CFRInsurances.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID);
                    var prCFRs = domain.CFRPatientRecruitments.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID);
                    var slsCFRs = domain.CFRSales.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID);

                    var totCalls = 0;
                    if (totCFR > 0)
                    {
                        foreach (var call in mtgCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in insCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in prCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in slsCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Telephone Etiquette % calculation
                    var mtgTE1 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var mtgTE2 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var mtgTE3 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var mtgTE4 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var insTE1 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var insTE2 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var insTE3 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var insTE4 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var prTE1 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var prTE2 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var prTE3 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var prTE4 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var slsTE1 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var slsTE2 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var slsTE3 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var slsTE4 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    decimal totalTE1 = mtgTE1 + insTE1 + prTE1 + slsTE1;
                    decimal totalTE2 = mtgTE2 + insTE2 + prTE2 + slsTE2;
                    decimal totalTE3 = mtgTE3 + insTE3 + prTE3 + slsTE3;
                    decimal totalTE4 = mtgTE4 + insTE4 + prTE4 + slsTE4;

                    decimal percentTE1 = 0;
                    decimal percentTE2 = 0;
                    decimal percentTE3 = 0;
                    decimal percentTE4 = 0;

                    if (totCFR > 0)
                    {
                        percentTE1 = Math.Round((totalTE1 * 10000) / totCFR) / 100;
                        percentTE2 = Math.Round((totalTE2 * 10000) / totCFR) / 100;
                        percentTE3 = Math.Round((totalTE3 * 10000) / totCFR) / 100;
                        percentTE4 = Math.Round((totalTE4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentTE1 = percentTE1;
                    ViewBag.PercentTE2 = percentTE2;
                    ViewBag.PercentTE3 = percentTE3;
                    ViewBag.PercentTE4 = percentTE4;

                    //Professionalism % calculation
                    var mtgP1 = mtgCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var mtgP2 = mtgCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var mtgP3 = mtgCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var mtgP4 = mtgCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var insP1 = insCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var insP2 = insCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var insP3 = insCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var insP4 = insCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var prP1 = prCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var prP2 = prCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var prP3 = prCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var prP4 = prCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var slsP1 = slsCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var slsP2 = slsCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var slsP3 = slsCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var slsP4 = slsCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    decimal totalP1 = mtgP1 + insP1 + prP1 + slsP1;
                    decimal totalP2 = mtgP2 + insP2 + prP2 + slsP2;
                    decimal totalP3 = mtgP3 + insP3 + prP3 + slsP3;
                    decimal totalP4 = mtgP4 + insP4 + prP4 + slsP4;

                    decimal percentP1 = 0;
                    decimal percentP2 = 0;
                    decimal percentP3 = 0;
                    decimal percentP4 = 0;

                    if (totCFR > 0)
                    {
                        percentP1 = Math.Round((totalP1 * 10000) / totCFR) / 100;
                        percentP2 = Math.Round((totalP2 * 10000) / totCFR) / 100;
                        percentP3 = Math.Round((totalP3 * 10000) / totCFR) / 100;
                        percentP4 = Math.Round((totalP4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentP1 = percentP1;
                    ViewBag.PercentP2 = percentP2;
                    ViewBag.PercentP3 = percentP3;
                    ViewBag.PercentP4 = percentP4;

                    //Compliance % calculation
                    var mtgC1 = mtgCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var mtgC2 = mtgCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var mtgC3 = mtgCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var mtgC4 = mtgCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var insC1 = insCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var insC2 = insCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var insC3 = insCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var insC4 = insCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var prC1 = prCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var prC2 = prCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var prC3 = prCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var prC4 = prCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var slsC1 = slsCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var slsC2 = slsCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var slsC3 = slsCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var slsC4 = slsCFRs.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalC1 = mtgC1 + insC1 + prC1 + slsC1;
                    decimal totalC2 = mtgC2 + insC2 + prC2 + slsC2;
                    decimal totalC3 = mtgC3 + insC3 + prC3 + slsC3;
                    decimal totalC4 = mtgC4 + insC4 + prC4 + slsC4;

                    decimal percentC1 = 0;
                    decimal percentC2 = 0;
                    decimal percentC3 = 0;
                    decimal percentC4 = 0;

                    if (totCFR > 0)
                    {
                        percentC1 = Math.Round((totalC1 * 10000) / totCFR) / 100;
                        percentC2 = Math.Round((totalC2 * 10000) / totCFR) / 100;
                        percentC3 = Math.Round((totalC3 * 10000) / totCFR) / 100;
                        percentC4 = Math.Round((totalC4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentC1 = percentC1;
                    ViewBag.PercentC2 = percentC2;
                    ViewBag.PercentC3 = percentC3;
                    ViewBag.PercentC4 = percentC4;

                    //Adherence % calculation
                    var mtgA1 = mtgCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var mtgA2 = mtgCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var mtgA3 = mtgCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var mtgA4 = mtgCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var insA1 = insCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var insA2 = insCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var insA3 = insCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var insA4 = insCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var prA1 = prCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var prA2 = prCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var prA3 = prCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var prA4 = prCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var slsA1 = slsCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var slsA2 = slsCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var slsA3 = slsCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var slsA4 = slsCFRs.Where(d => d.AdheranceRating == 4).Count();

                    decimal totalA1 = mtgA1 + insA1 + prA1 + slsA1;
                    decimal totalA2 = mtgA2 + insA2 + prA2 + slsA2;
                    decimal totalA3 = mtgA3 + insA3 + prA3 + slsA3;
                    decimal totalA4 = mtgA4 + insA4 + prA4 + slsA4;

                    decimal percentA1 = 0;
                    decimal percentA2 = 0;
                    decimal percentA3 = 0;
                    decimal percentA4 = 0;

                    if (totCFR > 0)
                    {
                        percentA1 = Math.Round((totalA1 * 10000) / totCFR) / 100;
                        percentA2 = Math.Round((totalA2 * 10000) / totCFR) / 100;
                        percentA3 = Math.Round((totalA3 * 10000) / totCFR) / 100;
                        percentA4 = Math.Round((totalA4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentA1 = percentA1;
                    ViewBag.PercentA2 = percentA2;
                    ViewBag.PercentA3 = percentA3;
                    ViewBag.PercentA4 = percentA4;

                    //Accuracy of Information % calculation
                    var mtgAOI1 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var mtgAOI2 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var mtgAOI3 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var mtgAOI4 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var insAOI1 = insCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var insAOI2 = insCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var insAOI3 = insCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var insAOI4 = insCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var prAOI1 = prCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var prAOI2 = prCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var prAOI3 = prCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var prAOI4 = prCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var slsAOI1 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var slsAOI2 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var slsAOI3 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var slsAOI4 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1 + slsAOI1;
                    decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2 + slsAOI2;
                    decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3 + slsAOI3;
                    decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4 + slsAOI4;

                    decimal percentAOI1 = 0;
                    decimal percentAOI2 = 0;
                    decimal percentAOI3 = 0;
                    decimal percentAOI4 = 0;

                    if (totCFR > 0)
                    {
                        percentAOI1 = Math.Round((totalAOI1 * 10000) / totCFR) / 100;
                        percentAOI2 = Math.Round((totalAOI2 * 10000) / totCFR) / 100;
                        percentAOI3 = Math.Round((totalAOI3 * 10000) / totCFR) / 100;
                        percentAOI4 = Math.Round((totalAOI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentAOI1 = percentAOI1;
                    ViewBag.PercentAOI2 = percentAOI2;
                    ViewBag.PercentAOI3 = percentAOI3;
                    ViewBag.PercentAOI4 = percentAOI4;

                    ViewBag.StartDate = start.Value.ToString("d");
                    ViewBag.EndDate = end.Value.ToString("d");
                    if (SiteID == 1)
                    {
                        ViewBag.Site = "Greensboro";
                    }
                    if (SiteID == 2)
                    {
                        ViewBag.Site = "Wichita";
                    }
                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName", SiteID);
                    ViewBag.Phrase = 1;
                }

                else if ((start == null || end == null) && SiteID != null)
                {
                    var mtgCFR = domain.CFRMortgages.Where(d => d.Employee.SiteID == SiteID).Count();
                    var insCFR = domain.CFRInsurances.Where(d => d.Employee1.SiteID == SiteID).Count();
                    var prCFR = domain.CFRPatientRecruitments.Where(d => d.Employee1.SiteID == SiteID).Count();
                    var slsCFR = domain.CFRSales.Where(d => d.Employee.SiteID == SiteID).Count();
                    decimal totCFR = mtgCFR + insCFR + prCFR + slsCFR;

                    var mtgCFRs = domain.CFRMortgages.Where(d => d.Employee.SiteID == SiteID);
                    var insCFRs = domain.CFRInsurances.Where(d => d.Employee1.SiteID == SiteID);
                    var prCFRs = domain.CFRPatientRecruitments.Where(d => d.Employee1.SiteID == SiteID);
                    var slsCFRs = domain.CFRSales.Where(d => d.Employee.SiteID == SiteID);

                    var totCalls = 0;
                    if (totCFR > 0)
                    {
                        foreach (var call in mtgCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in insCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in prCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                        foreach (var call in slsCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Telephone Etiquette % calculation
                    var mtgTE1 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var mtgTE2 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var mtgTE3 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var mtgTE4 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var insTE1 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var insTE2 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var insTE3 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var insTE4 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var prTE1 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var prTE2 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var prTE3 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var prTE4 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var slsTE1 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var slsTE2 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var slsTE3 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var slsTE4 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    decimal totalTE1 = mtgTE1 + insTE1 + prTE1 + slsTE1;
                    decimal totalTE2 = mtgTE2 + insTE2 + prTE2 + slsTE2;
                    decimal totalTE3 = mtgTE3 + insTE3 + prTE3 + slsTE3;
                    decimal totalTE4 = mtgTE4 + insTE4 + prTE4 + slsTE4;

                    decimal percentTE1 = 0;
                    decimal percentTE2 = 0;
                    decimal percentTE3 = 0;
                    decimal percentTE4 = 0;

                    if (totCFR > 0)
                    {
                        percentTE1 = Math.Round((totalTE1 * 10000) / totCFR) / 100;
                        percentTE2 = Math.Round((totalTE2 * 10000) / totCFR) / 100;
                        percentTE3 = Math.Round((totalTE3 * 10000) / totCFR) / 100;
                        percentTE4 = Math.Round((totalTE4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentTE1 = percentTE1;
                    ViewBag.PercentTE2 = percentTE2;
                    ViewBag.PercentTE3 = percentTE3;
                    ViewBag.PercentTE4 = percentTE4;

                    //Professionalism % calculation
                    var mtgP1 = mtgCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var mtgP2 = mtgCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var mtgP3 = mtgCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var mtgP4 = mtgCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var insP1 = insCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var insP2 = insCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var insP3 = insCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var insP4 = insCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var prP1 = prCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var prP2 = prCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var prP3 = prCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var prP4 = prCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var slsP1 = slsCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var slsP2 = slsCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var slsP3 = slsCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var slsP4 = slsCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    decimal totalP1 = mtgP1 + insP1 + prP1 + slsP1;
                    decimal totalP2 = mtgP2 + insP2 + prP2 + slsP2;
                    decimal totalP3 = mtgP3 + insP3 + prP3 + slsP3;
                    decimal totalP4 = mtgP4 + insP4 + prP4 + slsP4;

                    decimal percentP1 = 0;
                    decimal percentP2 = 0;
                    decimal percentP3 = 0;
                    decimal percentP4 = 0;

                    if (totCFR > 0)
                    {
                        percentP1 = Math.Round((totalP1 * 10000) / totCFR) / 100;
                        percentP2 = Math.Round((totalP2 * 10000) / totCFR) / 100;
                        percentP3 = Math.Round((totalP3 * 10000) / totCFR) / 100;
                        percentP4 = Math.Round((totalP4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentP1 = percentP1;
                    ViewBag.PercentP2 = percentP2;
                    ViewBag.PercentP3 = percentP3;
                    ViewBag.PercentP4 = percentP4;

                    //Compliance % calculation
                    var mtgC1 = mtgCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var mtgC2 = mtgCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var mtgC3 = mtgCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var mtgC4 = mtgCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var insC1 = insCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var insC2 = insCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var insC3 = insCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var insC4 = insCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var prC1 = prCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var prC2 = prCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var prC3 = prCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var prC4 = prCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var slsC1 = slsCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var slsC2 = slsCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var slsC3 = slsCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var slsC4 = slsCFRs.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalC1 = mtgC1 + insC1 + prC1 + slsC1;
                    decimal totalC2 = mtgC2 + insC2 + prC2 + slsC2;
                    decimal totalC3 = mtgC3 + insC3 + prC3 + slsC3;
                    decimal totalC4 = mtgC4 + insC4 + prC4 + slsC4;

                    decimal percentC1 = 0;
                    decimal percentC2 = 0;
                    decimal percentC3 = 0;
                    decimal percentC4 = 0;

                    if (totCFR > 0)
                    {
                        percentC1 = Math.Round((totalC1 * 10000) / totCFR) / 100;
                        percentC2 = Math.Round((totalC2 * 10000) / totCFR) / 100;
                        percentC3 = Math.Round((totalC3 * 10000) / totCFR) / 100;
                        percentC4 = Math.Round((totalC4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentC1 = percentC1;
                    ViewBag.PercentC2 = percentC2;
                    ViewBag.PercentC3 = percentC3;
                    ViewBag.PercentC4 = percentC4;

                    //Adherence % calculation
                    var mtgA1 = mtgCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var mtgA2 = mtgCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var mtgA3 = mtgCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var mtgA4 = mtgCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var insA1 = insCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var insA2 = insCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var insA3 = insCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var insA4 = insCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var prA1 = prCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var prA2 = prCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var prA3 = prCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var prA4 = prCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var slsA1 = slsCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var slsA2 = slsCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var slsA3 = slsCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var slsA4 = slsCFRs.Where(d => d.AdheranceRating == 4).Count();

                    decimal totalA1 = mtgA1 + insA1 + prA1 + slsA1;
                    decimal totalA2 = mtgA2 + insA2 + prA2 + slsA2;
                    decimal totalA3 = mtgA3 + insA3 + prA3 + slsA3;
                    decimal totalA4 = mtgA4 + insA4 + prA4 + slsA4;

                    decimal percentA1 = 0;
                    decimal percentA2 = 0;
                    decimal percentA3 = 0;
                    decimal percentA4 = 0;

                    if (totCFR > 0)
                    {
                        percentA1 = Math.Round((totalA1 * 10000) / totCFR) / 100;
                        percentA2 = Math.Round((totalA2 * 10000) / totCFR) / 100;
                        percentA3 = Math.Round((totalA3 * 10000) / totCFR) / 100;
                        percentA4 = Math.Round((totalA4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentA1 = percentA1;
                    ViewBag.PercentA2 = percentA2;
                    ViewBag.PercentA3 = percentA3;
                    ViewBag.PercentA4 = percentA4;

                    //Accuracy of Information % calculation
                    var mtgAOI1 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var mtgAOI2 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var mtgAOI3 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var mtgAOI4 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var insAOI1 = insCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var insAOI2 = insCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var insAOI3 = insCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var insAOI4 = insCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var prAOI1 = prCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var prAOI2 = prCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var prAOI3 = prCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var prAOI4 = prCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var slsAOI1 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var slsAOI2 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var slsAOI3 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var slsAOI4 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1 + slsAOI1;
                    decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2 + slsAOI2;
                    decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3 + slsAOI3;
                    decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4 + slsAOI4;

                    decimal percentAOI1 = 0;
                    decimal percentAOI2 = 0;
                    decimal percentAOI3 = 0;
                    decimal percentAOI4 = 0;

                    if (totCFR > 0)
                    {
                        percentAOI1 = Math.Round((totalAOI1 * 10000) / totCFR) / 100;
                        percentAOI2 = Math.Round((totalAOI2 * 10000) / totCFR) / 100;
                        percentAOI3 = Math.Round((totalAOI3 * 10000) / totCFR) / 100;
                        percentAOI4 = Math.Round((totalAOI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentAOI1 = percentAOI1;
                    ViewBag.PercentAOI2 = percentAOI2;
                    ViewBag.PercentAOI3 = percentAOI3;
                    ViewBag.PercentAOI4 = percentAOI4;

                    if (SiteID == 1)
                    {
                        ViewBag.Site = "Greensboro";
                    }
                    if (SiteID == 2)
                    {
                        ViewBag.Site = "Wichita";
                    }
                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName", SiteID);
                    ViewBag.Phrase = 2;
                }

                else if (start != null && end != null && SiteID == null)
                {
                    var mtgCFR = domain.CFRMortgages.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1)).Count();
                    var insCFR = domain.CFRInsurances.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1)).Count();
                    var prCFR = domain.CFRPatientRecruitments.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1)).Count();
                    var slsCFR = domain.CFRSales.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1)).Count();
                    decimal totCFR = mtgCFR + insCFR + prCFR + slsCFR;

                    var mtgCFRs = domain.CFRMortgages.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));
                    var insCFRs = domain.CFRInsurances.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));
                    var prCFRs = domain.CFRPatientRecruitments.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));
                    var slsCFRs = domain.CFRSales.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));

                    var totCalls = 0;
                    if (totCFR > 0)
                    {
                        foreach (var call in mtgCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in insCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in prCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                        foreach (var call in slsCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Telephone Etiquette % calculation
                    var mtgTE1 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var mtgTE2 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var mtgTE3 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var mtgTE4 = mtgCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var insTE1 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var insTE2 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var insTE3 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var insTE4 = insCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var prTE1 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var prTE2 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var prTE3 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var prTE4 = prCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var slsTE1 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var slsTE2 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var slsTE3 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var slsTE4 = slsCFRs.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    decimal totalTE1 = mtgTE1 + insTE1 + prTE1 + slsTE1;
                    decimal totalTE2 = mtgTE2 + insTE2 + prTE2 + slsTE2;
                    decimal totalTE3 = mtgTE3 + insTE3 + prTE3 + slsTE3;
                    decimal totalTE4 = mtgTE4 + insTE4 + prTE4 + slsTE4;

                    decimal percentTE1 = 0;
                    decimal percentTE2 = 0;
                    decimal percentTE3 = 0;
                    decimal percentTE4 = 0;

                    if (totCFR > 0)
                    {
                        percentTE1 = Math.Round((totalTE1 * 10000) / totCFR) / 100;
                        percentTE2 = Math.Round((totalTE2 * 10000) / totCFR) / 100;
                        percentTE3 = Math.Round((totalTE3 * 10000) / totCFR) / 100;
                        percentTE4 = Math.Round((totalTE4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentTE1 = percentTE1;
                    ViewBag.PercentTE2 = percentTE2;
                    ViewBag.PercentTE3 = percentTE3;
                    ViewBag.PercentTE4 = percentTE4;

                    //Professionalism % calculation
                    var mtgP1 = mtgCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var mtgP2 = mtgCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var mtgP3 = mtgCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var mtgP4 = mtgCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var insP1 = insCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var insP2 = insCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var insP3 = insCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var insP4 = insCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var prP1 = prCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var prP2 = prCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var prP3 = prCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var prP4 = prCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    var slsP1 = slsCFRs.Where(d => d.ProfessionalismRating == 1).Count();
                    var slsP2 = slsCFRs.Where(d => d.ProfessionalismRating == 2).Count();
                    var slsP3 = slsCFRs.Where(d => d.ProfessionalismRating == 3).Count();
                    var slsP4 = slsCFRs.Where(d => d.ProfessionalismRating == 4).Count();

                    decimal totalP1 = mtgP1 + insP1 + prP1 + slsP1;
                    decimal totalP2 = mtgP2 + insP2 + prP2 + slsP2;
                    decimal totalP3 = mtgP3 + insP3 + prP3 + slsP3;
                    decimal totalP4 = mtgP4 + insP4 + prP4 + slsP4;

                    decimal percentP1 = 0;
                    decimal percentP2 = 0;
                    decimal percentP3 = 0;
                    decimal percentP4 = 0;

                    if (totCFR > 0)
                    {
                        percentP1 = Math.Round((totalP1 * 10000) / totCFR) / 100;
                        percentP2 = Math.Round((totalP2 * 10000) / totCFR) / 100;
                        percentP3 = Math.Round((totalP3 * 10000) / totCFR) / 100;
                        percentP4 = Math.Round((totalP4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentP1 = percentP1;
                    ViewBag.PercentP2 = percentP2;
                    ViewBag.PercentP3 = percentP3;
                    ViewBag.PercentP4 = percentP4;

                    //Compliance % calculation
                    var mtgC1 = mtgCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var mtgC2 = mtgCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var mtgC3 = mtgCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var mtgC4 = mtgCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var insC1 = insCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var insC2 = insCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var insC3 = insCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var insC4 = insCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var prC1 = prCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var prC2 = prCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var prC3 = prCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var prC4 = prCFRs.Where(d => d.ComplianceRating == 4).Count();

                    var slsC1 = slsCFRs.Where(d => d.ComplianceRating == 1).Count();
                    var slsC2 = slsCFRs.Where(d => d.ComplianceRating == 2).Count();
                    var slsC3 = slsCFRs.Where(d => d.ComplianceRating == 3).Count();
                    var slsC4 = slsCFRs.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalC1 = mtgC1 + insC1 + prC1 + slsC1;
                    decimal totalC2 = mtgC2 + insC2 + prC2 + slsC2;
                    decimal totalC3 = mtgC3 + insC3 + prC3 + slsC3;
                    decimal totalC4 = mtgC4 + insC4 + prC4 + slsC4;

                    decimal percentC1 = 0;
                    decimal percentC2 = 0;
                    decimal percentC3 = 0;
                    decimal percentC4 = 0;

                    if (totCFR > 0)
                    {
                        percentC1 = Math.Round((totalC1 * 10000) / totCFR) / 100;
                        percentC2 = Math.Round((totalC2 * 10000) / totCFR) / 100;
                        percentC3 = Math.Round((totalC3 * 10000) / totCFR) / 100;
                        percentC4 = Math.Round((totalC4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentC1 = percentC1;
                    ViewBag.PercentC2 = percentC2;
                    ViewBag.PercentC3 = percentC3;
                    ViewBag.PercentC4 = percentC4;

                    //Adherence % calculation
                    var mtgA1 = mtgCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var mtgA2 = mtgCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var mtgA3 = mtgCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var mtgA4 = mtgCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var insA1 = insCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var insA2 = insCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var insA3 = insCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var insA4 = insCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var prA1 = prCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var prA2 = prCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var prA3 = prCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var prA4 = prCFRs.Where(d => d.AdheranceRating == 4).Count();

                    var slsA1 = slsCFRs.Where(d => d.AdheranceRating == 1).Count();
                    var slsA2 = slsCFRs.Where(d => d.AdheranceRating == 2).Count();
                    var slsA3 = slsCFRs.Where(d => d.AdheranceRating == 3).Count();
                    var slsA4 = slsCFRs.Where(d => d.AdheranceRating == 4).Count();

                    decimal totalA1 = mtgA1 + insA1 + prA1 + slsA1;
                    decimal totalA2 = mtgA2 + insA2 + prA2 + slsA2;
                    decimal totalA3 = mtgA3 + insA3 + prA3 + slsA3;
                    decimal totalA4 = mtgA4 + insA4 + prA4 + slsA4;

                    decimal percentA1 = 0;
                    decimal percentA2 = 0;
                    decimal percentA3 = 0;
                    decimal percentA4 = 0;

                    if (totCFR > 0)
                    {
                        percentA1 = Math.Round((totalA1 * 10000) / totCFR) / 100;
                        percentA2 = Math.Round((totalA2 * 10000) / totCFR) / 100;
                        percentA3 = Math.Round((totalA3 * 10000) / totCFR) / 100;
                        percentA4 = Math.Round((totalA4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentA1 = percentA1;
                    ViewBag.PercentA2 = percentA2;
                    ViewBag.PercentA3 = percentA3;
                    ViewBag.PercentA4 = percentA4;

                    //Accuracy of Information % calculation
                    var mtgAOI1 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var mtgAOI2 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var mtgAOI3 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var mtgAOI4 = mtgCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var insAOI1 = insCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var insAOI2 = insCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var insAOI3 = insCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var insAOI4 = insCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var prAOI1 = prCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var prAOI2 = prCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var prAOI3 = prCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var prAOI4 = prCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var slsAOI1 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var slsAOI2 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var slsAOI3 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var slsAOI4 = slsCFRs.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1 + slsAOI1;
                    decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2 + slsAOI2;
                    decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3 + slsAOI3;
                    decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4 + slsAOI4;

                    decimal percentAOI1 = 0;
                    decimal percentAOI2 = 0;
                    decimal percentAOI3 = 0;
                    decimal percentAOI4 = 0;

                    if (totCFR > 0)
                    {
                        percentAOI1 = Math.Round((totalAOI1 * 10000) / totCFR) / 100;
                        percentAOI2 = Math.Round((totalAOI2 * 10000) / totCFR) / 100;
                        percentAOI3 = Math.Round((totalAOI3 * 10000) / totCFR) / 100;
                        percentAOI4 = Math.Round((totalAOI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentAOI1 = percentAOI1;
                    ViewBag.PercentAOI2 = percentAOI2;
                    ViewBag.PercentAOI3 = percentAOI3;
                    ViewBag.PercentAOI4 = percentAOI4;

                    ViewBag.StartDate = start.Value.ToString("d");
                    ViewBag.EndDate = end.Value.ToString("d");
                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName");
                    ViewBag.Phrase = 3;
                }

                else
                {
                    var mtgCFR = domain.CFRMortgages.Count;
                    var insCFR = domain.CFRInsurances.Count;
                    var prCFR = domain.CFRPatientRecruitments.Count;
                    var slsCFR = domain.CFRSales.Count;
                    decimal totCFR = mtgCFR + insCFR + prCFR + slsCFR;
                    var totCalls = 0;

                    if (totCFR > 0)
                    {
                        var mtgCFRs = domain.CFRMortgages;
                        var insCFRs = domain.CFRInsurances;
                        var prCFRs = domain.CFRPatientRecruitments;
                        var slsCFRs = domain.CFRSales;

                        foreach (var call in mtgCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in insCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }

                        foreach (var call in prCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                        foreach (var call in slsCFRs)
                        {
                            totCalls = totCalls + call.C_Calls;
                        }
                    }

                    ViewBag.TotalCalls = totCalls;

                    //Telephone Etiquette % calculation
                    var mtgTE1 = domain.CFRMortgages.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var mtgTE2 = domain.CFRMortgages.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var mtgTE3 = domain.CFRMortgages.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var mtgTE4 = domain.CFRMortgages.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var insTE1 = domain.CFRInsurances.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var insTE2 = domain.CFRInsurances.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var insTE3 = domain.CFRInsurances.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var insTE4 = domain.CFRInsurances.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var prTE1 = domain.CFRPatientRecruitments.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var prTE2 = domain.CFRPatientRecruitments.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var prTE3 = domain.CFRPatientRecruitments.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var prTE4 = domain.CFRPatientRecruitments.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    var slsTE1 = domain.CFRSales.Where(d => d.TelephoneEtiquetteRating == 1).Count();
                    var slsTE2 = domain.CFRSales.Where(d => d.TelephoneEtiquetteRating == 2).Count();
                    var slsTE3 = domain.CFRSales.Where(d => d.TelephoneEtiquetteRating == 3).Count();
                    var slsTE4 = domain.CFRSales.Where(d => d.TelephoneEtiquetteRating == 4).Count();

                    decimal totalTE1 = mtgTE1 + insTE1 + prTE1 + slsTE1;
                    decimal totalTE2 = mtgTE2 + insTE2 + prTE2 + slsTE2;
                    decimal totalTE3 = mtgTE3 + insTE3 + prTE3 + slsTE3;
                    decimal totalTE4 = mtgTE4 + insTE4 + prTE4 + slsTE4;

                    decimal percentTE1 = 0;
                    decimal percentTE2 = 0;
                    decimal percentTE3 = 0;
                    decimal percentTE4 = 0;

                    if (totCFR > 0)
                    {
                        percentTE1 = Math.Round((totalTE1 * 10000) / totCFR) / 100;
                        percentTE2 = Math.Round((totalTE2 * 10000) / totCFR) / 100;
                        percentTE3 = Math.Round((totalTE3 * 10000) / totCFR) / 100;
                        percentTE4 = Math.Round((totalTE4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentTE1 = percentTE1;
                    ViewBag.PercentTE2 = percentTE2;
                    ViewBag.PercentTE3 = percentTE3;
                    ViewBag.PercentTE4 = percentTE4;

                    //Professionalism % calculation
                    var mtgP1 = domain.CFRMortgages.Where(d => d.ProfessionalismRating == 1).Count();
                    var mtgP2 = domain.CFRMortgages.Where(d => d.ProfessionalismRating == 2).Count();
                    var mtgP3 = domain.CFRMortgages.Where(d => d.ProfessionalismRating == 3).Count();
                    var mtgP4 = domain.CFRMortgages.Where(d => d.ProfessionalismRating == 4).Count();

                    var insP1 = domain.CFRInsurances.Where(d => d.ProfessionalismRating == 1).Count();
                    var insP2 = domain.CFRInsurances.Where(d => d.ProfessionalismRating == 2).Count();
                    var insP3 = domain.CFRInsurances.Where(d => d.ProfessionalismRating == 3).Count();
                    var insP4 = domain.CFRInsurances.Where(d => d.ProfessionalismRating == 4).Count();

                    var prP1 = domain.CFRPatientRecruitments.Where(d => d.ProfessionalismRating == 1).Count();
                    var prP2 = domain.CFRPatientRecruitments.Where(d => d.ProfessionalismRating == 2).Count();
                    var prP3 = domain.CFRPatientRecruitments.Where(d => d.ProfessionalismRating == 3).Count();
                    var prP4 = domain.CFRPatientRecruitments.Where(d => d.ProfessionalismRating == 4).Count();

                    var slsP1 = domain.CFRSales.Where(d => d.ProfessionalismRating == 1).Count();
                    var slsP2 = domain.CFRSales.Where(d => d.ProfessionalismRating == 2).Count();
                    var slsP3 = domain.CFRSales.Where(d => d.ProfessionalismRating == 3).Count();
                    var slsP4 = domain.CFRSales.Where(d => d.ProfessionalismRating == 4).Count();

                    decimal totalP1 = mtgP1 + insP1 + prP1 + slsP1;
                    decimal totalP2 = mtgP2 + insP2 + prP2 + slsP2;
                    decimal totalP3 = mtgP3 + insP3 + prP3 + slsP3;
                    decimal totalP4 = mtgP4 + insP4 + prP4 + slsP4;

                    decimal percentP1 = 0;
                    decimal percentP2 = 0;
                    decimal percentP3 = 0;
                    decimal percentP4 = 0;

                    if (totCFR > 0)
                    {
                        percentP1 = Math.Round((totalP1 * 10000) / totCFR) / 100;
                        percentP2 = Math.Round((totalP2 * 10000) / totCFR) / 100;
                        percentP3 = Math.Round((totalP3 * 10000) / totCFR) / 100;
                        percentP4 = Math.Round((totalP4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentP1 = percentP1;
                    ViewBag.PercentP2 = percentP2;
                    ViewBag.PercentP3 = percentP3;
                    ViewBag.PercentP4 = percentP4;

                    //Compliance % calculation
                    var mtgC1 = domain.CFRMortgages.Where(d => d.ComplianceRating == 1).Count();
                    var mtgC2 = domain.CFRMortgages.Where(d => d.ComplianceRating == 2).Count();
                    var mtgC3 = domain.CFRMortgages.Where(d => d.ComplianceRating == 3).Count();
                    var mtgC4 = domain.CFRMortgages.Where(d => d.ComplianceRating == 4).Count();

                    var insC1 = domain.CFRInsurances.Where(d => d.ComplianceRating == 1).Count();
                    var insC2 = domain.CFRInsurances.Where(d => d.ComplianceRating == 2).Count();
                    var insC3 = domain.CFRInsurances.Where(d => d.ComplianceRating == 3).Count();
                    var insC4 = domain.CFRInsurances.Where(d => d.ComplianceRating == 4).Count();

                    var prC1 = domain.CFRPatientRecruitments.Where(d => d.ComplianceRating == 1).Count();
                    var prC2 = domain.CFRPatientRecruitments.Where(d => d.ComplianceRating == 2).Count();
                    var prC3 = domain.CFRPatientRecruitments.Where(d => d.ComplianceRating == 3).Count();
                    var prC4 = domain.CFRPatientRecruitments.Where(d => d.ComplianceRating == 4).Count();

                    var slsC1 = domain.CFRSales.Where(d => d.ComplianceRating == 1).Count();
                    var slsC2 = domain.CFRSales.Where(d => d.ComplianceRating == 2).Count();
                    var slsC3 = domain.CFRSales.Where(d => d.ComplianceRating == 3).Count();
                    var slsC4 = domain.CFRSales.Where(d => d.ComplianceRating == 4).Count();

                    decimal totalC1 = mtgC1 + insC1 + prC1 + slsC1;
                    decimal totalC2 = mtgC2 + insC2 + prC2 + slsC2;
                    decimal totalC3 = mtgC3 + insC3 + prC3 + slsC3;
                    decimal totalC4 = mtgC4 + insC4 + prC4 + slsC4;

                    decimal percentC1 = 0;
                    decimal percentC2 = 0;
                    decimal percentC3 = 0;
                    decimal percentC4 = 0;

                    if (totCFR > 0)
                    {
                        percentC1 = Math.Round((totalC1 * 10000) / totCFR) / 100;
                        percentC2 = Math.Round((totalC2 * 10000) / totCFR) / 100;
                        percentC3 = Math.Round((totalC3 * 10000) / totCFR) / 100;
                        percentC4 = Math.Round((totalC4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentC1 = percentC1;
                    ViewBag.PercentC2 = percentC2;
                    ViewBag.PercentC3 = percentC3;
                    ViewBag.PercentC4 = percentC4;

                    //Adherence % calculation
                    var mtgA1 = domain.CFRMortgages.Where(d => d.AdheranceRating == 1).Count();
                    var mtgA2 = domain.CFRMortgages.Where(d => d.AdheranceRating == 2).Count();
                    var mtgA3 = domain.CFRMortgages.Where(d => d.AdheranceRating == 3).Count();
                    var mtgA4 = domain.CFRMortgages.Where(d => d.AdheranceRating == 4).Count();

                    var insA1 = domain.CFRInsurances.Where(d => d.AdheranceRating == 1).Count();
                    var insA2 = domain.CFRInsurances.Where(d => d.AdheranceRating == 2).Count();
                    var insA3 = domain.CFRInsurances.Where(d => d.AdheranceRating == 3).Count();
                    var insA4 = domain.CFRInsurances.Where(d => d.AdheranceRating == 4).Count();

                    var prA1 = domain.CFRPatientRecruitments.Where(d => d.AdheranceRating == 1).Count();
                    var prA2 = domain.CFRPatientRecruitments.Where(d => d.AdheranceRating == 2).Count();
                    var prA3 = domain.CFRPatientRecruitments.Where(d => d.AdheranceRating == 3).Count();
                    var prA4 = domain.CFRPatientRecruitments.Where(d => d.AdheranceRating == 4).Count();

                    var slsA1 = domain.CFRSales.Where(d => d.AdheranceRating == 1).Count();
                    var slsA2 = domain.CFRSales.Where(d => d.AdheranceRating == 2).Count();
                    var slsA3 = domain.CFRSales.Where(d => d.AdheranceRating == 3).Count();
                    var slsA4 = domain.CFRSales.Where(d => d.AdheranceRating == 4).Count();

                    decimal totalA1 = mtgA1 + insA1 + prA1 + slsA1;
                    decimal totalA2 = mtgA2 + insA2 + prA2 + slsA2;
                    decimal totalA3 = mtgA3 + insA3 + prA3 + slsA3;
                    decimal totalA4 = mtgA4 + insA4 + prA4 + slsA4;

                    decimal percentA1 = 0;
                    decimal percentA2 = 0;
                    decimal percentA3 = 0;
                    decimal percentA4 = 0;

                    if (totCFR > 0)
                    {
                        percentA1 = Math.Round((totalA1 * 10000) / totCFR) / 100;
                        percentA2 = Math.Round((totalA2 * 10000) / totCFR) / 100;
                        percentA3 = Math.Round((totalA3 * 10000) / totCFR) / 100;
                        percentA4 = Math.Round((totalA4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentA1 = percentA1;
                    ViewBag.PercentA2 = percentA2;
                    ViewBag.PercentA3 = percentA3;
                    ViewBag.PercentA4 = percentA4;

                    //Accuracy of Information % calculation
                    var mtgAOI1 = domain.CFRMortgages.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var mtgAOI2 = domain.CFRMortgages.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var mtgAOI3 = domain.CFRMortgages.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var mtgAOI4 = domain.CFRMortgages.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var insAOI1 = domain.CFRInsurances.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var insAOI2 = domain.CFRInsurances.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var insAOI3 = domain.CFRInsurances.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var insAOI4 = domain.CFRInsurances.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var prAOI1 = domain.CFRPatientRecruitments.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var prAOI2 = domain.CFRPatientRecruitments.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var prAOI3 = domain.CFRPatientRecruitments.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var prAOI4 = domain.CFRPatientRecruitments.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    var slsAOI1 = domain.CFRSales.Where(d => d.AccuracyOfInformationRating == 1).Count();
                    var slsAOI2 = domain.CFRSales.Where(d => d.AccuracyOfInformationRating == 2).Count();
                    var slsAOI3 = domain.CFRSales.Where(d => d.AccuracyOfInformationRating == 3).Count();
                    var slsAOI4 = domain.CFRSales.Where(d => d.AccuracyOfInformationRating == 4).Count();

                    decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1 + slsAOI1;
                    decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2 + slsAOI2;
                    decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3 + slsAOI3;
                    decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4 + slsAOI4;

                    decimal percentAOI1 = 0;
                    decimal percentAOI2 = 0;
                    decimal percentAOI3 = 0;
                    decimal percentAOI4 = 0;

                    if (totCFR > 0)
                    {
                        percentAOI1 = Math.Round((totalAOI1 * 10000) / totCFR) / 100;
                        percentAOI2 = Math.Round((totalAOI2 * 10000) / totCFR) / 100;
                        percentAOI3 = Math.Round((totalAOI3 * 10000) / totCFR) / 100;
                        percentAOI4 = Math.Round((totalAOI4 * 10000) / totCFR) / 100;
                    }

                    ViewBag.PercentAOI1 = percentAOI1;
                    ViewBag.PercentAOI2 = percentAOI2;
                    ViewBag.PercentAOI3 = percentAOI3;
                    ViewBag.PercentAOI4 = percentAOI4;

                    ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName");
                }
            }

           
            return View();
        }

        // GET: Quality/Evals
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult EvaluationCount()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var now = System.DateTime.Now;
            var todayYear = now.Year;
            var todayMonth = now.Month;
            var todayDay = now.Day;

            var employees = new List<Emp>();
            foreach (var employee in mb.Employees.Where(e => e.IsActive == true && (e.PositionID == 3)).OrderBy(e => e.FirstName))
            {
                var empMtgCFRs = mb.CFRMortgages.Where(c => c.EmployeeID == employee.EmployeeID && c.DateOfFeedback.Year == todayYear && c.DateOfFeedback.Month == todayMonth).Count();
                var empInsCFRs = mb.CFRInsurances.Where(c => c.EmployeeID == employee.EmployeeID && c.DateOfFeedback.Year == todayYear && c.DateOfFeedback.Month == todayMonth).Count();
                var empSlsCFRs = mb.CFRSales.Where(c => c.EmployeeID == employee.EmployeeID && c.DateOfFeedback.Year == todayYear && c.DateOfFeedback.Month == todayMonth).Count();
                var empPatRecCFRs = mb.CFRPatientRecruitments.Where(c => c.EmployeeID == employee.EmployeeID && c.DateOfFeedback.Year == todayYear && c.DateOfFeedback.Month == todayMonth).Count();
                var empAccCFRs = mb.CFRAcurians.Where(c => c.EmployeeID == employee.EmployeeID && c.DateOfFeedback.Year == todayYear && c.DateOfFeedback.Month == todayMonth).Count();
                var daysHere = (now - employee.HireDate).Days;
                var item = new Emp();
                item.EmployeeID = employee.EmployeeID;
                item.FullName = employee.FirstName + " " + employee.LastName;
                item.SiteID = employee.SiteID;
                item.TotalCFRsMonth = empMtgCFRs + empInsCFRs + empPatRecCFRs + empSlsCFRs + empAccCFRs;
                item.DaysHere = daysHere;
                var remaining = 0;
                if (item.TotalCFRsMonth >= 8)
                {
                    remaining = 0;
                }
                else
                {
                    remaining = 8 - item.TotalCFRsMonth;
                }
                item.RemainingCFRsNeeded = remaining;

                employees.Add(item);
            }
            if (user.SiteID == 1)
            {
                ViewBag.UserLocation = "greensboro";
                ViewBag.OtherLocation = "wichita";
            }
            else if (user.SiteID == 2)
            {
                ViewBag.UserLocation = "wichita";
                ViewBag.OtherLocation = "greensboro";
            }
            
            ViewBag.G_EmpsNeedingMoreEvals = employees.Where(e => e.SiteID == 1 && e.TotalCFRsMonth < 8).OrderByDescending(e => e.RemainingCFRsNeeded).ToList();
            ViewBag.W_EmpsNeedingMoreEvals = employees.Where(e => e.SiteID == 2 && e.TotalCFRsMonth < 8).OrderByDescending(e => e.RemainingCFRsNeeded).ToList();
            return View();
        }

        // GET: Quality/CallCriteria
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult CallCriteria()
        {           
            return View();
        }
    }
}