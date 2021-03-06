﻿using System;
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
using System.IO;

namespace AS_TestProject.Controllers
{
    public class EmployeesController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        public class Domain
        {
            public byte Id { get; set; }
            public string FileMaskPlusName { get; set; }
        }

        public class CompletedCFR
        {
            public int Id { get; set; }
            public string ForEmployee { get; set; }
            public int ForEmployeeID { get; set; }
            public DateTime DateSubmitted { get; set; }
            public string Type { get; set; }
        }

        public class EmployeeCFR
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public double Score { get; set; }
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,EmailAddress,HomePhone,CellPhone,BirthDate,HireDate,TerminationDate,IsManager,ManagerEmployeeID,PositionID,SiteID,IsActive,AddDate,AddByEmployeeID,RehireDate,FileNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                employee.AddByEmployeeID = user.EmployeeID;
                employee.IsActive = true;
                if (employee.AddressLine2 == null)
                {
                    employee.AddressLine2 = "";
                }
                if (employee.HomePhone == null)
                {
                    employee.HomePhone = "";
                }
                if (employee.CellPhone == null)
                {
                    employee.CellPhone = "";
                }
                mb.Employees.Add(employee);
                mb.SaveChanges();
                return RedirectToAction("Directory", "Home");
            }

            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName", employee.SiteID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = mb.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName", employee.SiteID);
            if (employee.EditByEmployeeID != null)
            {
                Employee editor = mb.Employees.Find(employee.EditByEmployeeID);
                ViewBag.Editor = editor.FirstName + " " + editor.LastName;
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,EmailAddress,HomePhone,CellPhone,BirthDate,HireDate,TerminationDate,IsManager,ManagerEmployeeID,PositionID,SiteID,IsActive,AddDate,AddByEmployeeID,RehireDate,FileNumber,EditByEmployeeID,EditTimeStamp")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                mb.Employees.Attach(employee);
                if (employee.AddressLine2 == null)
                {
                    employee.AddressLine2 = "";
                }
                if (employee.HomePhone == null)
                {
                    employee.HomePhone = "";
                }
                if (employee.CellPhone == null)
                {
                    employee.CellPhone = "";
                }
                var user = db.Users.Find(User.Identity.GetUserId());
                employee.EditByEmployeeID = user.EmployeeID;
                employee.EditTimeStamp = System.DateTime.Now;
                mb.Entry(employee).Property("FirstName").IsModified = true;
                mb.Entry(employee).Property("LastName").IsModified = true;
                mb.Entry(employee).Property("AddressLine1").IsModified = true;
                mb.Entry(employee).Property("AddressLine2").IsModified = true;
                mb.Entry(employee).Property("City").IsModified = true;
                mb.Entry(employee).Property("State").IsModified = true;
                mb.Entry(employee).Property("ZipCode").IsModified = true;
                mb.Entry(employee).Property("EmailAddress").IsModified = true;
                mb.Entry(employee).Property("HomePhone").IsModified = true;
                mb.Entry(employee).Property("CellPhone").IsModified = true;
                mb.Entry(employee).Property("BirthDate").IsModified = true;
                mb.Entry(employee).Property("HireDate").IsModified = true;
                mb.Entry(employee).Property("TerminationDate").IsModified = true;
                mb.Entry(employee).Property("IsManager").IsModified = true;
                mb.Entry(employee).Property("ManagerEmployeeID").IsModified = true;
                mb.Entry(employee).Property("PositionID").IsModified = true;
                mb.Entry(employee).Property("SiteID").IsModified = true;
                mb.Entry(employee).Property("IsActive").IsModified = true;
                mb.Entry(employee).Property("RehireDate").IsModified = true;
                mb.Entry(employee).Property("FileNumber").IsModified = true;
                mb.Entry(employee).Property("EditByEmployeeID").IsModified = true;
                mb.Entry(employee).Property("EditTimeStamp").IsModified = true;
                mb.SaveChanges();
                return RedirectToAction("Directory", "Home");
            }
            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName", employee.SiteID);
            return View(employee);
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Admin, HR, Quality, Operations")]
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee= mb.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true && d.FileMask != "D00").OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName");

            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");

            ViewBag.iTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.iAOIQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");

            ViewBag.pTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pCQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pCQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.pAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");

            ViewBag.sTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sPQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sPQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.sAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");

            ViewBag.aIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aSSQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCOQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCLQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCLQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.aCLQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");

            Employee manager = mb.Employees.Find(employee.ManagerEmployeeID);
            Employee creator = mb.Employees.Find(employee.AddByEmployeeID);
            if (employee.EditByEmployeeID != null)
            {
                Employee editor = mb.Employees.Find(employee.EditByEmployeeID);
                ViewBag.Editor = editor.FirstName + " " + editor.LastName;
            }
            
            ViewBag.Manager = manager.FirstName + " " + manager.LastName;
            ViewBag.Creator = creator.FirstName + " " + creator.LastName;
            ViewBag.Date = System.DateTime.Now.ToShortDateString();
            ViewBag.EmployeeID = employee.EmployeeID;

            ViewBag.DisciplinaryActions = mb.DisciplinaryActions.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(d => d.Date).ToList();
            if (user.Roles.Where(r => r.RoleId == "66282d26-5686-4267-bf05-edad26bd3bcc").Count() == 1 && user.Roles.Where(r => r.RoleId == "cf0c9cdc-c2d7-4abf-9da7-72b5d4245348").Count() == 0)
            {
                ViewBag.eFiles = db.EmployeeFiles.Where(f => f.EmployeeID == employee.EmployeeID && f.AuthorId == user.Id).OrderByDescending(f => f.Created).ToList();
            }
            else
            {
                ViewBag.eFiles = db.EmployeeFiles.Where(f => f.EmployeeID == employee.EmployeeID).OrderByDescending(f => f.Created).ToList();
            }

            //var mCFRs = new List<CFRMortgage>();
            //mCFRs = mb.CFRMortgages.Where(d => d.EmployeeID == employee.EmployeeID).ToList();
            //var iCFRs = new List<CFRInsurance>();
            //iCFRs = mb.CFRInsurances.Where(d => d.EmployeeID == employee.EmployeeID).ToList();
            //var pCFRs = new List<CFRPatientRecruitment>();
            //pCFRs = mb.CFRPatientRecruitments.Where(d => d.EmployeeID == employee.EmployeeID).ToList();
            ////var mCFRs = mb.CFRMortgages.Where(d => d.EmployeeID == employee.EmployeeID).ToList();
            ////var iCFRs = mb.CFRInsurances.Where(d => d.EmployeeID == employee.EmployeeID).ToList();
            ////var pCFRs = mb.CFRPatientRecruitments.Where(d => d.EmployeeID == employee.EmployeeID).ToList();
            //var allCFRs = mCFRs.Concat(iCFRs).Concat(pCFRs).ToList();

            ViewBag.mCFRs = mb.CFRMortgages.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList();
            ViewBag.iCFRs = mb.CFRInsurances.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList();
            ViewBag.pCFRs = mb.CFRPatientRecruitments.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList();
            ViewBag.sCFRs = mb.CFRSales.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList();
            ViewBag.aCFRs = mb.CFRAcurians.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList();
            double mScore = 0;
            double iScore = 0;
            double pScore = 0;
            double sScore = 0;
            double aScore = 0;

            var cfrs = new List<EmployeeCFR>();
            foreach (var cfr in ViewBag.mCFRs)
            {
                var item = new EmployeeCFR();
                item.Type = "Mortgage";
                int n = 0;
                double s = 0;               
                if (cfr.TelephoneEtiquetteRating == 1 || cfr.TelephoneEtiquetteRating == 2 || cfr.TelephoneEtiquetteRating == 3)
                {
                    n++;
                }
                if (cfr.ProfessionalismRating == 1 || cfr.ProfessionalismRating == 2 || cfr.ProfessionalismRating == 3)
                {
                    n++;
                }
                if (cfr.ComplianceRating == 1 || cfr.ComplianceRating == 2 || cfr.ComplianceRating == 3)
                {
                    n++;
                }
                if (cfr.AdheranceRating == 1 || cfr.AdheranceRating == 2 || cfr.AdheranceRating == 3)
                {
                    n++;
                }
                if (cfr.AccuracyOfInformationRating == 1 || cfr.AccuracyOfInformationRating == 2 || cfr.AccuracyOfInformationRating == 3)
                {
                    n++;
                }

                if (n == 0)
                {
                    item.Score = 100;
                    mScore = mScore + item.Score;
                }
                if (n == 1)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    mScore = mScore + item.Score;
                }
                else if (n == 2)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    mScore = mScore + item.Score;
                }
                else if (n == 3)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    mScore = mScore + item.Score;
                }
                else if (n == 4)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    mScore = mScore + item.Score;
                }
                else if (n == 5)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    mScore = mScore + item.Score;
                    cfrs.Add(item);
                }
            }
            foreach (var cfr in ViewBag.iCFRs)
            {
                var item = new EmployeeCFR();
                item.Type = "Insurance";
                int n = 0;
                double s = 0;
                if (cfr.TelephoneEtiquetteRating == 1 || cfr.TelephoneEtiquetteRating == 2 || cfr.TelephoneEtiquetteRating == 3)
                {
                    n++;
                }
                if (cfr.ProfessionalismRating == 1 || cfr.ProfessionalismRating == 2 || cfr.ProfessionalismRating == 3)
                {
                    n++;
                }
                if (cfr.ComplianceRating == 1 || cfr.ComplianceRating == 2 || cfr.ComplianceRating == 3)
                {
                    n++;
                }
                if (cfr.AdheranceRating == 1 || cfr.AdheranceRating == 2 || cfr.AdheranceRating == 3)
                {
                    n++;
                }
                if (cfr.AccuracyOfInformationRating == 1 || cfr.AccuracyOfInformationRating == 2 || cfr.AccuracyOfInformationRating == 3)
                {
                    n++;
                }

                if (n == 0)
                {
                    item.Score = 100;
                    iScore = iScore + item.Score;
                }
                if (n == 1)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    iScore = iScore + item.Score;
                }
                else if (n == 2)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    iScore = iScore + item.Score;
                }
                else if (n == 3)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    iScore = iScore + item.Score;
                }
                else if (n == 4)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    iScore = iScore + item.Score;
                }
                else if (n == 5)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    iScore = iScore + item.Score;
                    cfrs.Add(item);
                }
            }
            foreach (var cfr in ViewBag.pCFRs)
            {
                var item = new EmployeeCFR();
                item.Type = "Patient Recruitment";
                int n = 0;
                double s = 0;
                if (cfr.TelephoneEtiquetteRating == 1 || cfr.TelephoneEtiquetteRating == 2 || cfr.TelephoneEtiquetteRating == 3)
                {
                    n++;
                }
                if (cfr.ProfessionalismRating == 1 || cfr.ProfessionalismRating == 2 || cfr.ProfessionalismRating == 3)
                {
                    n++;
                }
                if (cfr.ComplianceRating == 1 || cfr.ComplianceRating == 2 || cfr.ComplianceRating == 3)
                {
                    n++;
                }
                if (cfr.AdheranceRating == 1 || cfr.AdheranceRating == 2 || cfr.AdheranceRating == 3)
                {
                    n++;
                }
                if (cfr.AccuracyOfInformationRating == 1 || cfr.AccuracyOfInformationRating == 2 || cfr.AccuracyOfInformationRating == 3)
                {
                    n++;
                }

                if (n == 0)
                {
                    item.Score = 100;
                    pScore = pScore + item.Score;
                }
                if (n == 1)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    pScore = pScore + item.Score;
                }
                else if (n == 2)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    pScore = pScore + item.Score;
                }
                else if (n == 3)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    pScore = pScore + item.Score;
                }
                else if (n == 4)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    pScore = pScore + item.Score;
                }
                else if (n == 5)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    pScore = pScore + item.Score;
                    cfrs.Add(item);
                }
            }
            foreach (var cfr in ViewBag.sCFRs)
            {
                var item = new EmployeeCFR();
                item.Type = "Sales";
                int n = 0;
                double s = 0;
                if (cfr.TelephoneEtiquetteRating == 1 || cfr.TelephoneEtiquetteRating == 2 || cfr.TelephoneEtiquetteRating == 3)
                {
                    n++;
                }
                if (cfr.ProfessionalismRating == 1 || cfr.ProfessionalismRating == 2 || cfr.ProfessionalismRating == 3)
                {
                    n++;
                }
                if (cfr.ComplianceRating == 1 || cfr.ComplianceRating == 2 || cfr.ComplianceRating == 3)
                {
                    n++;
                }
                if (cfr.AdheranceRating == 1 || cfr.AdheranceRating == 2 || cfr.AdheranceRating == 3)
                {
                    n++;
                }
                if (cfr.AccuracyOfInformationRating == 1 || cfr.AccuracyOfInformationRating == 2 || cfr.AccuracyOfInformationRating == 3)
                {
                    n++;
                }

                if (n == 0)
                {
                    item.Score = 100;
                    sScore = sScore + item.Score;
                }
                if (n == 1)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    sScore = sScore + item.Score;
                }
                else if (n == 2)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    sScore = sScore + item.Score;
                }
                else if (n == 3)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    sScore = sScore + item.Score;
                }
                else if (n == 4)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    sScore = sScore + item.Score;
                }
                else if (n == 5)
                {
                    if (cfr.TelephoneEtiquetteRating != 4)
                    {
                        if (cfr.TelephoneEtiquetteRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.TelephoneEtiquetteRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ProfessionalismRating != 4)
                    {
                        if (cfr.ProfessionalismRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ProfessionalismRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ProfessionalismRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AdheranceRating != 4)
                    {
                        if (cfr.AdheranceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AdheranceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AdheranceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.AccuracyOfInformationRating != 4)
                    {
                        if (cfr.AccuracyOfInformationRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.AccuracyOfInformationRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.AccuracyOfInformationRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    sScore = sScore + item.Score;
                    cfrs.Add(item);
                }
            }
            foreach (var cfr in ViewBag.aCFRs)
            {
                var item = new EmployeeCFR();
                item.Type = "Acurian";
                int n = 0;
                double s = 0;
                if (cfr.IntroductionRating == 1 || cfr.IntroductionRating == 2 || cfr.IntroductionRating == 3)
                {
                    n++;
                }
                if (cfr.CommunicationSkillsRating == 1 || cfr.CommunicationSkillsRating == 2 || cfr.CommunicationSkillsRating == 3)
                {
                    n++;
                }
                if (cfr.SoftSkillsRating == 1 || cfr.SoftSkillsRating == 2 || cfr.SoftSkillsRating == 3)
                {
                    n++;
                }
                if (cfr.ComplianceRating == 1 || cfr.ComplianceRating == 2 || cfr.ComplianceRating == 3)
                {
                    n++;
                }
                if (cfr.ClosingRating == 1 || cfr.ClosingRating == 2 || cfr.ClosingRating == 3)
                {
                    n++;
                }

                if (n == 0)
                {
                    item.Score = 100;
                    aScore = aScore + item.Score;
                }
                if (n == 1)
                {
                    if (cfr.IntroductionRating != 4)
                    {
                        if (cfr.IntroductionRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.IntroductionRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.IntroductionRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.CommunicationSkillsRating != 4)
                    {
                        if (cfr.CommunicationSkillsRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.CommunicationSkillsRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.CommunicationSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.SoftSkillsRating != 4)
                    {
                        if (cfr.SoftSkillsRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.SoftSkillsRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.SoftSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ClosingRating != 4)
                    {
                        if (cfr.ClosingRating == 1)
                        {
                            s = s + 100;
                        }
                        else if (cfr.ClosingRating == 2)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ClosingRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    aScore = aScore + item.Score;
                }
                else if (n == 2)
                {
                    if (cfr.IntroductionRating != 4)
                    {
                        if (cfr.IntroductionRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.IntroductionRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.IntroductionRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.CommunicationSkillsRating != 4)
                    {
                        if (cfr.CommunicationSkillsRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.CommunicationSkillsRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.CommunicationSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.SoftSkillsRating != 4)
                    {
                        if (cfr.SoftSkillsRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.SoftSkillsRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.SoftSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ClosingRating != 4)
                    {
                        if (cfr.ClosingRating == 1)
                        {
                            s = s + 50;
                        }
                        else if (cfr.ClosingRating == 2)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ClosingRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    aScore = aScore + item.Score;
                }
                else if (n == 3)
                {
                    if (cfr.IntroductionRating != 4)
                    {
                        if (cfr.IntroductionRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.IntroductionRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.IntroductionRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.CommunicationSkillsRating != 4)
                    {
                        if (cfr.CommunicationSkillsRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.CommunicationSkillsRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.CommunicationSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.SoftSkillsRating != 4)
                    {
                        if (cfr.SoftSkillsRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.SoftSkillsRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.SoftSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ClosingRating != 4)
                    {
                        if (cfr.ClosingRating == 1)
                        {
                            s = s + 33.33;
                        }
                        else if (cfr.ClosingRating == 2)
                        {
                            s = s + 16.67;
                        }
                        else if (cfr.ClosingRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    aScore = aScore + item.Score;
                }
                else if (n == 4)
                {
                    if (cfr.IntroductionRating != 4)
                    {
                        if (cfr.IntroductionRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.IntroductionRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.IntroductionRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.CommunicationSkillsRating != 4)
                    {
                        if (cfr.CommunicationSkillsRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.CommunicationSkillsRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.CommunicationSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.SoftSkillsRating != 4)
                    {
                        if (cfr.SoftSkillsRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.SoftSkillsRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.SoftSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ClosingRating != 4)
                    {
                        if (cfr.ClosingRating == 1)
                        {
                            s = s + 25;
                        }
                        else if (cfr.ClosingRating == 2)
                        {
                            s = s + 12.5;
                        }
                        else if (cfr.ClosingRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    aScore = aScore + item.Score;
                }
                else if (n == 5)
                {
                    if (cfr.IntroductionRating != 4)
                    {
                        if (cfr.IntroductionRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.IntroductionRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.IntroductionRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.CommunicationSkillsRating != 4)
                    {
                        if (cfr.CommunicationSkillsRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.CommunicationSkillsRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.CommunicationSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.SoftSkillsRating != 4)
                    {
                        if (cfr.SoftSkillsRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.SoftSkillsRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.SoftSkillsRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ComplianceRating != 4)
                    {
                        if (cfr.ComplianceRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ComplianceRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ComplianceRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    if (cfr.ClosingRating != 4)
                    {
                        if (cfr.ClosingRating == 1)
                        {
                            s = s + 20;
                        }
                        else if (cfr.ClosingRating == 2)
                        {
                            s = s + 10;
                        }
                        else if (cfr.ClosingRating == 3)
                        {
                            s = s + 0;
                        }
                    }
                    item.Score = Math.Round(s);
                    aScore = aScore + item.Score;
                    cfrs.Add(item);
                }
            }
            var mCFRs = ViewBag.mCFRs.Count;
            var iCFRs = ViewBag.iCFRs.Count;
            var pCFRs = ViewBag.pCFRs.Count;
            var sCFRs = ViewBag.sCFRs.Count;
            var aCFRs = ViewBag.aCFRs.Count;

            ViewBag.mScore = "N/A";
            ViewBag.iScore = "N/A";
            ViewBag.pScore = "N/A";
            ViewBag.sScore = "N/A";
            ViewBag.aScore = "N/A";

            if (mCFRs > 0)
            {
                ViewBag.mScore = Math.Round(mScore / mCFRs);
            }
            if (iCFRs > 0)
            {
                ViewBag.iScore = Math.Round(iScore / iCFRs);
            }
            if (pCFRs > 0)
            {
                ViewBag.pScore = Math.Round(pScore / pCFRs);
            }
            if (sCFRs > 0)
            {
                ViewBag.sScore = Math.Round(sScore / sCFRs);
            }
            if (aCFRs > 0)
            {
                ViewBag.aScore = Math.Round(aScore / aCFRs);
            }
            return View(employee);
        }

        //// GET: Employees/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employee employee = mb.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employee);
        //}

        //// POST: Employees/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Employee employee = mb.Employees.Find(id);
        //    mb.Employees.Remove(employee);
        //    mb.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, HR, Operations")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDA([Bind(Include = "DisciplinaryActionID,EmployeeID,FirstName,LastName,Date,Reason,Explanation,EditByEmployeeID,EditTimeStamp,File")] DisciplinaryAction DiscAct, HttpPostedFileBase files)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {             
                DiscAct.EditByEmployeeID = user.EmployeeID;
                DiscAct.EditTimeStamp = System.DateTime.Now;

                if (files != null)
                {
                    //Counter
                    var num = 0;
                    //Gets Filename without the extension
                    var fileName = Path.GetFileNameWithoutExtension(files.FileName);
                    var gPic = Path.Combine("/DisciplinaryActions/", fileName + Path.GetExtension(files.FileName));
                    //Checks if pPic matches any of the current attachments, 
                    //if so it will loop and add a (number) to the end of the filename
                    while (mb.DisciplinaryActions.Any(d => d.File == gPic))
                    {
                        //Sets "filename" back to the default value
                        fileName = Path.GetFileNameWithoutExtension(files.FileName);
                        //Add's parentheses after the name with a number ex. filename(4)
                        fileName = string.Format(fileName + "(" + ++num + ")");
                        //Makes sure pPic gets updated with the new filename so it could check
                        gPic = Path.Combine("/DisciplinaryActions/", fileName + Path.GetExtension(files.FileName));
                    }
                    files.SaveAs(Path.Combine(Server.MapPath("~/DisciplinaryActions/"), fileName + Path.GetExtension(files.FileName)));

                    DiscAct.File = gPic;
                    mb.SaveChanges();
                }
                mb.DisciplinaryActions.Add(DiscAct);
                mb.SaveChanges();

                return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
            }
            return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
        }

        // GET: Employees/EditDA/5
        [Authorize(Roles = "Admin, HR, Operations")]
        public ActionResult EditDA(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisciplinaryAction DiscAct = mb.DisciplinaryActions.Find(id);
            if (DiscAct == null)
            {
                return HttpNotFound();
            }
            return View(DiscAct);
        }

        // POST: Employees/EditDA/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, HR, Operations")]
        [ValidateAntiForgeryToken]
        public ActionResult EditDA([Bind(Include = "DisciplinaryActionID,EmployeeID,FirstName,LastName,Date,Reason,Explanation,EditByEmployeeID,EditTimeStamp,File")] DisciplinaryAction DiscAct, HttpPostedFileBase files)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                mb.DisciplinaryActions.Attach(DiscAct);
                mb.Entry(DiscAct).Property("Date").IsModified = true;
                mb.Entry(DiscAct).Property("Reason").IsModified = true;
                mb.Entry(DiscAct).Property("Explanation").IsModified = true;
                mb.Entry(DiscAct).Property("File").IsModified = true;
                
                if (files != null)
                {
                    //Counter
                    var num = 0;
                    //Gets Filename without the extension
                    var fileName = Path.GetFileNameWithoutExtension(files.FileName);
                    var gPic = Path.Combine("/DisciplinaryActions/", fileName + Path.GetExtension(files.FileName));
                    //Checks if pPic matches any of the current attachments, 
                    //if so it will loop and add a (number) to the end of the filename
                    while (mb.DisciplinaryActions.Any(d => d.File == gPic))
                    {
                        //Sets "filename" back to the default value
                        fileName = Path.GetFileNameWithoutExtension(files.FileName);
                        //Add's parentheses after the name with a number ex. filename(4)
                        fileName = string.Format(fileName + "(" + ++num + ")");
                        //Makes sure pPic gets updated with the new filename so it could check
                        gPic = Path.Combine("/DisciplinaryActions/", fileName + Path.GetExtension(files.FileName));
                    }
                    files.SaveAs(Path.Combine(Server.MapPath("~/DisciplinaryActions/"), fileName + Path.GetExtension(files.FileName)));

                    DiscAct.File = gPic;
                    mb.SaveChanges();
                }                  
                DiscAct.EditByEmployeeID = user.EmployeeID;
                DiscAct.EditTimeStamp = System.DateTime.Now;
                mb.SaveChanges();

                return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
            }
            return View(DiscAct);
        }

        // GET: Employees/DeleteDA/5
        [Authorize(Roles = "Admin, HR, Operations")]
        public ActionResult DeleteDA(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisciplinaryAction DiscAct= mb.DisciplinaryActions.Find(id);
            if (DiscAct == null)
            {
                return HttpNotFound();
            }
            return View(DiscAct);
        }

        // POST: Employees/DeleteDA/5
        [HttpPost, ActionName("DeleteDA")]
        [Authorize(Roles = "Admin, HR, Operations")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDAConfirmed(int id)
        {
            DisciplinaryAction DiscAct = mb.DisciplinaryActions.Find(id);
            mb.DisciplinaryActions.Remove(DiscAct);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
        }

        // GET: CFRMortgages/Details/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult MortgageCFR_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRMortgage cFRMortgage = mb.CFRMortgages.Find(id);
            if (cFRMortgage == null)
            {
                return HttpNotFound();
            }

            int n = 0;
            double s = 0;
            if (cFRMortgage.TelephoneEtiquetteRating == 1 || cFRMortgage.TelephoneEtiquetteRating == 2 || cFRMortgage.TelephoneEtiquetteRating == 3)
            {
                n++;
            }
            if (cFRMortgage.ProfessionalismRating == 1 || cFRMortgage.ProfessionalismRating == 2 || cFRMortgage.ProfessionalismRating == 3)
            {
                n++;
            }
            if (cFRMortgage.ComplianceRating == 1 || cFRMortgage.ComplianceRating == 2 || cFRMortgage.ComplianceRating == 3)
            {
                n++;
            }
            if (cFRMortgage.AdheranceRating == 1 || cFRMortgage.AdheranceRating == 2 || cFRMortgage.AdheranceRating == 3)
            {
                n++;
            }
            if (cFRMortgage.AccuracyOfInformationRating == 1 || cFRMortgage.AccuracyOfInformationRating == 2 || cFRMortgage.AccuracyOfInformationRating == 3)
            {
                n++;
            }

            if (n == 0)
            {
                ViewBag.OverallScore = "N/A";
            }
            if (n == 1)
            {
                if (cFRMortgage.TelephoneEtiquetteRating != 4)
                {
                    if (cFRMortgage.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ProfessionalismRating != 4)
                {
                    if (cFRMortgage.ProfessionalismRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ComplianceRating != 4)
                {
                    if (cFRMortgage.ComplianceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRMortgage.ComplianceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AdheranceRating != 4)
                {
                    if (cFRMortgage.AdheranceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRMortgage.AdheranceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AccuracyOfInformationRating != 4)
                {
                    if (cFRMortgage.AccuracyOfInformationRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 2)
            {
                if (cFRMortgage.TelephoneEtiquetteRating != 4)
                {
                    if (cFRMortgage.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ProfessionalismRating != 4)
                {
                    if (cFRMortgage.ProfessionalismRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ComplianceRating != 4)
                {
                    if (cFRMortgage.ComplianceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.ComplianceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AdheranceRating != 4)
                {
                    if (cFRMortgage.AdheranceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.AdheranceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AccuracyOfInformationRating != 4)
                {
                    if (cFRMortgage.AccuracyOfInformationRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 3)
            {
                if (cFRMortgage.TelephoneEtiquetteRating != 4)
                {
                    if (cFRMortgage.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ProfessionalismRating != 4)
                {
                    if (cFRMortgage.ProfessionalismRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ComplianceRating != 4)
                {
                    if (cFRMortgage.ComplianceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRMortgage.ComplianceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRMortgage.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AdheranceRating != 4)
                {
                    if (cFRMortgage.AdheranceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRMortgage.AdheranceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRMortgage.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AccuracyOfInformationRating != 4)
                {
                    if (cFRMortgage.AccuracyOfInformationRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 4)
            {
                if (cFRMortgage.TelephoneEtiquetteRating != 4)
                {
                    if (cFRMortgage.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ProfessionalismRating != 4)
                {
                    if (cFRMortgage.ProfessionalismRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ComplianceRating != 4)
                {
                    if (cFRMortgage.ComplianceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.ComplianceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRMortgage.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AdheranceRating != 4)
                {
                    if (cFRMortgage.AdheranceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.AdheranceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRMortgage.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AccuracyOfInformationRating != 4)
                {
                    if (cFRMortgage.AccuracyOfInformationRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 5)
            {
                if (cFRMortgage.TelephoneEtiquetteRating != 4)
                {
                    if (cFRMortgage.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRMortgage.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ProfessionalismRating != 4)
                {
                    if (cFRMortgage.ProfessionalismRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRMortgage.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.ComplianceRating != 4)
                {
                    if (cFRMortgage.ComplianceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRMortgage.ComplianceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRMortgage.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AdheranceRating != 4)
                {
                    if (cFRMortgage.AdheranceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRMortgage.AdheranceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRMortgage.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRMortgage.AccuracyOfInformationRating != 4)
                {
                    if (cFRMortgage.AccuracyOfInformationRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRMortgage.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            return View(cFRMortgage);
        }

        // POST: CFRMortgages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult MortgageCFR_Create([Bind(Include = "CFRMortgageID,EmployeeID,DomainMasterID,C_Calls,mTEQ1,mTEQ2,mTEQ3,mTEQ4,mTEQ5,mPQ1,mPQ2,mCQ1,mCQ2,mCQ3,mAQ1,mAQ2,mAQ3,mAQ4,mAQ5,mAOIQ1,mAOIQ2,mAOIQ3,mAOIQ4,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRMortgage cFRMortgage)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                cFRMortgage.ManagerID = user.EmployeeID;
                cFRMortgage.DateOfFeedback = System.DateTime.Now;

                // Telephone Etiquette Rating Calculation (MORTGAGE)
                int mTER = 0;
                if (cFRMortgage.mTEQ1 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ2 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ3 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ4 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ5 == 2)
                {
                    mTER++;
                }

                if (cFRMortgage.mTEQ1 == 3 && cFRMortgage.mTEQ2 == 3 && cFRMortgage.mTEQ3 == 3 && cFRMortgage.mTEQ4 == 3 && cFRMortgage.mTEQ5 == 3)
                {
                    cFRMortgage.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (mTER == 0)
                    {
                        cFRMortgage.TelephoneEtiquetteRating = 1;
                    }
                    if (mTER > 0 && mTER < 3)
                    {
                        cFRMortgage.TelephoneEtiquetteRating = 2;
                    }
                    if (mTER >= 3)
                    {
                        cFRMortgage.TelephoneEtiquetteRating = 3;
                    }
                }
                
                // Professionalism Rating Calculation (MORTGAGE)
                int mPR = 0;
                if (cFRMortgage.mPQ1 == 2)
                {
                    mPR++;
                }
                if (cFRMortgage.mPQ2 == 2)
                {
                    mPR++;
                }

                if (cFRMortgage.mPQ1 == 3 && cFRMortgage.mPQ2 == 3)
                {
                    cFRMortgage.ProfessionalismRating = 4;
                }
                else
                {
                    if (mPR == 0)
                    {
                        cFRMortgage.ProfessionalismRating = 1;
                    }
                    if (mPR == 1)
                    {
                        cFRMortgage.ProfessionalismRating = 2;
                    }
                    if (mPR >= 2)
                    {
                        cFRMortgage.ProfessionalismRating = 3;
                    }
                }
                
                // Compliance Rating Calculation (MORTGAGE)
                int mCR = 0;
                if (cFRMortgage.mCQ1 == 2)
                {
                    mCR++;
                }
                if (cFRMortgage.mCQ2 == 2)
                {
                    mCR++;
                }
                if (cFRMortgage.mCQ3 == 2)
                {
                    mCR++;
                }

                if (cFRMortgage.mCQ1 == 3 && cFRMortgage.mCQ2 == 3 && cFRMortgage.mCQ3 == 3)
                {
                    cFRMortgage.ComplianceRating = 4;
                }
                else
                {
                    if (mCR == 0)
                    {
                        cFRMortgage.ComplianceRating = 1;
                    }
                    if (mCR >= 1)
                    {
                        cFRMortgage.ComplianceRating = 3;
                    }
                }
                
                // Adherence Rating Calculation (MORTGAGE)
                int mAR = 0;
                if (cFRMortgage.mAQ1 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ2 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ3 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ4 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ5 == 2)
                {
                    mAR++;
                }

                if (cFRMortgage.mAQ1 == 3 && cFRMortgage.mAQ2 == 3 && cFRMortgage.mAQ3 == 3 && cFRMortgage.mAQ4 == 3 && cFRMortgage.mAQ5 == 3)
                {
                    cFRMortgage.AdheranceRating = 4;
                }
                else
                {
                    if (mAR == 0)
                    {
                        cFRMortgage.AdheranceRating = 1;
                    }
                    if (mAR > 0 && mAR < 3)
                    {
                        cFRMortgage.AdheranceRating = 2;
                    }
                    if (mAR >= 3)
                    {
                        cFRMortgage.AdheranceRating = 3;
                    }
                }
                
                // Accuracy of Information Rating Calculation (MORTGAGE)
                int mAOIR = 0;
                if (cFRMortgage.mAOIQ1 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.mAOIQ2 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.mAOIQ3 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.mAOIQ4 == 2)
                {
                    mAOIR++;
                }

                if (cFRMortgage.mAOIQ1 == 3 && cFRMortgage.mAOIQ2 == 3 && cFRMortgage.mAOIQ3 == 3 && cFRMortgage.mAOIQ4 == 3)
                {
                    cFRMortgage.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (mAOIR == 0)
                    {
                        cFRMortgage.AccuracyOfInformationRating = 1;
                    }
                    if (mAOIR >= 1)
                    {
                        cFRMortgage.AccuracyOfInformationRating = 3;
                    }
                }
                
                mb.CFRMortgages.Add(cFRMortgage);
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRMortgage.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRMortgage.DomainMasterID);
            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ1);
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ2);
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ3);
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ4);
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ5);
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ1);
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ2);
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ1);
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ2);
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ3);
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ1);
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ2);
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ3);
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ4);
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ5);
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ1);
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ2);
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ3);
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ4);
            return View(cFRMortgage);
        }

        // GET: CFRMortgages/Edit/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult MortgageCFR_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRMortgage cFRMortgage = mb.CFRMortgages.Find(id);
            if (cFRMortgage == null)
            {
                return HttpNotFound();
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRMortgage.DomainMasterID);
            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ1);
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ2);
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ3);
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ4);
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ5);
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ1);
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ2);
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ1);
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ2);
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ3);
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ1);
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ2);
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ3);
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ4);
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ5);
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ1);
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ2);
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ3);
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ4);
            ViewBag.EmployeeName = cFRMortgage.Employee.FirstName + " " + cFRMortgage.Employee.LastName;
            return View(cFRMortgage);
        }

        // POST: CFRMortgages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult MortgageCFR_Edit([Bind(Include = "CFRMortgageID,EmployeeID,DomainMasterID,C_Calls,mTEQ1,mTEQ2,mTEQ3,mTEQ4,mTEQ5,mPQ1,mPQ2,mCQ1,mCQ2,mCQ3,mAQ1,mAQ2,mAQ3,mAQ4,mAQ5,mAOIQ1,mAOIQ2,mAOIQ3,mAOIQ4,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRMortgage cFRMortgage)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                mb.CFRMortgages.Attach(cFRMortgage);
                mb.Entry(cFRMortgage).Property("DomainMasterID").IsModified = true;
                mb.Entry(cFRMortgage).Property("C_Calls").IsModified = true;
                mb.Entry(cFRMortgage).Property("mTEQ1").IsModified = true;
                mb.Entry(cFRMortgage).Property("mTEQ2").IsModified = true;
                mb.Entry(cFRMortgage).Property("mTEQ3").IsModified = true;
                mb.Entry(cFRMortgage).Property("mTEQ4").IsModified = true;
                mb.Entry(cFRMortgage).Property("mTEQ5").IsModified = true;
                mb.Entry(cFRMortgage).Property("mPQ1").IsModified = true;
                mb.Entry(cFRMortgage).Property("mPQ2").IsModified = true;
                mb.Entry(cFRMortgage).Property("mCQ1").IsModified = true;
                mb.Entry(cFRMortgage).Property("mCQ2").IsModified = true;
                mb.Entry(cFRMortgage).Property("mCQ3").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAQ1").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAQ2").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAQ3").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAQ4").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAQ5").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAOIQ1").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAOIQ2").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAOIQ3").IsModified = true;
                mb.Entry(cFRMortgage).Property("mAOIQ4").IsModified = true;
                mb.Entry(cFRMortgage).Property("TelephoneEtiquetteRating").IsModified = true;
                mb.Entry(cFRMortgage).Property("ProfessionalismRating").IsModified = true;
                mb.Entry(cFRMortgage).Property("ComplianceRating").IsModified = true;
                mb.Entry(cFRMortgage).Property("AdheranceRating").IsModified = true;
                mb.Entry(cFRMortgage).Property("AccuracyOfInformationRating").IsModified = true;
                mb.Entry(cFRMortgage).Property("Comments").IsModified = true;
                mb.Entry(cFRMortgage).Property("Strengths").IsModified = true;
                mb.Entry(cFRMortgage).Property("ActionPlan").IsModified = true;
                cFRMortgage.ManagerID = user.EmployeeID;

                // Telephone Etiquette Rating Calculation (MORTGAGE)
                int mTER = 0;
                if (cFRMortgage.mTEQ1 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ2 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ3 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ4 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.mTEQ5 == 2)
                {
                    mTER++;
                }

                if (cFRMortgage.mTEQ1 == 3 && cFRMortgage.mTEQ2 == 3 && cFRMortgage.mTEQ3 == 3 && cFRMortgage.mTEQ4 == 3 && cFRMortgage.mTEQ5 == 3)
                {
                    cFRMortgage.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (mTER == 0)
                    {
                        cFRMortgage.TelephoneEtiquetteRating = 1;
                    }
                    if (mTER > 0 && mTER < 3)
                    {
                        cFRMortgage.TelephoneEtiquetteRating = 2;
                    }
                    if (mTER >= 3)
                    {
                        cFRMortgage.TelephoneEtiquetteRating = 3;
                    }
                }

                // Professionalism Rating Calculation (MORTGAGE)
                int mPR = 0;
                if (cFRMortgage.mPQ1 == 2)
                {
                    mPR++;
                }
                if (cFRMortgage.mPQ2 == 2)
                {
                    mPR++;
                }

                if (cFRMortgage.mPQ1 == 3 && cFRMortgage.mPQ2 == 3)
                {
                    cFRMortgage.ProfessionalismRating = 4;
                }
                else
                {
                    if (mPR == 0)
                    {
                        cFRMortgage.ProfessionalismRating = 1;
                    }
                    if (mPR == 1)
                    {
                        cFRMortgage.ProfessionalismRating = 2;
                    }
                    if (mPR >= 2)
                    {
                        cFRMortgage.ProfessionalismRating = 3;
                    }
                }

                // Compliance Rating Calculation (MORTGAGE)
                int mCR = 0;
                if (cFRMortgage.mCQ1 == 2)
                {
                    mCR++;
                }
                if (cFRMortgage.mCQ2 == 2)
                {
                    mCR++;
                }
                if (cFRMortgage.mCQ3 == 2)
                {
                    mCR++;
                }

                if (cFRMortgage.mCQ1 == 3 && cFRMortgage.mCQ2 == 3 && cFRMortgage.mCQ3 == 3)
                {
                    cFRMortgage.ComplianceRating = 4;
                }
                else
                {
                    if (mCR == 0)
                    {
                        cFRMortgage.ComplianceRating = 1;
                    }
                    if (mCR >= 1)
                    {
                        cFRMortgage.ComplianceRating = 3;
                    }
                }

                // Adherence Rating Calculation (MORTGAGE)
                int mAR = 0;
                if (cFRMortgage.mAQ1 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ2 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ3 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ4 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.mAQ5 == 2)
                {
                    mAR++;
                }

                if (cFRMortgage.mAQ1 == 3 && cFRMortgage.mAQ2 == 3 && cFRMortgage.mAQ3 == 3 && cFRMortgage.mAQ4 == 3 && cFRMortgage.mAQ5 == 3)
                {
                    cFRMortgage.AdheranceRating = 4;
                }
                else
                {
                    if (mAR == 0)
                    {
                        cFRMortgage.AdheranceRating = 1;
                    }
                    if (mAR > 0 && mAR < 3)
                    {
                        cFRMortgage.AdheranceRating = 2;
                    }
                    if (mAR >= 3)
                    {
                        cFRMortgage.AdheranceRating = 3;
                    }
                }

                // Accuracy of Information Rating Calculation (MORTGAGE)
                int mAOIR = 0;
                if (cFRMortgage.mAOIQ1 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.mAOIQ2 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.mAOIQ3 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.mAOIQ4 == 2)
                {
                    mAOIR++;
                }

                if (cFRMortgage.mAOIQ1 == 3 && cFRMortgage.mAOIQ2 == 3 && cFRMortgage.mAOIQ3 == 3 && cFRMortgage.mAOIQ4 == 3)
                {
                    cFRMortgage.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (mAOIR == 0)
                    {
                        cFRMortgage.AccuracyOfInformationRating = 1;
                    }
                    if (mAOIR >= 1)
                    {
                        cFRMortgage.AccuracyOfInformationRating = 3;
                    }
                }
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRMortgage.EmployeeID });
            }
        
            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRMortgage.DomainMasterID);
            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ1);
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ2);
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ3);
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ4);
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ5);
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ1);
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ2);
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ1);
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ2);
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ3);
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ1);
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ2);
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ3);
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ4);
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ5);
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ1);
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ2);
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ3);
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ4);
            ViewBag.EmployeeName = cFRMortgage.Employee.FirstName + " " + cFRMortgage.Employee.LastName;
            return View(cFRMortgage);
        }

        // GET: CFRMortgages/Delete/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult MortgageCFR_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRMortgage cFRMortgage = mb.CFRMortgages.Find(id);
            if (cFRMortgage == null)
            {
                return HttpNotFound();
            }
            return View(cFRMortgage);
        }

        // POST: CFRMortgages/Delete/5
        [HttpPost, ActionName("MortgageCFR_Delete")]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult MortgageCFR_DeleteConfirmed(int id)
        {
            CFRMortgage cFRMortgage = mb.CFRMortgages.Find(id);
            mb.CFRMortgages.Remove(cFRMortgage);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRMortgage.EmployeeID });
        }

        // GET: CFRSales/Details/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult SalesCFR_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRSale cFRSales = mb.CFRSales.Find(id);
            if (cFRSales == null)
            {
                return HttpNotFound();
            }

            int n = 0;
            double s = 0;
            if (cFRSales.TelephoneEtiquetteRating == 1 || cFRSales.TelephoneEtiquetteRating == 2 || cFRSales.TelephoneEtiquetteRating == 3)
            {
                n++;
            }
            if (cFRSales.ProfessionalismRating == 1 || cFRSales.ProfessionalismRating == 2 || cFRSales.ProfessionalismRating == 3)
            {
                n++;
            }
            if (cFRSales.ComplianceRating == 1 || cFRSales.ComplianceRating == 2 || cFRSales.ComplianceRating == 3)
            {
                n++;
            }
            if (cFRSales.AdheranceRating == 1 || cFRSales.AdheranceRating == 2 || cFRSales.AdheranceRating == 3)
            {
                n++;
            }
            if (cFRSales.AccuracyOfInformationRating == 1 || cFRSales.AccuracyOfInformationRating == 2 || cFRSales.AccuracyOfInformationRating == 3)
            {
                n++;
            }
            
            if (n == 0)
            {
                ViewBag.OverallScore = "N/A";
            }
            if (n == 1)
            {
                if (cFRSales.TelephoneEtiquetteRating != 4)
                {
                    if (cFRSales.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ProfessionalismRating != 4)
                {
                    if (cFRSales.ProfessionalismRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRSales.ProfessionalismRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ComplianceRating != 4)
                {
                    if (cFRSales.ComplianceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRSales.ComplianceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AdheranceRating != 4)
                {
                    if (cFRSales.AdheranceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRSales.AdheranceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AccuracyOfInformationRating != 4)
                {
                    if (cFRSales.AccuracyOfInformationRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 2)
            {
                if (cFRSales.TelephoneEtiquetteRating != 4)
                {
                    if (cFRSales.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ProfessionalismRating != 4)
                {
                    if (cFRSales.ProfessionalismRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.ProfessionalismRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ComplianceRating != 4)
                {
                    if (cFRSales.ComplianceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.ComplianceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AdheranceRating != 4)
                {
                    if (cFRSales.AdheranceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.AdheranceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AccuracyOfInformationRating != 4)
                {
                    if (cFRSales.AccuracyOfInformationRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 3)
            {
                if (cFRSales.TelephoneEtiquetteRating != 4)
                {
                    if (cFRSales.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ProfessionalismRating != 4)
                {
                    if (cFRSales.ProfessionalismRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRSales.ProfessionalismRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRSales.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ComplianceRating != 4)
                {
                    if (cFRSales.ComplianceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRSales.ComplianceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRSales.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AdheranceRating != 4)
                {
                    if (cFRSales.AdheranceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRSales.AdheranceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRSales.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AccuracyOfInformationRating != 4)
                {
                    if (cFRSales.AccuracyOfInformationRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 4)
            {
                if (cFRSales.TelephoneEtiquetteRating != 4)
                {
                    if (cFRSales.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ProfessionalismRating != 4)
                {
                    if (cFRSales.ProfessionalismRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.ProfessionalismRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRSales.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ComplianceRating != 4)
                {
                    if (cFRSales.ComplianceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.ComplianceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRSales.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AdheranceRating != 4)
                {
                    if (cFRSales.AdheranceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.AdheranceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRSales.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AccuracyOfInformationRating != 4)
                {
                    if (cFRSales.AccuracyOfInformationRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 5)
            {
                if (cFRSales.TelephoneEtiquetteRating != 4)
                {
                    if (cFRSales.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRSales.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ProfessionalismRating != 4)
                {
                    if (cFRSales.ProfessionalismRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRSales.ProfessionalismRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRSales.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.ComplianceRating != 4)
                {
                    if (cFRSales.ComplianceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRSales.ComplianceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRSales.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AdheranceRating != 4)
                {
                    if (cFRSales.AdheranceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRSales.AdheranceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRSales.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRSales.AccuracyOfInformationRating != 4)
                {
                    if (cFRSales.AccuracyOfInformationRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRSales.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            return View(cFRSales);
        }

        // POST: CFRSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult SalesCFR_Create([Bind(Include = "CFRSalesID,EmployeeID,DomainMasterID,C_Calls,sTEQ1,sTEQ2,sTEQ3,sTEQ4,sPQ1,sPQ2,sPQ3,sPQ4,sCQ1,sCQ2,sAQ1,sAQ2,sAOIQ1,sAOIQ2,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRSale cFRSales)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                cFRSales.ManagerID = user.EmployeeID;
                cFRSales.DateOfFeedback = System.DateTime.Now;

                // Telephone Etiquette Rating Calculation (SALES)
                int sTER = 0;
                if (cFRSales.sTEQ1 == 2)
                {
                    sTER++;
                }
                if (cFRSales.sTEQ2 == 2)
                {
                    sTER++;
                }
                if (cFRSales.sTEQ3 == 2)
                {
                    sTER++;
                }
                if (cFRSales.sTEQ4 == 2)
                {
                    sTER++;
                }

                if (cFRSales.sTEQ1 == 3 && cFRSales.sTEQ2 == 3 && cFRSales.sTEQ3 == 3 && cFRSales.sTEQ4 == 3)
                {
                    cFRSales.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (sTER == 0)
                    {
                        cFRSales.TelephoneEtiquetteRating = 1;
                    }
                    if (sTER > 0 && sTER <= 3)
                    {
                        cFRSales.TelephoneEtiquetteRating = 2;
                    }
                    if (sTER == 4)
                    {
                        cFRSales.TelephoneEtiquetteRating = 3;
                    }
                }

                // Professionalism Rating Calculation (SALES)
                int sPR = 0;
                if (cFRSales.sPQ1 == 2)
                {
                    sPR++;
                }
                if (cFRSales.sPQ2 == 2)
                {
                    sPR++;
                }
                if (cFRSales.sPQ3 == 2)
                {
                    sPR++;
                }
                if (cFRSales.sPQ4 == 2)
                {
                    sPR++;
                }

                if (cFRSales.sPQ1 == 3 && cFRSales.sPQ2 == 3 && cFRSales.sPQ3 == 3 && cFRSales.sPQ4 == 3)
                {
                    cFRSales.ProfessionalismRating = 4;
                }
                else
                {
                    if (sPR == 0)
                    {
                        cFRSales.ProfessionalismRating = 1;
                    }
                    if (sPR > 0 && sPR <= 3)
                    {
                        cFRSales.ProfessionalismRating = 2;
                    }
                    if (sPR == 4)
                    {
                        cFRSales.ProfessionalismRating = 3;
                    }
                }

                // Compliance Rating Calculation (SALES)
                int sCR = 0;
                if (cFRSales.sCQ1 == 2)
                {
                    sCR++;
                }
                if (cFRSales.sCQ2 == 2)
                {
                    sCR++;
                }

                if (cFRSales.sCQ1 == 3 && cFRSales.sCQ2 == 3)
                {
                    cFRSales.ComplianceRating = 4;
                }
                else
                {
                    if (sCR == 0)
                    {
                        cFRSales.ComplianceRating = 1;
                    }
                    if (sCR > 0)
                    {
                        cFRSales.ComplianceRating = 3;
                    }
                }

                // Adherence Rating Calculation (SALES)
                int sAR = 0;
                if (cFRSales.sAQ1 == 2)
                {
                    sAR++;
                }
                if (cFRSales.sAQ2 == 2)
                {
                    sAR++;
                }

                if (cFRSales.sAQ1 == 3 && cFRSales.sAQ2 == 3)
                {
                    cFRSales.AdheranceRating = 4;
                }
                else
                {
                    if (sAR == 0)
                    {
                        cFRSales.AdheranceRating = 1;
                    }
                    if (sAR == 1)
                    {
                        cFRSales.AdheranceRating = 2;
                    }
                    if (sAR > 1)
                    {
                        cFRSales.AdheranceRating = 3;
                    }
                }

                // Accuracy of Information Rating Calculation (SALES)
                int sAOIR = 0;
                if (cFRSales.sAOIQ1 == 2)
                {
                    sAOIR++;
                }
                if (cFRSales.sAOIQ2 == 2)
                {
                    sAOIR++;
                }
                
                if (cFRSales.sAOIQ1 == 3 && cFRSales.sAOIQ2 == 3)
                {
                    cFRSales.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (sAOIR == 0)
                    {
                        cFRSales.AccuracyOfInformationRating = 1;
                    }
                    if (sAOIR > 0)
                    {
                        cFRSales.AccuracyOfInformationRating = 3;
                    }
                }

                mb.CFRSales.Add(cFRSales);
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRSales.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRSales.DomainMasterID);
            ViewBag.sTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ1);
            ViewBag.sTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ2);
            ViewBag.sTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ3);
            ViewBag.sTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ4);
            ViewBag.sPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ1);
            ViewBag.sPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ2);
            ViewBag.sPQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ3);
            ViewBag.sPQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ4);
            ViewBag.sCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sCQ1);
            ViewBag.sCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sCQ2);
            ViewBag.sAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAQ1);
            ViewBag.sAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAQ2);
            ViewBag.sAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAOIQ1);
            ViewBag.sAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAOIQ2);
            return View(cFRSales);
        }

        // GET: CFRSales/Edit/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult SalesCFR_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRSale cFRSales = mb.CFRSales.Find(id);
            if (cFRSales == null)
            {
                return HttpNotFound();
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true && d.FileMask != "D00").OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRSales.DomainMasterID);
            ViewBag.sTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ1);
            ViewBag.sTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ2);
            ViewBag.sTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ3);
            ViewBag.sTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ4);
            ViewBag.sPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ1);
            ViewBag.sPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ2);
            ViewBag.sPQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ3);
            ViewBag.sPQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ4);
            ViewBag.sCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sCQ1);
            ViewBag.sCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sCQ2);
            ViewBag.sAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAQ1);
            ViewBag.sAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAQ2);
            ViewBag.sAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAOIQ1);
            ViewBag.sAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAOIQ2);
            ViewBag.EmployeeName = cFRSales.Employee.FirstName + " " + cFRSales.Employee.LastName;
            return View(cFRSales);
        }

        // POST: CFRSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult SalesCFR_Edit([Bind(Include = "CFRSalesID,EmployeeID,DomainMasterID,C_Calls,sTEQ1,sTEQ2,sTEQ3,sTEQ4,sPQ1,sPQ2,sPQ3,sPQ4,sCQ1,sCQ2,sAQ1,sAQ2,sAOIQ1,sAOIQ2,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRSale cFRSales)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                mb.CFRSales.Attach(cFRSales);
                mb.Entry(cFRSales).Property("DomainMasterID").IsModified = true;
                mb.Entry(cFRSales).Property("C_Calls").IsModified = true;
                mb.Entry(cFRSales).Property("sTEQ1").IsModified = true;
                mb.Entry(cFRSales).Property("sTEQ2").IsModified = true;
                mb.Entry(cFRSales).Property("sTEQ3").IsModified = true;
                mb.Entry(cFRSales).Property("sTEQ4").IsModified = true;
                mb.Entry(cFRSales).Property("sPQ1").IsModified = true;
                mb.Entry(cFRSales).Property("sPQ2").IsModified = true;
                mb.Entry(cFRSales).Property("sPQ3").IsModified = true;
                mb.Entry(cFRSales).Property("sPQ4").IsModified = true;
                mb.Entry(cFRSales).Property("sCQ1").IsModified = true;
                mb.Entry(cFRSales).Property("sCQ2").IsModified = true;
                mb.Entry(cFRSales).Property("sAQ1").IsModified = true;
                mb.Entry(cFRSales).Property("sAQ2").IsModified = true;
                mb.Entry(cFRSales).Property("sAOIQ1").IsModified = true;
                mb.Entry(cFRSales).Property("sAOIQ2").IsModified = true;
                mb.Entry(cFRSales).Property("TelephoneEtiquetteRating").IsModified = true;
                mb.Entry(cFRSales).Property("ProfessionalismRating").IsModified = true;
                mb.Entry(cFRSales).Property("ComplianceRating").IsModified = true;
                mb.Entry(cFRSales).Property("AdheranceRating").IsModified = true;
                mb.Entry(cFRSales).Property("AccuracyOfInformationRating").IsModified = true;
                mb.Entry(cFRSales).Property("Comments").IsModified = true;
                mb.Entry(cFRSales).Property("Strengths").IsModified = true;
                mb.Entry(cFRSales).Property("ActionPlan").IsModified = true;
                cFRSales.ManagerID = user.EmployeeID;

                // Telephone Etiquette Rating Calculation (SALES)
                int sTER = 0;
                if (cFRSales.sTEQ1 == 2)
                {
                    sTER++;
                }
                if (cFRSales.sTEQ2 == 2)
                {
                    sTER++;
                }
                if (cFRSales.sTEQ3 == 2)
                {
                    sTER++;
                }
                if (cFRSales.sTEQ4 == 2)
                {
                    sTER++;
                }

                if (cFRSales.sTEQ1 == 3 && cFRSales.sTEQ2 == 3 && cFRSales.sTEQ3 == 3 && cFRSales.sTEQ4 == 3)
                {
                    cFRSales.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (sTER == 0)
                    {
                        cFRSales.TelephoneEtiquetteRating = 1;
                    }
                    if (sTER > 0 && sTER <= 3)
                    {
                        cFRSales.TelephoneEtiquetteRating = 2;
                    }
                    if (sTER == 4)
                    {
                        cFRSales.TelephoneEtiquetteRating = 3;
                    }
                }

                // Professionalism Rating Calculation (SALES)
                int sPR = 0;
                if (cFRSales.sPQ1 == 2)
                {
                    sPR++;
                }
                if (cFRSales.sPQ2 == 2)
                {
                    sPR++;
                }
                if (cFRSales.sPQ3 == 2)
                {
                    sPR++;
                }
                if (cFRSales.sPQ4 == 2)
                {
                    sPR++;
                }

                if (cFRSales.sPQ1 == 3 && cFRSales.sPQ2 == 3 && cFRSales.sPQ3 == 3 && cFRSales.sPQ4 == 3)
                {
                    cFRSales.ProfessionalismRating = 4;
                }
                else
                {
                    if (sPR == 0)
                    {
                        cFRSales.ProfessionalismRating = 1;
                    }
                    if (sPR > 0 && sPR <= 3)
                    {
                        cFRSales.ProfessionalismRating = 2;
                    }
                    if (sPR == 4)
                    {
                        cFRSales.ProfessionalismRating = 3;
                    }
                }

                // Compliance Rating Calculation (SALES)
                int sCR = 0;
                if (cFRSales.sCQ1 == 2)
                {
                    sCR++;
                }
                if (cFRSales.sCQ2 == 2)
                {
                    sCR++;
                }

                if (cFRSales.sCQ1 == 3 && cFRSales.sCQ2 == 3)
                {
                    cFRSales.ComplianceRating = 4;
                }
                else
                {
                    if (sCR == 0)
                    {
                        cFRSales.ComplianceRating = 1;
                    }
                    if (sCR > 0)
                    {
                        cFRSales.ComplianceRating = 3;
                    }
                }

                // Adherence Rating Calculation (SALES)
                int sAR = 0;
                if (cFRSales.sAQ1 == 2)
                {
                    sAR++;
                }
                if (cFRSales.sAQ2 == 2)
                {
                    sAR++;
                }

                if (cFRSales.sAQ1 == 3 && cFRSales.sAQ2 == 3)
                {
                    cFRSales.AdheranceRating = 4;
                }
                else
                {
                    if (sAR == 0)
                    {
                        cFRSales.AdheranceRating = 1;
                    }
                    if (sAR == 1)
                    {
                        cFRSales.AdheranceRating = 2;
                    }
                    if (sAR > 1)
                    {
                        cFRSales.AdheranceRating = 3;
                    }
                }

                // Accuracy of Information Rating Calculation (SALES)
                int sAOIR = 0;
                if (cFRSales.sAOIQ1 == 2)
                {
                    sAOIR++;
                }
                if (cFRSales.sAOIQ2 == 2)
                {
                    sAOIR++;
                }

                if (cFRSales.sAOIQ1 == 3 && cFRSales.sAOIQ2 == 3)
                {
                    cFRSales.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (sAOIR == 0)
                    {
                        cFRSales.AccuracyOfInformationRating = 1;
                    }
                    if (sAOIR > 0)
                    {
                        cFRSales.AccuracyOfInformationRating = 3;
                    }
                }

                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRSales.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRSales.DomainMasterID);
            ViewBag.sTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ1);
            ViewBag.sTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ2);
            ViewBag.sTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ3);
            ViewBag.sTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sTEQ4);
            ViewBag.sPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ1);
            ViewBag.sPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ2);
            ViewBag.sPQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ3);
            ViewBag.sPQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sPQ4);
            ViewBag.sCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sCQ1);
            ViewBag.sCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sCQ2);
            ViewBag.sAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAQ1);
            ViewBag.sAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAQ2);
            ViewBag.sAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAOIQ1);
            ViewBag.sAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRSales.sAOIQ2);
            ViewBag.EmployeeName = cFRSales.Employee.FirstName + " " + cFRSales.Employee.LastName;
            return View(cFRSales);
        }

        // GET: CFRSales/Delete/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult SalesCFR_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRSale cFRSales = mb.CFRSales.Find(id);
            if (cFRSales == null)
            {
                return HttpNotFound();
            }
            return View(cFRSales);
        }

        // POST: CFRSales/Delete/5
        [HttpPost, ActionName("SalesCFR_Delete")]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult SalesCFR_DeleteConfirmed(int id)
        {
            CFRSale cFRSales = mb.CFRSales.Find(id);
            mb.CFRSales.Remove(cFRSales);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRSales.EmployeeID });
        }

        // GET: CFRInsurances/Details/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult InsuranceCFR_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRInsurance cFRInsurance = mb.CFRInsurances.Find(id);
            if (cFRInsurance == null)
            {
                return HttpNotFound();
            }

            int n = 0;
            double s = 0;
            if (cFRInsurance.TelephoneEtiquetteRating == 1 || cFRInsurance.TelephoneEtiquetteRating == 2 || cFRInsurance.TelephoneEtiquetteRating == 3)
            {
                n++;
            }
            if (cFRInsurance.ProfessionalismRating == 1 || cFRInsurance.ProfessionalismRating == 2 || cFRInsurance.ProfessionalismRating == 3)
            {
                n++;
            }
            if (cFRInsurance.ComplianceRating == 1 || cFRInsurance.ComplianceRating == 2 || cFRInsurance.ComplianceRating == 3)
            {
                n++;
            }
            if (cFRInsurance.AdheranceRating == 1 || cFRInsurance.AdheranceRating == 2 || cFRInsurance.AdheranceRating == 3)
            {
                n++;
            }
            if (cFRInsurance.AccuracyOfInformationRating == 1 || cFRInsurance.AccuracyOfInformationRating == 2 || cFRInsurance.AccuracyOfInformationRating == 3)
            {
                n++;
            }

            if (n == 0)
            {
                ViewBag.OverallScore = "N/A";
            }
            if (n == 1)
            {
                if (cFRInsurance.TelephoneEtiquetteRating != 4)
                {
                    if (cFRInsurance.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ProfessionalismRating != 4)
                {
                    if (cFRInsurance.ProfessionalismRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ComplianceRating != 4)
                {
                    if (cFRInsurance.ComplianceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRInsurance.ComplianceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AdheranceRating != 4)
                {
                    if (cFRInsurance.AdheranceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRInsurance.AdheranceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AccuracyOfInformationRating != 4)
                {
                    if (cFRInsurance.AccuracyOfInformationRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 2)
            {
                if (cFRInsurance.TelephoneEtiquetteRating != 4)
                {
                    if (cFRInsurance.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ProfessionalismRating != 4)
                {
                    if (cFRInsurance.ProfessionalismRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ComplianceRating != 4)
                {
                    if (cFRInsurance.ComplianceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.ComplianceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AdheranceRating != 4)
                {
                    if (cFRInsurance.AdheranceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.AdheranceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AccuracyOfInformationRating != 4)
                {
                    if (cFRInsurance.AccuracyOfInformationRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 3)
            {
                if (cFRInsurance.TelephoneEtiquetteRating != 4)
                {
                    if (cFRInsurance.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ProfessionalismRating != 4)
                {
                    if (cFRInsurance.ProfessionalismRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ComplianceRating != 4)
                {
                    if (cFRInsurance.ComplianceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRInsurance.ComplianceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRInsurance.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AdheranceRating != 4)
                {
                    if (cFRInsurance.AdheranceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRInsurance.AdheranceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRInsurance.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AccuracyOfInformationRating != 4)
                {
                    if (cFRInsurance.AccuracyOfInformationRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 4)
            {
                if (cFRInsurance.TelephoneEtiquetteRating != 4)
                {
                    if (cFRInsurance.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ProfessionalismRating != 4)
                {
                    if (cFRInsurance.ProfessionalismRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ComplianceRating != 4)
                {
                    if (cFRInsurance.ComplianceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.ComplianceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRInsurance.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AdheranceRating != 4)
                {
                    if (cFRInsurance.AdheranceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.AdheranceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRInsurance.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AccuracyOfInformationRating != 4)
                {
                    if (cFRInsurance.AccuracyOfInformationRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 5)
            {
                if (cFRInsurance.TelephoneEtiquetteRating != 4)
                {
                    if (cFRInsurance.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRInsurance.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ProfessionalismRating != 4)
                {
                    if (cFRInsurance.ProfessionalismRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRInsurance.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.ComplianceRating != 4)
                {
                    if (cFRInsurance.ComplianceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRInsurance.ComplianceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRInsurance.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AdheranceRating != 4)
                {
                    if (cFRInsurance.AdheranceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRInsurance.AdheranceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRInsurance.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRInsurance.AccuracyOfInformationRating != 4)
                {
                    if (cFRInsurance.AccuracyOfInformationRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRInsurance.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            return View(cFRInsurance);
        }

        // POST: CFRInsurances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult InsuranceCFR_Create([Bind(Include = "CFRInsuranceID,EmployeeID,DomainMasterID,C_Calls,iTEQ1,iTEQ2,iTEQ3,iTEQ4,iTEQ5,iPQ1,iPQ2,iCQ1,iCQ2,iCQ3,iAQ1,iAQ2,iAQ3,iAQ4,iAQ5,iAOIQ1,iAOIQ2,iAOIQ3,iAOIQ4,iAOIQ5,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRInsurance cFRInsurance)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                cFRInsurance.ManagerID = user.EmployeeID;
                cFRInsurance.DateOfFeedback = System.DateTime.Now;

                // Telephone Etiquette Rating Calculation (INSURANCE)
                int iTER = 0;
                if (cFRInsurance.iTEQ1 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ2 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ3 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ4 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ5 == 2)
                {
                    iTER++;
                }

                if (cFRInsurance.iTEQ1 == 3 && cFRInsurance.iTEQ2 == 3 && cFRInsurance.iTEQ3 == 3 && cFRInsurance.iTEQ4 == 3 && cFRInsurance.iTEQ5 == 3)
                {
                    cFRInsurance.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (iTER == 0)
                    {
                        cFRInsurance.TelephoneEtiquetteRating = 1;
                    }
                    if (iTER > 0 && iTER < 3)
                    {
                        cFRInsurance.TelephoneEtiquetteRating = 2;
                    }
                    if (iTER >= 3)
                    {
                        cFRInsurance.TelephoneEtiquetteRating = 3;
                    }
                }
                
                // Professionalism Rating Calculation (INSURANCE)
                int iPR = 0;
                if (cFRInsurance.iPQ1 == 2)
                {
                    iPR++;
                }
                if (cFRInsurance.iPQ2 == 2)
                {
                    iPR++;
                }
                if (cFRInsurance.iPQ1 == 3 && cFRInsurance.iPQ2 == 3)
                {
                    cFRInsurance.ProfessionalismRating = 4;
                }
                else
                {
                    if (iPR == 0)
                    {
                        cFRInsurance.ProfessionalismRating = 1;
                    }
                    if (iPR == 1)
                    {
                        cFRInsurance.ProfessionalismRating = 2;
                    }
                    if (iPR >= 2)
                    {
                        cFRInsurance.ProfessionalismRating = 3;
                    }
                }
                
                // Compliance Rating Calculation (INSURANCE)
                int iCR = 0;
                if (cFRInsurance.iCQ1 == 2)
                {
                    iCR++;
                }
                if (cFRInsurance.iCQ2 == 2)
                {
                    iCR++;
                }
                if (cFRInsurance.iCQ3 == 2)
                {
                    iCR++;
                }

                if (cFRInsurance.iCQ1 == 3 && cFRInsurance.iCQ2 == 3 && cFRInsurance.iCQ3 == 3)
                {
                    cFRInsurance.ComplianceRating = 4;
                }
                else
                {
                    if (iCR == 0)
                    {
                        cFRInsurance.ComplianceRating = 1;
                    }
                    if (iCR >= 1)
                    {
                        cFRInsurance.ComplianceRating = 3;
                    }
                }
                
                // Adherence Rating Calculation (INSURANCE)
                int iAR = 0;
                if (cFRInsurance.iAQ1 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ2 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ3 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ4 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ5 == 2)
                {
                    iAR++;
                }

                if (cFRInsurance.iAQ1 == 3 && cFRInsurance.iAQ2 == 3 && cFRInsurance.iAQ3 == 3 && cFRInsurance.iAQ4 == 3 && cFRInsurance.iAQ5 == 3)
                {
                    cFRInsurance.AdheranceRating = 4;
                }
                else
                {
                    if (iAR == 0)
                    {
                        cFRInsurance.AdheranceRating = 1;
                    }
                    if (iAR > 0 && iAR < 3)
                    {
                        cFRInsurance.AdheranceRating = 2;
                    }
                    if (iAR >= 3)
                    {
                        cFRInsurance.AdheranceRating = 3;
                    }
                }
                
                // Accuracy of Information Rating Calculation (INSURANCE)
                int iAOIR = 0;
                if (cFRInsurance.iAOIQ1 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ2 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ3 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ4 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ5 == 2)
                {
                    iAOIR++;
                }

                if (cFRInsurance.iAOIQ1 == 3 && cFRInsurance.iAOIQ2 == 3 && cFRInsurance.iAOIQ3 == 3 && cFRInsurance.iAOIQ4 == 3 && cFRInsurance.iAOIQ5 == 3)
                {
                    cFRInsurance.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (iAOIR == 0)
                    {
                        cFRInsurance.AccuracyOfInformationRating = 1;
                    }
                    if (iAOIR >= 1)
                    {
                        cFRInsurance.AccuracyOfInformationRating = 3;
                    }
                }

                mb.CFRInsurances.Add(cFRInsurance);
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRInsurance.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRInsurance.DomainMasterID);
            ViewBag.iTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ1);
            ViewBag.iTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ2);
            ViewBag.iTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ3);
            ViewBag.iTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ4);
            ViewBag.iTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ5);
            ViewBag.iPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iPQ1);
            ViewBag.iPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iPQ2);
            ViewBag.iCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ1);
            ViewBag.iCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ2);
            ViewBag.iCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ3);
            ViewBag.iAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ1);
            ViewBag.iAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ2);
            ViewBag.iAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ3);
            ViewBag.iAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ4);
            ViewBag.iAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ5);
            ViewBag.iAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ1);
            ViewBag.iAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ2);
            ViewBag.iAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ3);
            ViewBag.iAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ4);
            ViewBag.iAOIQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ5);         
            return View(cFRInsurance);
        }

        // GET: CFRInsurances/Edit/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult InsuranceCFR_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRInsurance cFRInsurance = mb.CFRInsurances.Find(id);
            if (cFRInsurance == null)
            {
                return HttpNotFound();
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRInsurance.DomainMasterID);
            ViewBag.iTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ1);
            ViewBag.iTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ2);
            ViewBag.iTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ3);
            ViewBag.iTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ4);
            ViewBag.iTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ5);
            ViewBag.iPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iPQ1);
            ViewBag.iPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iPQ2);
            ViewBag.iCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ1);
            ViewBag.iCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ2);
            ViewBag.iCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ3);
            ViewBag.iAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ1);
            ViewBag.iAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ2);
            ViewBag.iAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ3);
            ViewBag.iAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ4);
            ViewBag.iAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ5);
            ViewBag.iAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ1);
            ViewBag.iAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ2);
            ViewBag.iAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ3);
            ViewBag.iAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ4);
            ViewBag.iAOIQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ5);
            ViewBag.EmployeeName = cFRInsurance.Employee1.FirstName + " " + cFRInsurance.Employee1.LastName;
            return View(cFRInsurance);
        }

        // POST: CFRInsurances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult InsuranceCFR_Edit([Bind(Include = "CFRInsuranceID,EmployeeID,DomainMasterID,C_Calls,iTEQ1,iTEQ2,iTEQ3,iTEQ4,iTEQ5,iPQ1,iPQ2,iCQ1,iCQ2,iCQ3,iAQ1,iAQ2,iAQ3,iAQ4,iAQ5,iAOIQ1,iAOIQ2,iAOIQ3,iAOIQ4,iAOIQ5,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRInsurance cFRInsurance)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                mb.CFRInsurances.Attach(cFRInsurance);
                mb.Entry(cFRInsurance).Property("DomainMasterID").IsModified = true;
                mb.Entry(cFRInsurance).Property("C_Calls").IsModified = true;
                mb.Entry(cFRInsurance).Property("iTEQ1").IsModified = true;
                mb.Entry(cFRInsurance).Property("iTEQ2").IsModified = true;
                mb.Entry(cFRInsurance).Property("iTEQ3").IsModified = true;
                mb.Entry(cFRInsurance).Property("iTEQ4").IsModified = true;
                mb.Entry(cFRInsurance).Property("iTEQ5").IsModified = true;
                mb.Entry(cFRInsurance).Property("iPQ1").IsModified = true;
                mb.Entry(cFRInsurance).Property("iPQ2").IsModified = true;
                mb.Entry(cFRInsurance).Property("iCQ1").IsModified = true;
                mb.Entry(cFRInsurance).Property("iCQ2").IsModified = true;
                mb.Entry(cFRInsurance).Property("iCQ3").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAQ1").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAQ2").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAQ3").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAQ4").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAQ5").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAOIQ1").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAOIQ2").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAOIQ3").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAOIQ4").IsModified = true;
                mb.Entry(cFRInsurance).Property("iAOIQ5").IsModified = true;
                mb.Entry(cFRInsurance).Property("TelephoneEtiquetteRating").IsModified = true;
                mb.Entry(cFRInsurance).Property("ProfessionalismRating").IsModified = true;
                mb.Entry(cFRInsurance).Property("ComplianceRating").IsModified = true;
                mb.Entry(cFRInsurance).Property("AdheranceRating").IsModified = true;
                mb.Entry(cFRInsurance).Property("AccuracyOfInformationRating").IsModified = true;
                mb.Entry(cFRInsurance).Property("Comments").IsModified = true;
                mb.Entry(cFRInsurance).Property("Strengths").IsModified = true;
                mb.Entry(cFRInsurance).Property("ActionPlan").IsModified = true;
                cFRInsurance.ManagerID = user.EmployeeID;

                // Telephone Etiquette Rating Calculation (INSURANCE)
                int iTER = 0;
                if (cFRInsurance.iTEQ1 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ2 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ3 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ4 == 2)
                {
                    iTER++;
                }
                if (cFRInsurance.iTEQ5 == 2)
                {
                    iTER++;
                }

                if (cFRInsurance.iTEQ1 == 3 && cFRInsurance.iTEQ2 == 3 && cFRInsurance.iTEQ3 == 3 && cFRInsurance.iTEQ4 == 3 && cFRInsurance.iTEQ5 == 3)
                {
                    cFRInsurance.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (iTER == 0)
                    {
                        cFRInsurance.TelephoneEtiquetteRating = 1;
                    }
                    if (iTER > 0 && iTER < 3)
                    {
                        cFRInsurance.TelephoneEtiquetteRating = 2;
                    }
                    if (iTER >= 3)
                    {
                        cFRInsurance.TelephoneEtiquetteRating = 3;
                    }
                }

                // Professionalism Rating Calculation (INSURANCE)
                int iPR = 0;
                if (cFRInsurance.iPQ1 == 2)
                {
                    iPR++;
                }
                if (cFRInsurance.iPQ2 == 2)
                {
                    iPR++;
                }
                if (cFRInsurance.iPQ1 == 3 && cFRInsurance.iPQ2 == 3)
                {
                    cFRInsurance.ProfessionalismRating = 4;
                }
                else
                {
                    if (iPR == 0)
                    {
                        cFRInsurance.ProfessionalismRating = 1;
                    }
                    if (iPR == 1)
                    {
                        cFRInsurance.ProfessionalismRating = 2;
                    }
                    if (iPR >= 2)
                    {
                        cFRInsurance.ProfessionalismRating = 3;
                    }
                }

                // Compliance Rating Calculation (INSURANCE)
                int iCR = 0;
                if (cFRInsurance.iCQ1 == 2)
                {
                    iCR++;
                }
                if (cFRInsurance.iCQ2 == 2)
                {
                    iCR++;
                }
                if (cFRInsurance.iCQ3 == 2)
                {
                    iCR++;
                }

                if (cFRInsurance.iCQ1 == 3 && cFRInsurance.iCQ2 == 3 && cFRInsurance.iCQ3 == 3)
                {
                    cFRInsurance.ComplianceRating = 4;
                }
                else
                {
                    if (iCR == 0)
                    {
                        cFRInsurance.ComplianceRating = 1;
                    }
                    if (iCR >= 1)
                    {
                        cFRInsurance.ComplianceRating = 3;
                    }
                }

                // Adherence Rating Calculation (INSURANCE)
                int iAR = 0;
                if (cFRInsurance.iAQ1 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ2 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ3 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ4 == 2)
                {
                    iAR++;
                }
                if (cFRInsurance.iAQ5 == 2)
                {
                    iAR++;
                }

                if (cFRInsurance.iAQ1 == 3 && cFRInsurance.iAQ2 == 3 && cFRInsurance.iAQ3 == 3 && cFRInsurance.iAQ4 == 3 && cFRInsurance.iAQ5 == 3)
                {
                    cFRInsurance.AdheranceRating = 4;
                }
                else
                {
                    if (iAR == 0)
                    {
                        cFRInsurance.AdheranceRating = 1;
                    }
                    if (iAR > 0 && iAR < 3)
                    {
                        cFRInsurance.AdheranceRating = 2;
                    }
                    if (iAR >= 3)
                    {
                        cFRInsurance.AdheranceRating = 3;
                    }
                }

                // Accuracy of Information Rating Calculation (INSURANCE)
                int iAOIR = 0;
                if (cFRInsurance.iAOIQ1 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ2 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ3 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ4 == 2)
                {
                    iAOIR++;
                }
                if (cFRInsurance.iAOIQ5 == 2)
                {
                    iAOIR++;
                }

                if (cFRInsurance.iAOIQ1 == 3 && cFRInsurance.iAOIQ2 == 3 && cFRInsurance.iAOIQ3 == 3 && cFRInsurance.iAOIQ4 == 3 && cFRInsurance.iAOIQ5 == 3)
                {
                    cFRInsurance.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (iAOIR == 0)
                    {
                        cFRInsurance.AccuracyOfInformationRating = 1;
                    }
                    if (iAOIR >= 1)
                    {
                        cFRInsurance.AccuracyOfInformationRating = 3;
                    }
                }
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRInsurance.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRInsurance.DomainMasterID);
            ViewBag.iTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ1);
            ViewBag.iTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ2);
            ViewBag.iTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ3);
            ViewBag.iTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ4);
            ViewBag.iTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iTEQ5);
            ViewBag.iPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iPQ1);
            ViewBag.iPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iPQ2);
            ViewBag.iCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ1);
            ViewBag.iCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ2);
            ViewBag.iCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iCQ3);
            ViewBag.iAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ1);
            ViewBag.iAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ2);
            ViewBag.iAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ3);
            ViewBag.iAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ4);
            ViewBag.iAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAQ5);
            ViewBag.iAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ1);
            ViewBag.iAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ2);
            ViewBag.iAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ3);
            ViewBag.iAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ4);
            ViewBag.iAOIQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRInsurance.iAOIQ5);
            ViewBag.EmployeeName = cFRInsurance.Employee1.FirstName + " " + cFRInsurance.Employee1.LastName;
            return View(cFRInsurance);
        }

        // GET: CFRInsurances/Delete/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult InsuranceCFR_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRInsurance cFRInsurance = mb.CFRInsurances.Find(id);
            if (cFRInsurance == null)
            {
                return HttpNotFound();
            }
            return View(cFRInsurance);
        }

        // POST: CFRInsurances/Delete/5
        [HttpPost, ActionName("InsuranceCFR_Delete")]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult InsuranceCFR_DeleteConfirmed(int id)
        {
            CFRInsurance cFRInsurance = mb.CFRInsurances.Find(id);
            mb.CFRInsurances.Remove(cFRInsurance);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRInsurance.EmployeeID });
        }

        // GET: CFRPatientRecruitments/Details/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult PatientRecruitmentCFR_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRPatientRecruitment cFRPatientRecruitment = mb.CFRPatientRecruitments.Find(id);
            if (cFRPatientRecruitment == null)
            {
                return HttpNotFound();
            }

            int n = 0;
            double s = 0;
            if (cFRPatientRecruitment.TelephoneEtiquetteRating == 1 || cFRPatientRecruitment.TelephoneEtiquetteRating == 2 || cFRPatientRecruitment.TelephoneEtiquetteRating == 3)
            {
                n++;
            }
            if (cFRPatientRecruitment.ProfessionalismRating == 1 || cFRPatientRecruitment.ProfessionalismRating == 2 || cFRPatientRecruitment.ProfessionalismRating == 3)
            {
                n++;
            }
            if (cFRPatientRecruitment.ComplianceRating == 1 || cFRPatientRecruitment.ComplianceRating == 2 || cFRPatientRecruitment.ComplianceRating == 3)
            {
                n++;
            }
            if (cFRPatientRecruitment.AdheranceRating == 1 || cFRPatientRecruitment.AdheranceRating == 2 || cFRPatientRecruitment.AdheranceRating == 3)
            {
                n++;
            }
            if (cFRPatientRecruitment.AccuracyOfInformationRating == 1 || cFRPatientRecruitment.AccuracyOfInformationRating == 2 || cFRPatientRecruitment.AccuracyOfInformationRating == 3)
            {
                n++;
            }

            if (n == 0)
            {
                ViewBag.OverallScore = 100;
            }
            if (n == 1)
            {
                if (cFRPatientRecruitment.TelephoneEtiquetteRating != 4)
                {
                    if (cFRPatientRecruitment.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ProfessionalismRating != 4)
                {
                    if (cFRPatientRecruitment.ProfessionalismRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ComplianceRating != 4)
                {
                    if (cFRPatientRecruitment.ComplianceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AdheranceRating != 4)
                {
                    if (cFRPatientRecruitment.AdheranceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AccuracyOfInformationRating != 4)
                {
                    if (cFRPatientRecruitment.AccuracyOfInformationRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 2)
            {
                if (cFRPatientRecruitment.TelephoneEtiquetteRating != 4)
                {
                    if (cFRPatientRecruitment.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ProfessionalismRating != 4)
                {
                    if (cFRPatientRecruitment.ProfessionalismRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ComplianceRating != 4)
                {
                    if (cFRPatientRecruitment.ComplianceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AdheranceRating != 4)
                {
                    if (cFRPatientRecruitment.AdheranceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AccuracyOfInformationRating != 4)
                {
                    if (cFRPatientRecruitment.AccuracyOfInformationRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 3)
            {
                if (cFRPatientRecruitment.TelephoneEtiquetteRating != 4)
                {
                    if (cFRPatientRecruitment.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ProfessionalismRating != 4)
                {
                    if (cFRPatientRecruitment.ProfessionalismRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ComplianceRating != 4)
                {
                    if (cFRPatientRecruitment.ComplianceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AdheranceRating != 4)
                {
                    if (cFRPatientRecruitment.AdheranceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AccuracyOfInformationRating != 4)
                {
                    if (cFRPatientRecruitment.AccuracyOfInformationRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 4)
            {
                if (cFRPatientRecruitment.TelephoneEtiquetteRating != 4)
                {
                    if (cFRPatientRecruitment.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ProfessionalismRating != 4)
                {
                    if (cFRPatientRecruitment.ProfessionalismRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ComplianceRating != 4)
                {
                    if (cFRPatientRecruitment.ComplianceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AdheranceRating != 4)
                {
                    if (cFRPatientRecruitment.AdheranceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AccuracyOfInformationRating != 4)
                {
                    if (cFRPatientRecruitment.AccuracyOfInformationRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 5)
            {
                if (cFRPatientRecruitment.TelephoneEtiquetteRating != 4)
                {
                    if (cFRPatientRecruitment.TelephoneEtiquetteRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRPatientRecruitment.TelephoneEtiquetteRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ProfessionalismRating != 4)
                {
                    if (cFRPatientRecruitment.ProfessionalismRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRPatientRecruitment.ProfessionalismRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.ComplianceRating != 4)
                {
                    if (cFRPatientRecruitment.ComplianceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRPatientRecruitment.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AdheranceRating != 4)
                {
                    if (cFRPatientRecruitment.AdheranceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRPatientRecruitment.AdheranceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRPatientRecruitment.AccuracyOfInformationRating != 4)
                {
                    if (cFRPatientRecruitment.AccuracyOfInformationRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRPatientRecruitment.AccuracyOfInformationRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            return View(cFRPatientRecruitment);
        }

        // POST: CFRPatientRecruitments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult PatientRecruitmentCFR_Create([Bind(Include = "CFRPatientRecruitmentID,EmployeeID,DomainMasterID,C_Calls,pTEQ1,pTEQ2,pTEQ3,pPQ1,pPQ2,pCQ1,pCQ2,pCQ3,pCQ4,pCQ5,pAQ1,pAQ2,pAQ3,pAQ4,pAQ5,pAOIQ1,pAOIQ2,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRPatientRecruitment cFRPatientRecruitment)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                cFRPatientRecruitment.ManagerID = user.EmployeeID;
                cFRPatientRecruitment.DateOfFeedback = System.DateTime.Now;

                // Telephone Etiquette Rating Calculation (PATIENT RECRUITMENT)
                int pTER = 0;
                if (cFRPatientRecruitment.pTEQ1 == 2)
                {
                    pTER++;
                }
                if (cFRPatientRecruitment.pTEQ2 == 2)
                {
                    pTER++;
                }
                if (cFRPatientRecruitment.pTEQ3 == 2)
                {
                    pTER++;
                }

                if (cFRPatientRecruitment.pTEQ1 == 3 && cFRPatientRecruitment.pTEQ2 == 3 && cFRPatientRecruitment.pTEQ3 == 3)
                {
                    cFRPatientRecruitment.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (pTER == 0)
                    {
                        cFRPatientRecruitment.TelephoneEtiquetteRating = 1;
                    }
                    if (pTER == 1)
                    {
                        cFRPatientRecruitment.TelephoneEtiquetteRating = 2;
                    }
                    if (pTER >= 2)
                    {
                        cFRPatientRecruitment.TelephoneEtiquetteRating = 3;
                    }
                }
                
                // Professionalism Rating Calculation (PATIENT RECRUITMENT)
                int pPR = 0;
                if (cFRPatientRecruitment.pPQ1 == 2)
                {
                    pPR++;
                }
                if (cFRPatientRecruitment.pPQ2 == 2)
                {
                    pPR++;
                }

                if (cFRPatientRecruitment.pPQ1 == 3 && cFRPatientRecruitment.pPQ2 == 3)
                {
                    cFRPatientRecruitment.ProfessionalismRating = 4;
                }
                else
                {
                    if (pPR == 0)
                    {
                        cFRPatientRecruitment.ProfessionalismRating = 1;
                    }
                    if (pPR == 1)
                    {
                        cFRPatientRecruitment.ProfessionalismRating = 2;
                    }
                    if (pPR >= 2)
                    {
                        cFRPatientRecruitment.ProfessionalismRating = 3;
                    }
                }
                
                // Compliance Rating Calculation (PATIENT RECRUITMENT)
                int pCR = 0;
                if (cFRPatientRecruitment.pCQ1 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ2 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ3 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ4 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ5 == 2)
                {
                    pCR++;
                }

                if (cFRPatientRecruitment.pCQ1 == 3 && cFRPatientRecruitment.pCQ2 == 3 && cFRPatientRecruitment.pCQ3 == 3 && cFRPatientRecruitment.pCQ4 == 3 && cFRPatientRecruitment.pCQ5 == 3)
                {
                    cFRPatientRecruitment.ComplianceRating = 4;
                }
                else
                {
                    if (pCR == 0)
                    {
                        cFRPatientRecruitment.ComplianceRating = 1;
                    }
                    if (pCR >= 1)
                    {
                        cFRPatientRecruitment.ComplianceRating = 3;
                    }
                }
                
                // Adherence Rating Calculation (PATIENT RECRUITMENT)
                int pAR = 0;
                if (cFRPatientRecruitment.pAQ1 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ2 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ3 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ4 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ5 == 2)
                {
                    pAR++;
                }

                if (cFRPatientRecruitment.pAQ1 == 3 && cFRPatientRecruitment.pAQ2 == 3 && cFRPatientRecruitment.pAQ3 == 3 && cFRPatientRecruitment.pAQ4 == 3 && cFRPatientRecruitment.pAQ5 == 3)
                {
                    cFRPatientRecruitment.AdheranceRating = 4;
                }
                else
                {
                    if (pAR == 0)
                    {
                        cFRPatientRecruitment.AdheranceRating = 1;
                    }
                    if (pAR >= 1)
                    {
                        cFRPatientRecruitment.AdheranceRating = 3;
                    }
                }
                
                // Accuracy of Information Rating Calculation (PATIENT RECRUITMENT)
                int pAOIR = 0;
                if (cFRPatientRecruitment.pAOIQ1 == 2)
                {
                    pAOIR++;
                }
                if (cFRPatientRecruitment.pAOIQ2 == 2)
                {
                    pAOIR++;
                }

                if (cFRPatientRecruitment.pAOIQ1 == 3 && cFRPatientRecruitment.pAOIQ2 == 3)
                {
                    cFRPatientRecruitment.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (pAOIR == 0)
                    {
                        cFRPatientRecruitment.AccuracyOfInformationRating = 1;
                    }
                    if (pAOIR >= 1)
                    {
                        cFRPatientRecruitment.AccuracyOfInformationRating = 3;
                    }
                }
                
                mb.CFRPatientRecruitments.Add(cFRPatientRecruitment);
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRPatientRecruitment.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRPatientRecruitment.DomainMasterID);
            ViewBag.pTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ1);
            ViewBag.pTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ2);
            ViewBag.pTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ3);
            ViewBag.pPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pPQ1);
            ViewBag.pPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pPQ2);
            ViewBag.pCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ1);
            ViewBag.pCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ2);
            ViewBag.pCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ3);
            ViewBag.pCQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ4);
            ViewBag.pCQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ5);
            ViewBag.pAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ1);
            ViewBag.pAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ2);
            ViewBag.pAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ3);
            ViewBag.pAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ4);
            ViewBag.pAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ5);
            ViewBag.pAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAOIQ1);
            ViewBag.pAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAOIQ2);           
            return View(cFRPatientRecruitment);
        }

        // GET: CFRPatientRecruitments/Edit/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult PatientRecruitmentCFR_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRPatientRecruitment cFRPatientRecruitment = mb.CFRPatientRecruitments.Find(id);
            if (cFRPatientRecruitment == null)
            {
                return HttpNotFound();
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRPatientRecruitment.DomainMasterID);
            ViewBag.pTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ1);
            ViewBag.pTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ2);
            ViewBag.pTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ3);
            ViewBag.pPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pPQ1);
            ViewBag.pPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pPQ2);
            ViewBag.pCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ1);
            ViewBag.pCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ2);
            ViewBag.pCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ3);
            ViewBag.pCQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ4);
            ViewBag.pCQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ5);
            ViewBag.pAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ1);
            ViewBag.pAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ2);
            ViewBag.pAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ3);
            ViewBag.pAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ4);
            ViewBag.pAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ5);
            ViewBag.pAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAOIQ1);
            ViewBag.pAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAOIQ2);
            ViewBag.EmployeeName = cFRPatientRecruitment.Employee1.FirstName + " " + cFRPatientRecruitment.Employee1.LastName;
            return View(cFRPatientRecruitment);
        }

        // POST: CFRPatientRecruitments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult PatientRecruitmentCFR_Edit([Bind(Include = "CFRPatientRecruitmentID,EmployeeID,DomainMasterID,C_Calls,pTEQ1,pTEQ2,pTEQ3,pPQ1,pPQ2,pCQ1,pCQ2,pCQ3,pCQ4,pCQ5,pAQ1,pAQ2,pAQ3,pAQ4,pAQ5,pAOIQ1,pAOIQ2,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRPatientRecruitment cFRPatientRecruitment)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                mb.CFRPatientRecruitments.Attach(cFRPatientRecruitment);
                mb.Entry(cFRPatientRecruitment).Property("DomainMasterID").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("C_Calls").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pTEQ1").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pTEQ2").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pTEQ3").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pPQ1").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pPQ2").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pCQ1").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pCQ2").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pCQ3").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pCQ4").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pCQ5").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pAQ1").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pAQ2").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pAQ3").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pAQ4").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pAQ5").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pAOIQ1").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("pAOIQ2").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("TelephoneEtiquetteRating").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("ProfessionalismRating").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("ComplianceRating").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("AdheranceRating").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("AccuracyOfInformationRating").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("Comments").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("Strengths").IsModified = true;
                mb.Entry(cFRPatientRecruitment).Property("ActionPlan").IsModified = true;
                cFRPatientRecruitment.ManagerID = user.EmployeeID;

                // Telephone Etiquette Rating Calculation (PATIENT RECRUITMENT)
                int pTER = 0;
                if (cFRPatientRecruitment.pTEQ1 == 2)
                {
                    pTER++;
                }
                if (cFRPatientRecruitment.pTEQ2 == 2)
                {
                    pTER++;
                }
                if (cFRPatientRecruitment.pTEQ3 == 2)
                {
                    pTER++;
                }

                if (cFRPatientRecruitment.pTEQ1 == 3 && cFRPatientRecruitment.pTEQ2 == 3 && cFRPatientRecruitment.pTEQ3 == 3)
                {
                    cFRPatientRecruitment.TelephoneEtiquetteRating = 4;
                }
                else
                {
                    if (pTER == 0)
                    {
                        cFRPatientRecruitment.TelephoneEtiquetteRating = 1;
                    }
                    if (pTER == 1)
                    {
                        cFRPatientRecruitment.TelephoneEtiquetteRating = 2;
                    }
                    if (pTER >= 2)
                    {
                        cFRPatientRecruitment.TelephoneEtiquetteRating = 3;
                    }
                }

                // Professionalism Rating Calculation (PATIENT RECRUITMENT)
                int pPR = 0;
                if (cFRPatientRecruitment.pPQ1 == 2)
                {
                    pPR++;
                }
                if (cFRPatientRecruitment.pPQ2 == 2)
                {
                    pPR++;
                }

                if (cFRPatientRecruitment.pPQ1 == 3 && cFRPatientRecruitment.pPQ2 == 3)
                {
                    cFRPatientRecruitment.ProfessionalismRating = 4;
                }
                else
                {
                    if (pPR == 0)
                    {
                        cFRPatientRecruitment.ProfessionalismRating = 1;
                    }
                    if (pPR == 1)
                    {
                        cFRPatientRecruitment.ProfessionalismRating = 2;
                    }
                    if (pPR >= 2)
                    {
                        cFRPatientRecruitment.ProfessionalismRating = 3;
                    }
                }

                // Compliance Rating Calculation (PATIENT RECRUITMENT)
                int pCR = 0;
                if (cFRPatientRecruitment.pCQ1 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ2 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ3 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ4 == 2)
                {
                    pCR++;
                }
                if (cFRPatientRecruitment.pCQ5 == 2)
                {
                    pCR++;
                }

                if (cFRPatientRecruitment.pCQ1 == 3 && cFRPatientRecruitment.pCQ2 == 3 && cFRPatientRecruitment.pCQ3 == 3 && cFRPatientRecruitment.pCQ4 == 3 && cFRPatientRecruitment.pCQ5 == 3)
                {
                    cFRPatientRecruitment.ComplianceRating = 4;
                }
                else
                {
                    if (pCR == 0)
                    {
                        cFRPatientRecruitment.ComplianceRating = 1;
                    }
                    if (pCR >= 1)
                    {
                        cFRPatientRecruitment.ComplianceRating = 3;
                    }
                }

                // Adherence Rating Calculation (PATIENT RECRUITMENT)
                int pAR = 0;
                if (cFRPatientRecruitment.pAQ1 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ2 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ3 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ4 == 2)
                {
                    pAR++;
                }
                if (cFRPatientRecruitment.pAQ5 == 2)
                {
                    pAR++;
                }

                if (cFRPatientRecruitment.pAQ1 == 3 && cFRPatientRecruitment.pAQ2 == 3 && cFRPatientRecruitment.pAQ3 == 3 && cFRPatientRecruitment.pAQ4 == 3 && cFRPatientRecruitment.pAQ5 == 3)
                {
                    cFRPatientRecruitment.AdheranceRating = 4;
                }
                else
                {
                    if (pAR == 0)
                    {
                        cFRPatientRecruitment.AdheranceRating = 1;
                    }
                    if (pAR >= 1)
                    {
                        cFRPatientRecruitment.AdheranceRating = 3;
                    }
                }

                // Accuracy of Information Rating Calculation (PATIENT RECRUITMENT)
                int pAOIR = 0;
                if (cFRPatientRecruitment.pAOIQ1 == 2)
                {
                    pAOIR++;
                }
                if (cFRPatientRecruitment.pAOIQ2 == 2)
                {
                    pAOIR++;
                }

                if (cFRPatientRecruitment.pAOIQ1 == 3 && cFRPatientRecruitment.pAOIQ2 == 3)
                {
                    cFRPatientRecruitment.AccuracyOfInformationRating = 4;
                }
                else
                {
                    if (pAOIR == 0)
                    {
                        cFRPatientRecruitment.AccuracyOfInformationRating = 1;
                    }
                    if (pAOIR >= 1)
                    {
                        cFRPatientRecruitment.AccuracyOfInformationRating = 3;
                    }
                }
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRPatientRecruitment.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRPatientRecruitment.DomainMasterID);
            ViewBag.pTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ1);
            ViewBag.pTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ2);
            ViewBag.pTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pTEQ3);
            ViewBag.pPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pPQ1);
            ViewBag.pPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pPQ2);
            ViewBag.pCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ1);
            ViewBag.pCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ2);
            ViewBag.pCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ3);
            ViewBag.pCQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ4);
            ViewBag.pCQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pCQ5);
            ViewBag.pAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ1);
            ViewBag.pAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ2);
            ViewBag.pAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ3);
            ViewBag.pAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ4);
            ViewBag.pAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAQ5);
            ViewBag.pAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAOIQ1);
            ViewBag.pAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRPatientRecruitment.pAOIQ2);
            ViewBag.EmployeeName = cFRPatientRecruitment.Employee1.FirstName + " " + cFRPatientRecruitment.Employee1.LastName;
            return View(cFRPatientRecruitment);
        }

        // GET: CFRPatientRecruitments/Delete/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult PatientRecruitmentCFR_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRPatientRecruitment cFRPatientRecruitment = mb.CFRPatientRecruitments.Find(id);
            if (cFRPatientRecruitment == null)
            {
                return HttpNotFound();
            }
            return View(cFRPatientRecruitment);
        }

        // POST: CFRPatientRecruitments/Delete/5
        [HttpPost, ActionName("PatientRecruitmentCFR_Delete")]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult PatientRecruitmentCFR_DeleteConfirmed(int id)
        {
            CFRPatientRecruitment cFRPatientRecruitment = mb.CFRPatientRecruitments.Find(id);
            mb.CFRPatientRecruitments.Remove(cFRPatientRecruitment);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRPatientRecruitment.EmployeeID });
        }

        // GET: CFRAcurians/Details/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult AcurianCFR_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRAcurian cFRAcurian = mb.CFRAcurians.Find(id);
            if (cFRAcurian == null)
            {
                return HttpNotFound();
            }

            int n = 0;
            double s = 0;
            if (cFRAcurian.IntroductionRating == 1 || cFRAcurian.IntroductionRating == 2 || cFRAcurian.IntroductionRating == 3)
            {
                n++;
            }
            if (cFRAcurian.CommunicationSkillsRating == 1 || cFRAcurian.CommunicationSkillsRating == 2 || cFRAcurian.CommunicationSkillsRating == 3)
            {
                n++;
            }
            if (cFRAcurian.SoftSkillsRating == 1 || cFRAcurian.SoftSkillsRating == 2 || cFRAcurian.SoftSkillsRating == 3)
            {
                n++;
            }
            if (cFRAcurian.ComplianceRating == 1 || cFRAcurian.ComplianceRating == 2 || cFRAcurian.ComplianceRating == 3)
            {
                n++;
            }
            if (cFRAcurian.ClosingRating == 1 || cFRAcurian.ClosingRating == 2 || cFRAcurian.ClosingRating == 3)
            {
                n++;
            }

            if (n == 0)
            {
                ViewBag.OverallScore = "N/A";
            }
            if (n == 1)
            {
                if (cFRAcurian.IntroductionRating != 4)
                {
                    if (cFRAcurian.IntroductionRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRAcurian.IntroductionRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.IntroductionRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.CommunicationSkillsRating != 4)
                {
                    if (cFRAcurian.CommunicationSkillsRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.SoftSkillsRating != 4)
                {
                    if (cFRAcurian.SoftSkillsRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ComplianceRating != 4)
                {
                    if (cFRAcurian.ComplianceRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRAcurian.ComplianceRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ClosingRating != 4)
                {
                    if (cFRAcurian.ClosingRating == 1)
                    {
                        s = s + 100;
                    }
                    else if (cFRAcurian.ClosingRating == 2)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.ClosingRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 2)
            {
                if (cFRAcurian.IntroductionRating != 4)
                {
                    if (cFRAcurian.IntroductionRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.IntroductionRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.IntroductionRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.CommunicationSkillsRating != 4)
                {
                    if (cFRAcurian.CommunicationSkillsRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.SoftSkillsRating != 4)
                {
                    if (cFRAcurian.SoftSkillsRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ComplianceRating != 4)
                {
                    if (cFRAcurian.ComplianceRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.ComplianceRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ClosingRating != 4)
                {
                    if (cFRAcurian.ClosingRating == 1)
                    {
                        s = s + 50;
                    }
                    else if (cFRAcurian.ClosingRating == 2)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.ClosingRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 3)
            {
                if (cFRAcurian.IntroductionRating != 4)
                {
                    if (cFRAcurian.IntroductionRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRAcurian.IntroductionRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRAcurian.IntroductionRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.CommunicationSkillsRating != 4)
                {
                    if (cFRAcurian.CommunicationSkillsRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.SoftSkillsRating != 4)
                {
                    if (cFRAcurian.SoftSkillsRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ComplianceRating != 4)
                {
                    if (cFRAcurian.ComplianceRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRAcurian.ComplianceRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRAcurian.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ClosingRating != 4)
                {
                    if (cFRAcurian.ClosingRating == 1)
                    {
                        s = s + 33.33;
                    }
                    else if (cFRAcurian.ClosingRating == 2)
                    {
                        s = s + 16.67;
                    }
                    else if (cFRAcurian.ClosingRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 4)
            {
                if (cFRAcurian.IntroductionRating != 4)
                {
                    if (cFRAcurian.IntroductionRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.IntroductionRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRAcurian.IntroductionRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.CommunicationSkillsRating != 4)
                {
                    if (cFRAcurian.CommunicationSkillsRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.SoftSkillsRating != 4)
                {
                    if (cFRAcurian.SoftSkillsRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ComplianceRating != 4)
                {
                    if (cFRAcurian.ComplianceRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.ComplianceRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRAcurian.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ClosingRating != 4)
                {
                    if (cFRAcurian.ClosingRating == 1)
                    {
                        s = s + 25;
                    }
                    else if (cFRAcurian.ClosingRating == 2)
                    {
                        s = s + 12.5;
                    }
                    else if (cFRAcurian.ClosingRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            else if (n == 5)
            {
                if (cFRAcurian.IntroductionRating != 4)
                {
                    if (cFRAcurian.IntroductionRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRAcurian.IntroductionRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRAcurian.IntroductionRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.CommunicationSkillsRating != 4)
                {
                    if (cFRAcurian.CommunicationSkillsRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRAcurian.CommunicationSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.SoftSkillsRating != 4)
                {
                    if (cFRAcurian.SoftSkillsRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRAcurian.SoftSkillsRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ComplianceRating != 4)
                {
                    if (cFRAcurian.ComplianceRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRAcurian.ComplianceRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRAcurian.ComplianceRating == 3)
                    {
                        s = s + 0;
                    }
                }
                if (cFRAcurian.ClosingRating != 4)
                {
                    if (cFRAcurian.ClosingRating == 1)
                    {
                        s = s + 20;
                    }
                    else if (cFRAcurian.ClosingRating == 2)
                    {
                        s = s + 10;
                    }
                    else if (cFRAcurian.ClosingRating == 3)
                    {
                        s = s + 0;
                    }
                }
                ViewBag.OverallScore = Math.Round(s);
            }
            return View(cFRAcurian);
        }

        // POST: CFRAcurians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult AcurianCFR_Create([Bind(Include = "CFRAcurianID,EmployeeID,DomainMasterID,C_Calls,aIQ1,aIQ2,aIQ3,aCSQ1,aCSQ2,aCSQ3,aCSQ4,aCSQ5,aCSQ6,aCSQ7,aSSQ1,aSSQ2,aSSQ3,aSSQ4,aSSQ5,aSSQ6,aSSQ7,aSSQ8,aCOQ1,aCOQ2,aCOQ3,aCOQ4,aCOQ5,aCOQ6,aCOQ7,aCOQ8,aCLQ1,aCLQ2,aCLQ3,IntroductionRating,CommunicationSkillsRating,SoftSkillsRating,ComplianceRating,ClosingRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRAcurian cFRAcurian)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                cFRAcurian.ManagerID = user.EmployeeID;
                cFRAcurian.DateOfFeedback = System.DateTime.Now;

                // Introduction Rating Calculation (ACURIAN)
                int aIR = 0;
                if (cFRAcurian.aIQ1 == 2)
                {
                    aIR++;
                }
                if (cFRAcurian.aIQ2 == 2)
                {
                    aIR++;
                }
                if (cFRAcurian.aIQ3 == 2)
                {
                    aIR++;
                }

                if (cFRAcurian.aIQ1 == 3 && cFRAcurian.aIQ2 == 3 && cFRAcurian.aIQ3 == 3)
                {
                    cFRAcurian.IntroductionRating = 4;
                }
                else
                {
                    if (aIR == 0)
                    {
                        cFRAcurian.IntroductionRating = 1;
                    }
                    if (aIR > 0 && aIR < 2)
                    {
                        cFRAcurian.IntroductionRating = 2;
                    }
                    if (aIR >= 2)
                    {
                        cFRAcurian.IntroductionRating = 3;
                    }
                }

                // Communication Skills Rating Calculation (ACURIAN)
                int aCSR = 0;
                if (cFRAcurian.aCSQ1 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ2 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ3 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ4 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ5 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ6 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ7 == 2)
                {
                    aCSR++;
                }

                if (cFRAcurian.aCSQ1 == 3 && cFRAcurian.aCSQ2 == 3 && cFRAcurian.aCSQ1 == 3 && cFRAcurian.aCSQ2 == 3 && cFRAcurian.aCSQ3 == 3 && cFRAcurian.aCSQ4 == 3 && cFRAcurian.aCSQ5 == 3 && cFRAcurian.aCSQ6 == 3 && cFRAcurian.aCSQ7 == 3)
                {
                    cFRAcurian.CommunicationSkillsRating = 4;
                }
                else
                {
                    if (aCSR == 0)
                    {
                        cFRAcurian.CommunicationSkillsRating = 1;
                    }
                    if (aCSR > 0 && aCSR < 3)
                    {
                        cFRAcurian.CommunicationSkillsRating = 2;
                    }
                    if (aCSR >= 3)
                    {
                        cFRAcurian.CommunicationSkillsRating = 3;
                    }
                }

                // Soft Skills Rating Calculation (ACURIAN)
                int aSSR = 0;
                if (cFRAcurian.aSSQ1 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ2 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ3 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ4 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ5 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ6 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ7 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ8 == 2)
                {
                    aSSR++;
                }

                if (cFRAcurian.aSSQ1 == 3 && cFRAcurian.aSSQ2 == 3 && cFRAcurian.aSSQ3 == 3 && cFRAcurian.aSSQ4 == 3 && cFRAcurian.aSSQ5 == 3 && cFRAcurian.aSSQ6 == 3 && cFRAcurian.aSSQ7 == 3 && cFRAcurian.aSSQ8 == 3)
                {
                    cFRAcurian.SoftSkillsRating = 4;
                }
                else
                {
                    if (aSSR == 0)
                    {
                        cFRAcurian.SoftSkillsRating = 1;
                    }
                    if (aSSR > 0 && aSSR < 4)
                    {
                        cFRAcurian.SoftSkillsRating = 2;
                    }
                    if (aSSR >= 4)
                    {
                        cFRAcurian.SoftSkillsRating = 3;
                    }
                }

                // Compliance Rating Calculation (ACURIAN)
                int aCOR = 0;
                if (cFRAcurian.aCOQ1 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ2 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ3 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ4 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ5 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ6 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ7 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ8 == 2)
                {
                    aCOR++;
                }

                if (cFRAcurian.aCOQ1 == 3 && cFRAcurian.aCOQ2 == 3 && cFRAcurian.aCOQ3 == 3 && cFRAcurian.aCOQ4 == 3 && cFRAcurian.aCOQ5 == 3 && cFRAcurian.aCOQ6 == 3 && cFRAcurian.aCOQ7 == 3 && cFRAcurian.aCOQ8 == 3)
                {
                    cFRAcurian.ComplianceRating = 4;
                }
                else
                {
                    if (aCOR == 0)
                    {
                        cFRAcurian.ComplianceRating = 1;
                    }
                    if (aCOR == 1 && cFRAcurian.aCOQ8 == 2)
                    {
                        cFRAcurian.ComplianceRating = 2;
                    }
                    if ((aCOR == 1 && cFRAcurian.aCOQ8 != 2) || aCOR > 1)
                    {
                        cFRAcurian.ComplianceRating = 3;
                    }
                }

                // Closing Rating Calculation (ACURIAN)
                int aCLR = 0;
                if (cFRAcurian.aCLQ1 == 2)
                {
                    aCLR++;
                }
                if (cFRAcurian.aCLQ2 == 2)
                {
                    aCLR++;
                }
                if (cFRAcurian.aCLQ3 == 2)
                {
                    aCLR++;
                }

                if (cFRAcurian.aCLQ1 == 3 && cFRAcurian.aCLQ2 == 3 && cFRAcurian.aCLQ3 == 3)
                {
                    cFRAcurian.ClosingRating = 4;
                }
                else
                {
                    if (aCLR == 0)
                    {
                        cFRAcurian.ClosingRating = 1;
                    }
                    if (aCLR > 0 && aCLR < 2)
                    {
                        cFRAcurian.ClosingRating = 2;
                    }
                    if (aCLR >= 2)
                    {
                        cFRAcurian.ClosingRating = 3;
                    }
                }

                mb.CFRAcurians.Add(cFRAcurian);
                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRAcurian.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRAcurian.DomainMasterID);
            ViewBag.aIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ1);
            ViewBag.aIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ2);
            ViewBag.aIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ3);
            ViewBag.aCSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ1);
            ViewBag.aCSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ2);
            ViewBag.aCSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ3);
            ViewBag.aCSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ4);
            ViewBag.aCSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ5);
            ViewBag.aCSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ6);
            ViewBag.aCSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ7);
            ViewBag.aSSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ1);
            ViewBag.aSSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ2);
            ViewBag.aSSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ3);
            ViewBag.aSSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ4);
            ViewBag.aSSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ5);
            ViewBag.aSSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ6);
            ViewBag.aSSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ7);
            ViewBag.aSSQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ8);
            ViewBag.aCOQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ1);
            ViewBag.aCOQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ2);
            ViewBag.aCOQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ3);
            ViewBag.aCOQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ4);
            ViewBag.aCOQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ5);
            ViewBag.aCOQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ6);
            ViewBag.aCOQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ7);
            ViewBag.aCOQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ8);
            ViewBag.aCLQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ1);
            ViewBag.aCLQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ2);
            ViewBag.aCLQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ3);
            return View(cFRAcurian);
        }

        // GET: CFRAcurians/Edit/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult AcurianCFR_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRAcurian cFRAcurian = mb.CFRAcurians.Find(id);
            if (cFRAcurian == null)
            {
                return HttpNotFound();
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRAcurian.DomainMasterID);
            ViewBag.aIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ1);
            ViewBag.aIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ2);
            ViewBag.aIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ3);
            ViewBag.aCSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ1);
            ViewBag.aCSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ2);
            ViewBag.aCSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ3);
            ViewBag.aCSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ4);
            ViewBag.aCSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ5);
            ViewBag.aCSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ6);
            ViewBag.aCSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ7);
            ViewBag.aSSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ1);
            ViewBag.aSSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ2);
            ViewBag.aSSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ3);
            ViewBag.aSSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ4);
            ViewBag.aSSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ5);
            ViewBag.aSSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ6);
            ViewBag.aSSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ7);
            ViewBag.aSSQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ8);
            ViewBag.aCOQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ1);
            ViewBag.aCOQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ2);
            ViewBag.aCOQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ3);
            ViewBag.aCOQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ4);
            ViewBag.aCOQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ5);
            ViewBag.aCOQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ6);
            ViewBag.aCOQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ7);
            ViewBag.aCOQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ8);
            ViewBag.aCLQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ1);
            ViewBag.aCLQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ2);
            ViewBag.aCLQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ3);
            ViewBag.EmployeeName = cFRAcurian.Employee.FirstName + " " + cFRAcurian.Employee.LastName;
            return View(cFRAcurian);
        }

        // POST: CFRAcurians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult AcurianCFR_Edit([Bind(Include = "CFRAcurianID,EmployeeID,DomainMasterID,C_Calls,aIQ1,aIQ2,aIQ3,aCSQ1,aCSQ2,aCSQ3,aCSQ4,aCSQ5,aCSQ6,aCSQ7,aSSQ1,aSSQ2,aSSQ3,aSSQ4,aSSQ5,aSSQ6,aSSQ7,aSSQ8,aCOQ1,aCOQ2,aCOQ3,aCOQ4,aCOQ5,aCOQ6,aCOQ7,aCOQ8,aCLQ1,aCLQ2,aCLQ3,IntroductionRating,CommunicationSkillsRating,SoftSkillsRating,ComplianceRating,ClosingRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRAcurian cFRAcurian)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                mb.CFRAcurians.Attach(cFRAcurian);
                mb.Entry(cFRAcurian).Property("C_Calls").IsModified = true;
                mb.Entry(cFRAcurian).Property("aIQ1").IsModified = true;
                mb.Entry(cFRAcurian).Property("aIQ2").IsModified = true;
                mb.Entry(cFRAcurian).Property("aIQ3").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCSQ1").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCSQ2").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCSQ3").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCSQ4").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCSQ5").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCSQ6").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCSQ7").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ1").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ2").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ3").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ4").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ5").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ6").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ7").IsModified = true;
                mb.Entry(cFRAcurian).Property("aSSQ8").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ1").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ2").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ3").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ4").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ5").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ6").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ7").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCOQ8").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCLQ1").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCLQ2").IsModified = true;
                mb.Entry(cFRAcurian).Property("aCLQ3").IsModified = true;
                mb.Entry(cFRAcurian).Property("IntroductionRating").IsModified = true;
                mb.Entry(cFRAcurian).Property("CommunicationSkillsRating").IsModified = true;
                mb.Entry(cFRAcurian).Property("SoftSkillsRating").IsModified = true;
                mb.Entry(cFRAcurian).Property("ComplianceRating").IsModified = true;
                mb.Entry(cFRAcurian).Property("ClosingRating").IsModified = true;
                mb.Entry(cFRAcurian).Property("Comments").IsModified = true;
                mb.Entry(cFRAcurian).Property("Strengths").IsModified = true;
                mb.Entry(cFRAcurian).Property("ActionPlan").IsModified = true;
                cFRAcurian.ManagerID = user.EmployeeID;

                // Introduction Rating Calculation (ACURIAN)
                int aIR = 0;
                if (cFRAcurian.aIQ1 == 2)
                {
                    aIR++;
                }
                if (cFRAcurian.aIQ2 == 2)
                {
                    aIR++;
                }
                if (cFRAcurian.aIQ3 == 2)
                {
                    aIR++;
                }

                if (cFRAcurian.aIQ1 == 3 && cFRAcurian.aIQ2 == 3 && cFRAcurian.aIQ3 == 3)
                {
                    cFRAcurian.IntroductionRating = 4;
                }
                else
                {
                    if (aIR == 0)
                    {
                        cFRAcurian.IntroductionRating = 1;
                    }
                    if (aIR > 0 && aIR < 2)
                    {
                        cFRAcurian.IntroductionRating = 2;
                    }
                    if (aIR >= 2)
                    {
                        cFRAcurian.IntroductionRating = 3;
                    }
                }

                // Communication Skills Rating Calculation (ACURIAN)
                int aCSR = 0;
                if (cFRAcurian.aCSQ1 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ2 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ3 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ4 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ5 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ6 == 2)
                {
                    aCSR++;
                }
                if (cFRAcurian.aCSQ7 == 2)
                {
                    aCSR++;
                }

                if (cFRAcurian.aCSQ1 == 3 && cFRAcurian.aCSQ2 == 3 && cFRAcurian.aCSQ1 == 3 && cFRAcurian.aCSQ2 == 3 && cFRAcurian.aCSQ3 == 3 && cFRAcurian.aCSQ4 == 3 && cFRAcurian.aCSQ5 == 3 && cFRAcurian.aCSQ6 == 3 && cFRAcurian.aCSQ7 == 3)
                {
                    cFRAcurian.CommunicationSkillsRating = 4;
                }
                else
                {
                    if (aCSR == 0)
                    {
                        cFRAcurian.CommunicationSkillsRating = 1;
                    }
                    if (aCSR > 0 && aCSR < 3)
                    {
                        cFRAcurian.CommunicationSkillsRating = 2;
                    }
                    if (aCSR >= 3)
                    {
                        cFRAcurian.CommunicationSkillsRating = 3;
                    }
                }

                // Soft Skills Rating Calculation (ACURIAN)
                int aSSR = 0;
                if (cFRAcurian.aSSQ1 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ2 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ3 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ4 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ5 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ6 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ7 == 2)
                {
                    aSSR++;
                }
                if (cFRAcurian.aSSQ8 == 2)
                {
                    aSSR++;
                }

                if (cFRAcurian.aSSQ1 == 3 && cFRAcurian.aSSQ2 == 3 && cFRAcurian.aSSQ3 == 3 && cFRAcurian.aSSQ4 == 3 && cFRAcurian.aSSQ5 == 3 && cFRAcurian.aSSQ6 == 3 && cFRAcurian.aSSQ7 == 3 && cFRAcurian.aSSQ8 == 3)
                {
                    cFRAcurian.SoftSkillsRating = 4;
                }
                else
                {
                    if (aSSR == 0)
                    {
                        cFRAcurian.SoftSkillsRating = 1;
                    }
                    if (aSSR > 0 && aSSR < 4)
                    {
                        cFRAcurian.SoftSkillsRating = 2;
                    }
                    if (aSSR >= 4)
                    {
                        cFRAcurian.SoftSkillsRating = 3;
                    }
                }

                // Compliance Rating Calculation (ACURIAN)
                int aCOR = 0;
                if (cFRAcurian.aCOQ1 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ2 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ3 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ4 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ5 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ6 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ7 == 2)
                {
                    aCOR++;
                }
                if (cFRAcurian.aCOQ8 == 2)
                {
                    aCOR++;
                }

                if (cFRAcurian.aCOQ1 == 3 && cFRAcurian.aCOQ2 == 3 && cFRAcurian.aCOQ3 == 3 && cFRAcurian.aCOQ4 == 3 && cFRAcurian.aCOQ5 == 3 && cFRAcurian.aCOQ6 == 3 && cFRAcurian.aCOQ7 == 3 && cFRAcurian.aCOQ8 == 3)
                {
                    cFRAcurian.ComplianceRating = 4;
                }
                else
                {
                    if (aCOR == 0)
                    {
                        cFRAcurian.ComplianceRating = 1;
                    }
                    if (aCOR == 1 && cFRAcurian.aCOQ8 == 2)
                    {
                        cFRAcurian.ComplianceRating = 2;
                    }
                    if ((aCOR == 1 && cFRAcurian.aCOQ8 != 2) || aCOR > 1)
                    {
                        cFRAcurian.ComplianceRating = 3;
                    }
                }

                // Closing Rating Calculation (ACURIAN)
                int aCLR = 0;
                if (cFRAcurian.aCLQ1 == 2)
                {
                    aCLR++;
                }
                if (cFRAcurian.aCLQ2 == 2)
                {
                    aCLR++;
                }
                if (cFRAcurian.aCLQ3 == 2)
                {
                    aCLR++;
                }

                if (cFRAcurian.aCLQ1 == 3 && cFRAcurian.aCLQ2 == 3 && cFRAcurian.aCLQ3 == 3)
                {
                    cFRAcurian.ClosingRating = 4;
                }
                else
                {
                    if (aCLR == 0)
                    {
                        cFRAcurian.ClosingRating = 1;
                    }
                    if (aCLR > 0 && aCLR < 2)
                    {
                        cFRAcurian.ClosingRating = 2;
                    }
                    if (aCLR >= 2)
                    {
                        cFRAcurian.ClosingRating = 3;
                    }
                }

                mb.SaveChanges();
                return RedirectToAction("Details", "Employees", new { id = cFRAcurian.EmployeeID });
            }

            var domains = new List<Domain>();
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName", cFRAcurian.DomainMasterID);
            ViewBag.aIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ1);
            ViewBag.aIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ2);
            ViewBag.aIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aIQ3);
            ViewBag.aCSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ1);
            ViewBag.aCSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ2);
            ViewBag.aCSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ3);
            ViewBag.aCSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ4);
            ViewBag.aCSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ5);
            ViewBag.aCSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ6);
            ViewBag.aCSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCSQ7);
            ViewBag.aSSQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ1);
            ViewBag.aSSQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ2);
            ViewBag.aSSQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ3);
            ViewBag.aSSQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ4);
            ViewBag.aSSQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ5);
            ViewBag.aSSQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ6);
            ViewBag.aSSQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ7);
            ViewBag.aSSQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aSSQ8);
            ViewBag.aCOQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ1);
            ViewBag.aCOQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ2);
            ViewBag.aCOQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ3);
            ViewBag.aCOQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ4);
            ViewBag.aCOQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ5);
            ViewBag.aCOQ6 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ6);
            ViewBag.aCOQ7 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ7);
            ViewBag.aCOQ8 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCOQ8);
            ViewBag.aCLQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ1);
            ViewBag.aCLQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ2);
            ViewBag.aCLQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRAcurian.aCLQ3);
            ViewBag.EmployeeName = cFRAcurian.Employee.FirstName + " " + cFRAcurian.Employee.LastName;
            return View(cFRAcurian);
        }

        // GET: CFRAcurians/Delete/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult AcurianCFR_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CFRAcurian cFRAcurian = mb.CFRAcurians.Find(id);
            if (cFRAcurian == null)
            {
                return HttpNotFound();
            }
            return View(cFRAcurian);
        }

        // POST: CFRAcurians/Delete/5
        [HttpPost, ActionName("AcurianCFR_Delete")]
        [Authorize(Roles = "Admin, Quality")]
        [ValidateAntiForgeryToken]
        public ActionResult AcurianCFR_DeleteConfirmed(int id)
        {
            CFRAcurian cFRAcurian = mb.CFRAcurians.Find(id);
            mb.CFRAcurians.Remove(cFRAcurian);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRAcurian.EmployeeID });
        }

        //// POST: Employees/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, HR, Operations")]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployeeFile(EmployeeFile eFile, IEnumerable<HttpPostedFileBase> file, int empId)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            bool directoryExists;
            string directory = "http://192.168.1.8:88/ASPortal/EmployeeFiles/" + empId + "/";

            var request = (HttpWebRequest)WebRequest.Create(directory);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential("intranet", "MyDevils#1");

            try
            {
                using (request.GetResponse())
                {
                    directoryExists = true;
                }
            }

            catch (WebException)
            {
                directoryExists = false;
            }

            if (directoryExists == false)
            {
                var path = Server.MapPath("~/EmployeeFiles/" + empId);
                Directory.CreateDirectory(path);
            }

            foreach (var doc in file)
            {
                eFile.Created = System.DateTime.Now;
                eFile.AuthorId = user.Id;
                eFile.EmployeeID = empId;
                //Counter
                var num = 0;
                //Gets Filename without the extension
                var fileName = Path.GetFileNameWithoutExtension(doc.FileName);
                var gPic = Path.Combine("/EmployeeFiles/" + empId, fileName + Path.GetExtension(doc.FileName));
                //Checks if pPic matches any of the current attachments, 
                //if so it will loop and add a (number) to the end of the filename
                while (db.EmployeeFiles.Where(p => p.EmployeeID == empId).Any(p => p.File == gPic))
                {
                    //Sets "filename" back to the default value
                    fileName = Path.GetFileNameWithoutExtension(doc.FileName);
                    //Add's parentheses after the name with a number ex. filename(4)
                    fileName = string.Format(fileName + "(" + ++num + ")");
                    //Makes sure pPic gets updated with the new filename so it could check
                    gPic = Path.Combine("/EmployeeFiles/" + empId, fileName + Path.GetExtension(doc.FileName));
                }
                doc.SaveAs(Path.Combine(Server.MapPath("~/EmployeeFiles/" + empId), fileName + Path.GetExtension(doc.FileName)));

                eFile.File = gPic;
                db.EmployeeFiles.Add(eFile);
                db.SaveChanges();

                var emp = mb.Employees.First(e => e.EmployeeID == empId);

                foreach (var HRuser in db.Users.Where(u => u.Roles.Any(r => r.RoleId == "cf0c9cdc-c2d7-4abf-9da7-72b5d4245348")).ToList())
                {
                    Notification n = new Notification()
                    {
                        EmployeeID = empId,
                        NotificationTypeId = 1,
                        Created = System.DateTime.Now,
                        Description = "A new employee file was added for " + emp.FirstName + " " + emp.LastName + ".",
                        Additional = fileName + Path.GetExtension(doc.FileName),
                        CorrespondingItemId = eFile.Id,
                        NotifyUserId = HRuser.Id,
                        New = true
                    };
                    db.Notifications.Add(n);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Details", "Employees", new { id = empId });
        }

        [Authorize(Roles = "Admin, HR")]
        public ActionResult DeleteEmployeeFile(int id, int empId)
        {
            var eFile = db.EmployeeFiles.Find(id);
            db.EmployeeFiles.Remove(eFile);
            db.SaveChanges();

            foreach (var notif in db.Notifications.Where(n => n.CorrespondingItemId == eFile.Id && n.NotificationTypeId == 1).ToList())
            {
                db.Notifications.Remove(notif);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Employees", new { id = empId });
        }

        // GET: Employees/CFRsCompleted/5
        [Authorize(Roles = "Admin, Quality")]
        public ActionResult CFRsCompleted(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = mb.Employees.Find(id);
            
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeName = employee.FirstName + " " + employee.LastName;
            ViewBag.EmployeeID = db.Users.FirstOrDefault(u => u.EmployeeID == id).Id;

            var cfrs = new List<CompletedCFR>();
            foreach (var cfr in mb.CFRMortgages.Where(c => c.Employee1.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList())
            {
                var item = new CompletedCFR();
                item.Id = cfr.CFRMortgageID;
                item.DateSubmitted = cfr.DateOfFeedback;
                item.ForEmployee = cfr.Employee.FirstName + " " + cfr.Employee.LastName;
                item.ForEmployeeID = cfr.EmployeeID;
                item.Type = "Mortgage";

                cfrs.Add(item);
            }
            foreach (var cfr in mb.CFRInsurances.Where(c => c.Employee2.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList())
            {
                var item = new CompletedCFR();
                item.Id = cfr.CFRInsuranceID;
                item.DateSubmitted = cfr.DateOfFeedback;
                item.ForEmployee = cfr.Employee1.FirstName + " " + cfr.Employee1.LastName;
                item.ForEmployeeID = cfr.EmployeeID;
                item.Type = "Insurance";

                cfrs.Add(item);
            }
            foreach (var cfr in mb.CFRPatientRecruitments.Where(c => c.Employee.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList())
            {
                var item = new CompletedCFR();
                item.Id = cfr.CFRPatientRecruitmentID;
                item.DateSubmitted = cfr.DateOfFeedback;
                item.ForEmployee = cfr.Employee1.FirstName + " " + cfr.Employee1.LastName;
                item.ForEmployeeID = cfr.EmployeeID;
                item.Type = "Patient Recruitment";

                cfrs.Add(item);
            }
            foreach (var cfr in mb.CFRSales.Where(c => c.Employee1.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList())
            {
                var item = new CompletedCFR();
                item.Id = cfr.CFRSalesID;
                item.DateSubmitted = cfr.DateOfFeedback;
                item.ForEmployee = cfr.Employee.FirstName + " " + cfr.Employee.LastName;
                item.ForEmployeeID = cfr.EmployeeID;
                item.Type = "Sales";

                cfrs.Add(item);
            }
            foreach (var cfr in mb.CFRAcurians.Where(c => c.Employee1.EmployeeID == employee.EmployeeID).OrderByDescending(c => c.DateOfFeedback).ToList())
            {
                var item = new CompletedCFR();
                item.Id = cfr.CFRAcurianID;
                item.DateSubmitted = cfr.DateOfFeedback;
                item.ForEmployee = cfr.Employee.FirstName + " " + cfr.Employee.LastName;
                item.ForEmployeeID = cfr.EmployeeID;
                item.Type = "Acurian";

                cfrs.Add(item);
            }

            ViewBag.CFRs = cfrs.OrderByDescending(c => c.DateSubmitted).ToList();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
