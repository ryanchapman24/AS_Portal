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

            Employee manager = mb.Employees.Find(employee.ManagerEmployeeID);
            Employee creator = mb.Employees.Find(employee.AddByEmployeeID);
            ViewBag.Manager = manager.FirstName + " " + manager.LastName;
            ViewBag.Creator = creator.FirstName + " " + creator.LastName;
            ViewBag.Date = System.DateTime.Now.ToShortDateString();

            ViewBag.DisciplinaryActions = mb.DisciplinaryActions.Where(d => d.EmployeeID == employee.EmployeeID).OrderByDescending(d => d.Date).ToList();

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
                if (mPR >= 2)
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

            // GET: CFRInsurances/Details/5
        [Authorize(Roles = "Admin")]
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
            return View(cFRInsurance);
        }

        // POST: CFRInsurances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                if (iCR == 0)
                {
                    cFRInsurance.ComplianceRating = 1;
                }
                if (iCR >= 1)
                {
                    cFRInsurance.ComplianceRating = 3;
                }

                // Adherance Rating Calculation (INSURANCE)
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
                if (iAOIR == 0)
                {
                    cFRInsurance.AccuracyOfInformationRating = 1;
                }
                if (iAOIR >= 1)
                {
                    cFRInsurance.AccuracyOfInformationRating = 3;
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
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.TelephoneEtiquetteRating);
            return View(cFRInsurance);
        }

        // GET: CFRInsurances/Edit/5
        [Authorize(Roles = "Admin")]
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
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.TelephoneEtiquetteRating);
            return View(cFRInsurance);
        }

        // POST: CFRInsurances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult InsuranceCFR_Edit([Bind(Include = "CFRInsuranceID,EmployeeID,DomainMasterID,C_Calls,iTEQ1,iTEQ2,iTEQ3,iTEQ4,iTEQ5,iPQ1,iPQ2,iCQ1,iCQ2,iCQ3,iAQ1,iAQ2,iAQ3,iAQ4,iAQ5,iAOIQ1,iAOIQ2,iAOIQ3,iAOIQ4,iAOIQ5,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRInsurance cFRInsurance)
        {
            if (ModelState.IsValid)
            {
                mb.Entry(cFRInsurance).State = EntityState.Modified;
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
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRInsurance.TelephoneEtiquetteRating);
            return View(cFRInsurance);
        }

        // GET: CFRInsurances/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult InsuranceCFR_DeleteConfirmed(int id)
        {
            CFRInsurance cFRInsurance = mb.CFRInsurances.Find(id);
            mb.CFRInsurances.Remove(cFRInsurance);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRInsurance.EmployeeID });
        }

        // GET: CFRPatientRecruitments/Details/5
        [Authorize(Roles = "Admin")]
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
            return View(cFRPatientRecruitment);
        }

        // POST: CFRPatientRecruitments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                if (pPR == 0)
                {
                    cFRPatientRecruitment.ProfessionalismRating = 1;
                }
                if (pPR == 1)
                {
                    cFRPatientRecruitment.ProfessionalismRating = 2;
                }
                if (pPR == 3)
                {
                    cFRPatientRecruitment.ProfessionalismRating = 3;
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
                if (pCR == 0)
                {
                    cFRPatientRecruitment.ComplianceRating = 1;
                }
                if (pCR >= 1)
                {
                    cFRPatientRecruitment.ComplianceRating = 3;
                }

                // Adherance Rating Calculation (PATIENT RECRUITMENT)
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
                if (pAR == 0)
                {
                    cFRPatientRecruitment.AdheranceRating = 1;
                }
                if (pAR >= 1)
                {
                    cFRPatientRecruitment.AdheranceRating = 3;
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
                if (pAOIR == 0)
                {
                    cFRPatientRecruitment.AccuracyOfInformationRating = 1;
                }
                if (pAOIR >= 1)
                {
                    cFRPatientRecruitment.AccuracyOfInformationRating = 3;
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
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.TelephoneEtiquetteRating);
            return View(cFRPatientRecruitment);
        }

        // GET: CFRPatientRecruitments/Edit/5
        [Authorize(Roles = "Admin")]
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
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.TelephoneEtiquetteRating);
            return View(cFRPatientRecruitment);
        }

        // POST: CFRPatientRecruitments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult PatientRecruitmentCFR_Edit([Bind(Include = "CFRPatientRecruitmentID,EmployeeID,DomainMasterID,C_Calls,pTEQ1,pTEQ2,pTEQ3,pPQ1,pPQ2,pCQ1,pCQ2,pCQ3,pCQ4,pCQ5,pAQ1,pAQ2,pAQ3,pAQ4,pAQ5,pAOIQ1,pAOIQ2,TelephoneEtiquetteRating,ProfessionalismRating,ComplianceRating,AdheranceRating,AccuracyOfInformationRating,ConversionRateToday,WeekToDate,Comments,Strengths,ActionPlan,ManagerID,DateOfFeedback")] CFRPatientRecruitment cFRPatientRecruitment)
        {
            if (ModelState.IsValid)
            {
                mb.Entry(cFRPatientRecruitment).State = EntityState.Modified;
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
            //ViewBag.AccuracyOfInformationRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.AccuracyOfInformationRating);
            //ViewBag.AdheranceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.AdheranceRating);
            //ViewBag.ComplianceRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.ComplianceRating);
            //ViewBag.ProfessionalismRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.ProfessionalismRating);
            //ViewBag.TelephoneEtiquetteRating = new SelectList(mb.CFRPerformanceAnalysis, "CFRPerformanceAnalysisID", "PerformanceRating", cFRPatientRecruitment.TelephoneEtiquetteRating);
            return View(cFRPatientRecruitment);
        }

        // GET: CFRPatientRecruitments/Delete/5
        [Authorize(Roles = "Admin")]
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
        [HttpPost, ActionName("MortgageCFR_Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult PatientRecruitmentCFR_DeleteConfirmed(int id)
        {
            CFRPatientRecruitment cFRPatientRecruitment = mb.CFRPatientRecruitments.Find(id);
            mb.CFRPatientRecruitments.Remove(cFRPatientRecruitment);
            mb.SaveChanges();
            return RedirectToAction("Details", "Employees", new { id = cFRPatientRecruitment.EmployeeID });
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
