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

namespace AS_TestProject.Controllers
{
    public class MessagesController : UserNames
    {
        // GET: Messages
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            ViewBag.Unread = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Read == false && m.Active == true).OrderByDescending(m => m.Id).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Read = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Read == true && m.Active == true).OrderByDescending(m => m.Id).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Outbox = db.OutboundMessages.Where(m => m.AuthorId == user.Id && m.Out == true && m.Active == true).OrderByDescending(m => m.Id).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Drafts = db.OutboundMessages.Where(m => m.AuthorId == user.Id && m.Out == false && m.Active == true).OrderByDescending(m => m.Id).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.TrashIn = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Active == false).OrderByDescending(m => m.Id).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.TrashOut = db.OutboundMessages.Where(m => m.AuthorId == user.Id && m.Active == false).OrderByDescending(m => m.Id).Include(m => m.Author).Include(m => m.Receiver).ToList();
            ViewBag.Users = db.Users.Where(u => u.Id != user.Id).OrderBy(u => u.FirstName).ToList();
            return View();
        }

        // GET: Messages/Details/5
        public ActionResult OutboundDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutboundMessage outboundMsg = db.OutboundMessages.Find(id);
            if (outboundMsg == null)
            {
                return HttpNotFound();
            }
            return View(outboundMsg);
        }

        // GET: Messages/Details/5
        public ActionResult InboundDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InboundMessage inboundMsg = db.InboundMessages.Find(id);
            if (inboundMsg == null)
            {
                return HttpNotFound();
            }
            return View(inboundMsg);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.ReceiverId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Sent,Subject,Content,File,AuthorId,ReceiverId,Out,Read,Urgent,Active")] OutboundMessage outboundMsg)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                outboundMsg.AuthorId = user.Id;
                outboundMsg.Sent = System.DateTime.Now;
                outboundMsg.Out = true;
                outboundMsg.Read = false;
                outboundMsg.Active = true;
                db.OutboundMessages.Add(outboundMsg);
                db.SaveChanges();

                var inboundMsg = new InboundMessage();
                inboundMsg.Sent = outboundMsg.Sent;
                inboundMsg.Subject = outboundMsg.Subject;
                inboundMsg.Content = outboundMsg.Content;
                inboundMsg.File = outboundMsg.File;
                inboundMsg.AuthorId = outboundMsg.AuthorId;
                inboundMsg.ReceiverId = outboundMsg.ReceiverId;
                inboundMsg.Out = outboundMsg.Out;
                inboundMsg.Read = outboundMsg.Read;
                inboundMsg.Urgent = outboundMsg.Urgent;
                inboundMsg.Active = outboundMsg.Active;

                db.InboundMessages.Add(inboundMsg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReceiverId = new SelectList(db.Users, "Id", "FirstName", outboundMsg.ReceiverId);
            return View(outboundMsg);
        }

        // GET: Messages/Delete/5
        public ActionResult DeleteInbound(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InboundMessage inboundMsg = db.InboundMessages.Find(id);
            if (inboundMsg == null)
            {
                return HttpNotFound();
            }
            return View(inboundMsg);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteInBoundConfirmed(int id)
        {
            InboundMessage inboundMsg = db.InboundMessages.Find(id);
            db.InboundMessages.Remove(inboundMsg);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Messages/Delete/5
        public ActionResult DeleteOutbound(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutboundMessage outboundMsg = db.OutboundMessages.Find(id);
            if (outboundMsg == null)
            {
                return HttpNotFound();
            }
            return View(outboundMsg);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOutBoundConfirmed(int id)
        {
            OutboundMessage outboundMsg = db.OutboundMessages.Find(id);
            db.OutboundMessages.Remove(outboundMsg);
            db.SaveChanges();
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
