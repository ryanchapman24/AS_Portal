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
            ViewBag.AOIQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AOIQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AOIQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AOIQ4_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.TEQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.TEQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.TEQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.TEQ4_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.TEQ5_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AQ4_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.AQ5_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.CQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.CQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.CQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.PQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");
            ViewBag.PQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption");

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
        public ActionResult MortgageCFR_Create([Bind(Include = "CFRMortgageID,EmployeeID,DomainMasterID,C_Calls,TEQ1,TEQ2,TEQ3,TEQ4,TEQ5,PQ1,PQ2,CQ1,CQ2,CQ3,AQ1,AQ2,AQ3,AQ4,AQ5,AOIQ1,AOIQ2,AOIQ3,AOIQ4,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRMortgage cFRMortgage)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                cFRMortgage.ManagerID = user.EmployeeID;
                cFRMortgage.DateOfFeedback = System.DateTime.Now;

                // Telephone Etiquette Rating Calculation (MORTGAGE)
                int mTER = 0;
                if (cFRMortgage.TEQ1 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.TEQ2 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.TEQ3 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.TEQ4 == 2)
                {
                    mTER++;
                }
                if (cFRMortgage.TEQ5 == 2)
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
                if (cFRMortgage.PQ1 == 2)
                {
                    mPR++;
                }
                if (cFRMortgage.PQ2 == 2)
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
                if (cFRMortgage.CQ1 == 2)
                {
                    mCR++;
                }
                if (cFRMortgage.CQ2 == 2)
                {
                    mCR++;
                }
                if (cFRMortgage.CQ3 == 2)
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
                if (cFRMortgage.AQ1 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.AQ2 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.AQ3 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.AQ4 == 2)
                {
                    mAR++;
                }
                if (cFRMortgage.AQ5 == 2)
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
                if (cFRMortgage.AOIQ1 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.AOIQ2 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.AOIQ3 == 2)
                {
                    mAOIR++;
                }
                if (cFRMortgage.AOIQ4 == 2)
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
            ViewBag.AOIQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ1);
            ViewBag.AOIQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ2);
            ViewBag.AOIQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ3);
            ViewBag.AOIQ4_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ4);
            ViewBag.TEQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ1);
            ViewBag.TEQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ2);
            ViewBag.TEQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ3);
            ViewBag.TEQ4_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ4);
            ViewBag.TEQ5_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ5);
            ViewBag.AQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ1);
            ViewBag.AQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ2);
            ViewBag.AQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ3);
            ViewBag.AQ4_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ4);
            ViewBag.AQ5_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ5);
            ViewBag.CQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ1);
            ViewBag.CQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ2);
            ViewBag.CQ3_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ3);
            ViewBag.PQ1_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.PQ1);
            ViewBag.PQ2_Mortgage = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.PQ2);
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
            ViewBag.AOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ1);
            ViewBag.AOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ2);
            ViewBag.AOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ3);
            ViewBag.AOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ4);
            ViewBag.TEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ1);
            ViewBag.TEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ2);
            ViewBag.TEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ3);
            ViewBag.TEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ4);
            ViewBag.TEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ5);
            ViewBag.AQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ1);
            ViewBag.AQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ2);
            ViewBag.AQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ3);
            ViewBag.AQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ4);
            ViewBag.AQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ5);
            ViewBag.CQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ1);
            ViewBag.CQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ2);
            ViewBag.CQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ3);
            ViewBag.PQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.PQ1);
            ViewBag.PQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.PQ2);
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
        public ActionResult MortgageCFR_Edit([Bind(Include = "CFRMortgageID,EmployeeID,DomainMasterID,C_Calls,TEQ1,TEQ2,TEQ3,TEQ4,TEQ5,PQ1,PQ2,CQ1,CQ2,CQ3,AQ1,AQ2,AQ3,AQ4,AQ5,AOIQ1,AOIQ2,AOIQ3,AOIQ4,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRMortgage cFRMortgage)
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
            ViewBag.AOIQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ1);
            ViewBag.AOIQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ2);
            ViewBag.AOIQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ3);
            ViewBag.AOIQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AOIQ4);
            ViewBag.TEQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ1);
            ViewBag.TEQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ2);
            ViewBag.TEQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ3);
            ViewBag.TEQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ4);
            ViewBag.TEQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.TEQ5);
            ViewBag.AQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ1);
            ViewBag.AQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ2);
            ViewBag.AQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ3);
            ViewBag.AQ4 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ4);
            ViewBag.AQ5 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.AQ5);
            ViewBag.CQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ1);
            ViewBag.CQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ2);
            ViewBag.CQ3 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.CQ3);
            ViewBag.PQ1 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.PQ1);
            ViewBag.PQ2 = new SelectList(mb.AnswerKeys, "AnswerKeyID", "AnswerOption", cFRMortgage.PQ2);
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
