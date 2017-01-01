using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Web.Models;
using CMS_Web.ViewModels;
using CMS_Web.DAL;
using WebMatrix.WebData;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;
using CMS_Web.Helpers;


namespace CMS_Web.Controllers
{
    public class ToDoController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();

        
        #region myToDo

        public ActionResult myToDo()
        {

            return View();

        }

        public ActionResult myCompletedTasks()
        {

            return View();

        }

        public ActionResult StaffCompletedTasks()
        {
            return View();
        }

        public ActionResult StaffActive()
        {
            return View();
        }


        public ActionResult getStaffCompletedToDoList([DataSourceRequest]DataSourceRequest request, string userID)
        {
            int intStaffID = Convert.ToInt16(userID);

            DataSourceResult result = db.ToDoListItems.Where(rec => rec.Complete && rec.StaffID == intStaffID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getStaffActiveToDoList([DataSourceRequest]DataSourceRequest request, string userID)
        {
            int intStaffID = Convert.ToInt16(userID);

            DataSourceResult result = db.ToDoListItems.Where(rec => !rec.Complete && rec.StaffID == intStaffID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult getMyToDoList([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.ToDoListItems.Where(rec => rec.StaffID == WebSecurity.CurrentUserId).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getMyIncompleteToDoList([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.ToDoListItems.Where(rec => !rec.Complete && rec.StaffID == WebSecurity.CurrentUserId).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getMyCompleteToDoList([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.ToDoListItems.Where(rec => rec.Complete && rec.StaffID == WebSecurity.CurrentUserId).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffToDoEditingInline_Create([DataSourceRequest] DataSourceRequest request, ToDoListItem passedEntry)
        {

            if (passedEntry != null)
            {

                ToDoListItem newEntry = new ToDoListItem();
                newEntry.Description = passedEntry.Description;
                newEntry.RequiredCompletionBy = passedEntry.RequiredCompletionBy;
                newEntry.RelatedClientID = passedEntry.RelatedClientID;
                newEntry.RelatedLocationID = passedEntry.RelatedLocationID;
                newEntry.StaffID = WebSecurity.CurrentUserId;
                db.ToDoListItems.Add(newEntry);
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }
     
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffToDoEditingInline_Update([DataSourceRequest] DataSourceRequest request, ToDoListItem passedEntry)
        {

            if (passedEntry != null)
            {
                if (passedEntry.Complete)
                {
                    ToDoListItem newEntry = db.ToDoListItems.Find(passedEntry.ID);
                    newEntry.Complete = true;
                    newEntry.ID = passedEntry.ID;
                    newEntry.CompletedDate = DateTime.Now;
                    newEntry.CompletedNote = passedEntry.CompletedNote;
                    newEntry.StaffID = WebSecurity.CurrentUserId;
                    db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffToDoEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ToDoListItem passedEntry)
        {

            if (passedEntry != null)
            {
                ToDoListItem newEntry = db.ToDoListItems.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.ToDoListItems.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region ClientTemplate

        public ActionResult ClientTemplate()
        {
            return View();
        }

        public ActionResult getClients([DataSourceRequest]DataSourceRequest request)
        {

            DataTable Clients = new DataTable();
            Clients.Columns.Add("ID");
            Clients.Columns.Add("FirstName");
            Clients.Columns.Add("LastName");
            Clients.Columns.Add("FullName");

            foreach (var Client in db.Clients)
            {
                Clients.Rows.Add(Client.ID, Client.FirstName, Client.LastName, Client.FirstName + " " + Client.LastName);
            }

            DataView dv = Clients.DefaultView;
            dv.Sort = "LastName DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult getClientFullName([DataSourceRequest]DataSourceRequest request)
        {

            DataTable Client = new DataTable();
            Client.Columns.Add("ID");
            Client.Columns.Add("Name");
            Client.Columns.Add("LastName");


            foreach (var client in db.Clients)
            {
                Client.Rows.Add(client.ID, client.FirstName + " " + client.LastName, client.LastName);
            }

            DataView dv = Client.DefaultView;
            dv.Sort = "LastName ASC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetClientRecurringToDoTask([DataSourceRequest] DataSourceRequest request, string userID)
        {
            int clientIDInt = Convert.ToInt32(userID);

            DataSourceResult clientRecurringToDoItems = new DataSourceResult();

            if (clientIDInt > 0)
            {
                var toDoItems = db.ToDoListRecurringItems.Where(od => od.ClientID == clientIDInt);
                var returnToDoListItems = new List<ToDoListRecurringItem>();
                foreach (ToDoListRecurringItem j in toDoItems)
                {
                    ToDoListRecurringItem newEntry = new ToDoListRecurringItem();
                    newEntry.ID = j.ID;
                    newEntry.AutoGenerateEvent= (string)j.AutoGenerateEvent;
                    newEntry.ClientID = (int)j.ClientID;
                    newEntry.Task = j.Task;
                    newEntry.TimeToComplete = (DateTime)j.TimeToComplete;
                    if (j.DayOfWeek != null)
                        newEntry.DayOfWeek = (DayOfWeek)j.DayOfWeek;
                    returnToDoListItems.Add(newEntry);
                }

                clientRecurringToDoItems = returnToDoListItems.ToDataSourceResult(request);
                return Json(clientRecurringToDoItems, JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientsRecurringToDoEditingInline_Create([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry, string userID)
        {

            if (passedEntry != null)
            {

                ToDoListRecurringItem newEntry = new ToDoListRecurringItem();
                newEntry.ClientID = Convert.ToInt32(userID);
                newEntry.AutoGenerateEvent = passedEntry.AutoGenerateEvent;
                newEntry.Task = passedEntry.Task;
                newEntry.TimeToComplete = passedEntry.TimeToComplete;
                newEntry.DayOfWeek = passedEntry.DayOfWeek;
                db.ToDoListRecurringItems.Add(newEntry);
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientsRecurringToDoEditingInline_Update([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry, string userID)
        {

            if (passedEntry != null)
            {
                ToDoListRecurringItem newEntry = db.ToDoListRecurringItems.Find(passedEntry.ID);
                newEntry.ClientID = Convert.ToInt32(userID);
                newEntry.AutoGenerateEvent = passedEntry.AutoGenerateEvent;
                newEntry.TimeToComplete = passedEntry.TimeToComplete;
                newEntry.DayOfWeek = passedEntry.DayOfWeek;
                newEntry.ID = passedEntry.ID;
                newEntry.Task = passedEntry.Task;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientsRecurringToDoEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry)
        {

            if (passedEntry != null)
            {
                ToDoListRecurringItem newEntry = db.ToDoListRecurringItems.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.ToDoListRecurringItems.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region SiteTemplate

        public ActionResult SiteTemplate()
        {
            return View();
        }

        public ActionResult getSites([DataSourceRequest]DataSourceRequest request)
        {

            DataTable Sites = new DataTable();
            Sites.Columns.Add("ID");
            Sites.Columns.Add("Name");

            foreach (var Site in db.Locations)
            {
                Sites.Rows.Add(Site.ID, Site.Name);
            }

            DataView dv = Sites.DefaultView;
            dv.Sort = "Name DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetSiteRecurringToDoTask([DataSourceRequest] DataSourceRequest request, string userID)
        {
            int siteIDInt = Convert.ToInt32(userID);

            DataSourceResult siteRecurringToDoItems = new DataSourceResult();

            if (siteIDInt > 0)
            {
                var toDoItems = db.ToDoListRecurringItems.Where(od => od.SiteID == siteIDInt);
                var returnToDoListItems = new List<ToDoListRecurringItem>();
                foreach (ToDoListRecurringItem j in toDoItems)
                {
                    ToDoListRecurringItem newEntry = new ToDoListRecurringItem();
                    newEntry.ID = j.ID;
                    newEntry.AutoGenerateEvent = (string)j.AutoGenerateEvent;
                    newEntry.SiteID = (int)j.SiteID;
                    newEntry.Task = j.Task;
                    newEntry.TimeToComplete = (DateTime)j.TimeToComplete;
                    newEntry.DayOfWeek = (DayOfWeek)j.DayOfWeek;
                    returnToDoListItems.Add(newEntry);
                }

                siteRecurringToDoItems = returnToDoListItems.ToDataSourceResult(request);
                return Json(siteRecurringToDoItems, JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult sitesRecurringToDoEditingInline_Create([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry, string userID)
        {

            if (passedEntry != null)
            {

                ToDoListRecurringItem newEntry = new ToDoListRecurringItem();
                newEntry.SiteID = Convert.ToInt32(userID);
                newEntry.AutoGenerateEvent = passedEntry.AutoGenerateEvent;
                newEntry.Task = passedEntry.Task;
                newEntry.TimeToComplete = passedEntry.TimeToComplete;
                newEntry.DayOfWeek = passedEntry.DayOfWeek;
                db.ToDoListRecurringItems.Add(newEntry);
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult sitesRecurringToDoEditingInline_Update([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry, string userID)
        {

            if (passedEntry != null)
            {
                ToDoListRecurringItem newEntry = db.ToDoListRecurringItems.Find(passedEntry.ID);
                newEntry.SiteID = Convert.ToInt32(userID);
                newEntry.AutoGenerateEvent = passedEntry.AutoGenerateEvent;
                newEntry.TimeToComplete = passedEntry.TimeToComplete;
                newEntry.DayOfWeek = passedEntry.DayOfWeek;
                newEntry.ID = passedEntry.ID;
                newEntry.Task = passedEntry.Task;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult sitesRecurringToDoEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry)
        {

            if (passedEntry != null)
            {
                ToDoListRecurringItem newEntry = db.ToDoListRecurringItems.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.ToDoListRecurringItems.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region StaffTemplate

        public ActionResult StaffTemplate()
        {
            return View();
        }

        public ActionResult getStaff([DataSourceRequest]DataSourceRequest request)
        {

            DataTable Staff = new DataTable();
            Staff.Columns.Add("ID");
            Staff.Columns.Add("FirstName");
            Staff.Columns.Add("LastName");


            foreach (var staffMember in db.UserProfiles)
            {
                Staff.Rows.Add(staffMember.UserId, staffMember.FirstName, staffMember.LastName);
            }

            DataView dv = Staff.DefaultView;
            dv.Sort = "LastName DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getStaffFullName([DataSourceRequest]DataSourceRequest request)
        {

            DataTable Staff = new DataTable();
            Staff.Columns.Add("ID");
            Staff.Columns.Add("Name");
            Staff.Columns.Add("LastName");


            foreach (var staffMember in db.UserProfiles)
            {
                Staff.Rows.Add(staffMember.UserId, staffMember.FirstName + " " + staffMember.LastName, staffMember.LastName);
            }

            DataView dv = Staff.DefaultView;
            dv.Sort = "LastName ASC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetStaffRecurringToDoTask([DataSourceRequest] DataSourceRequest request, int userID)
        {

            DataSourceResult staffRecurringToDoItems = new DataSourceResult();

            if (userID > 0)
            {
                var toDoItems = db.ToDoListRecurringItems.Where(od => od.StaffID == userID);
                //var returnToDoListItems = new List<ToDoListRecurringItem>();
                //foreach (ToDoListRecurringItem j in toDoItems)
                //{
                //    ToDoListRecurringItem newEntry = new ToDoListRecurringItem();
                //    newEntry.ID = j.ID;
                //    newEntry.AutoGenerateEvent = (string)j.AutoGenerateEvent;
                //    newEntry.StaffID = (int)j.StaffID;
                //    newEntry.SiteID = (int)j.SiteID;
                //    if (j.TimeToComplete != null)
                //        newEntry.TimeToComplete = (DateTime)j.TimeToComplete;
                //    if (j.DayOfWeek != null)
                //        newEntry.DayOfWeek = (DayOfWeek)j.DayOfWeek;
                //    newEntry.Task = j.Task;
                //    returnToDoListItems.Add(newEntry);
                //}

                //staffRecurringToDoItems = returnToDoListItems.ToDataSourceResult(request);

                staffRecurringToDoItems = toDoItems.ToDataSourceResult(request);
                return Json(staffRecurringToDoItems, JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffRecurringToDoEditingInline_Create([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry, string userID)
        {

            if (passedEntry != null)
            {

                ToDoListRecurringItem newEntry = new ToDoListRecurringItem();
                newEntry.StaffID = Convert.ToInt32(userID);
                newEntry.AutoGenerateEvent = passedEntry.AutoGenerateEvent;
                newEntry.Task = passedEntry.Task;
                newEntry.TimeToComplete = passedEntry.TimeToComplete;
                newEntry.DayOfWeek = passedEntry.DayOfWeek;
                db.ToDoListRecurringItems.Add(newEntry);
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffRecurringToDoEditingInline_Update([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry, int userID)
        {

            if (passedEntry != null)
            {
                ToDoListRecurringItem newEntry = db.ToDoListRecurringItems.Find(passedEntry.ID);
                newEntry.StaffID = userID;
                newEntry.SiteID = passedEntry.SiteID;
                newEntry.ClientID = passedEntry.ClientID;
                newEntry.AutoGenerateEvent = passedEntry.AutoGenerateEvent;
                newEntry.TimeToComplete = passedEntry.TimeToComplete;
                newEntry.DayOfWeek = passedEntry.DayOfWeek;
                newEntry.ID = passedEntry.ID;
                newEntry.Task = passedEntry.Task;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffRecurringToDoEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ToDoListRecurringItem passedEntry)
        {

            if (passedEntry != null)
            {
                ToDoListRecurringItem newEntry = db.ToDoListRecurringItems.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.ToDoListRecurringItems.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        [HttpPost]
        public ActionResult AllocateToDoToStaffFromClient(int selectedStaff, int selectedToDoTemplate, DateTime targetDate)
        {

            HttpRequestBase r = Request;
            string returnString = "";
            string currentUserName = "";
            string selectedStaffName = "";
            int staffID = selectedStaff;
            DateTime date = targetDate;
            Helpers.Data dataHelper = new Data();

            int CurrentUserID = WebSecurity.CurrentUserId;
            currentUserName = dataHelper.getStaffFullName(CurrentUserID);
            selectedStaffName = dataHelper.getStaffFullName(selectedStaff);

            var toDoItems = db.ToDoListRecurringItems.Where(od => od.ClientID == selectedToDoTemplate);

            foreach (ToDoListRecurringItem j in toDoItems)
            {
                if (dataHelper.taskExist(selectedStaff, j.Task, targetDate + j.TimeToComplete.Value.TimeOfDay))
                    returnString += "<p>Task '" + j.Task + "' already exists and has not been added again.</p>";
                else
                {
                    ToDoListItem newEntry = new ToDoListItem();
                    newEntry.CreatedBy = currentUserName + " from template.";
                    newEntry.CreatedDate = DateTime.Now;
                    newEntry.Description = j.Task;
                    newEntry.RelatedClientID = j.ClientID;
                    newEntry.RelatedLocationID = j.SiteID;
                    newEntry.RequiredCompletionBy = targetDate + j.TimeToComplete.Value.TimeOfDay;
                    newEntry.StaffID = selectedStaff;
                    if (j.ClientID != null)
                        newEntry.RelatedTo = dataHelper.getClientFullName((int)j.ClientID);
                    else
                        if (j.SiteID != null)
                            newEntry.RelatedTo = dataHelper.getSiteName((int)j.SiteID);
                        else
                            newEntry.RelatedTo = "General";

                    db.ToDoListItems.Add(newEntry);
                    returnString += "<p>Task '" + j.Task + "' added.</p>";
                }
            }

            db.SaveChanges();

            return Json(returnString);

        }

        [HttpPost]
        public ActionResult AllocateToDoToStaffFromSite(int selectedStaff, int selectedToDoTemplate, DateTime targetDate)
        {

            HttpRequestBase r = Request;
            string returnString = "";
            string currentUserName = "";
            string selectedStaffName = "";
            int staffID = selectedStaff;
            DateTime date = targetDate;
            Helpers.Data dataHelper = new Data();

            int CurrentUserID = WebSecurity.CurrentUserId;
            currentUserName = dataHelper.getStaffFullName(CurrentUserID);
            selectedStaffName = dataHelper.getStaffFullName(selectedStaff);
            
            var toDoItems = db.ToDoListRecurringItems.Where(od => od.SiteID == selectedToDoTemplate);

            foreach (ToDoListRecurringItem j in toDoItems)
            {
                if (dataHelper.taskExist(selectedStaff, j.Task, targetDate + j.TimeToComplete.Value.TimeOfDay))
                    returnString += "<p>Task '" + j.Task + "' already exists and has not been added again.</p>";
                else
                {
                    ToDoListItem newEntry = new ToDoListItem();
                    newEntry.CreatedBy = currentUserName + " from template.";
                    newEntry.CreatedDate = DateTime.Now;
                    newEntry.Description = j.Task;
                    newEntry.RelatedClientID = j.ClientID;
                    newEntry.RelatedLocationID = j.SiteID;
                    newEntry.RequiredCompletionBy = targetDate + j.TimeToComplete.Value.TimeOfDay;
                    newEntry.StaffID = selectedStaff;
                    if (j.ClientID != null)
                        newEntry.RelatedTo = dataHelper.getClientFullName((int)j.ClientID);
                    else
                        if (j.SiteID != null)
                            newEntry.RelatedTo = dataHelper.getSiteName((int)j.SiteID);
                        else
                            newEntry.RelatedTo = "General";
                            
                    db.ToDoListItems.Add(newEntry);
                    returnString += "<p>Task '" + j.Task + "' added.</p>";
                }
            }

            db.SaveChanges();

            return Json(returnString);
            
        }

        [HttpPost]
        public ActionResult AllocateToDoToStaffFromStaff(int selectedStaff, int selectedToDoTemplate, DateTime targetDate)
        {

            HttpRequestBase r = Request;
            string returnString = "";
            string currentUserName = "";
            string selectedStaffName = "";
            int staffID = selectedStaff;
            DateTime date = targetDate;
            Helpers.Data dataHelper = new Data();

            int CurrentUserID = WebSecurity.CurrentUserId;
            currentUserName = dataHelper.getStaffFullName(CurrentUserID);
            selectedStaffName = dataHelper.getStaffFullName(selectedStaff);

            var toDoItems = db.ToDoListRecurringItems.Where(od => od.StaffID == selectedToDoTemplate);

            foreach (ToDoListRecurringItem j in toDoItems)
            {
                if (dataHelper.taskExist(selectedStaff, j.Task, targetDate))
                    returnString += "<p>Task '" + j.Task + "' already exists and has not been added again.</p>";
                else
                {
                    ToDoListItem newEntry = new ToDoListItem();
                    newEntry.CreatedBy = currentUserName + " from template.";
                    newEntry.CreatedDate = DateTime.Now;
                    newEntry.Description = j.Task;
                    newEntry.RelatedClientID = j.ClientID;
                    newEntry.RelatedLocationID = j.SiteID;
                    //newEntry.RequiredCompletionBy = targetDate + j.TimeToComplete.Value.TimeOfDay;
                    newEntry.RequiredCompletionBy = targetDate;
                    newEntry.StaffID = selectedStaff;
                    if (j.ClientID != null)
                        newEntry.RelatedTo = dataHelper.getClientFullName((int)j.ClientID);
                    else
                        if (j.SiteID != null)
                            newEntry.RelatedTo = dataHelper.getSiteName((int)j.SiteID);
                        else
                            newEntry.RelatedTo = "General";

                    db.ToDoListItems.Add(newEntry);
                    returnString += "<p>Task '" + j.Task + "' added.</p>";
                }
            }

            db.SaveChanges();

            return Json(returnString);

        }

        [HttpPost]
        public ActionResult addNewToDoTaskForMe(int forSite, int forClient, DateTime targetDate, string taskDescription)
        {

            HttpRequestBase r = Request;
            string returnString = "";
            string currentUserName = "";
            string forClientName = "";
            string forSiteName = "";

            Helpers.Data dataHelper = new Data();

            currentUserName = dataHelper.getStaffFullName(WebSecurity.CurrentUserId);
            forSiteName = dataHelper.getSiteName(forSite);
            forClientName = dataHelper.getClientFullName(forClient);

            ToDoListItem newEntry = new ToDoListItem();
            newEntry.CreatedBy = currentUserName + " created manually.";
            newEntry.CreatedDate = DateTime.Now;
            newEntry.Description = taskDescription;
            newEntry.RelatedClientID = forClient;
            newEntry.RelatedLocationID = forSite;
            newEntry.RequiredCompletionBy = targetDate;
            newEntry.StaffID = WebSecurity.CurrentUserId;

            if (forClient != 0 && forSite != 0)
                newEntry.RelatedTo = forClientName + " & " + forSiteName;
            else
                if (forSite != 0)
                    newEntry.RelatedTo = forSiteName;
                else
                    if (forClient != 0)
                        newEntry.RelatedTo = forClientName;
                    else
                        newEntry.RelatedTo = "General";

            db.ToDoListItems.Add(newEntry);

            returnString += "<p>Task '" + taskDescription + "' added.</p>";

            db.SaveChanges();

            return Json(returnString);

        }

        [HttpPost]
        public ActionResult addNewToDoTaskForStaff(int forSite, int forClient, DateTime targetDate, string taskDescription, int forStaff)
        {

            HttpRequestBase r = Request;
            string returnString = "";
            string staffName = "";
            string forClientName = "";
            string forSiteName = "";

            Helpers.Data dataHelper = new Data();

            staffName = dataHelper.getStaffFullName(forStaff);
            forSiteName = dataHelper.getSiteName(forSite);
            forClientName = dataHelper.getClientFullName(forClient);

            ToDoListItem newEntry = new ToDoListItem();
            newEntry.CreatedBy = staffName + " created manually.";
            newEntry.CreatedDate = DateTime.Now;
            newEntry.Description = taskDescription;
            newEntry.RelatedClientID = forClient;
            newEntry.RelatedLocationID = forSite;
            newEntry.RequiredCompletionBy = targetDate;
            newEntry.StaffID = forStaff;

            if (forClient != 0 && forSite != 0)
                newEntry.RelatedTo = forClientName + " & " + forSiteName;
            else
                if (forSite != 0)
                    newEntry.RelatedTo = forSiteName;
                else
                    if (forClient != 0)
                        newEntry.RelatedTo = forClientName;
                    else
                        newEntry.RelatedTo = "General";

            db.ToDoListItems.Add(newEntry);

            returnString += "<p>Task '" + taskDescription + "' added.</p>";

            db.SaveChanges();

            return Json(returnString);

        }

        public ActionResult getDayOfWeek()
        {

            return Json(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList(), JsonRequestBehavior.AllowGet);

        }




    }
}
