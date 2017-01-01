using System.Data;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using CMS_Web.Models;
using CMS_Web.DAL;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System;
using WebMatrix.WebData;

namespace CMS_Web.Controllers
{
    public class IncidentsController : Controller
    {
        //
        // GET: /IncidentTypes/

        private CMS_WebContext db = new CMS_WebContext();

        public ActionResult MyOpenIncidents()
        {
            ViewData["UserProfiles"] = db.UserProfiles.Where(r => r.Inactive != true);
            return View();
        }

        public ActionResult MyClosedIncidents()
        {
            ViewData["UserProfiles"] = db.UserProfiles.Where(r => r.Inactive != true);
            return View();
        }

        public ActionResult addIncident()
        {
            return View();
        }

        public ActionResult closeIncident(int ID, string returnPage)
        {
            Incident incidentRecord = db.Incidents.Where(p => p.ID == ID).SingleOrDefault();
            ViewData["incidentDescription"] = incidentRecord.Description;
            ViewData["incidentID"] = incidentRecord.ID;
            ViewData["UserProfiles"] = db.UserProfiles;
            ViewData["returnPage"] = returnPage;

            ViewData["filesExist"] = uploadedIncidentFilesExist(incidentRecord.ID);

            var newDirectoryPath = Server.MapPath(@"~\App_Data\Incident\" + ID.ToString());
            FileInfo noFile = new FileInfo("No Completed Incident Files are uploaded");
            try
            {
                DirectoryInfo directory = new DirectoryInfo(newDirectoryPath);
                var files = directory.GetFiles().ToList();
                if (files.Count == 0)
                    files.Add(noFile);
                ViewData["uploadedFiles"] = files;
            }
            catch
            {
                List<FileInfo> files = new List<FileInfo>()
                {
                    noFile
                };
                ViewData["uploadedFiles"] = files;
            }
            return View();

        }

        public ActionResult setIncidentStatusToClose(int ID)
        {
            Incident incidentToClose = db.Incidents.Find(ID);

            if (incidentToClose != null)
            {
                incidentToClose.currentState = incidentState.Closed;
                db.Entry(incidentToClose).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                IncidentHistory i = new IncidentHistory();
                i.currentActionDescription = "Incident Closed.";
                i.currentStaffID = WebSecurity.CurrentUserId;
                i.historyEntryCreationDate = DateTime.Now;
                i.incidentID = ID;
                i.state = incidentState.Closed;
                i.actionByDate = null;
                addIncidentHistoryItem(i);

            }


            return RedirectToAction("MyOpenIncidents", new { Controller = "Incidents", action = "MyOpenIncidents"});
        }

        public ActionResult reOpenIncident(int ID)
        {
            CMS_Web.Helpers.Data dataHelper = new Helpers.Data();

            Incident incidentToReOpen = db.Incidents.Find(ID);

            if (incidentToReOpen != null)
            {
                incidentToReOpen.currentState = incidentState.AwaitingManager1ACK;
                incidentToReOpen.CurrentManagerID = dataHelper.getIncidentTypeEscalationManagerID(incidentToReOpen.TypeID, 1);
                incidentToReOpen.CurrentAction = "Created - Awaiting " + dataHelper.getIncidentTypeEscalationManagerName(incidentToReOpen.TypeID, 1) + " acknowledgement.";
                db.Entry(incidentToReOpen).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                IncidentHistory i = new IncidentHistory();
                i.currentActionDescription = "Incident Re-Opened.";
                i.currentStaffID = WebSecurity.CurrentUserId;
                i.historyEntryCreationDate = DateTime.Now;
                i.incidentID = ID;
                i.state = incidentState.Closed;
                i.actionByDate = null;
                addIncidentHistoryItem(i);

            }


            return RedirectToAction("MyOpenIncidents", new { Controller = "Incidents", action = "MyOpenIncidents" });
        }

        public ActionResult escalateIncident(int ID)
        {
            CMS_Web.Helpers.Data dataHelper = new Helpers.Data();
            string managerName = "";

            TimeSpan escalationTimespan = new TimeSpan();

            Incident incidentToEscalate = db.Incidents.Find(ID);

            if (incidentToEscalate != null)
            {
                if (incidentToEscalate.currentState == incidentState.Created && dataHelper.getIncidentTypeEscalationManagerName(incidentToEscalate.TypeID, 1) != null)
                {
                    managerName = dataHelper.getIncidentTypeEscalationManagerName(incidentToEscalate.TypeID, 1);
                    escalationTimespan = dataHelper.getIncidentTypeEscalationTimeFrame(incidentToEscalate.TypeID, 1);
                    incidentToEscalate.currentState = incidentState.AwaitingManager1ACK;
                    incidentToEscalate.CurrentManagerID = dataHelper.getIncidentTypeEscalationManagerID(incidentToEscalate.TypeID, 1);
                    incidentToEscalate.CurrentAction = "Awaiting " + managerName + " acknowledgement.";
                }
                else
                if (incidentToEscalate.currentState == incidentState.AwaitingManager1ACK && dataHelper.getIncidentTypeEscalationManagerName(incidentToEscalate.TypeID, 2) != null)
                {
                    managerName = dataHelper.getIncidentTypeEscalationManagerName(incidentToEscalate.TypeID, 2);
                    escalationTimespan = dataHelper.getIncidentTypeEscalationTimeFrame(incidentToEscalate.TypeID, 2);
                    incidentToEscalate.currentState = incidentState.AwaitingManager2ACK;
                    incidentToEscalate.CurrentManagerID = dataHelper.getIncidentTypeEscalationManagerID(incidentToEscalate.TypeID, 2);
                    incidentToEscalate.CurrentAction = "Awaiting " + managerName + " acknowledgement.";
                }
                else
                if (incidentToEscalate.currentState == incidentState.AwaitingManager2ACK && dataHelper.getIncidentTypeEscalationManagerName(incidentToEscalate.TypeID, 3) != null)
                {
                    managerName = dataHelper.getIncidentTypeEscalationManagerName(incidentToEscalate.TypeID, 3);
                    escalationTimespan = dataHelper.getIncidentTypeEscalationTimeFrame(incidentToEscalate.TypeID, 3);
                    incidentToEscalate.currentState = incidentState.AwaitingManager3ACK;
                    incidentToEscalate.CurrentManagerID = dataHelper.getIncidentTypeEscalationManagerID(incidentToEscalate.TypeID, 3);
                    incidentToEscalate.CurrentAction = "Awaiting " + managerName + " acknowledgement.";
                }

                if (managerName != null && managerName != "")
                {
              
                    db.Entry(incidentToEscalate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    IncidentHistory i = new IncidentHistory();
                    i.currentActionDescription = "Incident Manually Escallated to " + managerName;
                    i.currentStaffID = WebSecurity.CurrentUserId;
                    i.actionByStaffID = incidentToEscalate.CurrentManagerID;
                    i.historyEntryCreationDate = DateTime.Now;
                    i.incidentID = ID;
                    i.actionByDate = DateTime.Now + escalationTimespan;
                    i.state = incidentToEscalate.currentState;
                    addIncidentHistoryItem(i);
                }
            }


            return RedirectToAction("MyOpenIncidents", new { Controller = "Incidents", action = "MyOpenIncidents" });
        }

        public ActionResult getIncidentTypeTemplateFiles(int ID)
        {
            string fileList = "";
            var newDirectoryPath = Server.MapPath(@"~\App_Data\Incident_Templates\" + ID.ToString());
            FileInfo noFile = new FileInfo("No Template Files are uploaded");
            try
            {
                DirectoryInfo directory = new DirectoryInfo(newDirectoryPath);
                var files = directory.GetFiles().ToList();
                if (files.Count == 0)
                    fileList = "No Files";
                else
                {
                    bool first = true;

                    foreach (var file in files)
                    {   
                        if (first)
                        {
                            first = false;
                            fileList += file.Name;
                        }
                        else
                            fileList += "  |  " + file.Name;

                    }

                }
            }
            catch
            {
                fileList = "No Files";
            }

            return Content(fileList);
        }

        public ActionResult getUploadedIncidentFiles(int ID)
        {
            string fileList = "";
            var newDirectoryPath = Server.MapPath(@"~\App_Data\Incidents\" + ID.ToString());
            FileInfo noFile = new FileInfo("No Incident Files are uploaded");
            try
            {
                DirectoryInfo directory = new DirectoryInfo(newDirectoryPath);
                var files = directory.GetFiles().ToList();
                if (files.Count == 0)
                    fileList = "No Files";
                else
                {
                    bool first = true;

                    foreach (var file in files)
                    {
                        if (first)
                        {
                            first = false;
                            fileList += file.Name;
                        }
                        else
                            fileList += "  |  " + file.Name;

                    }

                }
            }
            catch
            {
                fileList = "No Files";
            }

            return Content(fileList);
        }

        public bool uploadedIncidentFilesExist(int ID)
        {
            bool filesExists = false;

            var newDirectoryPath = Server.MapPath(@"~\App_Data\Incident\" + ID.ToString());
            try
            {
                DirectoryInfo directory = new DirectoryInfo(newDirectoryPath);
                var files = directory.GetFiles().ToList();
                if (files.Count == 0)
                    filesExists = false;
                else
                {
                    filesExists = true;
                }
            }
            catch
            {
                filesExists = false;
            }

            return filesExists;
        }

        public ActionResult getEscList(int ID)
        {
            string escList = "";
            Helpers.Data dataHelper = new Helpers.Data();

            IncidentType newEntry = db.IncidentTypes.Find(ID);

            if (newEntry != null)
            {
                if (newEntry.firstStaffEscID != 0)
                {
                    if (newEntry.firstEscDays == 0 && newEntry.firstEscHours == 0 && newEntry.firstEscMinutes == 0)
                        escList = dataHelper.getStaffFullName(newEntry.firstStaffEscID) + " will be notified immediately.";
                    else
                    {
                        escList = dataHelper.getStaffFullName(newEntry.firstStaffEscID) + " will be notified after ";
                        if (newEntry.firstEscDays != 0)
                            escList += newEntry.firstEscDays.ToString() + " days ";
                        if (newEntry.firstEscHours != 0)
                            escList += newEntry.firstEscHours.ToString() + " hours ";
                        if (newEntry.firstEscMinutes != 0)
                            escList += newEntry.firstEscMinutes.ToString() + " minutes.";
                    }
                    escList += "<br>";
                }
                if (newEntry.secondStaffEscID != 0)
                {
                    if (newEntry.secondEscDays == 0 && newEntry.secondEscHours == 0 && newEntry.secondEscMinutes == 0)
                        escList += dataHelper.getStaffFullName(newEntry.secondStaffEscID) + " will be then notified immediately.";
                    else
                    { 
                        escList += dataHelper.getStaffFullName(newEntry.secondStaffEscID) + " will be then notified if not acknowledged after ";
                        if (newEntry.secondEscDays != 0)
                            escList += newEntry.secondEscDays.ToString() + " days ";
                        if (newEntry.secondEscHours != 0)
                            escList += newEntry.secondEscHours.ToString() + " hours ";
                        if (newEntry.secondEscMinutes != 0)
                            escList += newEntry.secondEscMinutes.ToString() + " minutes";
                    }
                    escList += "<br>";
                }

                if (newEntry.thirdStaffEscID != 0)
                {
                    if (newEntry.thirdEscDays == 0 && newEntry.thirdEscHours == 0 && newEntry.thirdEscMinutes == 0)
                        escList += dataHelper.getStaffFullName(newEntry.thirdStaffEscID) + " will be finally notified Immediately.";
                    else
                    {
                        escList += dataHelper.getStaffFullName(newEntry.thirdStaffEscID) + " will be finally notified if not acknowledged after ";
                        if (newEntry.thirdEscDays != 0)
                            escList += newEntry.thirdEscDays.ToString() + " days ";
                        if (newEntry.thirdEscHours != 0)
                            escList += newEntry.thirdEscHours.ToString() + " hours ";
                        if (newEntry.thirdEscMinutes != 0)
                            escList += newEntry.thirdEscMinutes.ToString() + " minutes";
                    }
                    escList += "<br>";
                }
            }

            return Content(escList,"html");
        }

        public ActionResult DeleteTemplate(string fName, int ID)
        {
            var fileToDelete = Server.MapPath(@"~\App_Data\Incident_Templates\" + ID.ToString() + @"\" + fName);
            System.IO.File.Delete(fileToDelete);
            return RedirectToAction("incidentTypeTemplateFiles", new { Controller = "Incidents", action = "incidentTypeTemplateFiles", ID = ID });
        }

        public ActionResult DeleteUploadedFile(string fName, int ID)
        {
            var fileToDelete = Server.MapPath(@"~\App_Data\Incident\" + ID.ToString() + @"\" + fName);
            var destDirectory = Server.MapPath(@"~\App_Data\Incident\" + ID.ToString() + @"\Deleted");

            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            var fileToMoveTo = Server.MapPath(@"~\App_Data\Incident\" + ID.ToString() + @"\Deleted\" + DateTime.Now.ToString("yyyyMMddHHmmss ") + fName);
            System.IO.File.Move(fileToDelete, fileToMoveTo);

            IncidentHistory i = new Models.IncidentHistory();
            i.currentActionDescription = "Uploaded file " + fName + " deleted.";
            i.currentStaffID = WebSecurity.CurrentUserId;
            i.historyEntryCreationDate = DateTime.Now;
            i.incidentID = ID;
            i.state = incidentState.UploadFileDelete;
            i.actionByDate = null;
            addIncidentHistoryItem(i);

            return RedirectToAction("uploadedIncidentFiles", new { Controller = "Incidents", action = "uploadedIncidentFiles", ID = ID });
        }


        public void addIncidentHistoryItem(IncidentHistory item)
        {
            db.IncidentHistoryEntries.Add(item);
            db.SaveChanges();
        }


        public ActionResult incidentTypeTemplateFiles(int ID)
        {
            var newDirectoryPath = Server.MapPath(@"~\App_Data\Incident_Templates\" + ID.ToString());
            FileInfo noFile = new FileInfo("No Template Files are uploaded");
            try
            {
                DirectoryInfo directory = new DirectoryInfo(newDirectoryPath);
                var files = directory.GetFiles().ToList();
                if (files.Count == 0)
                    files.Add(noFile);
                ViewData["files"] = files;
            }
            catch
            {
                List<FileInfo> files = new List<FileInfo>()
                {
                    noFile
                };
                ViewData["files"] = files;
            }

            var incidentTypeRecord = db.IncidentTypes.SingleOrDefault(it => it.ID == ID);
            return View(incidentTypeRecord);
        }



        public ActionResult uploadedIncidentFiles(int ID)
        {
            var newDirectoryPath = Server.MapPath(@"~\App_Data\Incident\" + ID.ToString());
            FileInfo noFile = new FileInfo("No Incident Files are uploaded");
            try
            {
                DirectoryInfo directory = new DirectoryInfo(newDirectoryPath);
                var files = directory.GetFiles().ToList();
                if (files.Count == 0)
                    files.Add(noFile);
                ViewData["files"] = files;
            }
            catch
            {
                List<FileInfo> files = new List<FileInfo>()
                {
                    noFile
                };
                ViewData["files"] = files;
            }

            var incidentRecord = db.Incidents.SingleOrDefault(it => it.ID == ID);
            return View(incidentRecord);
        }


        public ActionResult Submit(IEnumerable<HttpPostedFileBase> files, int ID)
        {
            var destDirectory = Server.MapPath(@"~\App_Data\Incident_Templates\" + ID.ToString());

            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            if (files != null)
                foreach (var file in files)
                {
                    var filename = string.Format("{0}\\{1}", Server.MapPath(@"~\App_Data\Incident_Templates\" + ID.ToString()), file.FileName);
                    file.SaveAs(filename);
                }


            return RedirectToAction("incidentTypeTemplateFiles", new { Controller = "Incidents", action = "incidentTypeTemplateFiles", ID = ID });
        }

        public ActionResult SubmitCompleteIncidentFiles(IEnumerable<HttpPostedFileBase> files, int ID)
        {
            var destDirectory = Server.MapPath(@"~\App_Data\Incident\" + ID.ToString());

            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            if (files != null)
                foreach (var file in files)
                {
                    var filename = string.Format("{0}\\{1}", Server.MapPath(@"~\App_Data\Incident\" + ID.ToString()), Path.GetFileName(file.FileName));
                    file.SaveAs(filename);

                    IncidentHistory i = new Models.IncidentHistory();
                    i.currentActionDescription = "File " + file.FileName + " uploaded.";
                    i.currentStaffID = WebSecurity.CurrentUserId;
                    i.historyEntryCreationDate = DateTime.Now;
                    i.incidentID = ID;
                    i.state = incidentState.UploadFileAdd;
                    i.actionByDate = null;
                    addIncidentHistoryItem(i);

                }

            return RedirectToAction("uploadedIncidentFiles", new { Controller = "Incidents", action = "uploadedIncidentFiles", ID = ID });
        }

        public ActionResult Add(Incident newIncident)
        {

            Helpers.Data dataHelper = new Helpers.Data();

            // Create master incident record
            newIncident.reportedDate = DateTime.Now;
            newIncident.StaffReportedID  = WebSecurity.CurrentUserId;
            newIncident.CurrentManagerID = dataHelper.getIncidentTypeEscalationManagerID(newIncident.TypeID,1);
            newIncident.CurrentAction = "Created - Awaiting " + dataHelper.getIncidentTypeEscalationManagerName(newIncident.TypeID,1) + " acknowledgement.";
            db.Incidents.Add(newIncident);
            db.SaveChanges();

            // Create incident history record
            IncidentHistory newHistoryEntry = new IncidentHistory();
            newHistoryEntry.incidentID = newIncident.ID;
            newHistoryEntry.state = incidentState.Created;
            newHistoryEntry.historyEntryCreationDate = DateTime.Now;
            newHistoryEntry.currentStaffID = newIncident.StaffReportedID;
            newHistoryEntry.actionByStaffID = newIncident.CurrentManagerID;
            newHistoryEntry.currentActionDescription = newIncident.CurrentAction;
            newHistoryEntry.actionByDate = DateTime.Now + dataHelper.getIncidentTypeEscalationTimeFrame(newIncident.TypeID, 1);
            db.IncidentHistoryEntries.Add(newHistoryEntry);
            db.SaveChanges();

            return RedirectToAction("openIncidents");

        }

        public ActionResult IncidentHistory(int ID, string returnPage)
        {
            Incident incidentRecord = db.Incidents.Where(p => p.ID == ID).SingleOrDefault();
            ViewData["incidentDescription"] = incidentRecord.Description;
            ViewData["incidentID"] = incidentRecord.ID;
            ViewData["UserProfiles"] = db.UserProfiles;
            ViewData["returnPage"] = returnPage;

            return View();
        }

        public ActionResult GetIncidentHistory([DataSourceRequest]DataSourceRequest request, int incidentID)
        {
            DataSourceResult result = db.IncidentHistoryEntries.Where(p => p.incidentID == incidentID).ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("historyEntryCreationDate", System.ComponentModel.ListSortDirection.Descending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult OpenIncidents()
        {

            ViewData["UserProfiles"] = db.UserProfiles;
            ViewData["Locations"] = db.Locations;
            ViewData["Clients"] = db.Clients;

            return View();
        }

        public ActionResult ClosedIncidents()
        {

            ViewData["UserProfiles"] = db.UserProfiles;
            ViewData["Locations"] = db.Locations;
            ViewData["Clients"] = db.Clients;

            return View();
        }

        public ActionResult GetMyOpenIncidents([DataSourceRequest]DataSourceRequest request, int UserID, bool getOnlyIncidentsIMustAction)
        {
            Helpers.Data dataHelper = new Helpers.Data();

            DataSourceResult result = dataHelper.getStaffIncidentsViaHistory(UserID, true, getOnlyIncidentsIMustAction).ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("incidentDate", System.ComponentModel.ListSortDirection.Descending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetMyClosedIncidents([DataSourceRequest]DataSourceRequest request, int UserID)
        {
            Helpers.Data dataHelper = new Helpers.Data();

            DataSourceResult result = dataHelper.getStaffIncidentsViaHistory(UserID, false, false).ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("incidentDate", System.ComponentModel.ListSortDirection.Descending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetOpenIncidents([DataSourceRequest]DataSourceRequest request)
        {
            
            DataSourceResult result = db.Incidents.Where(m => m.currentState != incidentState.Closed).ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("incidentDate", System.ComponentModel.ListSortDirection.Descending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetClosedIncidents([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.Incidents.Where(m => m.currentState == incidentState.Closed).ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("incidentDate", System.ComponentModel.ListSortDirection.Descending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult incidentsEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Incident passedEntry)
        {

            if (passedEntry != null)
            {
                Incident newEntry = db.Incidents.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.Incidents.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult incidentsEditingInline_Create([DataSourceRequest] DataSourceRequest request, Incident passedEntry)
        {

            if (passedEntry != null)
            {

                Incident newEntry = new Incident();
                newEntry.TypeID = passedEntry.TypeID;
                db.Incidents.Add(newEntry);
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult incidentsEditingInline_Update([DataSourceRequest] DataSourceRequest request, Incident passedEntry)
        {

            if (passedEntry != null)
            {

                Incident newEntry = db.Incidents.Find(passedEntry.ID);
                newEntry.ClientID = passedEntry.ClientID;
                newEntry.CurrentAction = passedEntry.CurrentAction;
                newEntry.Description = passedEntry.Description;
                newEntry.incidentDate = passedEntry.incidentDate;
                newEntry.LocationID = passedEntry.LocationID;
                newEntry.reportedDate = passedEntry.reportedDate;
                newEntry.UserProfileID = passedEntry.UserProfileID;

                newEntry.ClientID = passedEntry.ClientID;

                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult Types()
        {
            ViewData["UserProfiles"] = db.UserProfiles;
            ViewData["Locations"] = db.Locations;
            ViewData["Clients"] = db.Clients;

            return View();
        }

        public ActionResult GetIncidentTypes([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.IncidentTypes.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult incidentTypesEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, IncidentType passedEntry)
        {

            if (passedEntry != null)
            {
                IncidentType newEntry = db.IncidentTypes.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.IncidentTypes.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult incidentTypesEditingInline_Create([DataSourceRequest] DataSourceRequest request, IncidentType passedEntry)
        {

            if (passedEntry != null)
            {
                var checkEntry = db.IncidentTypes.Where(x => x.Type == passedEntry.Type);
                if (checkEntry.Count() == 0)
                {
                    IncidentType newEntry = new IncidentType();
                    newEntry.onCallCalendarID = passedEntry.onCallCalendarID;
                    newEntry.useOnCallProcessing = passedEntry.useOnCallProcessing;
                    newEntry.Type = passedEntry.Type;
                    if (passedEntry.firstStaffEscID == null)
                        newEntry.firstStaffEscID = 0;
                    else
                        newEntry.firstStaffEscID = passedEntry.firstStaffEscID;
                    if (passedEntry.firstEscDays == null)
                        newEntry.firstEscDays = 0;
                    else
                        newEntry.firstEscDays = passedEntry.firstEscDays;
                    if (passedEntry.firstEscHours == null)
                        newEntry.firstEscHours = 0;
                    else
                        newEntry.firstEscHours = passedEntry.firstEscHours;

                    if (passedEntry.firstEscMinutes == null)
                        newEntry.firstEscMinutes = 0;
                    else
                        newEntry.firstEscMinutes = passedEntry.firstEscMinutes;

                    if (passedEntry.secondStaffEscID == null)
                        newEntry.secondStaffEscID = 0;
                    else
                        newEntry.secondStaffEscID = passedEntry.secondStaffEscID;
                    if (passedEntry.secondEscDays == null)
                        newEntry.secondEscDays = 0;
                    else
                        newEntry.secondEscDays = passedEntry.secondEscDays;
                    if (passedEntry.secondEscHours == null)
                        newEntry.secondEscHours = 0;
                    else
                        newEntry.secondEscHours = passedEntry.secondEscHours;
                    if (passedEntry.secondEscMinutes == null)
                        newEntry.secondEscMinutes = 0;
                    else
                        newEntry.secondEscMinutes = passedEntry.secondEscMinutes;

                    if (passedEntry.thirdStaffEscID == null)
                        newEntry.thirdStaffEscID = 0;
                    else
                        newEntry.thirdStaffEscID = passedEntry.thirdStaffEscID;
                    if (passedEntry.thirdEscDays == null)
                        newEntry.thirdEscDays = 0;
                    else
                        newEntry.thirdEscDays = passedEntry.thirdEscDays;

                    if (passedEntry.thirdEscHours == null)
                        newEntry.thirdEscHours = 0;
                    else
                        newEntry.thirdEscHours = passedEntry.thirdEscHours;

                    if (passedEntry.thirdEscMinutes == null)
                        newEntry.thirdEscMinutes = 0;
                    else
                        newEntry.thirdEscMinutes = passedEntry.thirdEscMinutes;

                    if (passedEntry.onCallCalendarID == null)
                        newEntry.onCallCalendarID = 0;
                    else
                        newEntry.onCallCalendarID = passedEntry.onCallCalendarID;

                    newEntry.useOnCallProcessing = passedEntry.useOnCallProcessing;

                    db.IncidentTypes.Add(newEntry);
                    db.SaveChanges();

                    // Create the template file directory for this incident type

                    var newDirectoryPath = Server.MapPath(@"~\App_Data\Incident_Templates\" + newEntry.ID.ToString());

                    Directory.CreateDirectory(newDirectoryPath);

                }
                else
                {
                    ModelState.AddModelError("", "Sorry, a incident type of '" + passedEntry.Type + "' already exists."); 
                }

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult incidentTypesEditingInline_Update([DataSourceRequest] DataSourceRequest request, IncidentType passedEntry)
        {

            if (passedEntry != null)
            {

                IncidentType newEntry = db.IncidentTypes.Find(passedEntry.ID);
                newEntry.ID = passedEntry.ID;
                newEntry.onCallCalendarID = passedEntry.onCallCalendarID;
                newEntry.useOnCallProcessing = passedEntry.useOnCallProcessing;
                newEntry.Type = passedEntry.Type;
                if (passedEntry.firstStaffEscID == null)
                    newEntry.firstStaffEscID = 0;
                else
                    newEntry.firstStaffEscID = passedEntry.firstStaffEscID;
                if (passedEntry.firstEscDays == null)
                    newEntry.firstEscDays = 0;
                else
                    newEntry.firstEscDays = passedEntry.firstEscDays;
                if (passedEntry.firstEscHours == null)
                    newEntry.firstEscHours = 0;
                else
                    newEntry.firstEscHours = passedEntry.firstEscHours;

                if (passedEntry.firstEscMinutes == null)
                    newEntry.firstEscMinutes = 0;
                else
                    newEntry.firstEscMinutes = passedEntry.firstEscMinutes;

                if (passedEntry.secondStaffEscID == null)
                    newEntry.secondStaffEscID = 0;
                else
                    newEntry.secondStaffEscID = passedEntry.secondStaffEscID;
                if (passedEntry.secondEscDays == null)
                    newEntry.secondEscDays = 0;
                else
                    newEntry.secondEscDays = passedEntry.secondEscDays;
                if (passedEntry.secondEscHours == null)
                    newEntry.secondEscHours = 0;
                else
                    newEntry.secondEscHours = passedEntry.secondEscHours;
                if (passedEntry.secondEscMinutes == null)
                    newEntry.secondEscMinutes = 0;
                else
                    newEntry.secondEscMinutes = passedEntry.secondEscMinutes;

                if (passedEntry.thirdStaffEscID == null)
                    newEntry.thirdStaffEscID = 0;
                else
                    newEntry.thirdStaffEscID = passedEntry.thirdStaffEscID;
                if (passedEntry.thirdEscDays == null)
                    newEntry.thirdEscDays = 0;
                else
                    newEntry.thirdEscDays = passedEntry.thirdEscDays;

                if (passedEntry.thirdEscHours == null)
                    newEntry.thirdEscHours = 0;
                else
                    newEntry.thirdEscHours = passedEntry.thirdEscHours;

                if (passedEntry.thirdEscMinutes == null)
                    newEntry.thirdEscMinutes = 0;
                else
                    newEntry.thirdEscMinutes = passedEntry.thirdEscMinutes;

                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

    }
}
