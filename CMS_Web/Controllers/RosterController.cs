using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Web.Models;
using CMS_Web.DAL;
using WebMatrix.WebData;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;

namespace CMS_Web.Controllers
{
    public class RosterController : Controller
    {
        //
        // GET: /Roster/
        private CMS_WebContext db = new CMS_WebContext();

        public ActionResult MyRoster()
        {

            List<RosterModel> schedules = new List<RosterModel>();
            return View(schedules);

        }

        public ActionResult AllRosters()
        {

            List<RosterModel> schedules = new List<RosterModel>();
            return View(schedules);

        }

        public void updateRosters()
        {
            Helpers.Data dataHelper = new Helpers.Data();

            dataHelper.addUserActivityLog("Started Manual Roster Import", WebSecurity.CurrentUserId, "");
            dataHelper.addLog("Manual Roster Import Started.", systemLogType.User, "updateRoster", "Roster", WebSecurity.CurrentUserId);

            using (var context = new LDS_RosterContext())
            {

                //Check for staff in rosters that are not in this system
                string queryString = "SELECT * FROM dbo.STAFF_ROSTER_WEBVIEW WHERE (Start >= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' AND (Deleted = 0 OR Deleted IS NULL))";
                var rosterEntries = context.Roster.SqlQuery(queryString);

                // Refine the list to unique names only

                List<LDSRosterEntry> uniqueStaffNames = rosterEntries.GroupBy(r => r.StaffFullName).Select(g => g.First()).ToList();

                // Check if the user is in the system and if not add a error entry
                foreach (LDSRosterEntry staff in uniqueStaffNames)
                {
                    int staffID = dataHelper.getStaffIDFromFullName(staff.StaffFullName);
                    if (staffID == 0)
                        dataHelper.addRosterImportError(staff.StaffFullName + " is in the source roster but not in this system.  If management of the staff member " + staff.StaffFullName + " is required please add the staff member to this systems Users.");

                }

                List<LDSRosterEntry> uniqueLocationNames = rosterEntries.GroupBy(r => r.SiteName).Select(g => g.First()).ToList();
                // Check if the location is in the system and if not add a error entry
                foreach (LDSRosterEntry locations in uniqueLocationNames)
                {
                    int siteID = dataHelper.getLocationIDFromName(locations.SiteName);
                    if (siteID == 0)
                        dataHelper.addRosterImportError("The location " + locations.SiteName + " is in the source roster but not in this system.  If management of the location " + locations.SiteName + " is required please add the locaiton to this system.");
                }

                // Check for rosters entries that exist but are not in the new set and set them as deleted to maintain their escallation status
                queryString = "SELECT * FROM dbo.STAFF_ROSTER_WEBVIEW WHERE (Start >= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' AND (Deleted = 0 OR Deleted IS NULL))";
                var newRosters = context.Roster.SqlQuery(queryString);
                var existingRosters = db.Rosters.Where(re => re.Start >= DateTime.Now && (re.Deleted != true));

                // Check for rosters entries that exist but are not in the new set and set them as deleted to maintain their escallation status

                foreach (RosterModel r in existingRosters)
                {
                    var rosterEntry = newRosters.FirstOrDefault(re => re.Start == r.Start && re.Finish == r.End && re.StaffFullName == r.StaffFullName && re.SiteName == r.SiteName);

                    if (rosterEntry == null)
                    {
                        r.Deleted = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;

                        if (r.exceptionID.Count != 0)
                        {
                            //TODO - Update Exception status
                        }
                    }
                }

                foreach (UserProfile user in db.UserProfiles)
                {

                    queryString = "SELECT * FROM dbo.STAFF_ROSTER_WEBVIEW WHERE (Start >= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' AND (Deleted = 0 OR Deleted IS NULL) AND StaffFullName LIKE '" + user.FullName + "')";
                    newRosters = context.Roster.SqlQuery(queryString);
                  
                    // Check all the new entries and only add entries that don't already exist.  This is so that escallation tracking is not overwritten

                    foreach (LDSRosterEntry r in newRosters)
                    {
                        var rosterEntry = db.Rosters.FirstOrDefault(re => re.Start == r.Start && re.End == r.Finish && re.StaffFullName == r.StaffFullName && re.SiteName == r.SiteName);

                        // The entry was not found so add it to the roster table
                        if (rosterEntry == null)
                        {
                            RosterModel newRoster = new RosterModel();
                            newRoster.Title = "Site : " + r.SiteName + " Client : " + r.ClientName;
                            newRoster.Start = r.Start;
                            newRoster.End = r.Finish;
                            newRoster.SiteName = r.SiteName;
                            newRoster.SiteID = dataHelper.getSiteID(r.SiteName);
                            newRoster.StaffFullName = r.StaffFullName;
                            newRoster.StaffFirstName = r.StaffFirstName;
                            newRoster.StaffLastName = r.StaffLastName;
                            newRoster.StaffID = dataHelper.getStaffIDFromName(r.StaffFirstName, r.StaffLastName);
                            if (r.ClientName != null)
                            {
                                newRoster.ClientFullName = r.ClientName;
                                newRoster.ClientFirstName = r.ClientFirstName;
                                newRoster.ClientLastName = r.ClientLastName;
                                newRoster.ClientID = dataHelper.getClientIDFromName(r.ClientFirstName, r.ClientLastName);
                            }
                            newRoster.Deleted = false;
                            db.Rosters.Add(newRoster);
                          
                        }
                    }

                    
                }
            }
            db.SaveChanges();
            dataHelper.addLog("Manual Roster Import Completed.", systemLogType.User, "updateRoster", "Roster", WebSecurity.CurrentUserId);

        }

