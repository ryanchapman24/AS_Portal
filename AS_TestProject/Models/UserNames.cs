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
                if (User.IsInRole("Admin") || (User.IsInRole("Suggestions") && User.IsInRole("Quality") && User.IsInRole("Marketing")))
                {
                    ViewBag.Suggestions = db.Suggestions.Where(s => s.New == true).OrderByDescending(s => s.Created).ToList();
                }
                else if ((User.IsInRole("Suggestions") && User.IsInRole("Quality")))
                {
                    ViewBag.Suggestions = db.Suggestions.Where(s => s.SuggestionType.Department == "Quality" && s.New == true).OrderByDescending(s => s.Created).ToList();
                }
                else if ((User.IsInRole("Suggestions") && User.IsInRole("Marketing")))
                {
                    ViewBag.Suggestions = db.Suggestions.Where(s => s.SuggestionType.Department == "Marketing" && s.New == true).OrderByDescending(s => s.Created).ToList();
                }
                else
                {
                    ViewBag.Suggestions = db.Suggestions.Where(s => s.SuggestionType.Department == "None").ToList(); ;
                }
                ViewBag.Notifications = user.Notifications.Where(n => n.New == true).OrderByDescending(n => n.Created).ToList();
                if (user.Roles.Any(r => r.RoleId == "580182ec-c40a-4f5d-87bf-227f48e7d221") && user.Roles.Count() == 1)
                {
                    ViewBag.Team = db.Users.Where(t => t.Roles.Where(r => r.RoleId == "039c88d0-5882-4dcc-a892-82700cf1a803").Count() == 0 && (t.PositionID == 1 || t.PositionID == 5 || t.PositionID == 26 || t.PositionID == 29 || t.PositionID == 37) && (t.SiteID == user.SiteID)).Where(t => t.Id != user.Id).OrderBy(t => t.FirstName);
                }
                else
                {
                    ViewBag.Team = db.Users.Where(t => t.Roles.Where(r => r.RoleId == "039c88d0-5882-4dcc-a892-82700cf1a803").Count() == 0).Where(t => t.Id != user.Id).OrderBy(t => t.FirstName);
                }

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
                /////////////////////////////// Sales CFR Questions
                ViewBag.sTE1 = "Did the CSR use proper greetings and instructions?";
                ViewBag.sTE2 = "Did the CSR display a professional attitude?";
                ViewBag.sTE3 = "Did the CSR  use appropriate tone of voice and phrasing, avoids jargon, and speaks clearly?";
                ViewBag.sTE4 = "Did the CSR use appropriate closing; confirms information and actions taken?";
                ViewBag.sP1 = "Does the CSR listen to and empathize with customers; acknowledges customers concerns?";
                ViewBag.sP2 = "Did the CSR gather information to determine customer’s needs and apply problem solving skills effectively?";
                ViewBag.sP3 = "Did the CSR communicate information about the products to the customer clearly and completely?";
                ViewBag.sP4 = "Did the CSR control the pace and flow of the conversation?";
                ViewBag.sC1 = "Did the CSR state the recorded call disclosure?";
                ViewBag.sC2 = "Did the CSR demonstrate compliance in role as CSR; has a good understanding of call center policies and procedures: DNC, Permission Requirements or/ Misleading  Information?";
                ViewBag.sA1 = "Did the CSR efficiently handle the customer’s request; keeps the pace of the call moving and manages call time effectively?";
                ViewBag.sA2 = "Did the CSR utilize the tools and rebuttals (at least three times) before ending the call?";
                ViewBag.sAOI1 = "Did the CSR verify caller identity, provided accurate information to the customer, read the confirmation scripting, correctly enter account changes and notes, and properly codes call (for call tracking)?";
                ViewBag.sAOI2 = "Did the CSR demonstrate good procedural skills, effectively uses systems and tools, and follows call guidelines and scripts?";
                /////////////////////////////// Acurian CFR Questions
                ViewBag.aI1 = "Opening/Greeting";
                ViewBag.aI2 = "Capturing Personal Identifiable Information";
                ViewBag.aI3 = "Privacy Policy";
                ViewBag.aCS1 = "Tone and Inflection";
                ViewBag.aCS2 = "Pronunciation";
                ViewBag.aCS3 = "Speech";
                ViewBag.aCS4 = "Pace";
                ViewBag.aCS5 = "Behavior";
                ViewBag.aCS6 = "Attitude";
                ViewBag.aCS7 = "Professionalism";
                ViewBag.aSS1 = "Listening";
                ViewBag.aSS2 = "Personalization";
                ViewBag.aSS3 = "Rapport";
                ViewBag.aSS4 = "Empathy and Acknowledgement";
                ViewBag.aSS5 = "Respect";
                ViewBag.aSS6 = "Confidence";
                ViewBag.aSS7 = "Engaging";
                ViewBag.aSS8 = "Control";
                ViewBag.aCO1 = "Follows Script";
                ViewBag.aCO2 = "Reading";
                ViewBag.aCO3 = "Transitional Phrases and Holds";
                ViewBag.aCO4 = "Sites";
                ViewBag.aCO5 = "Follows Protocol";
                ViewBag.aCO6 = "Resources and Tools";
                ViewBag.aCO7 = "Accuracy";
                ViewBag.aCO8 = "Rebuttals";
                ViewBag.aCL1 = "Site Information";
                ViewBag.aCL2 = "Screening Result";
                ViewBag.aCL3 = "Wrap-up / Closing";

                base.OnActionExecuting(filterContext);
            }
        }
    }
}