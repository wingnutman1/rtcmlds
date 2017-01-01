using CMS_Web.DAL;
using CMS_Web.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class QualificationsController : Controller
    {

        private CMS_WebContext db = new CMS_WebContext();

        public ActionResult QualificationTypes()
        {

            return View();
        }

        public ActionResult GetQualificationTypes([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = db.Qualifications.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult qualificationsEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Qualification passedEntry)
        {

            if (passedEntry != null)
            {
                Qualification newEntry = db.Qualifications.Find(passedEntry.ID);
                if (newEntry != null)
                {
                    db.Qualifications.Remove(newEntry);
                    db.SaveChanges();
                }
            }

            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult qualificationsEditingInline_Create([DataSourceRequest] DataSourceRequest request, Qualification passedEntry)
        {

            if (passedEntry != null)
            {

                db.Qualifications.Add(passedEntry);
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult qualificationsEditingInline_Update([DataSourceRequest] DataSourceRequest request, Qualification passedEntry)
        {

            if (passedEntry != null)
            {

                db.Entry(passedEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Json(new[] { passedEntry }.ToDataSourceResult(request, ModelState));
        }



    }
}
