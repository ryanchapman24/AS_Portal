using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_TestProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Threading.Tasks;
using AS_TestProject.Entities;

namespace AS_TestProject.Controllers
{
    [Authorize(Roles = "Admin, HR")]
    public class HRController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        // GET: HR
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Index()
        {
            ViewBag.Documents = db.Documents.Where(d => d.Department == "HR").OrderByDescending(d => d.Id).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocument(IEnumerable<HttpPostedFileBase> file, Document document)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            foreach (var doc in file)
            {
                //Counter
                var num = 0;
                //Gets Filename without the extension
                var fileName = Path.GetFileNameWithoutExtension(doc.FileName);
                var gPic = Path.Combine("/Documents/HR/", fileName + Path.GetExtension(doc.FileName));
                //Checks if pPic matches any of the current attachments, 
                //if so it will loop and add a (number) to the end of the filename
                while (db.Documents.Any(p => p.File == gPic))
                {
                    //Sets "filename" back to the default value
                    fileName = Path.GetFileNameWithoutExtension(doc.FileName);
                    //Add's parentheses after the name with a number ex. filename(4)
                    fileName = string.Format(fileName + "(" + ++num + ")");
                    //Makes sure pPic gets updated with the new filename so it could check
                    gPic = Path.Combine("/Documents/HR/", fileName + Path.GetExtension(doc.FileName));
                }
                doc.SaveAs(Path.Combine(Server.MapPath("~/Documents/HR/"), fileName + Path.GetExtension(doc.FileName)));

                document.Created = System.DateTime.Now;
                document.AuthorId = user.Id;
                document.Department = "HR";
                document.File = gPic;

                db.Documents.Add(document);
                db.SaveChanges();

                foreach (var HRuser in db.Users.Where(u => u.Roles.Any(r => r.RoleId == "cf0c9cdc-c2d7-4abf-9da7-72b5d4245348")).ToList())
                {
                    Notification n = new Notification()
                    {
                        NotificationTypeId = 3,
                        Created = System.DateTime.Now,
                        Description = "A new file was added to the HR Hub.",
                        Additional = fileName + Path.GetExtension(doc.FileName),
                        CorrespondingItemId = document.Id,
                        NotifyUserId = HRuser.Id,
                        New = true
                    };
                    db.Notifications.Add(n);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "HR");
        }

        [Authorize(Roles = "Admin, HR")]
        public ActionResult DeleteDocument(int id)
        {
            var document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();

            foreach (var notif in db.Notifications.Where(n => n.CorrespondingItemId == document.Id && n.NotificationTypeId == 3).ToList())
            {
                db.Notifications.Remove(notif);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "HR");
        }

        // GET: HR
        [Authorize(Roles = "Admin, HR")]
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

            ViewBag.ThisMonthOvrH = mb.Employees.Where( e=> e.HireDate.Month == thisMonth && e.HireDate.Year == thisMonthYear && (e.SiteID == 1 || e.SiteID == 2)).Count();
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

            return View();
        }
    }
}