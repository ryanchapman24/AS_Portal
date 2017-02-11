using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS_TestProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Threading.Tasks;

namespace AS_TestProject.Controllers
{
    [Authorize (Roles = "Admin, IT")]
    public class ITController : UserNames
    {
        // GET: IT
        [Authorize(Roles = "Admin, IT")]
        public ActionResult Index()
        {
            ViewBag.Documents = db.Documents.Where(d => d.Department == "IT").OrderByDescending(d => d.Id).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, IT")]
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
                var gPic = Path.Combine("/Documents/IT/", fileName + Path.GetExtension(doc.FileName));
                //Checks if pPic matches any of the current attachments, 
                //if so it will loop and add a (number) to the end of the filename
                while (db.Documents.Any(p => p.File == gPic))
                {
                    //Sets "filename" back to the default value
                    fileName = Path.GetFileNameWithoutExtension(doc.FileName);
                    //Add's parentheses after the name with a number ex. filename(4)
                    fileName = string.Format(fileName + "(" + ++num + ")");
                    //Makes sure pPic gets updated with the new filename so it could check
                    gPic = Path.Combine("/Documents/IT/", fileName + Path.GetExtension(doc.FileName));
                }
                doc.SaveAs(Path.Combine(Server.MapPath("~/Documents/IT/"), fileName + Path.GetExtension(doc.FileName)));

                document.Created = System.DateTime.Now;
                document.AuthorId = user.Id;
                document.Department = "IT";
                document.File = gPic;

                db.Documents.Add(document);
                db.SaveChanges();

                foreach (var ITuser in db.Users.Where(u => u.Roles.Any(r => r.RoleId == "cf0c9cdc-c2d7-4abf-9da7-72b5d4245348")).ToList())
                {
                    Notification n = new Notification()
                    {
                        NotificationTypeId = 2,
                        Created = System.DateTime.Now,
                        Description = "A new file was added to the IT Hub.",
                        Additional = fileName + Path.GetExtension(doc.FileName),
                        CorrespondingItemId = document.Id,
                        NotifyUserId = ITuser.Id,
                        New = true
                    };
                    db.Notifications.Add(n);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "IT");
        }

        [Authorize(Roles = "Admin, IT")]
        public ActionResult DeleteDocument(int id)
        {
            var document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();

            foreach (var notif in db.Notifications.Where(n => n.CorrespondingItemId == document.Id && n.NotificationTypeId == 2).ToList())
            {
                db.Notifications.Remove(notif);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "IT");
        }
    }
}