        public ActionResult getRosters([DataSourceRequest]DataSourceRequest request, string username, string start, string finish)
        {

            DateTime startDate = Convert.ToDateTime(start);
            DateTime finishDate = Convert.ToDateTime(finish) + TimeSpan.FromDays(1);

            var rosterEntries = db.Rosters.Where(re => re.StaffFullName == username && 
                                                ((re.Start <= startDate && re.End > startDate ) ||
                                                 (re.Start >= startDate && re.End <= finishDate) ||
                                                 (re.Start < finishDate && re.End >= finishDate)) &&
                                                 (re.Deleted == false || re.Deleted == null));

            DataSourceResult rostersResults = rosterEntries.ToDataSourceResult(request);

            return Json(rostersResults);

        }

        public ActionResult getMyRosters([DataSourceRequest]DataSourceRequest request, int userID, string start, string finish)
        {

            Helpers.Data dataHelper = new Helpers.Data();

            DateTime startDate = Convert.ToDateTime(start);
            DateTime finishDate = Convert.ToDateTime(finish) + TimeSpan.FromDays(1);
            string selectedUser = dataHelper.getStaffFullName(userID);

            var rosterEntries = db.Rosters.Where(re => re.StaffFullName == selectedUser &&
                                                ((re.Start <= startDate && re.End > startDate) ||
                                                 (re.Start >= startDate && re.End <= finishDate) ||
                                                 (re.Start < finishDate && re.End >= finishDate)) &&
                                                 (re.Deleted == false || re.Deleted == null));

            DataSourceResult rostersResults = rosterEntries.ToDataSourceResult(request);

            return Json(rostersResults);

        }

        public ActionResult getStaff([DataSourceRequest]DataSourceRequest request)
        {

            var staff = new CMS_WebContext().UserProfiles;
            CMS_WebContext db = new CMS_WebContext();

            DataTable Users = new DataTable();
            Users.Columns.Add("UserID");
            Users.Columns.Add("FirstName");
            Users.Columns.Add("LastName");
            Users.Columns.Add("UnreadMessages");
            Users.Columns.Add("Online");


            foreach (var staffMember in db.UserProfiles)
            {
                if (staffMember.UserId != WebSecurity.CurrentUserId)
                {
                    var count = db.Messages
                        .AsEnumerable()
                        .Where(p => p.SenderStaff == staffMember.UserId && p.RecipientStaff == WebSecurity.CurrentUserId && !p.MessageRead && !p.MessageDeleted).Count();

                    var onLine = "offline";

                    if (staffMember.SignalRID != null)
                        onLine = "online";

                    Users.Rows.Add(staffMember.UserId, staffMember.FirstName, staffMember.LastName, count, onLine);
                }
            }

            DataView dv = Users.DefaultView;
            dv.Sort = "UnreadMessages DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result);

        }

        public ActionResult GetRosterImportErrors([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = db.RosterImportErrors.ToDataSourceResult(request);

            request.Sorts.Add(new Kendo.Mvc.SortDescriptor("ImportDate", System.ComponentModel.ListSortDirection.Ascending));

            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
