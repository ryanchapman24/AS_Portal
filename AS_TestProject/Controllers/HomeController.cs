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

namespace AS_TestProject.Controllers
{
    [Authorize]
    public class HomeController : UserNames
    {
        public ActionResult Index()
        {
            var mb = new ReportEntities();
            var user = db.Users.Find(User.Identity.GetUserId());
            var todayYear = System.DateTime.Now.Year;
            var todayMonth = System.DateTime.Now.Month;
            var todayDay = System.DateTime.Now.Day;

            ViewBag.birthdayList = mb.Employees.Where(e => e.BirthDate.Month == todayMonth && e.BirthDate.Day == todayDay && e.SiteID == user.SiteID).ToList();
            ViewBag.MonthlyTasks = user.Tasks.Where(t => t.Complete == true && t.Completed.Value.Month == todayMonth && t.Completed.Value.Year == todayYear).ToList();
            ViewBag.DailyMessages = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Out == true && m.Sent.Year == todayYear && m.Sent.Month == todayMonth && m.Sent.Day == todayDay).ToList();

            return View();
        }

        public ActionResult Tools()
        {
            return View();
        }

        public ActionResult Directory()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var mb = new ReportEntities();                       
            ViewBag.Directory = mb.Employees.Where(d => d.SiteID == user.SiteID).OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToList();
            ViewBag.PositionID = new SelectList(mb.Positions, "PositionID", "PositionName");
            ViewBag.SiteID = new SelectList(mb.Sites, "SiteID", "SiteName");

