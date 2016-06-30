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

        public ActionResult ProfilePage()
        {
            return View();
        }

        public ActionResult Forms()
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
                var fileName = Path.GetFileName(image.FileName);
                image.SaveAs(Path.Combine(Server.MapPath("~/GalleryPhotos/"), fileName));
                galleryPhoto.File = "/GalleryPhotos/" + fileName;
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
        public ActionResult Publish(PhotoJudgment model)
        {
            if (model.PhotoList.Any(m => m.Published == true))
            {
                db.SaveChanges();
            }
            
            return RedirectToAction("Gallery", "Home");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Publish(IEnumerable<GalleryPhoto> galleryPhotos, bool published)
        //{
        //    var user = db.Users.Find(User.Identity.GetUserId());

        //    foreach (var photo in galleryPhotos)
        //    {
        //        if (published == true)
        //        {
        //            photo.Published = true;
        //            db.SaveChanges();
        //        }
        //    }

        //    return RedirectToAction("Gallery", "Home");
        //}
    }
}