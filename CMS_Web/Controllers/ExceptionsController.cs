using System;
using System.Linq;
using System.Web.Mvc;
using CMS_Web.Models;
using CMS_Web.DAL;
using Kendo.Mvc.UI;
using WebMatrix.WebData;
using Kendo.Mvc.Extensions;
using CMS_Web.Helpers;
using System.Collections.Generic;

namespace CMS_Web.Controllers
{
    public class ExceptionsController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();
        private Data dataHelper = new Data();

        public ActionResult MyOpenExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetMyOpenExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {

            DataSourceResult result = db.Exceptions.Where(p => p.Active == true && p.state != exceptionState.Closed && p.CurrentActionByStaff == WebSecurity.CurrentUserId).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MyClosedExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetMyClosedExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {

            DataSourceResult result = db.Exceptions.Where(p => (p.Active != true || p.state == exceptionState.Closed) && p.CurrentActionByStaff == WebSecurity.CurrentUserId).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

      

        public ActionResult AllOpenExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetAllOpenExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {

            DataSourceResult result = db.Exceptions.Where(p => p.Active == true && p.state != exceptionState.Closed).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AllClosedExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetAllClosedExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {

            DataSourceResult result = db.Exceptions.Where(p => p.Active != true || p.state == exceptionState.Closed).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MyReportsOpenExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetMyReportsOpenExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {
            List<UserProfile> subordinateList = dataHelper.getAllSubordinates(UserID);
            List<ExceptionDetail> exceptionList = new List<ExceptionDetail>();

            foreach (UserProfile u in subordinateList)
            {
                var userExceptions = db.Exceptions.Where(p => (p.Active == true && p.state != exceptionState.Closed) && p.relatedStaffID == u.UserId);
                foreach (ExceptionDetail e in userExceptions)
                    exceptionList.Add(e);
            }
            
            DataSourceResult result = exceptionList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MyReportsClosedExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetMyReportsClosedExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {

            List<UserProfile> subordinateList = dataHelper.getAllSubordinates(UserID);
            List<ExceptionDetail> exceptionList = new List<ExceptionDetail>();

            foreach (UserProfile u in subordinateList)
            {
                var userExceptions = db.Exceptions.Where(p => (p.Active != true || p.state == exceptionState.Closed) && p.relatedStaffID == u.UserId);
                foreach (ExceptionDetail e in userExceptions)
                    exceptionList.Add(e);
            }

            DataSourceResult result = exceptionList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MyTeamsOpenExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetMyTeamsOpenExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {
            List<UserProfile> subordinateList = dataHelper.getAllTeamMembers(UserID);
            List<ExceptionDetail> exceptionList = new List<ExceptionDetail>();

            foreach (UserProfile u in subordinateList)
            {
                var userExceptions = db.Exceptions.Where(p => (p.Active == true && p.state != exceptionState.Closed) && p.relatedStaffID == u.UserId);
                foreach (ExceptionDetail e in userExceptions)
                    exceptionList.Add(e);
            }

            DataSourceResult result = exceptionList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MyTeamsClosedExceptions()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetMyTeamsClosedExceptions([DataSourceRequest]DataSourceRequest request, int UserID)
        {

            List<UserProfile> subordinateList = dataHelper.getAllTeamMembers(UserID);
            List<ExceptionDetail> exceptionList = new List<ExceptionDetail>();

            foreach (UserProfile u in subordinateList)
            {
                var userExceptions = db.Exceptions.Where(p => (p.Active != true || p.state == exceptionState.Closed) && p.relatedStaffID == u.UserId);
                foreach (ExceptionDetail e in userExceptions)
                    exceptionList.Add(e);
            }

            DataSourceResult result = exceptionList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ExceptionHistory(int exceptionID, string returnPage)
        {

            Data dataHelper = new Data();
            ExceptionDetail exceptionRecord = db.Exceptions.Where(p => p.ID == exceptionID).SingleOrDefault();
            ViewData["exceptionDescription"] = exceptionRecord.Description;
            ViewData["exceptionStaffName"] = dataHelper.getStaffFullName(exceptionRecord.relatedStaffID);
            ViewData["exceptionID"] = exceptionRecord.ID;
            ViewData["UserProfiles"] = db.UserProfiles;
            ViewData["returnPage"] = returnPage;

            return View();
        }



        public ActionResult CloseException(int ID, string returnPage)
        {
            ExceptionDetail exceptionRecord = db.Exceptions.Where(p => p.ID == ID).SingleOrDefault();
            ViewData["ExceptionDescription"] = exceptionRecord.Description;
            ViewData["ExceptionID"] = exceptionRecord.ID;
            ViewData["returnPage"] = returnPage;

            return View();

        }

        public ActionResult EscalateException(int ID, string returnPage)
        {
            ExceptionDetail exceptionRecord = db.Exceptions.Where(p => p.ID == ID).SingleOrDefault();
            ViewData["ExceptionDescription"] = exceptionRecord.Description;
            ViewData["ExceptionID"] = exceptionRecord.ID;
            ViewData["returnPage"] = returnPage;

            return View();

        }

        public ActionResult setExceptionStatusToClose(int ID, string Reason)
        {
            ExceptionDetail exceptionToClose = db.Exceptions.Find(ID);

            if (exceptionToClose != null)
            {
                exceptionToClose.state = exceptionState.Closed;
                exceptionToClose.CurrentActionByStaff = WebSecurity.CurrentUserId;
                db.Entry(exceptionToClose).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                ExceptionHistory i = new ExceptionHistory();
                i.ActionDescription = "Exception Closed.  " + Reason;
                i.ActionStaffID = WebSecurity.CurrentUserId;
                i.ActionDate = DateTime.Now;
                i.ParentID = ID;
                i.stateAtHistoryRecordCreation = exceptionState.Closed;
                addExceptionHistoryItem(i);

            }

            return RedirectToAction("MyOpenExceptions", new { Controller = "Exceptions", action = "MyOpenExceptions" });
        }

        public void addExceptionHistoryItem(ExceptionHistory item)
        {
            db.ExceptionHistory.Add(item);
            db.SaveChanges();
        }

        public ActionResult GetExceptionHistory([DataSourceRequest]DataSourceRequest request, int exceptionID)
        {
            
            DataSourceResult result = db.ExceptionHistory.Where(p => p.ParentID == exceptionID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetExceptionNotes([DataSourceRequest]DataSourceRequest request, int exceptionID)
        {

            DataSourceResult result = db.ExceptionNotes.Where(p => p.ParentID == exceptionID && p.Deleted != true).ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("NoteDate", System.ComponentModel.ListSortDirection.Descending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult exceptionNoteInline_Create([DataSourceRequest] DataSourceRequest request, ExceptionNote passedEntry, string exceptionID)
        {
           
            if (passedEntry != null)
            {
                passedEntry.CreationStaffID = WebSecurity.CurrentUserId;
                passedEntry.LastEditStaffID = WebSecurity.CurrentUserId;
                passedEntry.NoteDate = DateTime.Now;
                passedEntry.EditDate = DateTime.Now;
                //passedEntry.NoteText = passedEntry.NoteText;
                passedEntry.ParentID = Convert.ToInt32(exceptionID);
                db.ExceptionNotes.Add(passedEntry);
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult exceptionNoteInline_Update([DataSourceRequest] DataSourceRequest request, ExceptionNote passedEntry, string exceptionID)
        {
           
            if (passedEntry != null)
            {
                var updatedEntry = new ExceptionNote();
                updatedEntry = db.ExceptionNotes.Where(r => r.ID == passedEntry.ID).FirstOrDefault();
                if (updatedEntry != null)
                {
                    updatedEntry.NoteText = passedEntry.NoteText;
                    updatedEntry.LastEditStaffID = WebSecurity.CurrentUserId;
                    updatedEntry.EditDate = DateTime.Now;
                    db.Entry(updatedEntry).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult exceptionNoteInline_Destroy([DataSourceRequest] DataSourceRequest request, ExceptionNote passedEntry)
        {

            if (passedEntry != null)
            {
                ExceptionNote entryToEdit = db.ExceptionNotes.Find(passedEntry.ID);
                if (entryToEdit != null)
                {
                    entryToEdit.Deleted = true;
                    db.Entry(entryToEdit).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult escalateExceptionWithReason(int ID, string Reason)
        {
            CMS_Web.Helpers.Data dataHelper = new Helpers.Data();

            GlobalSettingsModel gs = dataHelper.getGlobalSettings();


            ExceptionDetail exceptionToEscalate = db.Exceptions.Find(ID);

            if (exceptionToEscalate != null)
            {
                
                int upline = dataHelper.getExceptionEscallationManagerName(exceptionToEscalate.CurrentActionByStaff);
                if (upline != -1)
                {
                    string uplineStaffName = dataHelper.getStaffFullName(upline);

                    ExceptionHistory newHistoryItem = new Models.ExceptionHistory();

                    newHistoryItem.ParentID = ID;
                    newHistoryItem.stateAtHistoryRecordCreation = exceptionToEscalate.state;
                    newHistoryItem.ActionStaffID = upline;
                    newHistoryItem.ActionDate = DateTime.Now;
                    newHistoryItem.ActionDescription = "Manually Escallated by " + dataHelper.getStaffFullName(WebSecurity.CurrentUserId) + " to " + uplineStaffName + " due to " + Reason;
                    db.ExceptionHistory.Add(newHistoryItem);
                    
                    exceptionToEscalate.CurrentActionByStaff = upline;
                    exceptionToEscalate.CurrentActionByDate = DateTime.Now + TimeSpan.FromHours(gs.hoursBetweenExceptionEscallation);
                    db.Entry(exceptionToEscalate).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    dataHelper.addUserActivityLog("Exception '" + exceptionToEscalate.Description + "' manually escallated to " + uplineStaffName + " due to '" + Reason, WebSecurity.CurrentUserId, "");

                }
            }


            return RedirectToAction("MyOpenExceptions", new { Controller = "Exceptions", action = "MyOpenExceptions" });
        }
    }
}
