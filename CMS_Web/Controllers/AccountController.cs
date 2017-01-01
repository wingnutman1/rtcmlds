using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using CMS_Web.Filters;
using CMS_Web.Models;
using CMS_Web.DAL;
using CMS_Web.ViewModels;
using System.Net.Mail;
using Kendo.Mvc.UI;
using System.Data;
using Kendo.Mvc.Extensions;

namespace CMS_Web.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {

        private CMS_WebContext db = new CMS_WebContext();

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        public ActionResult GetUserList([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.UserProfiles.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSubordinateList([DataSourceRequest]DataSourceRequest request, int UserID)
        {

            Helpers.Data dataHelper = new Helpers.Data();

            List<UserProfile> usersThatReportToMe = new List<UserProfile>();

            UserProfile me = db.UserProfiles.Where(p => p.UserId == WebSecurity.CurrentUserId).FirstOrDefault();

            usersThatReportToMe = dataHelper.getAllSubordinates(me.UserId);
            
            DataSourceResult result = usersThatReportToMe.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        


        public ActionResult UpdateUserList([DataSourceRequest]DataSourceRequest request, int id, int? reportsTo)
        {

            var userToUpdate = db.UserProfiles.Where(r => r.UserId == id).FirstOrDefault();


            if (userToUpdate != null)
            {
                //if (reportsTo == 0)
                //    userToUpdate.reportsTo = null;
                //else
                    userToUpdate.reportsTo = reportsTo;
                db.Entry(userToUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            DataSourceResult result = db.UserProfiles.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult StaffTeams()
        {
            return View();
        }


        public ActionResult UpdateUser([DataSourceRequest]DataSourceRequest request)
        {

            return View("ListUsers");

        }

        public ActionResult editTeamList(int teamListID)
        {
            StaffTeam team = db.StaffTeams.Where(p => p.ID == teamListID).FirstOrDefault();
            ViewData["teamListID"] = teamListID;
            ViewData["teamName"] = team.Name;
            ViewData["returnPage"] = "StaffTeams";
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetStaffTeams([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.StaffTeams.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetStaffTeamList([DataSourceRequest]DataSourceRequest request, int teamListID)
        {

            DataSourceResult result = db.StaffInTeams.Where(p => p.TeamID == teamListID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetStaffForTeamList([DataSourceRequest]DataSourceRequest request, int teamListID)
        {

            List<UserProfile> userToReturn = new List<UserProfile>();
            bool notFound = true;

            var usersInTeamCurrently = db.StaffInTeams.Where(p => p.TeamID == teamListID);

            foreach (var s in db.UserProfiles)
            {
                notFound = true;
                foreach (var u in usersInTeamCurrently)
                {
                    if (s.UserId == u.StaffID)
                        notFound = false;
                }
                if (notFound)
                    userToReturn.Add(s);
            }

            DataSourceResult result = userToReturn.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RemoveStaffFromTeamList(int staffID, int teamID)
        {

            var staffEntryToDelete = db.StaffInTeams.Where(p => p.StaffID == staffID && p.TeamID == teamID).FirstOrDefault();
            if (staffEntryToDelete != null)
            {
                db.StaffInTeams.Remove(staffEntryToDelete);
                db.SaveChanges();
            }

            return RedirectToAction("editTeamList", new { teamListID = teamID });

        }

        public ActionResult AddStaffToTeamList(int staffID, int teamID)
        {

            StaffInTeam staffEntryToAdd = new StaffInTeam();

            staffEntryToAdd.StaffID = staffID;
            staffEntryToAdd.TeamID = teamID;

            db.StaffInTeams.Add(staffEntryToAdd);
            db.SaveChanges();

            return RedirectToAction("editTeamList", new { teamListID = teamID });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult StaffTeamsEditingInline_Create([DataSourceRequest] DataSourceRequest request, StaffTeam passedEntry)
        {

            if (passedEntry != null)
            {

                db.StaffTeams.Add(passedEntry);
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult StaffTeamsEditingInline_Update([DataSourceRequest] DataSourceRequest request, StaffTeam passedEntry)
        {

            if (passedEntry != null)
            {
                db.Entry(passedEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult StaffTeamsEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, StaffTeam passedEntry)
        {

            if (passedEntry != null)
            {
                StaffTeam newEntry = db.StaffTeams.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.StaffTeams.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UserActivity(int userID, string returnPage)
        {
            UserProfile user = db.UserProfiles.Where(p => p.UserId == userID).SingleOrDefault();
            ViewData["UserFullName"] = user.FullName;
            ViewData["returnPage"] = returnPage;
            ViewData["userID"] = userID;
            return View();
        }

        public ActionResult GetUserActivity([DataSourceRequest]DataSourceRequest request, int userID)
        {

            DataSourceResult result = db.UserActivityLog.Where(p => p.StaffID == userID).ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }


        public ActionResult getAllRoles()
        {
            var roles = Roles.GetAllRoles();
            return View(roles);
        }

        public ActionResult ListUsers()
        {
            return View();
        }

        public ActionResult TeamStatus()
        {
            return View();
        }

        public ActionResult UserDetails()
        {
             return View();
        }




        

        public ActionResult EditUserQualifications(int id = 0)
        {
            var user = db.UserProfiles.Where(p => p.UserId == id).FirstOrDefault();
            ViewData["userID"] = user.UserId;
            ViewData["staffName"] = user.FullName;
            ViewData["Qualifications"] = db.Qualifications;
            ViewData["ReturnPage"] = "ListUsers";

            return View();

        }

        public ActionResult GetSelectableQualificationsForStaff([DataSourceRequest]DataSourceRequest request, int userID)
        {

            List<Qualification> qualificationsToReturn = new List<Qualification>();
            bool notFound = true;

            var qualificationsUserHas = db.StaffQualifications.Where(p => p.StaffID == userID);

            foreach (var q in db.Qualifications)
            {
                notFound = true;
                foreach (var quh in qualificationsUserHas)
                {
                    if (q.ID == quh.QualificationID)
                        notFound = false;
                }
                if (notFound)
                    qualificationsToReturn.Add(q);
            }

            DataSourceResult result = qualificationsToReturn.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult renewQualificationForStaff(int staffID, int qualificationID)
        {

            var qualification = db.Qualifications.Where(p => p.ID == qualificationID).FirstOrDefault();

            if (qualification != null)
            {
                var staffQualification = db.StaffQualifications.Where(p => p.StaffID == staffID && p.QualificationID == qualificationID).FirstOrDefault();

                if (staffQualification != null)
                {
                    staffQualification.expired = false;
                    // Update exceptions to closed if they exist.
                    // TODO 
                    if (staffQualification.expiryExceptionRaised)
                    {
                        staffQualification.expiryExceptionRaised = false;
                        // Update exception to closed

                    }
                    if (staffQualification.expiryWarningExceptionRaised)
                    { 
                        staffQualification.expiryWarningExceptionRaised = false;
                        // Update exception to closed
                    }
                    staffQualification.ExpiryDate = DateTime.Now.AddMonths(qualification.MonthsValidFor);
                    staffQualification.RenewalDate = DateTime.Now;
                    db.Entry(staffQualification).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }

            return RedirectToAction("editUserQualifications", new { id = staffID });

        }


        public ActionResult removeQualificationFromStaff(int staffID, int qualificationID)
        {

            var qualificationEntryToDelete = db.StaffQualifications.Where(p => p.StaffID == staffID && p.QualificationID == qualificationID).FirstOrDefault();
            if (qualificationEntryToDelete != null)
            {
                db.StaffQualifications.Remove(qualificationEntryToDelete);
                db.SaveChanges();
            }

            return RedirectToAction("editUSerQualifications", new { id = staffID});

        }

        public ActionResult GetUserQualificationList([DataSourceRequest]DataSourceRequest request, int userID)
        {

            DataSourceResult result = db.StaffQualifications.Where(p => p.StaffID == userID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult AddQualificationToUser(int staffID, int qualificationID)
        {

            StaffQualification staffQualificationToAdd = new StaffQualification();

            staffQualificationToAdd.StaffID = staffID;
            staffQualificationToAdd.QualificationID = qualificationID;
            staffQualificationToAdd.expired = true;
            staffQualificationToAdd.ExpiryDate = DateTime.Now;
            staffQualificationToAdd.RenewalDate = DateTime.Now;

            db.StaffQualifications.Add(staffQualificationToAdd);
            db.SaveChanges();

            return RedirectToAction("EditUserQualifications", new { id = staffID});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult StaffQualificationEditingInline_Update([DataSourceRequest] DataSourceRequest request, StaffQualification passedEntry)
        {

            if (passedEntry != null)
            {
                if (passedEntry.ExpiryDate >= DateTime.Now)
                    passedEntry.expired = false;
                else
                    passedEntry.expired = true;

                db.Entry(passedEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }




        public ActionResult getUserRoles([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = Roles.GetAllRoles().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getUsersExtendedData([DataSourceRequest] DataSourceRequest request)
        {
            
            List<staffFullNameModel> users = new List<staffFullNameModel>();
            
            var userList = db.UserProfiles;

            foreach (UserProfile up in userList)
            {
                staffFullNameModel newUser = new staffFullNameModel();
                newUser.UserId = up.UserId;
                newUser.FullName = up.FullName;
                users.Add(newUser);
            }

            DataSourceResult result = users.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Users/Edit/5

        public ActionResult EditUsers(int id = 0)
        {
            using (var userCtx = new UsersContext())
            {
                UserProfile user = userCtx.UserProfiles.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                ExtendedAccountModel userData = new ExtendedAccountModel();

                var roles = (SimpleRoleProvider)Roles.Provider;
                var allRoles = roles.GetAllRoles();
                var userRoles = roles.GetRolesForUser(user.UserName);
                userData.roles = userRoles;
                ViewBag.AllRoles = allRoles;
                ViewBag.Users = db.UserProfiles.Where(r => (r.Inactive == false || r.Inactive == null) && r.UserId != id);
                userData.user = user;
                return View(userData);

            }
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsers(ExtendedAccountModel userData)
        {

            List<string> rolesToChange = new List<string>();
            List<string> usersToChange = new List<string>();

            TryValidateModel(userData);

            if (ModelState.IsValid)
            {


                var rolesProvider = (WebMatrix.WebData.SimpleRoleProvider)Roles.Provider;
                string[] currentRoles = rolesProvider.GetRolesForUser(userData.user.UserName);
                foreach (string r in currentRoles)
                {
                    if (r != "")
                    {
                        rolesToChange.Add(r);
                        usersToChange.Add(userData.user.UserName);
                    }
                }

                if (rolesToChange.Count() > 0)
                {
                    try
                    {
                        rolesProvider.RemoveUsersFromRoles(usersToChange.ToArray(), rolesToChange.ToArray());
                    }
                    catch
                    { }
                }


                rolesToChange.Clear();
                usersToChange.Clear();

                // Add user to the new roles selected
                foreach (string r in userData.roles)
                {
                    if (r != "" && r != "System.String[]")
                    {
                        rolesToChange.Add(r);
                        usersToChange.Add(userData.user.UserName);
                    }
                }
                

                if (rolesToChange.Count() > 0)
                    for (int i = 0; i < rolesToChange.Count(); i++)
                        rolesProvider.AddUsersToRoles(new[] { usersToChange[i] }, new[] { rolesToChange[i] });

                //                rolesProvider.AddUsersToRoles(usersToChange.ToArray(), rolesToChange.ToArray());


                if (userData.user.Inactive == null)
                    userData.user.Inactive = false;

                using (var userCtx = new UsersContext())
                { 
                    userCtx.Entry(userData.user).State = System.Data.Entity.EntityState.Modified;
                    userCtx.SaveChanges();
                    return RedirectToAction("ListUsers");
                }
            }
            return RedirectToAction("ListUsers");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordResetSuccess()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordResetFailure()
        {
            return View();
        }

        // GET: Account/LostPassword
        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        // POST: Account/LostPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordModel model)
        {
           if (ModelState.IsValid)
           {
              UserProfile user;
              using (var context = new UsersContext())
              {
                 int foundUserID = (from u in context.UserProfiles
                                      where u.UserName == model.Email
                                      select u.UserId).FirstOrDefault();
                 if (foundUserID != 0)
                 {
                    user = context.UserProfiles.Find(foundUserID);
                 }
                 else
                 {
                    user = null;
                 }
              }
              if (user != null)
              {
                 // Generae password token that will be used in the email link to authenticate user
                 var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
                 // Generate the html link sent via email
                 string resetLink = "<a href='"
                    + Url.Action("ResetPassword", "Account", new { rt = token }, "http") 
                    + "'>Reset Password Link</a>";
 
                 // Email stuff
                 string subject = "Reset your password for LDS CMS";
                 string body = "Your link: " + resetLink;
                 string from = "donotreply@ldssa.org.au";
 
                 MailMessage message = new MailMessage(from, model.Email);
                 message.Subject = subject;
                 message.Body = body;
                 SmtpClient client = new SmtpClient();
                 client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                 client.PickupDirectoryLocation = @"C:\email";

                 // Attempt to send the email
                 try
                 {
                    client.Send(message);
                 }
                 catch (Exception e)
                 {
                    ModelState.AddModelError("", "Email cannot be sent : " + e.Message);
                 }
              }         
              else // Email not found
              {
                 /* Note: You may not want to provide the following information
                 * since it gives an intruder information as to whether a
                 * certain email address is registered with this website or not.
                 * If you're really concerned about privacy, you may want to
                 * forward to the same "Success" page regardless whether an
                 * user was found or not. This is only for illustration purposes.
                 */
                 ModelState.AddModelError("", "Email not found.");
              }
           }
 
           /* You may want to send the user to a "Success" page upon the successful
           * sending of the reset email link. Right now, if we are 100% successful
           * nothing happens on the page. :P
           */
           return RedirectToAction("EmailSentConfirmation", "Account");
        }

        // GET: /Account/EmailResetConfirmation
        [AllowAnonymous]
        public ActionResult EmailSentConfirmation()
        {
            return View();
        }


        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.ReturnToken = rt;
            return View(model);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, model.Password);
                if (resetResponse)
                {
                    return RedirectToAction("PasswordResetSuccess", "Account");
                }
                else
                {
                    return RedirectToAction("PasswordResetFailure", "Account");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                        
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
