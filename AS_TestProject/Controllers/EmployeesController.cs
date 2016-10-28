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
    public class EmployeesController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        public class Domain
        {
            public byte Id { get; set; }
            public string FileMaskPlusName { get; set; }
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
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,EmailAddress,HomePhone,CellPhone,BirthDate,HireDate,TerminationDate,IsManager,ManagerEmployeeID,PositionID,SiteID,IsActive,AddDate,AddByEmployeeID,RehireDate,FileNumber")] Employee employee)
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
                mb.SaveChanges();
                return RedirectToAction("Directory", "Home");
            }
            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName", employee.SiteID);
            return View(employee);
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Details(int? id)
        {
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
            foreach (var domain in mb.DomainMasters.Where(d => d.IsActive == true).OrderBy(d => d.FileMask))
            {
                var selection = new Domain();
                selection.Id = domain.DomainMasterID;
                selection.FileMaskPlusName = domain.FileMask + " - " + domain.DomainName;

                domains.Add(selection);
            }
            ViewBag.DomainMasterID = new SelectList(domains, "Id", "FileMaskPlusName");
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");

            Employee manager = mb.Employees.Find(employee.ManagerEmployeeID);
            Employee creator = mb.Employees.Find(employee.AddByEmployeeID);
            ViewBag.Manager = manager.FirstName + " " + manager.LastName;
            ViewBag.Creator = creator.FirstName + " " + creator.LastName;
            ViewBag.Date = System.DateTime.Now.ToShortDateString();

            ViewBag.DisciplinaryActions = mb.DisciplinaryActions.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(d => d.Date).ToList();
            ViewBag.CFRs = mb.CFRMortgages.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(d => d.DateOfFeedback).ToList();
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
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDA([Bind(Include = "DisciplinaryActionID,EmployeeID,FirstName,LastName,Date,Reason,Explanation,EditByEmployeeID,EditTimeStamp")] DisciplinaryAction DiscAct)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                
                DiscAct.EditByEmployeeID = user.EmployeeID;
                DiscAct.EditTimeStamp = System.DateTime.Now;
                mb.DisciplinaryActions.Add(DiscAct);
                mb.SaveChanges();

                return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
            }
            return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
        }

        // GET: Employees/EditDA/5
        [Authorize(Roles = "Admin, HR")]
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
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult EditDA([Bind(Include = "DisciplinaryActionID,EmployeeID,FirstName,LastName,Date,Reason,Explanation,EditByEmployeeID,EditTimeStamp")] DisciplinaryAction DiscAct)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                mb.DisciplinaryActions.Attach(DiscAct);
                mb.Entry(DiscAct).Property("Date").IsModified = true;
                mb.Entry(DiscAct).Property("Reason").IsModified = true;
                mb.Entry(DiscAct).Property("Explanation").IsModified = true;
                DiscAct.EditByEmployeeID = user.EmployeeID;
                DiscAct.EditTimeStamp = System.DateTime.Now;
                mb.SaveChanges();

                return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
            }
            return View(DiscAct);
        }

        // GET: Employees/DeleteDA/5
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDAConfirmed(int id)
        {
            DisciplinaryAction DiscAct = mb.DisciplinaryActions.Find(id);
            mb.DisciplinaryActions.Remove(DiscAct);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = DiscAct.EmployeeID });
        }

        // GET: CFRMortgages/Details/5
        [Authorize(Roles = "Admin")]
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
            return View(cFRMortgage);
        }

        // POST: CFRMortgages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                if (mPR == 0)
                {
                    cFRMortgage.ProfessionalismRating = 1;
                }
                if (mPR == 1)
                {
                    cFRMortgage.ProfessionalismRating = 2;
                }
                if (mPR == 3)
                {
                    cFRMortgage.ProfessionalismRating = 3;
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
                if (mCR == 0)
                {
                    cFRMortgage.ComplianceRating = 1;
                }
                if (mCR >= 1)
                {
                    cFRMortgage.ComplianceRating = 3;
                }

                // Adherance Rating Calculation (MORTGAGE)
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
                if (mAOIR == 0)
                {
                    cFRMortgage.AccuracyOfInformationRating = 1;
                }
                if (mAOIR >= 1)
                {
                    cFRMortgage.AccuracyOfInformationRating = 3;
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
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ1);
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ2);
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ3);
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ4);
            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ1);
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ2);
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ3);
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ4);
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ5);
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ1);
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ2);
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ3);
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ4);
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ5);
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ1);
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ2);
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ3);
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ1);
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ2);
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.TelephoneEtiquetteRating);
            return View(cFRMortgage);
        }

        // GET: CFRMortgages/Edit/5
        [Authorize(Roles = "Admin")]
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
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ1);
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ2);
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ3);
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ4);
            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ1);
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ2);
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ3);
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ4);
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ5);
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ1);
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ2);
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ3);
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ4);
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ5);
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ1);
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ2);
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ3);
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ1);
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ2);
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.TelephoneEtiquetteRating);
            return View(cFRMortgage);
        }

        // POST: CFRMortgages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult MortgageCFR_Edit([Bind(Include = "CFRMortgageID,EmployeeID,DomainMasterID,C_Calls,mTEQ1,mTEQ2,mTEQ3,mTEQ4,mTEQ5,mPQ1,mPQ2,mCQ1,mCQ2,mCQ3,mAQ1,mAQ2,mAQ3,mAQ4,mAQ5,mAOIQ1,mAOIQ2,mAOIQ3,mAOIQ4,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRMortgage cFRMortgage)
        {
            if (ModelState.IsValid)
            {
                mb.Entry(cFRMortgage).State = EntityState.Modified;
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
            ViewBag.mAOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ1);
            ViewBag.mAOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ2);
            ViewBag.mAOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ3);
            ViewBag.mAOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAOIQ4);
            ViewBag.mTEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ1);
            ViewBag.mTEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ2);
            ViewBag.mTEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ3);
            ViewBag.mTEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ4);
            ViewBag.mTEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mTEQ5);
            ViewBag.mAQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ1);
            ViewBag.mAQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ2);
            ViewBag.mAQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ3);
            ViewBag.mAQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ4);
            ViewBag.mAQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mAQ5);
            ViewBag.mCQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ1);
            ViewBag.mCQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ2);
            ViewBag.mCQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mCQ3);
            ViewBag.mPQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ1);
            ViewBag.mPQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.mPQ2);
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRMortgage.TelephoneEtiquetteRating);
            return View(cFRMortgage);
        }

        // GET: CFRMortgages/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult MortgageCFR_DeleteConfirmed(int id)
        {
            CFRMortgage cFRMortgage = mb.CFRMortgages.Find(id);
            mb.CFRMortgages.Remove(cFRMortgage);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRMortgage.EmployeeID });
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
