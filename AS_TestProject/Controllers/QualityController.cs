using System;
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
        public ActionResult Index()
        {
            ViewBag.Domains = mb.DomainMasters.Where(d => d.IsActive == true && d.DomainMasterID != 21 && d.DomainMasterID != 28).OrderBy(d => d.FileMask).ToList();
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

            if (start != null && end != null && SiteID != null)
            {
                var mtgCFR = domain.CFRMortgages.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID).Count();
                var insCFR = domain.CFRInsurances.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID).Count();
                var prCFR = domain.CFRPatientRecruitments.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID).Count();
                decimal totCFR = mtgCFR + insCFR + prCFR;

                var mtgCFRs = domain.CFRMortgages.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee.SiteID == SiteID);
                var insCFRs = domain.CFRInsurances.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID);
                var prCFRs = domain.CFRPatientRecruitments.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1) && d.Employee1.SiteID == SiteID);

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

                decimal totalTE1 = mtgTE1 + insTE1 + prTE1;
                decimal totalTE2 = mtgTE2 + insTE2 + prTE2;
                decimal totalTE3 = mtgTE3 + insTE3 + prTE3;
                decimal totalTE4 = mtgTE4 + insTE4 + prTE4;

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

                decimal totalP1 = mtgP1 + insP1 + prP1;
                decimal totalP2 = mtgP2 + insP2 + prP2;
                decimal totalP3 = mtgP3 + insP3 + prP3;
                decimal totalP4 = mtgP4 + insP4 + prP4;

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

                decimal totalC1 = mtgC1 + insC1 + prC1;
                decimal totalC2 = mtgC2 + insC2 + prC2;
                decimal totalC3 = mtgC3 + insC3 + prC3;
                decimal totalC4 = mtgC4 + insC4 + prC4;

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

                decimal totalA1 = mtgA1 + insA1 + prA1;
                decimal totalA2 = mtgA2 + insA2 + prA2;
                decimal totalA3 = mtgA3 + insA3 + prA3;
                decimal totalA4 = mtgA4 + insA4 + prA4;

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

                decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1;
                decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2;
                decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3;
                decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4;

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
                decimal totCFR = mtgCFR + insCFR + prCFR;

                var mtgCFRs = domain.CFRMortgages.Where(d => d.Employee.SiteID == SiteID);
                var insCFRs = domain.CFRInsurances.Where(d => d.Employee1.SiteID == SiteID);
                var prCFRs = domain.CFRPatientRecruitments.Where(d => d.Employee1.SiteID == SiteID);

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

                decimal totalTE1 = mtgTE1 + insTE1 + prTE1;
                decimal totalTE2 = mtgTE2 + insTE2 + prTE2;
                decimal totalTE3 = mtgTE3 + insTE3 + prTE3;
                decimal totalTE4 = mtgTE4 + insTE4 + prTE4;

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

                decimal totalP1 = mtgP1 + insP1 + prP1;
                decimal totalP2 = mtgP2 + insP2 + prP2;
                decimal totalP3 = mtgP3 + insP3 + prP3;
                decimal totalP4 = mtgP4 + insP4 + prP4;

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

                decimal totalC1 = mtgC1 + insC1 + prC1;
                decimal totalC2 = mtgC2 + insC2 + prC2;
                decimal totalC3 = mtgC3 + insC3 + prC3;
                decimal totalC4 = mtgC4 + insC4 + prC4;

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

                decimal totalA1 = mtgA1 + insA1 + prA1;
                decimal totalA2 = mtgA2 + insA2 + prA2;
                decimal totalA3 = mtgA3 + insA3 + prA3;
                decimal totalA4 = mtgA4 + insA4 + prA4;

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

                decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1;
                decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2;
                decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3;
                decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4;

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
                decimal totCFR = mtgCFR + insCFR + prCFR;

                var mtgCFRs = domain.CFRMortgages.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));
                var insCFRs = domain.CFRInsurances.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));
                var prCFRs = domain.CFRPatientRecruitments.Where(d => d.DateOfFeedback >= start.Value && d.DateOfFeedback < end.Value.AddDays(1));

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

                decimal totalTE1 = mtgTE1 + insTE1 + prTE1;
                decimal totalTE2 = mtgTE2 + insTE2 + prTE2;
                decimal totalTE3 = mtgTE3 + insTE3 + prTE3;
                decimal totalTE4 = mtgTE4 + insTE4 + prTE4;

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

                decimal totalP1 = mtgP1 + insP1 + prP1;
                decimal totalP2 = mtgP2 + insP2 + prP2;
                decimal totalP3 = mtgP3 + insP3 + prP3;
                decimal totalP4 = mtgP4 + insP4 + prP4;

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

                decimal totalC1 = mtgC1 + insC1 + prC1;
                decimal totalC2 = mtgC2 + insC2 + prC2;
                decimal totalC3 = mtgC3 + insC3 + prC3;
                decimal totalC4 = mtgC4 + insC4 + prC4;

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

                decimal totalA1 = mtgA1 + insA1 + prA1;
                decimal totalA2 = mtgA2 + insA2 + prA2;
                decimal totalA3 = mtgA3 + insA3 + prA3;
                decimal totalA4 = mtgA4 + insA4 + prA4;

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

                decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1;
                decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2;
                decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3;
                decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4;

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
                decimal totCFR = mtgCFR + insCFR + prCFR;
                var totCalls = 0;

                if (totCFR > 0)
                {
                    var mtgCFRs = domain.CFRMortgages;
                    var insCFRs = domain.CFRInsurances;
                    var prCFRs = domain.CFRPatientRecruitments;

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

                decimal totalTE1 = mtgTE1 + insTE1 + prTE1;
                decimal totalTE2 = mtgTE2 + insTE2 + prTE2;
                decimal totalTE3 = mtgTE3 + insTE3 + prTE3;
                decimal totalTE4 = mtgTE4 + insTE4 + prTE4;

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

                decimal totalP1 = mtgP1 + insP1 + prP1;
                decimal totalP2 = mtgP2 + insP2 + prP2;
                decimal totalP3 = mtgP3 + insP3 + prP3;
                decimal totalP4 = mtgP4 + insP4 + prP4;

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

                decimal totalC1 = mtgC1 + insC1 + prC1;
                decimal totalC2 = mtgC2 + insC2 + prC2;
                decimal totalC3 = mtgC3 + insC3 + prC3;
                decimal totalC4 = mtgC4 + insC4 + prC4;

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

                decimal totalA1 = mtgA1 + insA1 + prA1;
                decimal totalA2 = mtgA2 + insA2 + prA2;
                decimal totalA3 = mtgA3 + insA3 + prA3;
                decimal totalA4 = mtgA4 + insA4 + prA4;

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

                decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1;
                decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2;
                decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3;
                decimal totalAOI4 = mtgAOI4 + insAOI4 + prAOI4;

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
           
            return View();
        }

        // GET: Quality/Index
        [Authorize(Roles = "Admin, Quality")]
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
            decimal mA1no = mb.CFRMortgages.Where(c => c.mAQ1== 2).Count();
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
                var empPatRecCFRs = mb.CFRPatientRecruitments.Where(c => c.EmployeeID == employee.EmployeeID && c.DateOfFeedback.Year == todayYear && c.DateOfFeedback.Month == todayMonth).Count();
                var daysHere = (now - employee.HireDate).Days;
                var item = new Emp();
                item.EmployeeID = employee.EmployeeID;
                item.FullName = employee.FirstName + " " + employee.LastName;
                item.SiteID = employee.SiteID;
                item.TotalCFRsMonth = empMtgCFRs + empInsCFRs + empPatRecCFRs;
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