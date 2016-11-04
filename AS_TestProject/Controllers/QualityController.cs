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

        // GET: Quality/Index

        public ActionResult Index()
        {
            ViewBag.Domains = mb.DomainMasters.Where(d => d.IsActive == true).ToList();
            return View();
        }

        // GET: Quality/Details
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult Details(int? id)
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

            ViewBag.Domain = domain.FileMask + " - " + domain.DomainName;
         
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

            var insTE1 = domain.CFRInsurances.Where(d => d.TelephoneEtiquetteRating == 1).Count();
            var insTE2 = domain.CFRInsurances.Where(d => d.TelephoneEtiquetteRating == 2).Count();
            var insTE3 = domain.CFRInsurances.Where(d => d.TelephoneEtiquetteRating == 3).Count();

            var prTE1 = domain.CFRPatientRecruitments.Where(d => d.TelephoneEtiquetteRating == 1).Count();
            var prTE2 = domain.CFRPatientRecruitments.Where(d => d.TelephoneEtiquetteRating == 2).Count();
            var prTE3 = domain.CFRPatientRecruitments.Where(d => d.TelephoneEtiquetteRating == 3).Count();

            decimal totalTE1 = mtgTE1 + insTE1 + prTE1;
            decimal totalTE2 = mtgTE2 + insTE2 + prTE2;
            decimal totalTE3 = mtgTE3 + insTE3 + prTE3;

            decimal percentTE1 = 0;
            decimal percentTE2 = 0;
            decimal percentTE3 = 0;

            if (totCFR > 0)
            {
                percentTE1 = Math.Round((totalTE1 * 10000) / totCFR) / 100;
                percentTE2 = Math.Round((totalTE2 * 10000) / totCFR) / 100;
                percentTE3 = Math.Round((totalTE3 * 10000) / totCFR) / 100;
            }
            
            ViewBag.PercentTE1 = percentTE1;
            ViewBag.PercentTE2 = percentTE2;
            ViewBag.PercentTE3 = percentTE3;

            //Professionalism % calculation
            var mtgP1 = domain.CFRMortgages.Where(d => d.ProfessionalismRating == 1).Count();
            var mtgP2 = domain.CFRMortgages.Where(d => d.ProfessionalismRating == 2).Count();
            var mtgP3 = domain.CFRMortgages.Where(d => d.ProfessionalismRating == 3).Count();

            var insP1 = domain.CFRInsurances.Where(d => d.ProfessionalismRating == 1).Count();
            var insP2 = domain.CFRInsurances.Where(d => d.ProfessionalismRating == 2).Count();
            var insP3 = domain.CFRInsurances.Where(d => d.ProfessionalismRating == 3).Count();

            var prP1 = domain.CFRPatientRecruitments.Where(d => d.ProfessionalismRating == 1).Count();
            var prP2 = domain.CFRPatientRecruitments.Where(d => d.ProfessionalismRating == 2).Count();
            var prP3 = domain.CFRPatientRecruitments.Where(d => d.ProfessionalismRating == 3).Count();

            decimal totalP1 = mtgP1 + insP1 + prP1;
            decimal totalP2 = mtgP2 + insP2 + prP2;
            decimal totalP3 = mtgP3 + insP3 + prP3;

            decimal percentP1 = 0;
            decimal percentP2 = 0;
            decimal percentP3 = 0;

            if (totCFR > 0)
            {
                percentP1 = Math.Round((totalP1 * 10000) / totCFR) / 100;
                percentP2 = Math.Round((totalP2 * 10000) / totCFR) / 100;
                percentP3 = Math.Round((totalP3 * 10000) / totCFR) / 100;
            }
            
            ViewBag.PercentP1 = percentP1;
            ViewBag.PercentP2 = percentP2;
            ViewBag.PercentP3 = percentP3;

            //Compliance % calculation
            var mtgC1 = domain.CFRMortgages.Where(d => d.ComplianceRating == 1).Count();
            var mtgC2 = domain.CFRMortgages.Where(d => d.ComplianceRating == 2).Count();
            var mtgC3 = domain.CFRMortgages.Where(d => d.ComplianceRating == 3).Count();

            var insC1 = domain.CFRInsurances.Where(d => d.ComplianceRating == 1).Count();
            var insC2 = domain.CFRInsurances.Where(d => d.ComplianceRating == 2).Count();
            var insC3 = domain.CFRInsurances.Where(d => d.ComplianceRating == 3).Count();

            var prC1 = domain.CFRPatientRecruitments.Where(d => d.ComplianceRating == 1).Count();
            var prC2 = domain.CFRPatientRecruitments.Where(d => d.ComplianceRating == 2).Count();
            var prC3 = domain.CFRPatientRecruitments.Where(d => d.ComplianceRating == 3).Count();

            decimal totalC1 = mtgC1 + insC1 + prC1;
            decimal totalC2 = mtgC2 + insC2 + prC2;
            decimal totalC3 = mtgC3 + insC3 + prC3;

            decimal percentC1 = 0;
            decimal percentC2 = 0;
            decimal percentC3 = 0;

            if (totCFR > 0)
            {
                percentC1 = Math.Round((totalC1 * 10000) / totCFR) / 100;
                percentC2 = Math.Round((totalC2 * 10000) / totCFR) / 100;
                percentC3 = Math.Round((totalC3 * 10000) / totCFR) / 100;
            }
            
            ViewBag.PercentC1 = percentC1;
            ViewBag.PercentC2 = percentC2;
            ViewBag.PercentC3 = percentC3;

            //Adherence % calculation
            var mtgA1 = domain.CFRMortgages.Where(d => d.AdheranceRating == 1).Count();
            var mtgA2 = domain.CFRMortgages.Where(d => d.AdheranceRating == 2).Count();
            var mtgA3 = domain.CFRMortgages.Where(d => d.AdheranceRating == 3).Count();

            var insA1 = domain.CFRInsurances.Where(d => d.AdheranceRating == 1).Count();
            var insA2 = domain.CFRInsurances.Where(d => d.AdheranceRating == 2).Count();
            var insA3 = domain.CFRInsurances.Where(d => d.AdheranceRating == 3).Count();

            var prA1 = domain.CFRPatientRecruitments.Where(d => d.AdheranceRating == 1).Count();
            var prA2 = domain.CFRPatientRecruitments.Where(d => d.AdheranceRating == 2).Count();
            var prA3 = domain.CFRPatientRecruitments.Where(d => d.AdheranceRating == 3).Count();

            decimal totalA1 = mtgA1 + insA1 + prA1;
            decimal totalA2 = mtgA2 + insA2 + prA2;
            decimal totalA3 = mtgA3 + insA3 + prA3;

            decimal percentA1 = 0;
            decimal percentA2 = 0;
            decimal percentA3 = 0;

            if (totCFR > 0)
            {
                percentA1 = Math.Round((totalA1 * 10000) / totCFR) / 100;
                percentA2 = Math.Round((totalA2 * 10000) / totCFR) / 100;
                percentA3 = Math.Round((totalA3 * 10000) / totCFR) / 100;
            }

            ViewBag.PercentA1 = percentA1;
            ViewBag.PercentA2 = percentA2;
            ViewBag.PercentA3 = percentA3;

            //Accuracy of Information % calculation
            var mtgAOI1 = domain.CFRMortgages.Where(d => d.AccuracyOfInformationRating == 1).Count();
            var mtgAOI2 = domain.CFRMortgages.Where(d => d.AccuracyOfInformationRating == 2).Count();
            var mtgAOI3 = domain.CFRMortgages.Where(d => d.AccuracyOfInformationRating == 3).Count();

            var insAOI1 = domain.CFRInsurances.Where(d => d.AccuracyOfInformationRating == 1).Count();
            var insAOI2 = domain.CFRInsurances.Where(d => d.AccuracyOfInformationRating == 2).Count();
            var insAOI3 = domain.CFRInsurances.Where(d => d.AccuracyOfInformationRating == 3).Count();

            var prAOI1 = domain.CFRPatientRecruitments.Where(d => d.AccuracyOfInformationRating == 1).Count();
            var prAOI2 = domain.CFRPatientRecruitments.Where(d => d.AccuracyOfInformationRating == 2).Count();
            var prAOI3 = domain.CFRPatientRecruitments.Where(d => d.AccuracyOfInformationRating == 3).Count();

            decimal totalAOI1 = mtgAOI1 + insAOI1 + prAOI1;
            decimal totalAOI2 = mtgAOI2 + insAOI2 + prAOI2;
            decimal totalAOI3 = mtgAOI3 + insAOI3 + prAOI3;

            decimal percentAOI1 = 0;
            decimal percentAOI2 = 0;
            decimal percentAOI3 = 0;

            if (totCFR > 0)
            {
                percentAOI1 = Math.Round((totalAOI1 * 10000) / totCFR) / 100;
                percentAOI2 = Math.Round((totalAOI2 * 10000) / totCFR) / 100;
                percentAOI3 = Math.Round((totalAOI3 * 10000) / totCFR) / 100;
            }
            
            ViewBag.PercentAOI1 = percentAOI1;
            ViewBag.PercentAOI2 = percentAOI2;
            ViewBag.PercentAOI3 = percentAOI3;

            return View();
        }

        // GET: Quality/Index
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult CFRStats()
        {
            var mtgCFR = mb.CFRMortgages.Count();
            var insCFR = mb.CFRInsurances.Count();
            var prCFR = mb.CFRPatientRecruitments.Count();

            var mTE1yes = mb.CFRMortgages.Where(c => c.mTEQ1 == 1).Count();
            var mTE1no = mb.CFRMortgages.Where(c => c.mTEQ1 == 2).Count();
            var mTE1na = mb.CFRMortgages.Where(c => c.mTEQ1 == 3).Count();

            var mTE2yes = mb.CFRMortgages.Where(c => c.mTEQ2 == 1).Count();
            var mTE2no = mb.CFRMortgages.Where(c => c.mTEQ2 == 2).Count();
            var mTE2na = mb.CFRMortgages.Where(c => c.mTEQ2 == 3).Count();

            var mTE3yes = mb.CFRMortgages.Where(c => c.mTEQ3 == 1).Count();
            var mTE3no = mb.CFRMortgages.Where(c => c.mTEQ3 == 2).Count();
            var mTE3na = mb.CFRMortgages.Where(c => c.mTEQ3 == 3).Count();

            var mTE4yes = mb.CFRMortgages.Where(c => c.mTEQ4 == 1).Count();
            var mTE4no = mb.CFRMortgages.Where(c => c.mTEQ4 == 2).Count();
            var mTE4na = mb.CFRMortgages.Where(c => c.mTEQ4 == 3).Count();

            var mTE5yes = mb.CFRMortgages.Where(c => c.mTEQ5 == 1).Count();
            var mTE5no = mb.CFRMortgages.Where(c => c.mTEQ5 == 2).Count();
            var mTE5na = mb.CFRMortgages.Where(c => c.mTEQ5 == 3).Count();

            var mP1yes = mb.CFRMortgages.Where(c => c.mPQ1 == 1).Count();
            var mP1no = mb.CFRMortgages.Where(c => c.mPQ1 == 2).Count();
            var mP1na = mb.CFRMortgages.Where(c => c.mPQ1 == 3).Count();

            var mP2yes = mb.CFRMortgages.Where(c => c.mPQ2 == 1).Count();
            var mP2no = mb.CFRMortgages.Where(c => c.mPQ2 == 2).Count();
            var mP2na = mb.CFRMortgages.Where(c => c.mPQ2 == 3).Count();

            var mC1yes = mb.CFRMortgages.Where(c => c.mCQ1 == 1).Count();
            var mC1no = mb.CFRMortgages.Where(c => c.mCQ1 == 2).Count();
            var mC1na = mb.CFRMortgages.Where(c => c.mCQ1 == 3).Count();

            var mC2yes = mb.CFRMortgages.Where(c => c.mCQ2 == 1).Count();
            var mC2no = mb.CFRMortgages.Where(c => c.mCQ2 == 2).Count();
            var mC2na = mb.CFRMortgages.Where(c => c.mCQ2 == 3).Count();


            return View();
        }
    }
}