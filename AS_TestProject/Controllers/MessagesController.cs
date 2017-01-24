using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_TestProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace AS_TestProject.Controllers
{
    [Authorize]
    public class MessagesController : UserNames
    {
        // GET: Messages
        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            ViewBag.Unread = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Read == false && m.Out == true && m.Active == true && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Read = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Read == true && m.Out == true && m.Active == true && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Outbox = db.OutboundMessages.Where(m => m.AuthorId == user.Id && m.Out == true && m.Active == true && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Drafts = db.OutboundMessages.Where(m => m.AuthorId == user.Id && m.Out == false && m.Active == true && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.TrashIn = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Active == false && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.TrashOut = db.OutboundMessages.Where(m => m.AuthorId == user.Id && m.Active == false && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Users = db.Users.Where(t => t.Roles.Where(r => r.RoleId == "039c88d0-5882-4dcc-a892-82700cf1a803").Count() == 0).Where(u => u.Id != user.Id).OrderBy(u => u.FirstName).ToList();
            ViewBag.ReceiverId = new SelectList(db.Users.Where(t => t.Roles.Where(r => r.RoleId == "039c88d0-5882-4dcc-a892-82700cf1a803").Count() == 0).Where(u => u.Id != user.Id).OrderBy(u => u.FirstName), "Id", "DisplayName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Sent,Subject,Content,AuthorId,ReceiverId,File,Out,Read,Urgent,Active,Ghost")] OutboundMessage outboundMsg, string pId, HttpPostedFileBase file)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {

                if (Request.Form["saveAsDraft"] != null)
                {
                    // Code for saving message as a draft
                    outboundMsg.Sent = System.DateTime.Now;
                    outboundMsg.AuthorId = user.Id;
                    outboundMsg.Out = false;
                    outboundMsg.Read = false;
                    outboundMsg.Active = true;
                    outboundMsg.Ghost = false;
                    outboundMsg.File = null;
                    db.OutboundMessages.Add(outboundMsg);
                    db.SaveChanges();

                    var inboundMsg = new InboundMessage();
                    inboundMsg.Sent = outboundMsg.Sent;
                    inboundMsg.AuthorId = outboundMsg.AuthorId;
                    inboundMsg.ReceiverId = outboundMsg.ReceiverId;
                    inboundMsg.Out = outboundMsg.Out;
                    inboundMsg.Read = outboundMsg.Read;
                    inboundMsg.Urgent = outboundMsg.Urgent;
                    inboundMsg.Active = outboundMsg.Active;
                    inboundMsg.Ghost = outboundMsg.Ghost;
                    inboundMsg.Subject = outboundMsg.Subject;
                    inboundMsg.Content = outboundMsg.Content;

                    db.InboundMessages.Add(inboundMsg);
                    db.SaveChanges();

                    if (pId != null)
                    {
                        return RedirectToAction("ProfilePage", "Home", new { id = pId });
                    }
                    return RedirectToAction("Index");
                }

                else if (Request.Form["createMessage"] != null)
                {
                    // Code for creating an outbound message
                    outboundMsg.Sent = System.DateTime.Now;
                    outboundMsg.AuthorId = user.Id;
                    outboundMsg.Out = true;
                    outboundMsg.Read = false;
                    outboundMsg.Active = true;
                    outboundMsg.Ghost = false;
                    if (file != null)
                    {
                        //Counter
                        var num = 0;
                        //Gets Filename without the extension
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var gPic = Path.Combine("/MessageFiles/", fileName + Path.GetExtension(file.FileName));
                        //Checks if pPic matches any of the current attachments, 
                        //if so it will loop and add a (number) to the end of the filename
                        while (db.OutboundMessages.Any(d => d.File == gPic))
                        {
                            //Sets "filename" back to the default value
                            fileName = Path.GetFileNameWithoutExtension(file.FileName);
                            //Add's parentheses after the name with a number ex. filename(4)
                            fileName = string.Format(fileName + "(" + ++num + ")");
                            //Makes sure pPic gets updated with the new filename so it could check
                            gPic = Path.Combine("/MessageFiles/", fileName + Path.GetExtension(file.FileName));
                        }
                        file.SaveAs(Path.Combine(Server.MapPath("~/MessageFiles/"), fileName + Path.GetExtension(file.FileName)));
                        outboundMsg.File = gPic;
                    }
                    db.OutboundMessages.Add(outboundMsg);
                    db.SaveChanges();

                    var inboundMsg = new InboundMessage();
                    inboundMsg.Sent = outboundMsg.Sent;
                    inboundMsg.AuthorId = outboundMsg.AuthorId;
                    inboundMsg.ReceiverId = outboundMsg.ReceiverId;
                    inboundMsg.Out = outboundMsg.Out;
                    inboundMsg.Read = outboundMsg.Read;
                    inboundMsg.Urgent = outboundMsg.Urgent;
                    inboundMsg.Active = outboundMsg.Active;
                    inboundMsg.Ghost = outboundMsg.Ghost;
                    inboundMsg.Subject = outboundMsg.Subject;
                    inboundMsg.Content = outboundMsg.Content;
                    inboundMsg.File = outboundMsg.File;

                    db.InboundMessages.Add(inboundMsg);
                    db.SaveChanges();

                    if (pId != null)
                    {
                        return RedirectToAction("ProfilePage", "Home", new { id = pId });
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ReceiverId = new SelectList(db.Users.Where(u => u.Id != user.Id).OrderBy(u => u.FirstName), "Id", "DisplayName", outboundMsg.ReceiverId);
            if (pId != null)
            {
                return RedirectToAction("ProfilePage", "Home", new { id = pId });
            }
            return RedirectToAction("Index");
        }

        // POST: Messages/SendDraft
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult SendDraft([Bind(Include = "Id,Sent,Subject,Content,AuthorId,ReceiverId,File,Out,Read,Urgent,Active,Ghost")] OutboundMessage outboundMsg, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                db.OutboundMessages.Attach(outboundMsg);
                outboundMsg.Sent = System.DateTime.Now;
                outboundMsg.Out = true;
                if (file != null)
                {
                    //Counter
                    var num = 0;
                    //Gets Filename without the extension
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var gPic = Path.Combine("/MessageFiles/", fileName + Path.GetExtension(file.FileName));
                    //Checks if pPic matches any of the current attachments, 
                    //if so it will loop and add a (number) to the end of the filename
                    while (db.OutboundMessages.Any(d => d.File == gPic))
                    {
                        //Sets "filename" back to the default value
                        fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        //Add's parentheses after the name with a number ex. filename(4)
                        fileName = string.Format(fileName + "(" + ++num + ")");
                        //Makes sure pPic gets updated with the new filename so it could check
                        gPic = Path.Combine("/MessageFiles/", fileName + Path.GetExtension(file.FileName));
                    }
                    file.SaveAs(Path.Combine(Server.MapPath("~/MessageFiles/"), fileName + Path.GetExtension(file.FileName)));
                    outboundMsg.File = gPic;
                }
                db.Entry(outboundMsg).Property("Subject").IsModified = true;
                db.Entry(outboundMsg).Property("Content").IsModified = true;
                db.Entry(outboundMsg).Property("Urgent").IsModified = true;
                db.SaveChanges();

                var inboundMsg = db.InboundMessages.Find(outboundMsg.Id);
                inboundMsg.Sent = outboundMsg.Sent;
                inboundMsg.Out = outboundMsg.Out;
                inboundMsg.Subject = outboundMsg.Subject;
                inboundMsg.Content = outboundMsg.Content;
                inboundMsg.Urgent = outboundMsg.Urgent;
                inboundMsg.File = outboundMsg.File;
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }

            return RedirectToAction("Index");
        }

        // POST: Messages/UpdateInbox
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult UpdateInbox(List<int> trash, List<int> read)
        {
            if (trash != null)
            {
                int total = trash.Count();
                for (int i = 0; i < total; i++)
                {
                    var inboundMsg = db.InboundMessages.Find(trash[i]);
                    inboundMsg.Active = false;
                    db.SaveChanges();
                }
            }
            if (read != null)
            {
                int total = read.Count();
                for (int i = 0; i < total; i++)
                {
                    var inboundMsg = db.InboundMessages.Find(read[i]);
                    inboundMsg.Read = true;
                    db.SaveChanges();

                    var outboundMsg = db.OutboundMessages.Find(read[i]);
                    outboundMsg.Read = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Messages/UpdateOutbox
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult UpdateOutbox(List<int> trash)
        {
            if (trash != null)
            {
                int total = trash.Count();
                for (int i = 0; i < total; i++)
                {
                    var outboundMsg = db.OutboundMessages.Find(trash[i]);
                    outboundMsg.Active = false;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Messages/UpdateDrafts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult UpdateDrafts(List<int> delete)
        {
            if (delete != null)
            {
                int total = delete.Count();
                for (int i = 0; i < total; i++)
                {
                    var outboundMsg = db.OutboundMessages.Find(delete[i]);
                    db.OutboundMessages.Remove(outboundMsg);
                    db.SaveChanges();

                    var inboundMsg = db.InboundMessages.Find(delete[i]);
                    db.InboundMessages.Remove(inboundMsg);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: Messages/UpdateInTrash
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult UpdateInTrash(List<int> delete)
        {
            if (delete != null)
            {
                int total = delete.Count();
                for (int i = 0; i < total; i++)
                {
                    var inboundMsg = db.InboundMessages.Find(delete[i]);
                    inboundMsg.Ghost = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Messages/UpdateOutTrash
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult UpdateOutTrash(List<int> delete)
        {
            if (delete != null)
            {
                int total = delete.Count();
                for (int i = 0; i < total; i++)
                {
                    var outboundMsg = db.OutboundMessages.Find(delete[i]);
                    outboundMsg.Ghost = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
