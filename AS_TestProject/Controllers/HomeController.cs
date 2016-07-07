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
            return View();
        }

        public ActionResult Directory()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var mb = new ReportEntities();                       
            ViewBag.Directory = mb.Employees.Where(d => d.SiteID == user.SiteID).OrderBy(d => d.LastName).ThenBy(d => d.FirstName).ToList();
            
            return View();
        }

        public ActionResult ProfilePage(string id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

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

                    return View(userCheck);
                }

            }

            //might need later when I want to send specific info to profile page
            //var user1 = User.Identity.GetUserId();
            ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "Id", "Priority");
            ViewBag.Urgent = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
            ViewBag.High = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
            ViewBag.Medium = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
            ViewBag.Low = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();
            ViewBag.TaskCounter = user.TaskTally;

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