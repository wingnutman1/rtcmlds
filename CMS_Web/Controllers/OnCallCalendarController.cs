using CMS_Web.DAL;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data;
using System.Web.Mvc;
using CMS_Web.Models;

namespace CMS_Web.Controllers
{
    public class OnCallCalendarController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();

        public ActionResult onCallCalendars()
        {
            return View();
        }

       
        public ActionResult GetOnCallCalendars([DataSourceRequest]DataSourceRequest request)
        {
            DataTable onCallCalendarTable = new DataTable();
            onCallCalendarTable.Columns.Add("ID");
            onCallCalendarTable.Columns.Add("Description");

            foreach (var calendars in db.OnCallCalendars)
            {
                onCallCalendarTable.Rows.Add(calendars.ID, calendars.Description);
            }

            DataView dv = onCallCalendarTable.DefaultView;
            dv.Sort = "Description DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult onCallCalendarsEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, OnCallCalendar passedEntry)
        {

            if (passedEntry != null)
            {
                OnCallCalendar newEntry = db.OnCallCalendars.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.OnCallCalendars.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult onCallCalendarsEditingInline_Create([DataSourceRequest] DataSourceRequest request, OnCallCalendar passedEntry)
        {

            if (passedEntry != null)
            {
                var checkEntry = db.OnCallCalendars.Where(x => x.Description == passedEntry.Description);
                if (checkEntry.Count() == 0)
                {
                    OnCallCalendar newEntry = new OnCallCalendar();
                    newEntry.Description = passedEntry.Description;
                    db.OnCallCalendars.Add(newEntry);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, an On Call Calendar of '" + passedEntry.Description.ToString() + "' already exists.");
                }

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult onCallCalendarsEditingInline_Update([DataSourceRequest] DataSourceRequest request, OnCallCalendar passedEntry)
        {

            if (passedEntry != null)
            {

                OnCallCalendar newEntry = db.OnCallCalendars.Find(passedEntry.ID);
                newEntry.Description = passedEntry.Description;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult onCallCalendarDetails()
        {

            ViewData["UserProfiles"] = db.UserProfiles;
            return View();
        }

        public ActionResult GetOnCallCalendarDetails([DataSourceRequest]DataSourceRequest request, int selectedCalendar)
        {

            DataSourceResult result = db.OnCallCalendarItems.Where(x => x.calendarID == selectedCalendar).ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("dateStart", System.ComponentModel.ListSortDirection.Descending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult onCallCalendarItemsEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, OnCallCalendarItem passedEntry)
        {

            if (passedEntry != null)
            {
                OnCallCalendarItem newEntry = db.OnCallCalendarItems.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.OnCallCalendarItems.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult onCallCalendarItemsEditingInline_Create([DataSourceRequest] DataSourceRequest request, OnCallCalendarItem passedEntry, int selectedCalendar)
        {

            if (passedEntry != null)
            {
                var checkEntry = db.OnCallCalendarItems.Where(x => x.calendarID == passedEntry.calendarID && x.dateStart == passedEntry.dateStart);
                if (checkEntry.Count() == 0)
                {
                    passedEntry.calendarID = selectedCalendar;
                    db.OnCallCalendarItems.Add(passedEntry);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, an On Call Calendar Item using these paramters already exists.");
                }

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult onCallCalendarItemsEditingInline_Update([DataSourceRequest] DataSourceRequest request, OnCallCalendarItem passedEntry)
        {

            if (passedEntry != null)
            {

                OnCallCalendarItem newEntry = db.OnCallCalendarItems.Find(passedEntry.ID);
                newEntry.dateStart = passedEntry.dateStart;
                newEntry.dateEnd = passedEntry.dateEnd;
                newEntry.timeStart = passedEntry.timeStart;
                newEntry.timeEnd = passedEntry.timeEnd;
                newEntry.staffID = passedEntry.staffID;
                newEntry.calendarID = passedEntry.calendarID;
                db.Entry(newEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

    }
}
