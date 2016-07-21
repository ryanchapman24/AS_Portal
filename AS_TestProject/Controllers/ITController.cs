﻿using System;
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
    [Authorize (Roles = "IT")]
    public class ITController : UserNames
    {
        // GET: IT
        [Authorize(Roles = "IT")]
        public ActionResult Index()
        {
            ViewBag.Documents = db.Documents.Where(d => d.Department == "IT").OrderByDescending(d => d.Id).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "IT")]
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
                var gPic = Path.Combine("/Documents/", fileName + Path.GetExtension(doc.FileName));
                //Checks if pPic matches any of the current attachments, 
                //if so it will loop and add a (number) to the end of the filename
                while (db.Documents.Any(p => p.File == gPic))
                {
                    //Sets "filename" back to the default value
                    fileName = Path.GetFileNameWithoutExtension(doc.FileName);
                    //Add's parentheses after the name with a number ex. filename(4)
                    fileName = string.Format(fileName + "(" + ++num + ")");
                    //Makes sure pPic gets updated with the new filename so it could check
                    gPic = Path.Combine("/Documents/", fileName + Path.GetExtension(doc.FileName));
                }
                doc.SaveAs(Path.Combine(Server.MapPath("~/Documents/"), fileName + Path.GetExtension(doc.FileName)));

                document.Created = System.DateTime.Now;
                document.AuthorId = user.Id;
                document.Department = "IT";
                document.File = gPic;

                db.Documents.Add(document);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "IT");
        }
    }
}