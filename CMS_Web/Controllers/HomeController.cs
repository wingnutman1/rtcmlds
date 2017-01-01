using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Security;
using CMS_Web.Models;
using Kendo.Mvc;
using System.Data;
using System.Data.Entity;
using CMS_Web.ViewModels;
using CMS_Web.DAL;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;
using CMS_Web.Helpers;

namespace CMS_Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();
        private Data dataHelper = new Data();
        private GlobalSettingsModel gs = new GlobalSettingsModel();

       
        public ActionResult Index()
        {
            gs = dataHelper.getGlobalSettings();

            ViewData["toDoCompletionRateAll"] = dataHelper.toDoCompletionRateForAll(DateTime.Now - TimeSpan.FromDays(gs.numberOfDaysDurationToDoPerformanceAnalysis), DateTime.Now);
            ViewData["toDoCompletionRateSubordinates"] = dataHelper.toDoCompletionRateForSubordinates(WebSecurity.CurrentUserId, DateTime.Now - TimeSpan.FromDays(28), DateTime.Now);
            ViewData["toDoCompletionRateMe"] = dataHelper.toDoCompletionRateForStaff(WebSecurity.CurrentUserId, DateTime.Now - TimeSpan.FromDays(28), DateTime.Now);

            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastMonthStart = currentMonthStart.AddMonths(-1);
            DateTime lastMonthEnd = currentMonthStart.AddDays(-1);


            ViewData["incidentsRaisedThisMonth"] = dataHelper.incidentsRaisedAll(currentMonthStart, DateTime.Now);
            ViewData["incidentsRaisedLastMonth"] = dataHelper.incidentsRaisedAll(lastMonthStart, lastMonthEnd);
            ViewData["incidentsClosedThisMonth"] = dataHelper.incidentsClosedAll(currentMonthStart, DateTime.Now);
            ViewData["incidentsClosedLastMonth"] = dataHelper.incidentsClosedAll(lastMonthStart, lastMonthEnd);
            ViewData["OpenIncidents"] = dataHelper.incidentsOpenAll();

            ViewData["exceptionsRaisedThisMonth"] = dataHelper.exceptionsRaisedAll(currentMonthStart, DateTime.Now);
            ViewData["exceptionsRaisedLastMonth"] = dataHelper.exceptionsRaisedAll(lastMonthStart, lastMonthEnd);
            ViewData["exceptionsClosedThisMonth"] = dataHelper.exceptionsClosedAll(currentMonthStart, DateTime.Now);
            ViewData["exceptionsClosedLastMonth"] = dataHelper.exceptionsClosedAll(lastMonthStart, lastMonthEnd);
            ViewData["OpenExceptions"] = dataHelper.exceptionsOpenAll();

            if (!SiteMapManager.SiteMaps.ContainsKey("CMSWeb"))
                SiteMapManager.SiteMaps.Register<XmlSiteMap>("CMSWeb", sitemap => sitemap.LoadFrom("~/CMSWeb.sitemap"));

            return View();
        }

       

        [HttpGet]
        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");

        }


        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult getMyToDoListForToday([DataSourceRequest]DataSourceRequest request)
        {
            DateTime startOfToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0,0,0);
            DateTime startOfNextDay = startOfToday + TimeSpan.FromDays(1);

            var result = (from p in db.ToDoListItems
                                       where p.StaffID == WebSecurity.CurrentUserId && p.RequiredCompletionBy >= startOfToday && p.RequiredCompletionBy <= startOfNextDay 
                                       select p).ToList();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }


    }
}
