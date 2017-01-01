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

namespace CMS_Web.Controllers
{
    public class JournalController : Controller
    {
       private CMS_WebContext db = new CMS_WebContext();

        [HttpGet]
        public ActionResult Clients()
        {
            List<SelectListItem> locations = new List<SelectListItem>();
            locations.Add(new SelectListItem {Text = "Select Location", Value = "Select Location"});
            
            foreach (Location l in db.Locations)
            {
                locations.Add(new SelectListItem { Text = l.Name, Value = l.ID.ToString() });
            }

            ViewData["locations"] = locations;

            List<SelectListItem> clients = new List<SelectListItem>();
            clients.Add(new SelectListItem { Text = "Select Client", Value = "Select Client" });

            foreach (Client c in db.Clients)
            {
                clients.Add(new SelectListItem { Text = c.FirstName + " " + c.LastName, Value = c.ID.ToString() });
            }

            ViewData["clients"] = clients;

            List<SelectListItem> types = new List<SelectListItem>();
            types.Add(new SelectListItem { Text = "Select Type", Value = "Select Type" });

            foreach (JournalEntryType t in db.JournalEntryTypes)
            {
                types.Add(new SelectListItem { Text = t.Type, Value = t.ID.ToString() });
            }

            ViewData["types"] = types;

            return View(db.JournalEntries);
        }

        [HttpGet]
        public ActionResult Staff()
        {
            List<SelectListItem> locations = new List<SelectListItem>();
            locations.Add(new SelectListItem { Text = "Select Location", Value = "Select Location" });

            foreach (Location l in db.Locations)
            {
                locations.Add(new SelectListItem { Text = l.Name, Value = l.ID.ToString() });
            }

            ViewData["locations"] = locations;

            List<SelectListItem> clients = new List<SelectListItem>();
            clients.Add(new SelectListItem { Text = "Select Client", Value = "Select Client" });

            foreach (Client c in db.Clients)
            {
                clients.Add(new SelectListItem { Text = c.FirstName + " " + c.LastName, Value = c.ID.ToString() });
            }

            ViewData["clients"] = clients;

            List<SelectListItem> types = new List<SelectListItem>();
            types.Add(new SelectListItem { Text = "Select Type", Value = "Select Type" });

            foreach (JournalEntryType t in db.JournalEntryTypes)
            {
                types.Add(new SelectListItem { Text = t.Type, Value = t.ID.ToString() });
            }

            ViewData["types"] = types;

            return View(db.JournalEntries);
        }

