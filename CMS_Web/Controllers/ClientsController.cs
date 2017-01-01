using System.Data;
using System.Linq;
using System.Web.Mvc;
using CMS_Web.Models;
using CMS_Web.ViewModels;
using CMS_Web.DAL;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data.Entity;
using System.Collections.Generic;
using System;

namespace CMS_Web.Controllers
{
    public class ClientsController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClientSitesAndStaff(int clientID)
        {
            var clientRecord = db.Clients.Where(p => p.ID == clientID).FirstOrDefault();

            ViewData["ClientID"] = clientID;
            ViewData["ClientName"] = clientRecord.FullName;

            ClientSiteAndStaffViewModel staffAndLocaitons = new ClientSiteAndStaffViewModel();

            staffAndLocaitons.allLocations = db.Locations;
            staffAndLocaitons.allStaff = db.UserProfiles;
       
            var relatedStaff = db.ClientPreferredStaffItems.Where(p => p.ClientID == clientID);
            var relatedLocations = db.ClientsAtSitesList.Where(p => p.ClientID == clientID);

            staffAndLocaitons.selectedLocations = new int[relatedLocations.Count()];
            staffAndLocaitons.selectedStaff = new int[relatedStaff.Count()];

            int count = 0;

            foreach (var l in relatedLocations)
            {
                staffAndLocaitons.selectedLocations[count] = l.SiteID;
                count++;
            }
            count = 0;
            foreach (var s in relatedStaff)
            {
                staffAndLocaitons.selectedStaff[count] = s.StaffID;
                count++;
            }
            return View(staffAndLocaitons);
        }

        [HttpPost]
        public ActionResult SaveClientSitesAndStaff( string client, string[] locations, string[] staff)
        {

            List<int> locationIDs = new List<int>();
            List<int> staffIDs = new List<int>();
            string[] splitter;

            int clientID = Convert.ToInt16(client);

            splitter = locations[0].Split(',');

            if (splitter[0] != "")
                foreach (string l in splitter)
                {
                    int locID = Convert.ToInt16(l);
                    locationIDs.Add(locID);
                }

            splitter = staff[0].Split(',');

            if (splitter[0] != "")
                foreach (string s in splitter)
                {
                    int staffID = Convert.ToInt16(s);
                    staffIDs.Add(staffID);
                }

            // remove all sites
            var siteRecords = db.ClientsAtSitesList.Where(p => p.ClientID == clientID);
            foreach (var r in siteRecords)
            {
                db.ClientsAtSitesList.Remove(r);
            }
            db.SaveChanges();

            // remove all staff
            var staffRecords = db.ClientPreferredStaffItems.Where(p => p.ClientID == clientID);
            foreach (var r in staffRecords)
            {
                db.ClientPreferredStaffItems.Remove(r);
            }
            db.SaveChanges();

            // add sites

            foreach (int locID in locationIDs)
            {
                ClientsAtSites newRec = new ClientsAtSites();
                newRec.ClientID = clientID;
                newRec.SiteID = locID;
                db.ClientsAtSitesList.Add(newRec);
            }
            db.SaveChanges();

            foreach (int staffID in staffIDs)
            {
                ClientPreferredStaff newRec = new ClientPreferredStaff();
                newRec.ClientID = clientID;
                newRec.StaffID = staffID;
                db.ClientPreferredStaffItems.Add(newRec);
            }
            db.SaveChanges();

            return View("Index");
        }

      

        public ActionResult GetClients([DataSourceRequest]DataSourceRequest request)
        {
            DataTable clientsTable = new DataTable();
            clientsTable.Columns.Add("ID");
            clientsTable.Columns.Add("FirstName");
            clientsTable.Columns.Add("LastName");


            foreach (var clients in db.Clients)
            {
                clientsTable.Rows.Add(clients.ID, clients.FirstName, clients.LastName);
            }

            DataView dv = clientsTable.DefaultView;
            dv.Sort = "LastName DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Client passedEntry)
        {

            if (passedEntry != null)
            {
                Client newEntry = db.Clients.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.Clients.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientEditingInline_Create([DataSourceRequest] DataSourceRequest request, Client passedEntry)
        {

            if (passedEntry != null)
            {
                var checkEntry = db.Clients.Where(x => x.ID == passedEntry.ID);
                if (checkEntry.Count() == 0)
                {
                    Client newEntry = new Client();
                    newEntry.ID = passedEntry.ID;
                    db.Clients.Add(newEntry);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, a client of '" + passedEntry.FirstName.ToString() + " " + passedEntry.LastName.ToString() + "' already exists.");
                }

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult clientEditingInline_Update([DataSourceRequest] DataSourceRequest request, Client passedEntry)
        {

            if (passedEntry != null)
            {

                Client newEntry = db.Clients.Find(passedEntry.ID);
                newEntry.FirstName = passedEntry.FirstName;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult GetLocations([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.Locations.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult editClientRequiredQualifications(int id = 0)
        {
            var client = db.Clients.Where(p => p.ID == id).FirstOrDefault();
            ViewData["clientID"] = client.ID;
            ViewData["clientName"] = client.FullName;
            ViewData["Qualifications"] = db.Qualifications;
            ViewData["ReturnPage"] = "Index";

            return View();

        }

        public ActionResult GetSelectableQualificationsForClient([DataSourceRequest]DataSourceRequest request, int clientID)
        {

            List<Qualification> qualificationsToReturn = new List<Qualification>();
            bool notFound = true;

            var qualificationsRequiredForClient = db.ClientRequiredQualifications.Where(p => p.ClientID == clientID);

            foreach (var q in db.Qualifications)
            {
                notFound = true;
                foreach (var quh in qualificationsRequiredForClient)
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


        public ActionResult removeQualificationFromClient(int clientID, int qualificationID)
        {

            var qualificationEntryToDelete = db.ClientRequiredQualifications.Where(p => p.ClientID == clientID && p.QualificationID == qualificationID).FirstOrDefault();
            if (qualificationEntryToDelete != null)
            {
                db.ClientRequiredQualifications.Remove(qualificationEntryToDelete);
                db.SaveChanges();
            }

            return RedirectToAction("editClientRequiredQualifications", new { id = clientID });

        }

        public ActionResult GetClientQualificationList([DataSourceRequest]DataSourceRequest request, int clientID)
        {

            DataSourceResult result = db.ClientRequiredQualifications.Where(p => p.ClientID == clientID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult AddQualificationToClient(int clientID, int qualificationID)
        {

            ClientRequiredQualification clientQualificationToAdd = new ClientRequiredQualification();

            clientQualificationToAdd.ClientID = clientID;
            clientQualificationToAdd.QualificationID = qualificationID;

            db.ClientRequiredQualifications.Add(clientQualificationToAdd);
            db.SaveChanges();

            return RedirectToAction("editClientRequiredQualifications", new { id = clientID });
        }

    }
}