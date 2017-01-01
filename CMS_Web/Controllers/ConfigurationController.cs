using CMS_Web.DAL;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Linq;
using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using CMS_Web.Models;
using CMS_Web.Helpers;

namespace CMS_Web.Controllers
{
    public class ConfigurationController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();

        [HttpGet]
        public ActionResult GlobalSettings()
        {
            var settingsRecord = db.GlobalSettings.Find(1);
            return View(settingsRecord);
        }

        [HttpPost]
        public ActionResult GlobalSettings(GlobalSettingsModel updatedRecord)
        {
            var settingsRecord = db.GlobalSettings.Find(1);
            if (settingsRecord != null)
            {
                settingsRecord.daysBeforeQualificaitonExpiryForReminder = updatedRecord.daysBeforeQualificaitonExpiryForReminder;
                settingsRecord.daysBeforeShiftToCheckQualifications = updatedRecord.daysBeforeShiftToCheckQualifications;
                settingsRecord.hoursBetweenExceptionEscallation = updatedRecord.hoursBetweenExceptionEscallation;
                settingsRecord.maximumPercentEarlyShiftLeaving = updatedRecord.maximumPercentEarlyShiftLeaving;
                settingsRecord.maximumPercentShiftCancellations = updatedRecord.maximumPercentShiftCancellations;
                settingsRecord.minimumPercentOnTimeShiftArrivals = updatedRecord.minimumPercentOnTimeShiftArrivals;
                settingsRecord.minimumPercentToDoListCompletions = updatedRecord.minimumPercentToDoListCompletions;
                settingsRecord.minutesAllowedForEarlyShiftLeave = updatedRecord.minutesAllowedForEarlyShiftLeave;
                settingsRecord.minutesAllowedForLateShiftArrival = updatedRecord.minutesAllowedForLateShiftArrival;
                settingsRecord.minutesAllowedForStaffOfflineDuringShift = updatedRecord.minutesAllowedForStaffOfflineDuringShift;
                settingsRecord.minutesBeforeShiftToCheckIfStaffOnline = updatedRecord.minutesBeforeShiftToCheckIfStaffOnline;
                settingsRecord.staffIDForPerformanceReporting = updatedRecord.staffIDForPerformanceReporting;
                settingsRecord.numberOfDaysDurationPerformanceMetricAnalysis = updatedRecord.numberOfDaysDurationPerformanceMetricAnalysis;
                settingsRecord.numberOfDaysDurationToDoPerformanceAnalysis = updatedRecord.numberOfDaysDurationToDoPerformanceAnalysis;
                db.Entry(settingsRecord).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.GlobalSettings.Add(updatedRecord);
            }
            db.SaveChanges();
            return View(updatedRecord);
        }


        public ActionResult SystemStatus()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            var statusRecord = db.SystemStatusRecord.Find(1);
            return View(statusRecord);
            
        }

        public ActionResult incidentEngineStart()
        {
            var statusRecord = db.SystemStatusRecord.Find(1);
            statusRecord.incidentEngineEnabled = true;

            statusRecord.incidentEngineStatus = "Started by " + getCurrentUserName() + " at " + DateTime.Now;
            db.Entry(statusRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Helpers.Data dh = new Helpers.Data();
            dh.addLog("Incident Engine Start", Models.systemLogType.User, "incidentEngineStart", "ConfigurationController", WebSecurity.CurrentUserId);


            return View(statusRecord);
        }

        public ActionResult incidentEngineStop()
        {
            var statusRecord = db.SystemStatusRecord.Find(1);
            statusRecord.incidentEngineEnabled = false;

            statusRecord.incidentEngineStatus = "Stopped by " + getCurrentUserName() + " at " + DateTime.Now;
            db.Entry(statusRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Helpers.Data dh = new Helpers.Data();
            dh.addLog("Incident Engine Stop", Models.systemLogType.User, "incidentEngineStop", "ConfigurationController", WebSecurity.CurrentUserId);


            return View(statusRecord);
        }

        public ActionResult todoEngineStart()
        {
            var statusRecord = db.SystemStatusRecord.Find(1);
            statusRecord.todoEngineEnabled = true;
            statusRecord.todoEngineStatus = "Started by " + getCurrentUserName() + " at " + DateTime.Now;

            db.Entry(statusRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Helpers.Data dh = new Helpers.Data();
            dh.addLog("ToDo Engine Start", Models.systemLogType.User, "todoEngineStart", "ConfigurationController", WebSecurity.CurrentUserId);

            return View(statusRecord);
        }

        public ActionResult todoEngineStop()
        {
            var statusRecord = db.SystemStatusRecord.Find(1);
            statusRecord.todoEngineEnabled = false;
            statusRecord.todoEngineStatus = "Stopped by " + getCurrentUserName() + " at " + DateTime.Now;

            db.Entry(statusRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Helpers.Data dh = new Helpers.Data();
            dh.addLog("ToDo Engine Stop", Models.systemLogType.User, "todoEngineStop", "ConfigurationController", WebSecurity.CurrentUserId);


            return View(statusRecord);
        }
        public ActionResult exceptionEngineStart()
        {
            exceptionEngine ee = new exceptionEngine();

            var statusRecord = db.SystemStatusRecord.Find(1);
            statusRecord.exceptionEngineEnabled = true;
            statusRecord.exceptionEngineStatus = "Started by " + getCurrentUserName() + " at " + DateTime.Now;

            db.Entry(statusRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Helpers.Data dh = new Helpers.Data();
            dh.addLog("Exception Engine Start", Models.systemLogType.User, "exceptionEngineStart", "ConfigurationController", WebSecurity.CurrentUserId);

            ee.processQualificationsExpiryExceptions();
            ee.processShiftQualificationsExceptions();
            ee.processToDoExceptions();
            ee.processIncidentExceptions();

            return View(statusRecord);
        }

        public ActionResult exceptionEngineStop()
        {
            var statusRecord = db.SystemStatusRecord.Find(1);
            statusRecord.exceptionEngineEnabled = false;
            statusRecord.exceptionEngineStatus = "Stopped by " + getCurrentUserName() + " at " + DateTime.Now;

            db.Entry(statusRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Helpers.Data dh = new Helpers.Data();
            dh.addLog("Exception Engine Stop", Models.systemLogType.User, "exceptionEngineStop", "ConfigurationController", WebSecurity.CurrentUserId);

            return View(statusRecord);
        }


        private string getCurrentUserName()
        {
            int UserID = WebSecurity.CurrentUserId;
            var userRecord = (from row in db.UserProfiles where row.UserId == UserID select row).FirstOrDefault();
            return userRecord.FirstName + " " + userRecord.LastName;
        }


        public ActionResult GetSystemLog([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = db.SystemLog.ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("EventDate", System.ComponentModel.ListSortDirection.Ascending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        
    }
}
