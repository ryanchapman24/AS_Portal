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
using System.IO;

namespace AS_TestProject.Controllers
{
    public class EventsController : UserNames
    {
        // GET: Events/Edit/5
        public ActionResult EditEvent(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event calEvent = db.Events.Find(id);
            if (calEvent == null)
            {
                return HttpNotFound();
            }
            return View(calEvent);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent([Bind(Include = "Id,Title,AuthorId,StartDate,EndDate,AllDay,Universal")] Event calEvent)
        {
            if (ModelState.IsValid)
            {
                db.Events.Attach(calEvent);
                db.Entry(calEvent).Property("Title").IsModified = true;
                db.Entry(calEvent).Property("StartDate").IsModified = true;
                db.Entry(calEvent).Property("EndDate").IsModified = true;
                db.Entry(calEvent).Property("AllDay").IsModified = true;
                db.Entry(calEvent).Property("Universal").IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Calendar", "Home");
            }
            return View(calEvent);
        }

        // GET: Events/Delete/5
        public ActionResult DeleteEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event calEvent = db.Events.Find(id);
            if (calEvent == null)
            {
                return HttpNotFound();
            }
            return View(calEvent);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEventConfirmed(int id)
        {
            Event calEvent = db.Events.Find(id);
            db.Events.Remove(calEvent);
            db.SaveChanges();
            return RedirectToAction("Calendar", "Home");
        }
    }
}