using CMS_Web.DAL;
using CMS_Web.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using WebMatrix.WebData;
using System.Text;
using Kendo.Mvc.Extensions;
using System.IO;
using MvcSiteMapProvider.Linq;

namespace CMS_Web.Controllers
{
    public class ExceptionEscallationsController : Controller
    {
        //
        // GET: /Exception/
        private CMS_WebContext db = new CMS_WebContext();

        public ActionResult ExceptionEscallationsMain()
        {

            List<SelectListItem> staff = new List<SelectListItem>();
            staff.Add(new SelectListItem { Text = "Select Staff", Value = "Select Staff" });

            foreach (UserProfile u in db.UserProfiles)
            {
                staff.Add(new SelectListItem { Text = u.FirstName + " " + u.LastName, Value = u.UserId.ToString() });
            }

            ViewData["staff"] = staff;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetStaffList()
        {
            List<SelectListItem> staff = new List<SelectListItem>();
            staff.Add(new SelectListItem { Text = "Select Staff", Value = "Select Staff" });

            foreach (UserProfile u in db.UserProfiles)
            {
                staff.Add(new SelectListItem { Text = u.FirstName + " " + u.LastName, Value = u.UserId.ToString() });
            }

            ViewData["staff"] = staff;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetEscallations([DataSourceRequest] DataSourceRequest request)
        {

            DataSourceResult e = db.ExceptionEscallations.ToDataSourceResult(request);
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        public ActionResult escallationsDelete(int ID)
        {

            if (ID  != 0)
            {
                var e = db.ExceptionEscallations.Find(ID);
                db.ExceptionEscallations.Remove(e);

                var detail = db.ExceptionEscallationDetails.Where(r => r.ParentID == ID);
                foreach (var ed in detail)
                {
                    db.ExceptionEscallationDetails.Remove(ed);
                }

                db.SaveChanges();

            }

            return RedirectToAction("ExceptionEscallationsMain", new { Controller = "ExceptionEscallations", action = "ExceptionEscallationsMain" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult escallationsEditingInline_Create([DataSourceRequest] DataSourceRequest request, ExceptionEscallation passedEntry, string userID)
        {

            if (passedEntry != null)
            {
                db.ExceptionEscallations.Add(passedEntry);
                db.SaveChanges();
            }

            return Json(request);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult escallationsEditingInline_Update([DataSourceRequest] DataSourceRequest request, ExceptionEscallation passedEntry, string userID)
        {

            if (passedEntry != null)
            {
                db.Entry(passedEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(request);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetEscallationDetails([DataSourceRequest] DataSourceRequest request, string escallationID)
        {
            int escIDInt = Convert.ToInt32(escallationID);
            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Position", System.ComponentModel.ListSortDirection.Descending));
            DataSourceResult e = db.ExceptionEscallationDetails.Where(od => od.ParentID == escIDInt).ToDataSourceResult(request);
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult escallationDetailsEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ExceptionEscallationDetail passedEntry)
        {
           

            if (passedEntry != null)
            {
                var e = db.ExceptionEscallationDetails.Find(passedEntry.ID);
                db.ExceptionEscallationDetails.Remove(e);
                db.SaveChanges();

                sortPositionOfExceptionEscalationDetails(passedEntry.ParentID);


            }

            return Json(request);
        }

        private void sortPositionOfExceptionEscalationDetails(int parentID)
        {
            //Reorder all other items if there are any
            int lastPos = 0;

            List<int> recIndexes = new List<int>();

            var roRecs = db.ExceptionEscallationDetails.Where(r => r.ParentID == parentID).OrderBy(r => r.Position);
            foreach (var r in roRecs)
                recIndexes.Add(r.ID);            


            foreach (var index in recIndexes)
            {
                var currRec = db.ExceptionEscallationDetails.Where(r => r.ID == index).FirstOrDefault();

                if (currRec.Position - lastPos >= 2)
                    currRec.Position--;
                else
                    if (currRec.Position == lastPos)
                    currRec.Position++;

                lastPos = currRec.Position;
                db.Entry(currRec).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult escallationDetailsEditingInline_Create([DataSourceRequest] DataSourceRequest request, ExceptionEscallationDetail passedEntry, string escallationID)
        {

            int parentEscIDInt = Convert.ToInt32(escallationID);

            if (passedEntry != null)
            {
                passedEntry.ParentID = parentEscIDInt;
                db.ExceptionEscallationDetails.Add(passedEntry);
                db.SaveChanges();
            }
            sortPositionOfExceptionEscalationDetails(passedEntry.ParentID);
            return Json(request);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult escallationDetailsEditingInline_Update([DataSourceRequest] DataSourceRequest request, ExceptionEscallationDetail passedEntry, string escallationID)
        {

            if (passedEntry != null)
            {
                db.Entry(passedEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            sortPositionOfExceptionEscalationDetails(passedEntry.ParentID);
            return Json(request);
        }


    }
}
