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
    public class LocationsController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetLocations([DataSourceRequest]DataSourceRequest request)
        {
            DataTable locationsTable = new DataTable();
            locationsTable.Columns.Add("ID");
            locationsTable.Columns.Add("Name");

            foreach (var locations in db.Locations)
            {
                locationsTable.Rows.Add(locations.ID, locations.Name);
            }

            DataView dv = locationsTable.DefaultView;
            dv.Sort = "Name DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult locationEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Location passedEntry)
        {

            if (passedEntry != null)
            {
                Location newEntry = db.Locations.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.Locations.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult locationEditingInline_Create([DataSourceRequest] DataSourceRequest request, Location passedEntry)
        {

            if (passedEntry != null)
            {
                var checkEntry = db.Locations.Where(x => x.ID == passedEntry.ID);
                if (checkEntry.Count() == 0)
                {
                    db.Locations.Add(passedEntry);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, a location of '" + passedEntry.Name.ToString() + "' already exists.");
                }

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult locationEditingInline_Update([DataSourceRequest] DataSourceRequest request, Location passedEntry)
        {

            if (passedEntry != null)
            {

                Location newEntry = db.Locations.Find(passedEntry.ID);
                newEntry.Name = passedEntry.Name;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult editLocationRequiredQualifications(int id = 0)
        {
            var location = db.Locations.Where(p => p.ID == id).FirstOrDefault();
            ViewData["locationID"] = location.ID;
            ViewData["locationName"] = location.Name;
            ViewData["Qualifications"] = db.Qualifications;
            ViewData["ReturnPage"] = "Index";

            return View();

        }

        public ActionResult GetSelectableQualificationsForLocation([DataSourceRequest]DataSourceRequest request, int locationID)
        {

            List<Qualification> qualificationsToReturn = new List<Qualification>();
            bool notFound = true;

            var qualificationsRequiredForLocation = db.LocationRequiredQualifications.Where(p => p.LocationID == locationID);

            foreach (var q in db.Qualifications)
            {
                notFound = true;
                foreach (var quh in qualificationsRequiredForLocation)
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


        public ActionResult removeQualificationFromLocation(int locationID, int qualificationID)
        {

            var qualificationEntryToDelete = db.LocationRequiredQualifications.Where(p => p.LocationID == locationID && p.QualificationID == qualificationID).FirstOrDefault();
            if (qualificationEntryToDelete != null)
            {
                db.LocationRequiredQualifications.Remove(qualificationEntryToDelete);
                db.SaveChanges();
            }

            return RedirectToAction("editLocationRequiredQualifications", new { id = locationID });

        }

        public ActionResult GetLocationQualificationList([DataSourceRequest]DataSourceRequest request, int locationID)
        {

            DataSourceResult result = db.LocationRequiredQualifications.Where(p => p.LocationID == locationID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult AddQualificationToLocation(int locationID, int qualificationID)
        {

            LocationRequiredQualification locationQualificationToAdd = new LocationRequiredQualification();

            locationQualificationToAdd.LocationID = locationID;
            locationQualificationToAdd.QualificationID = qualificationID;

            db.LocationRequiredQualifications.Add(locationQualificationToAdd);
            db.SaveChanges();

            return RedirectToAction("editLocationRequiredQualifications", new { id = locationID });
        }
    }
}