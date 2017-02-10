using AS_TestProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AS_TestProject.Entities;

namespace AS_TestProject.Models
{
    public class UserNames : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        private ReportEntities mb = new ReportEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                ViewBag.DisplayName = user.DisplayName;
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Position = user.Position.PositionName;
                ViewBag.PositionDescr = user.Position.PositionDescription;
                ViewBag.Site = user.Site.SiteName;
                ViewBag.Email = user.Email;
                ViewBag.PhoneNumber = user.PhoneNumber;
                ViewBag.ProfilePic = user.ProfilePic;
                ViewBag.StartYear = mb.Employees.FirstOrDefault(e => e.EmployeeID == user.EmployeeID).HireDate.Year;

                ViewBag.TaskTally = user.TaskTally;
                ViewBag.UrgentTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 4).OrderBy(t => t.Id).ToList();
                ViewBag.HighTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 3).OrderBy(t => t.Id).ToList();
                ViewBag.MediumTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 2).OrderBy(t => t.Id).ToList();
                ViewBag.LowTasks = db.Tasks.Where(t => t.AuthorId == user.Id && t.Complete == false && t.TaskPriorityId == 1).OrderBy(t => t.Id).ToList();

                ViewBag.Messages = db.InboundMessages.Where(m => m.ReceiverId == user.Id && m.Read == false && m.Out == true && m.Active == true && m.Ghost == false).OrderByDescending(m => m.Sent).Include(m => m.Author).Include(m => m.Receiver).ToList();
                ViewBag.Notifications = user.Notifications.Where(n => n.New = true).OrderByDescending(n => n.Created).ToList();
                ViewBag.Team = db.Users.Where(t => t.Roles.Where(r => r.RoleId == "039c88d0-5882-4dcc-a892-82700cf1a803").Count() == 0).Where(t => t.Id != user.Id).OrderBy(t => t.FirstName);

                /////////////////////////////// Mortgage CFR Questions
                ViewBag.mTE1 = "Does the CSR state 'Hello' and pause to identify gender?";
                ViewBag.mTE2 = "Does the CSR state the contact name assumptively without asking a question?";
                ViewBag.mTE3 = "Does the CSR state the website of inquiry (if applicable)?";
                ViewBag.mTE4 = "Does the CSR transition into the script without hesitation?";
                ViewBag.mTE5 = "Does the CSR display confidence and clarity throughout the call?";
                ViewBag.mP1 = "Does the CSR speak with a delivery rate that is clearly understood?";
                ViewBag.mP2 = "Does the CSR convey a sense of empathy and have a professional tone?";
                ViewBag.mC1 = "Does the CSR identify him/herself with confidence using first and last name?";
                ViewBag.mC2 = "Does the CSR state that he/she is calling on a recorded line?";
                ViewBag.mC3 = "Does the CSR avoid prohibited references to rates and closing costs?";
                ViewBag.mA1 = "Does the CSR ask the questions listed using the word for word script?";
                ViewBag.mA2 = "Does the CSR listen carefully to the answers provided by the contact?";
                ViewBag.mA3 = "Does the CSR ask the verifying questions to determine transfer eligibility?";
                ViewBag.mA4 = "Does the CSR use all 3 steps of the rebuttal process (T.Phrase, Rebuttal, Close)?";
                ViewBag.mA5 = "Does the CSR exercise good judgement based on the answers to ensure excellent quality?";
                ViewBag.mAOI1 = "Does the CSR input the LO's NMLS/ITT ID number correctly?";
                ViewBag.mAOI2 = "Does the CSR warm transfer properly to the LO in a professional tone?";
                ViewBag.mAOI3 = "Does the CSR update Browser/Five9 to reflect the proper disposition needed?";
                ViewBag.mAOI4 = "Does the CSR connect to the correct department?";
                /////////////////////////////// Insurance CFR Questions
                ViewBag.iTE1 = "Does the CSR state 'Hello' and pause to identify gender?";
                ViewBag.iTE2 = "Does the CSR state the contact name assumptively without asking a question?";
                ViewBag.iTE3 = "Does the CSR state the website of inquiry (if applicable)?";
                ViewBag.iTE4 = "Does the CSR transition into the script without hesitation?";
                ViewBag.iTE5 = "Does the CSR display confidence and clarity throughout the call?";
                ViewBag.iP1 = "Does the CSR speak with a delivery rate that is clearly understood?";
                ViewBag.iP2 = "Does the CSR convey a sense of empathy and have a professional tone?";
                ViewBag.iC1 = "Does the CSR identify him/herself with confidence using first and last name?";
                ViewBag.iC2 = "Does the CSR state that he/she is calling on a recorded line?";
                ViewBag.iC3 = "Does the CSR avoid prohibited references to rates?";
                ViewBag.iA1 = "Does the CSR state the name of the insurance agency?";
                ViewBag.iA2 = "Does the CSR listen carefully to the answers provided by the contact?";
                ViewBag.iA3 = "Does the CSR ask the verifying questions to determine transfer eligibility?";
                ViewBag.iA4 = "Does the CSR provide a soft rebuttal when necessary?";
                ViewBag.iA5 = "Does the CSR exercise good judgement based on the answers to ensure excellent quality?";
                ViewBag.iAOI1 = "Does the CSR communicate all information to the Insurance Agent in a professional tone?";
                ViewBag.iAOI2 = "Does the CSR introduce the Agent to the customer?";
                ViewBag.iAOI3 = "Does the CSR ask for a bilingual speaking Agent when necessary?";
                ViewBag.iAOI4 = "Does the CSR connect to the correct department?";
                ViewBag.iAOI5 = "Does the CSR select the correct disposition?";
                /////////////////////////////// Patient Recruitment CFR Questions
                ViewBag.pTE1 = "Does the CSR ask if the contact name is available with clarity?";
                ViewBag.pTE2 = "Does the CSR transition into the script without hesitation?";
                ViewBag.pTE3 = "Does the CSR display confidence and clarity throughout the call?";
                ViewBag.pP1 = "Does the CSR speak with a delivery rate that is clearly understood?";
                ViewBag.pP2 = "Does the CSR convey a sense of empathy and have a professional tone?";
                ViewBag.pC1 = "Does the CSR identify him/herself with confidence using first and last name?";
                ViewBag.pC2 = "Does the CSR state that he/she is calling on a recorded line?";
                ViewBag.pC3 = "Does the CSR follow HIPAA compliance procedures and state the full privacy policy?";
                ViewBag.pC4 = "Does the CSR refer to the FAQ's link and training binder correctly?";
                ViewBag.pC5 = "Does the CSR avoid prohibited statements about trail status (if vs when) and diagnosis?";
                ViewBag.pA1 = "Does the CSR ask the questions listed using the word for word script? *(specialty only: Did the CSR also verify all contact information including name, address, and phone number?)";
                ViewBag.pA2 = "Does the CSR listen carefully to the answers provided by the contact and rebuttal when necessary?";
                ViewBag.pA3 = "Does the CSR use proper pronunciation of the medical terms and conditions?";
                ViewBag.pA4 = "Does the CSR select the correct answers before proceeding?";
                ViewBag.pA5 = "Does the CSR exercise good judgement based on the answers to ensure excellent quality?";
                ViewBag.pAOI1 = "Does the CSR update Browser/Five9 to reflect the proper disposition as needed?";
                ViewBag.pAOI2 = "Does the CSR connect to the correct department and select the correct disposition?";

                base.OnActionExecuting(filterContext);
            }
        }
    }
}