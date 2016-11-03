using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AS_TestProject.Models;
using AS_TestProject.Entities;
using AS_TestProject.Models.Helpers;

namespace AS_TestProject.Controllers
{
    [Authorize(Roles = "Quality")]
    public class QualityController : UserNames
    {
        private ReportEntities mb = new ReportEntities();

        // GET: Quality
        public ActionResult Index()
        {
            ViewBag.Domains = mb.DomainMasters.Where(d => d.IsActive == true).ToList();
            return View();
        }
    }
}