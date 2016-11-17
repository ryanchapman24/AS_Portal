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
    public class SisenseController : UserNames
    {
        // GET: Sisense
        public ActionResult Index()
        {
            return View();
        }
    }
}