        [HttpGet]
        public ActionResult Sites()
        {
            List<SelectListItem> locations = new List<SelectListItem>();
            locations.Add(new SelectListItem { Text = "Select Location", Value = "Select Location" });

            foreach (Location l in db.Locations)
            {
                locations.Add(new SelectListItem { Text = l.Name, Value = l.ID.ToString() });
            }

            ViewData["locations"] = locations;

            List<SelectListItem> clients = new List<SelectListItem>();
            clients.Add(new SelectListItem { Text = "Select Client", Value = "Select Client" });

            foreach (Client c in db.Clients)
            {
                clients.Add(new SelectListItem { Text = c.FirstName + " " + c.LastName, Value = c.ID.ToString() });
            }

            ViewData["clients"] = clients;

            List<SelectListItem> types = new List<SelectListItem>();
            types.Add(new SelectListItem { Text = "Select Type", Value = "Select Type" });

            foreach (JournalEntryType t in db.JournalEntryTypes)
            {
                types.Add(new SelectListItem { Text = t.Type, Value = t.ID.ToString() });
            }

            ViewData["types"] = types;

            return View(db.JournalEntries);
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

        public ActionResult getClients([DataSourceRequest]DataSourceRequest request)
        {

            DataTable Clients = new DataTable();
            Clients.Columns.Add("ID");
            Clients.Columns.Add("FirstName");
            Clients.Columns.Add("LastName");
          
            foreach (var Client in db.Clients)
            {
                Clients.Rows.Add(Client.ID, Client.FirstName, Client.LastName);
            }

            DataView dv = Clients.DefaultView;
            dv.Sort = "LastName DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getLocations([DataSourceRequest]DataSourceRequest request)
        {

            DataTable Locations = new DataTable();
            Locations.Columns.Add("ID");
            Locations.Columns.Add("FirstName");
            Locations.Columns.Add("LastName");

            foreach (var Location in db.Locations)
            {
                Locations.Rows.Add(Location.ID, Location.Name);
            }

            DataView dv = Locations.DefaultView;
            dv.Sort = "LastName DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result);

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
        public ActionResult GetClientJournals([DataSourceRequest] DataSourceRequest request, string userID)
        {
            int clientIDInt = Convert.ToInt32(userID);
            //int clientIDInt = clientID;

            DataSourceResult clientJournals = new DataSourceResult();

            if (clientIDInt > 0)
            {
                var journals = db.JournalEntries.Where(od => od.ClientID == clientIDInt);
                var returnJournalEntries = new List<journalViewModel>();
                foreach (JournalEntry j in journals)
                {
                    journalViewModel newEntry = new journalViewModel();
                    newEntry.ID = j.ID;
                    newEntry.clientID = (int)j.ClientID;
                    newEntry.locationID = (int)j.LocationID;
                    newEntry.lastModifiedDate = j.lastActionDate;
                    newEntry.note = j.Note;
                    newEntry.staffID = (int)j.StaffID;
                    newEntry.typeID = (int)j.TypeID;
                    newEntry.creationDate = j.creationDate;
                    returnJournalEntries.Add(newEntry);
                }

                clientJournals = returnJournalEntries.ToDataSourceResult(request);
                return Json(clientJournals, JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientsEditingInline_Create([DataSourceRequest] DataSourceRequest request,  journalViewModel passedEntry, string userID)
        {

            if (passedEntry != null)
            {

                JournalEntry newEntry = new JournalEntry();
                newEntry.ClientID = Convert.ToInt32(userID);
                newEntry.creationDate = DateTime.Now;
                newEntry.lastActionDate = DateTime.Now;
                newEntry.lastActionStaffID = WebSecurity.CurrentUserId;
                newEntry.LocationID = 0;
                newEntry.StaffID = 0;
                newEntry.TypeID = passedEntry.typeID;
                newEntry.Note = passedEntry.note;
                db.JournalEntries.Add(newEntry);
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientsEditingInline_Update([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry, string userID)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = db.JournalEntries.Find(passedEntry.ID);
                newEntry.ClientID = Convert.ToInt32(userID);
                newEntry.LocationID = 0;
                newEntry.StaffID = 0;
                newEntry.lastActionDate = DateTime.Now;
                newEntry.lastActionStaffID = WebSecurity.CurrentUserId;
                newEntry.TypeID = passedEntry.typeID;
                newEntry.Note = passedEntry.note;
                newEntry.ID = passedEntry.ID;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientsEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = db.JournalEntries.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.JournalEntries.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetStaffJournals([DataSourceRequest] DataSourceRequest request, string userID)
        {
            int staffIDInt = Convert.ToInt32(userID);
            //int clientIDInt = clientID;

            DataSourceResult staffJournals = new DataSourceResult();

            if (staffIDInt > 0)
            {
                var journals = db.JournalEntries.Where(od => od.StaffID == staffIDInt);
                var returnJournalEntries = new List<journalViewModel>();
                foreach (JournalEntry j in journals)
                {
                    journalViewModel newEntry = new journalViewModel();
                    newEntry.ID = j.ID;
                    newEntry.clientID = (int)j.ClientID;
                    newEntry.locationID = (int)j.LocationID;
                    newEntry.note = j.Note;
                    newEntry.lastModifiedStaffID = (int)j.lastActionStaffID;
                    newEntry.staffID = (int)j.StaffID;
                    newEntry.typeID = (int)j.TypeID;
                    newEntry.creationDate = j.creationDate;
                    returnJournalEntries.Add(newEntry);
                }

                staffJournals = returnJournalEntries.ToDataSourceResult(request);
                return Json(staffJournals, JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffEditingInline_Create([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry, string userID)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = new JournalEntry();
                newEntry.StaffID= Convert.ToInt32(userID);
                newEntry.creationDate = DateTime.Now;
                newEntry.lastActionDate = DateTime.Now;
                newEntry.lastActionStaffID = WebSecurity.CurrentUserId;
                newEntry.LocationID = 0;
                newEntry.ClientID = 0;
                newEntry.TypeID = passedEntry.typeID;
                newEntry.Note = passedEntry.note;
                db.JournalEntries.Add(newEntry);
                db.SaveChanges();
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffEditingInline_Update([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry, string userID)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = db.JournalEntries.Find(passedEntry.ID);
                newEntry.ClientID = 0;
                newEntry.LocationID = 0;
                newEntry.StaffID = Convert.ToInt32(userID);
                newEntry.lastActionDate = DateTime.Now;
                newEntry.lastActionStaffID = WebSecurity.CurrentUserId;
                newEntry.TypeID = passedEntry.typeID;
                newEntry.Note = passedEntry.note;
                newEntry.ID = passedEntry.ID;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult staffEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = db.JournalEntries.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.JournalEntries.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetSiteJournals([DataSourceRequest] DataSourceRequest request, string siteID)
        {
            int siteDInt = Convert.ToInt32(siteID);
            //int clientIDInt = clientID;

            DataSourceResult siteJournals = new DataSourceResult();

            if (siteDInt > 0)
            {
                var journals = db.JournalEntries.Where(od => od.LocationID == siteDInt);
                var returnJournalEntries = new List<journalViewModel>();
                foreach (JournalEntry j in journals)
                {
                    journalViewModel newEntry = new journalViewModel();
                    newEntry.ID = j.ID;
                    newEntry.clientID = (int)j.ClientID;
                    newEntry.locationID = (int)j.LocationID;
                    newEntry.note = j.Note;
                    newEntry.lastModifiedStaffID = (int)j.lastActionStaffID;
                    newEntry.staffID = (int)j.StaffID;
                    newEntry.typeID = (int)j.TypeID;
                    newEntry.creationDate = j.creationDate;
                    returnJournalEntries.Add(newEntry);
                }

                siteJournals = returnJournalEntries.ToDataSourceResult(request);
                return Json(siteJournals, JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult siteEditingInline_Create([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry, string siteID)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = new JournalEntry();
                newEntry.LocationID = Convert.ToInt32(siteID);
                newEntry.creationDate = DateTime.Now;
                newEntry.lastActionDate = DateTime.Now;
                newEntry.lastActionStaffID = WebSecurity.CurrentUserId;
                newEntry.StaffID = 0;
                newEntry.ClientID = 0;
                newEntry.TypeID = passedEntry.typeID;
                newEntry.Note = passedEntry.note;
                db.JournalEntries.Add(newEntry);
                db.SaveChanges();
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult siteEditingInline_Update([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry, string siteID)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = db.JournalEntries.Find(passedEntry.ID);
                newEntry.ClientID = 0;
                newEntry.StaffID = 0;
                newEntry.LocationID = Convert.ToInt32(siteID);
                newEntry.lastActionDate = DateTime.Now;
                newEntry.lastActionStaffID = WebSecurity.CurrentUserId;
                newEntry.TypeID = passedEntry.typeID;
                newEntry.Note = passedEntry.note;
                newEntry.ID = passedEntry.ID;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult siteEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, journalViewModel passedEntry)
        {

            if (passedEntry != null)
            {
                JournalEntry newEntry = db.JournalEntries.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.JournalEntries.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Types()
        {
            return View();
        }

        public ActionResult GetJournalTypes([DataSourceRequest]DataSourceRequest request)
        {
            DataTable Types = new DataTable();
            Types.Columns.Add("ID");
            Types.Columns.Add("Type");


            foreach (var journalType in db.JournalEntryTypes)
            {
                Types.Rows.Add(journalType.ID, journalType.Type);
            }

            DataView dv = Types.DefaultView;
            dv.Sort = "Type DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult journalTypesEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, JournalEntryType passedEntry)
        {

            if (passedEntry != null)
            {
                JournalEntryType newEntry = db.JournalEntryTypes.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.JournalEntryTypes.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult journalTypesEditingInline_Create([DataSourceRequest] DataSourceRequest request, JournalEntryType passedEntry)
        {

            if (passedEntry != null)
            {
                var checkEntry = db.JournalEntryTypes.Where(x => x.Type == passedEntry.Type);
                if (checkEntry.Count() == 0)
                {
                    JournalEntryType newEntry = new JournalEntryType();
                    newEntry.Type = passedEntry.Type;
                    db.JournalEntryTypes.Add(newEntry);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, a journal type of '" + passedEntry.Type + "' already exists.");
                }

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult journalTypesEditingInline_Update([DataSourceRequest] DataSourceRequest request, JournalEntryType passedEntry)
        {

            if (passedEntry != null)
            {

                JournalEntryType newEntry = db.JournalEntryTypes.Find(passedEntry.ID);
                newEntry.ID = passedEntry.ID;
                newEntry.Type = passedEntry.Type;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

    }

}