            return View();
        }

        public ActionResult ProfilePage(string id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var mb = new ReportEntities();

            if (!string.IsNullOrWhiteSpace(id))
            {
                var userCheck = db.Users.Find(id);
                if (userCheck != null)
                {
                    var userA = userCheck.Id;

                    ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "Id", "Priority");
                    ViewBag.Urgent = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
                    ViewBag.High = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
                    ViewBag.Medium = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
                    ViewBag.Low = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();
                    ViewBag.TaskCounter = userCheck.TaskTally;

                    // Computing Time With Company
                    var userFromEmpTable = mb.Employees.First(e => e.EmployeeID == userCheck.EmployeeID);
                    var hireDate = userFromEmpTable.HireDate;
                    var today = System.DateTime.Now;
                    DateTime date1 = hireDate;
                    DateTime date2 = today;
                    int oldMonth = date2.Month;
                    while (oldMonth == date2.Month)
                    {
                        date1 = date1.AddDays(-1);
                        date2 = date2.AddDays(-1);
                    }
                    int years = 0, months = 0, days = 0;
                    // getting number of years
                    while (date2.CompareTo(date1) >= 0)
                    {
                        years++;
                        date2 = date2.AddYears(-1);
                    }
                    date2 = date2.AddYears(1);
                    years--;
                    // getting number of months and days
                    oldMonth = date2.Month;
                    while (date2.CompareTo(date1) >= 0)
                    {
                        days++;
                        date2 = date2.AddDays(-1);
                        if ((date2.CompareTo(date1) >= 0) && (oldMonth != date2.Month))
                        {
                            months++;
                            days = 0;
                            oldMonth = date2.Month;
                        }
                    }
                    date2 = date2.AddDays(1);
                    days--;
                    // Formatting string possibilities
                    var y = "";
                    if (years == 1)
                    {
                        y = " year, ";
                    }
                    else if (years > 1)
                    {
                        y = " years, ";
                    }
                    var m = "";
                    if (months == 1)
                    {
                        m = " month, ";
                    }
                    else if (months > 1)
                    {
                        m = " months, ";
                    }
                    var d = "";
                    if (days == 1)
                    {
                        d = " day";
                    }
                    else if (days > 1)
                    {
                        d = " days";
                    }
                    if (years == 0 && months > 0 && days > 0)
                    {
                        ViewBag.TimeWithCompany = months.ToString() + m + days.ToString() + d;
                    }
                    else if (years > 0 && months == 0 && days > 0)
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y + days.ToString() + d;
                    }
                    else if (years > 0 && months > 0 && days == 0)
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y + months.ToString() + m;
                    }
                    else if (years == 0 && months == 0 && days > 0)
                    {
                        ViewBag.TimeWithCompany = days.ToString() + d;
                    }
                    else if (years == 0 && months > 0 && days == 0)
                    {
                        ViewBag.TimeWithCompany = months.ToString() + m;
                    }
                    else if (years > 0 && months == 0 && days == 0)
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y;
                    }
                    else
                    {
                        ViewBag.TimeWithCompany = years.ToString() + y + months.ToString() + m + days.ToString() + d;
                    }               
                    return View(userCheck);
                }
            }

            ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "Id", "Priority");
            ViewBag.Urgent = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
            ViewBag.High = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
            ViewBag.Medium = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
            ViewBag.Low = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();
            ViewBag.TaskCounter = user.TaskTally;

            // Computing MY Time With Company
            var selfUserFromEmpTable = mb.Employees.First(e => e.EmployeeID == user.EmployeeID);
            var selfHireDate = selfUserFromEmpTable.HireDate;
            var selfToday = System.DateTime.Now;
            DateTime selfDate1 = selfHireDate;
            DateTime selfDate2 = selfToday;
            int selfOldMonth = selfDate2.Month;
            while (selfOldMonth == selfDate2.Month)
            {
                selfDate1 = selfDate1.AddDays(-1);
                selfDate2 = selfDate2.AddDays(-1);
            }
            int selfYears = 0, selfMonths = 0, selfDays = 0;
            // getting number of years
            while (selfDate2.CompareTo(selfDate1) >= 0)
            {
                selfYears++;
                selfDate2 = selfDate2.AddYears(-1);
            }
            selfDate2 = selfDate2.AddYears(1);
            selfYears--;
            // getting number of months and days
            selfOldMonth = selfDate2.Month;
            while (selfDate2.CompareTo(selfDate1) >= 0)
            {
                selfDays++;
                selfDate2 = selfDate2.AddDays(-1);
                if ((selfDate2.CompareTo(selfDate1) >= 0) && (selfOldMonth != selfDate2.Month))
                {
                    selfMonths++;
                    selfDays = 0;
                    selfOldMonth = selfDate2.Month;
                }
            }
            selfDate2 = selfDate2.AddDays(1);
            selfDays--;

            // Formatting string possibilities
            var selfY = "";
            if (selfYears == 1)
            {
                selfY = " year, ";
            }
            else if (selfYears > 1)
            {
                selfY = " years, ";
            }
            var selfM = "";
            if (selfMonths == 1)
            {
                selfM = " month, ";
            }
            else if (selfMonths > 1)
            {
                selfM = " months, ";
            }
            var selfD = "";
            if (selfDays == 1)
            {
                selfD = " day";
            }
            else if (selfDays > 1)
            {
                selfD = " days";
            }
            if (selfYears == 0 && selfMonths > 0 && selfDays > 0)
            {
                ViewBag.TimeWithCompany = selfMonths.ToString() + selfM + selfDays.ToString() + selfD;
            }
            else if (selfYears > 0 && selfMonths == 0 && selfDays > 0)
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY + selfDays.ToString() + selfD;
            }
            else if (selfYears > 0 && selfMonths > 0 && selfDays == 0)
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY + selfMonths.ToString() + selfM;
            }
            else if (selfYears == 0 && selfMonths == 0 && selfDays > 0)
            {
                ViewBag.TimeWithCompany = selfDays.ToString() + selfD;
            }
            else if (selfYears == 0 && selfMonths > 0 && selfDays == 0)
            {
                ViewBag.TimeWithCompany = selfMonths.ToString() + selfM;
            }
            else if (selfYears > 0 && selfMonths == 0 && selfDays == 0)
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY;
            }
            else
            {
                ViewBag.TimeWithCompany = selfYears.ToString() + selfY + selfMonths.ToString() + selfM + selfDays.ToString() + selfD;
            }
            return View(user);
        }

        // POST: Tasks/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AddTask(WorkTask task)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.IsValid)
            {
                task.AuthorId = User.Identity.GetUserId();
                task.Created = System.DateTime.Now;
                task.Complete = false;
                db.Tasks.Add(task);
                db.SaveChanges();               
            }
            ViewBag.TicketTypeId = new SelectList(db.TaskPriorities, "Id", "Priority", task.TaskPriorityId);
            ViewBag.Urgent = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
            ViewBag.High = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
            ViewBag.Medium = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
            ViewBag.Low = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();
            return RedirectToAction("ProfilePage");
        }

        //POST: Tasks/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult SubmitTask(List<int> Change)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            if (Change != null)
            {
                if (Request.Form["complete"] != null)
                {
                    int count = Change.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var task = db.Tasks.Find(Change[i]);
                        task.Complete = true;
                        task.Completed = System.DateTime.Now;
                        user.TaskTally = user.TaskTally + 1;
                        db.SaveChanges();
                    }
                }
                else if (Request.Form["abort"] != null)
                {
                    int count = Change.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var task = db.Tasks.Find(Change[i]);
                        db.Tasks.Remove(task);
                        db.SaveChanges();
                    }
                }              
            }

            return RedirectToAction("ProfilePage");
        }

        public ActionResult Forms()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            var model = new PhotoJudgment();
            var unpubPhotos = db.GalleryPhotos.Where(p => p.Published == false).OrderByDescending(p => p.Id).ToList();

            foreach (var photo in unpubPhotos)
            {
                model.PhotoList.Add(photo);
            }
            db.SaveChanges();

            ViewBag.Unpublished = db.GalleryPhotos.Where(p => p.Published == false).OrderByDescending(p => p.Id).ToList();
            ViewBag.GalleryPhotos = db.GalleryPhotos.Where(p => p.Published == true).OrderByDescending(p => p.Id).ToList();
           
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhotoToGallery(GalleryPhoto galleryPhoto, HttpPostedFileBase image)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ImageUploadValidator.IsWebFriendlyImage(image))
            {
                //var fileName = Path.GetFileName(image.FileName);
                //image.SaveAs(Path.Combine(Server.MapPath("~/GalleryPhotos/"), fileName));
                //galleryPhoto.File = "/GalleryPhotos/" + fileName;

                //Counter
                var num = 0;
                //Gets Filename without the extension
                var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                var gPic = Path.Combine("/GalleryPhotos/", fileName + Path.GetExtension(image.FileName));
                //Checks if pPic matches any of the current attachments, 
                //if so it will loop and add a (number) to the end of the filename
                while (db.GalleryPhotos.Any(p => p.File == gPic))
                {
                    //Sets "filename" back to the default value
                    fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    //Add's parentheses after the name with a number ex. filename(4)
                    fileName = string.Format(fileName + "(" + ++num + ")");
                    //Makes sure pPic gets updated with the new filename so it could check
                    gPic = Path.Combine("/GalleryPhotos/", fileName + Path.GetExtension(image.FileName));
                }
                image.SaveAs(Path.Combine(Server.MapPath("~/GalleryPhotos/"), fileName + Path.GetExtension(image.FileName)));
                galleryPhoto.File = gPic;
                db.SaveChanges();
            }

            galleryPhoto.Created = System.DateTime.Now;
            galleryPhoto.AuthorId = user.Id;
            galleryPhoto.Published = false;
            db.GalleryPhotos.Add(galleryPhoto);
            db.SaveChanges();

            return RedirectToAction("Gallery", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Marketing")]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(List<int> Published, List<int> Delete)
        {
            if (Published != null)
            {
                int count = Published.Count();
                for (int i = 0; i < count; i++)
                {
                    var photo = db.GalleryPhotos.Find(Published[i]);
                    photo.Published = true;
                    db.SaveChanges();
                }
            }

            if (Delete != null)
            {
                int total = Delete.Count();
                for (int i = 0; i < total; i++)
                {
                    var photo = db.GalleryPhotos.Find(Delete[i]);
                    db.GalleryPhotos.Remove(photo);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Gallery", "Home");
        }
    }
}