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

namespace CMS_Web.Helpers
{
    public class Data
    {

        CMS_WebContext db = new CMS_WebContext();

        public Data()
        {

        }

        public List<UserProfile> getAllTeamMembers(int staffID)
        {
            List<UserProfile> usersInMyTeam = new List<UserProfile>();

            var teamList = db.StaffInTeams.Where(r => r.StaffID == staffID);

            foreach (StaffInTeam s in teamList)
            {
                var teamStaff = db.StaffInTeams.Where(r => r.TeamID == s.TeamID).Distinct();
                foreach (StaffInTeam sa in teamStaff)
                {
                    var user = db.UserProfiles.Where(p => p.UserId == sa.StaffID).FirstOrDefault();
                    if (!usersInMyTeam.Contains(user))
                        usersInMyTeam.Add(user);
                }
            }
            return usersInMyTeam;
        }

        public List<UserProfile> getAllSubordinates(int managerID)
        {
            List<UserProfile> usersThatReportToMe = new List<UserProfile>();

            UserProfile me = db.UserProfiles.Where(p => p.UserId == WebSecurity.CurrentUserId).FirstOrDefault();

            usersThatReportToMe.Add(me);

            buildListOfReportableUsers(me.UserId, ref usersThatReportToMe);

            return usersThatReportToMe;
        }

        // Recursive call to parse the entire user reporting structure
        private void buildListOfReportableUsers(int managerID, ref List<UserProfile> usersInReportStructure)
        {
            var usersThatReportToMe = db.UserProfiles.Where(p => p.reportsTo == managerID);

            foreach (UserProfile user in usersThatReportToMe)
            {
                usersInReportStructure.Add(user);
                buildListOfReportableUsers(user.UserId, ref usersInReportStructure);
            }

        }

        public string getStaffFullName(int? ID)
        {

            if (ID == null)
                return "";

            string userName = "";

            var userRec = db.UserProfiles.Where(pro => pro.UserId == ID);

            foreach (UserProfile up in userRec)
            {
                userName = up.FirstName + " " + up.LastName;
            }

            return userName;

        }

        public string getClientFullName(int ID)
        {

            string clientName = "";

            var clientRec = db.Clients.Where(pro => pro.ID == ID);

            foreach (Client cr in clientRec)
            {
                clientName = cr.FirstName + " " + cr.LastName;
            }

            return clientName;

        }

        public string getSiteName(int ID)
        {

            string siteName = "";

            var siteRec = db.Locations.Where(pro => pro.ID == ID);

            foreach (Location sr in siteRec)
            {
                siteName = sr.Name;
            }

            return siteName;

        }

        public int getSiteID(string siteName)
        {

            var siteRec = db.Locations.Where(pro => pro.Name == siteName).FirstOrDefault();
            if (siteRec != null)
                return siteRec.ID;
            else
                return -1;

        }

        public bool taskExist(int selectedStaff, string taskDescription, DateTime targetDate)
        {

            var staffToDoItems = db.ToDoListItems.Where(od => od.StaffID == selectedStaff && od.RequiredCompletionBy == targetDate && od.Description == taskDescription);

            if (staffToDoItems.Count() > 0)
                return true;
            else
                return false;
        }

        public int incompleteToDoItemCount()
        {

            DateTime fromDate = DateTime.Now;

            var staffToDoItems = db.ToDoListItems.Where(od => od.StaffID == WebSecurity.CurrentUserId && od.Complete == false);

            return staffToDoItems.Count();

        }

        public int overdueToDoItemCount()
        {

            DateTime fromDate = DateTime.Now;

            var staffToDoItems = db.ToDoListItems.Where(od => od.StaffID == WebSecurity.CurrentUserId && od.RequiredCompletionBy < fromDate && od.Complete == false);

            return staffToDoItems.Count();

        }

        public int unreadMessageCount()
        {

            var messagesToRead = db.Messages.Where(od => od.RecipientStaff == WebSecurity.CurrentUserId && od.MessageRead == false && od.MessageDeleted == false);
            return messagesToRead.Count();

        }

        public int openExceptionCount()
        {
            var exceptions = db.Exceptions.Where(r => r.CurrentActionByStaff == WebSecurity.CurrentUserId && r.state != exceptionState.Closed);
            return exceptions.Count();
        }

        public int getIncidentTypeEscalationManagerID(int incidentTypeID, int escalationStage)
        {
            var incidentTypeRecord = new IncidentType();
            incidentTypeRecord = db.IncidentTypes.Where(od => od.ID == incidentTypeID).Single();

            if (incidentTypeRecord != null)
                switch (escalationStage)
                {
                    case 1:
                        if (incidentTypeRecord.firstStaffEscID != null)
                            return (int)incidentTypeRecord.firstStaffEscID;
                        break;
                    case 2:
                        if (incidentTypeRecord.secondStaffEscID != null)
                            return (int)incidentTypeRecord.secondStaffEscID;
                        break;
                    case 3:
                        if (incidentTypeRecord.thirdStaffEscID != null)
                            return (int)incidentTypeRecord.thirdStaffEscID;
                        break;
                    default:
                        return 0;
                }

            return 0;
        }

        public string getIncidentTypeEscalationManagerName(int incidentTypeID, int escalationStage)
        {
            int managerId = 0;
            var incidentTypeRecord = new IncidentType();
            incidentTypeRecord = db.IncidentTypes.Where(od => od.ID == incidentTypeID).Single();


            if (incidentTypeRecord != null)
            {
                switch (escalationStage)
                {
                    case 1:
                        managerId = (int)incidentTypeRecord.firstStaffEscID;
                        break;
                    case 2:
                        managerId = (int)incidentTypeRecord.secondStaffEscID;
                        break;
                    case 3:
                        managerId = (int)incidentTypeRecord.thirdStaffEscID;
                        break;
                    default:
                        break;
                }
                var managerRecord = db.UserProfiles.Where(od => od.UserId == managerId).Single();
                if (managerRecord != null)
                    return managerRecord.FirstName + " " + managerRecord.LastName;

            }
            return "";

        }

        public int getExceptionEscallationManagerName(int staffID)
        {

            var staffRecord = db.UserProfiles.Find(staffID);

            if (staffID != 0)
            {
                if (staffRecord.reportsTo != null)
                    return (int)staffRecord.reportsTo;
                else
                    return -1;
            }
            else
                return -1;

        }

        public TimeSpan getIncidentTypeEscalationTimeFrame(int incidentTypeID, int escalationStage)
        {
            TimeSpan escalationTypeTimespan = new TimeSpan(0);

            var incidentTypeRecord = new IncidentType();
            incidentTypeRecord = db.IncidentTypes.Where(od => od.ID == incidentTypeID).Single();

            if (incidentTypeRecord != null)
                switch (escalationStage)
                {
                    case 1:
                        if (incidentTypeRecord.firstStaffEscID != null)
                        {
                            if (incidentTypeRecord.firstEscDays != null)
                                escalationTypeTimespan += new TimeSpan((int)incidentTypeRecord.firstEscDays, 0, 0, 0);
                            if (incidentTypeRecord.firstEscHours != null)
                                escalationTypeTimespan += new TimeSpan(0, (int)incidentTypeRecord.firstEscHours, 0, 0);
                            if (incidentTypeRecord.firstEscMinutes != null)
                                escalationTypeTimespan += new TimeSpan(0, 0, (int)incidentTypeRecord.firstEscMinutes, 0);
                            return escalationTypeTimespan;
                        }
                        break;
                    case 2:
                        if (incidentTypeRecord.secondStaffEscID != null)
                        {
                            if (incidentTypeRecord.secondEscDays != null)
                                escalationTypeTimespan += new TimeSpan((int)incidentTypeRecord.secondEscDays, 0, 0, 0);
                            if (incidentTypeRecord.secondEscHours != null)
                                escalationTypeTimespan += new TimeSpan(0, (int)incidentTypeRecord.secondEscHours, 0, 0);
                            if (incidentTypeRecord.secondEscMinutes != null)
                                escalationTypeTimespan += new TimeSpan(0, 0, (int)incidentTypeRecord.secondEscMinutes, 0);
                            return escalationTypeTimespan;
                        }
                        break;
                    case 3:
                        if (incidentTypeRecord.thirdStaffEscID != null)
                        {
                            if (incidentTypeRecord.thirdEscDays != null)
                                escalationTypeTimespan += new TimeSpan((int)incidentTypeRecord.thirdEscDays, 0, 0, 0);
                            if (incidentTypeRecord.thirdEscHours != null)
                                escalationTypeTimespan += new TimeSpan(0, (int)incidentTypeRecord.thirdEscHours, 0, 0);
                            if (incidentTypeRecord.thirdEscMinutes != null)
                                escalationTypeTimespan += new TimeSpan(0, 0, (int)incidentTypeRecord.thirdEscMinutes, 0);
                            return escalationTypeTimespan;
                        }
                        break;

                    default:
                        return new TimeSpan(0);
                }

            return new TimeSpan(0);
        }

        /// <summary>
        /// Gets all incidents related to the staff member by searching the history and incidents
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public List<Incident> getStaffIncidentsViaHistory(int staffID, bool getOpenIncidentsOnly, bool getIncidentsOnlyIMustAction)
        {

            List<int> incidentIDs = new List<int>();
            List<IncidentHistory> incidentHistoryRecords = new List<IncidentHistory>();

            List<Incident> incidents = db.Incidents.Where(m => m.StaffReportedID == staffID || m.UserProfileID == staffID).ToList();

            incidentHistoryRecords = db.IncidentHistoryEntries.Where(i => i.actionByStaffID == staffID || i.currentStaffID == staffID).ToList();

            foreach (IncidentHistory i in incidentHistoryRecords)
            {
                if (incidentIDs.IndexOf(i.incidentID) != -1)
                    incidentIDs.Add(i.incidentID);
            }

            foreach (int i in incidentIDs)
            {
                incidents.Add(db.Incidents.Where(m => m.ID == i).SingleOrDefault());
            }

            int maxIncidents = incidents.Count();
            int incidentIndex = 0;

            if (getOpenIncidentsOnly)
            {
                while (incidentIndex < maxIncidents)
                {
                    if (incidents[incidentIndex].currentState == incidentState.Closed)
                        incidents.Remove(incidents[incidentIndex]);
                    else
                        incidentIndex++;

                    maxIncidents = incidents.Count();
                }

                if (getIncidentsOnlyIMustAction)
                {
                    maxIncidents = incidents.Count();
                    incidentIndex = 0;

                    while (incidentIndex < maxIncidents)
                    {
                        if (incidents[incidentIndex].CurrentManagerID != staffID)
                            incidents.Remove(incidents[incidentIndex]);
                        else
                            incidentIndex++;

                        maxIncidents = incidents.Count();
                    }
                }
            }
            else
            {
                while (incidentIndex < maxIncidents)
                {
                    if (incidents[incidentIndex].currentState != incidentState.Closed)
                        incidents.Remove(incidents[incidentIndex]);
                    else
                        incidentIndex++;

                    maxIncidents = incidents.Count();
                }
            }

            return incidents;
        }

        public void addLog(string description, systemLogType type, string function, string module, int? staffID)
        {

            SystemLogEntry newEntry = new SystemLogEntry();
            newEntry.EventDate = DateTime.Now;
            newEntry.Description = description;
            newEntry.function = function;
            newEntry.LogType = type;
            newEntry.module = module;
            newEntry.StaffID = staffID;
            db.SystemLog.Add(newEntry);
            db.SaveChanges();
 
        }

        public void addUserActivityLog(string description, int staffID, string location)
        {

            UserActivityLogEntry newEntry = new UserActivityLogEntry();
            newEntry.EventDate = DateTime.Now;
            newEntry.Description = description;
            newEntry.StaffID = staffID;
            if (location != "")
                newEntry.Location = location;
            else
                newEntry.Location = getStaffLocationName(staffID);
            db.UserActivityLog.Add(newEntry);
            db.SaveChanges();

        }

        public void addRosterImportError(string errorDetail)
        {
            RosterImportErrorDetail newError = new RosterImportErrorDetail();
            newError.ErrorDetail = errorDetail;
            newError.ImportDate = DateTime.Now;
            newError.StaffID = WebSecurity.CurrentUserId;
            db.RosterImportErrors.Add(newError);
            db.SaveChanges();

        }

        public string getQualificationName(int ID)
        {
            Qualification q = db.Qualifications.Where(p => p.ID == ID).FirstOrDefault();

            if (q != null)
                return q.Name;
            else
                return "NOT FOUND";
    
        }

        public int getStaffIDFromName(string firstName, string lastName)
        {
            var staffRecord = db.UserProfiles.Where(r => r.FirstName == firstName && r.LastName == lastName).FirstOrDefault();
            if (staffRecord != null)
            {
                return staffRecord.UserId;
            }
            else
                return 0;
        }

        public int getStaffIDFromFullName(string staffFullName)
        {
            string correctedFullName = staffFullName.Replace(")", string.Empty);
            correctedFullName = correctedFullName.Replace("(", string.Empty);
            string[] nameParts = correctedFullName.Split(' ');
            string fName = nameParts[0];
            string lName = nameParts[1];
            var staffRecord = db.UserProfiles.Where(r => r.FirstName == fName && r.LastName == lName).FirstOrDefault();
            if (staffRecord != null)
            {
                return staffRecord.UserId;
            }
            else
                return 0;
        }

        public bool staffOnline(int? staffID)
        {
            if (staffID == null)
                return false;

            var staffRecord = db.UserProfiles.Where(r => r.UserId == staffID).FirstOrDefault();

            if (staffRecord != null)
            {
                return (bool)staffRecord.LoggedOn;
            }
            else
                return false;
        }

        public int getStaffLocationID(int? staffID)
        {
            if (staffID == null)
                return 0;

            var staffRecord = db.UserProfiles.Where(r => r.UserId == staffID).FirstOrDefault();

            if (staffRecord != null)
            {
                return staffRecord.currentLocationID;
            }
            else
                return 0;
        }

        public string getStaffLocationName(int? staffID)
        {
            if (staffID == null)
                return "unknown";

            var staffRecord = db.UserProfiles.Where(r => r.UserId == staffID).FirstOrDefault();

            if (staffRecord != null)
            {
                return staffRecord.currentLocation;
            }
            else
                return "unknown";
        }

        public DateTime getStaffLastOnlineEventTime(int? staffID)
        {
            if (staffID == null)
                return DateTime.MinValue;

            var staffRecord = db.UserProfiles.Where(r => r.UserId == staffID).FirstOrDefault();

            if (staffRecord != null)
            {
                return (DateTime)staffRecord.lastOnlineEventTime;
            }
            else
                return DateTime.MinValue;
        }

        public DateTime getStaffLastOfflineEventTime(int? staffID)
        {
            if (staffID == null)
                return DateTime.MinValue;

            var staffRecord = db.UserProfiles.Where(r => r.UserId == staffID).FirstOrDefault();

            if (staffRecord != null)
            {
                return (DateTime)staffRecord.lastOfflineEventTime;
            }
            else
                return DateTime.MinValue;
        }

        public int getClientIDFromName(string firstName, string lastName)
        {
            var clientRecord = db.Clients.Where(r => r.FirstName == firstName && r.LastName == lastName).FirstOrDefault();

            if (clientRecord != null)
            {
                return clientRecord.ID;
            }

            return 0;
        }

        public int getLocationIDFromName(string siteName)
        {
            var locationRecord = db.Locations.Where(r => r.Name == siteName).FirstOrDefault();

            if (locationRecord != null)
            {
                return locationRecord.ID;
            }

            return 0;
        }

        public string getLocationNameFromID(int locationID)
        {
            var locationRecord = db.Locations.Where(r => r.ID == locationID).FirstOrDefault();

            if (locationRecord != null)
            {
                return locationRecord.Name;
            }

            return "";
        }

        public int getIDOfStaffUpline(int staffID)
        {
            // If there is no upline report then report the queried user ID as that will be the top of the tree.

            var user = db.UserProfiles.Where(r => r.UserId == staffID).FirstOrDefault();

            if (user != null)
            {
                if (user.reportsTo != null)
                    return (int)user.reportsTo;
                else
                    return staffID;
            }
            else
                return staffID;
        }

        public GlobalSettingsModel getGlobalSettings()
        {
            return db.GlobalSettings.Find(1);
        }

        public int getGlobalSetting_DaysBeforeShiftToCheckQualifications()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.daysBeforeShiftToCheckQualifications;
            else
                return 0;

        }

        public int getGlobalSetting_HoursBetweenExceptionEscallation()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.hoursBetweenExceptionEscallation;
            else
                return 0;

        }

        public int getGlobalSetting_DaysBeforeQualificaitonExpiryForReminder()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.daysBeforeQualificaitonExpiryForReminder;
            else
                return 0;

        }

        public int getGlobalSetting_MinutesBeforeShiftToCheckIfStaffOnline()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.minutesBeforeShiftToCheckIfStaffOnline;
            else
                return 0;

        }

        public int getGlobalSetting_MinutesAllowedForLateShiftArrival()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.minutesAllowedForLateShiftArrival;
            else
                return 0;

        }

        public int getGlobalSetting_MinutesAllowedForEarlyShiftLeave()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.minutesAllowedForEarlyShiftLeave;
            else
                return 0;

        }

        public int getGlobalSetting_MinutesAllowedForStaffOfflineDuringShift()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.minutesAllowedForStaffOfflineDuringShift;
            else
                return 0;

        }


        public bool getGlobalSetting_todoRateUnderTarget()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.todoRateUnderTarget;
            else
                return false;

        }

        public void setGlobalSetting_todoRateUnderTarget(bool val)
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
            {
                globalSettings.todoRateUnderTarget = val;
                db.Entry(globalSettings).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool getGlobalSetting_staffShiftArrivalsRateUnderTarget()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.staffShiftArrivalsRateUnderTarget;
            else
                return false;

        }

        public void setGlobalSetting_staffShiftArrivalsRateUnderTarget(bool val)
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
            {
                globalSettings.staffShiftArrivalsRateUnderTarget = val;
                db.Entry(globalSettings).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool getGlobalSetting_staffShiftCancelRateOverTarget()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.staffShiftCancelRateOverTarget;
            else
                return false;

        }

        public void setGlobalSetting_staffShiftCancelRateOverTarget(bool val)
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
            {
                globalSettings.staffShiftCancelRateOverTarget = val;
                db.Entry(globalSettings).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool getGlobalSetting_staffShiftLeaveRateOverTarget()
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
                return globalSettings.staffShiftLeaveRateOverTarget;
            else
                return false;

        }

        public void setGlobalSetting_staffShiftLeaveRateOverTarget(bool val)
        {
            var globalSettings = db.GlobalSettings.Find(1);

            if (globalSettings != null)
            {
                globalSettings.staffShiftLeaveRateOverTarget = val;
                db.Entry(globalSettings).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public int checkForDelegateNeeded(int staffID)
        {
            var user = db.UserProfiles.Where(r => r.UserId == staffID).FirstOrDefault();

            if (user != null)
            {
                if ((bool)user.available)
                    return staffID;
                else
                    return (int)user.delegateID;
            }

            else
                return 0;
        }


        public int getOpenIncidentCount()
        {
            var openIncidents = db.Incidents.Where(r => r.CurrentManagerID == WebSecurity.CurrentUserId && r.currentState != incidentState.Closed);
            return openIncidents.Count();
        }

        public int getOverdueOpenIncidentCount()
        {
            var overdueOpenIncidents = db.Incidents.Where(r => r.CurrentManagerID == WebSecurity.CurrentUserId && r.currentState != incidentState.Closed && r.currentActionByDate > DateTime.Now);
            return overdueOpenIncidents.Count();
        }

        public Incident getIncidentByID(int ID)
        {
            var incidentRecord = db.Incidents.Where(r => r.ID == ID).FirstOrDefault();

            if (incidentRecord != null)
                return incidentRecord;
            else
                return null;
        }


        public int incidentsRaisedAll(DateTime start, DateTime end)
        {
            var incidentsRaised = db.Incidents.Where(r => r.incidentDate >= start && r.incidentDate <= end);

            return incidentsRaised.Count();

        }

        public int incidentsClosedAll(DateTime start, DateTime end)
        {
            var incidentsClosed = db.IncidentHistoryEntries.Where(r => r.historyEntryCreationDate >= start && r.historyEntryCreationDate <= end && r.state == incidentState.Closed);

            return incidentsClosed.Count();

        }

        public int incidentsOpenAll()
        {
            var incidentsOpen = db.Incidents.Where(r => r.currentState != incidentState.Closed);

            return incidentsOpen.Count();

        }


        public int exceptionsRaisedAll(DateTime start, DateTime end)
        {
            var exceptionsRaised = db.Exceptions.Where(r => r.CreationDate >= start && r.CreationDate <= end);

            return exceptionsRaised.Count();

        }

        public int exceptionsClosedAll(DateTime start, DateTime end)
        {
            var exceptionsClosed = db.Exceptions.Where(r => r.CurrentActionByDate >= start && r.CurrentActionByDate <= end && r.state == exceptionState.Closed);

            return exceptionsClosed.Count();

        }

        public int exceptionsOpenAll()
        {
            var exceptionsOpen = db.Exceptions.Where(r => r.state != exceptionState.Closed);

            return exceptionsOpen.Count();

        }


        public double staffLateArrivalRateForAll(DateTime start, DateTime end)
        {

            int totalLateArrivalCount = 0;
            int totalShiftCount = 0;
            var staffList = db.UserProfiles;

            foreach (UserProfile s in staffList)
            {
                totalLateArrivalCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.notArrivedToStartShiftExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalLateArrivalCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double staffLateArrivalRateForTeam(int teamID, DateTime start, DateTime end)
        {

            int totalLateArrivalCount = 0;
            int totalShiftCount = 0;
            var staffList = db.StaffInTeams.Where(r => r.TeamID == teamID);

            foreach (StaffInTeam s in staffList)
            {
                totalLateArrivalCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.notArrivedToStartShiftExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.StaffID).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.StaffID).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalLateArrivalCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double staffLateArrivalRateForSubordinates(int managerID, DateTime start, DateTime end)
        {

            List<UserProfile> users = getAllSubordinates(managerID);
            int totalLateArrivalCount = 0;
            int totalShiftCount = 0;

            foreach (UserProfile s in users)
            {
                totalLateArrivalCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.notArrivedToStartShiftExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalLateArrivalCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double lateArrivalRateForStaff(int staffID, DateTime start, DateTime end)
        {

            int totalLateArrivalCount = db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.notArrivedToStartShiftExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == staffID).Count();
            int totalShiftCount = db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == staffID).Count();
       
            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalLateArrivalCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        /////////////////////////////


        


        public double toDoCompletionRateForAll(DateTime start, DateTime end)
        {

            int totalToDoCompletionCount = 0;
            int totalToDoCount = 0;
            var staffList = db.UserProfiles.Where(r => r.Inactive != true); 

            foreach (UserProfile s in staffList)
            {
                totalToDoCompletionCount += db.ToDoListItems.Where(r => r.Deleted != true && r.RequiredCompletionBy >= start && r.RequiredCompletionBy <= end && r.Complete == true && r.StaffID == s.UserId).Count();
                totalToDoCount += db.ToDoListItems.Where(r => r.Deleted != true && r.RequiredCompletionBy >= start  && r.RequiredCompletionBy <= end   && r.StaffID == s.UserId).Count();
            }

            if (totalToDoCount > 0)
                return (Convert.ToDouble(totalToDoCompletionCount) / Convert.ToDouble(totalToDoCount)) * 100.0;
            else
                return 100.0;

        }

        public double toDoCompletionRateForTeam(int teamID, DateTime start, DateTime end)
        {

            int totalToDoCompletionCount = 0;
            int totalToDoCount = 0;
            var staffList = db.StaffInTeams.Where(r => r.TeamID == teamID);

            foreach (StaffInTeam s in staffList)
            {
                totalToDoCompletionCount += db.ToDoListItems.Where(r => r.Deleted != true && r.Complete == true && (r.RequiredCompletionBy >= start || r.CompletedDate >= start) && r.StaffID == s.StaffID).Count();
                totalToDoCount += db.ToDoListItems.Where(r => r.Deleted != true && (r.RequiredCompletionBy >= start || r.CompletedDate >= start) && r.StaffID == s.StaffID).Count();
            }

            if (totalToDoCount > 0)
                return (Convert.ToDouble(totalToDoCompletionCount) / Convert.ToDouble(totalToDoCount)) * 100.0;
            else
                return 100.0;
        }

        public double toDoCompletionRateForSubordinates(int managerID, DateTime start, DateTime end)
        {

            List<UserProfile> staffList = getAllSubordinates(managerID);
            int totalToDoCompletionCount = 0;
            int totalToDoCount = 0;

            foreach (UserProfile s in staffList)
            {
                totalToDoCompletionCount += db.ToDoListItems.Where(r => r.Deleted != true && r.Complete == true && (r.RequiredCompletionBy >= start || r.CompletedDate >= start) && r.StaffID == s.UserId).Count();
                totalToDoCount += db.ToDoListItems.Where(r => r.Deleted != true && (r.RequiredCompletionBy >= start || r.CompletedDate >= start) && r.StaffID == s.UserId).Count();
            }

            if (totalToDoCount > 0)
                return (Convert.ToDouble(totalToDoCompletionCount) / Convert.ToDouble(totalToDoCount)) * 100.0;
            else
                return 100.0;
        }

        public double toDoCompletionRateForStaff(int staffID, DateTime start, DateTime end)
        {

            int totalToDoCompletionCount = db.ToDoListItems.Where(r => r.Deleted != true && r.Complete == true && (r.RequiredCompletionBy >= start || r.CompletedDate >= start) && r.StaffID == staffID).Count();
            int totalToDoCount = db.ToDoListItems.Where(r => r.Deleted != true && (r.RequiredCompletionBy >= start || r.CompletedDate >= start) && r.StaffID == staffID).Count();

            if (totalToDoCount > 0)
                return (Convert.ToDouble(totalToDoCompletionCount) / Convert.ToDouble(totalToDoCount)) * 100.0;
            else
                return 100.0;
        }

        public double earlyShiftCompletionRateForAll(DateTime start, DateTime end)
        {

            int totalEarlyShiftCompletionCount = 0;
            int totalShiftCount = 0;
            var staffList = db.UserProfiles;

            foreach (UserProfile s in staffList)
            {
                totalEarlyShiftCompletionCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double earlyShiftCompletionRateForTeam(int teamID, DateTime start, DateTime end)
        {

            int totalEarlyShiftCompletionCount = 0;
            int totalShiftCount = 0;
            var staffList = db.StaffInTeams.Where(r => r.TeamID == teamID);

            foreach (StaffInTeam s in staffList)
            {
                totalEarlyShiftCompletionCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.StaffID).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.StaffID).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double earlyShiftCompletionRateForSubordinates(int managerID, DateTime start, DateTime end)
        {

            List<UserProfile> users = getAllSubordinates(managerID);
            int totalEarlyShiftCompletionCount = 0;
            int totalShiftCount = 0;

            foreach (UserProfile s in users)
            {
                totalEarlyShiftCompletionCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double earlyShiftCompletionRateForStaff(int staffID, DateTime start, DateTime end)
        {

            int totalEarlyShiftCompletionCount = db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == staffID).Count();
            int totalShiftCount = db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == staffID).Count();

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        ///////////////////

        public double staffShiftCancellationRateForAll(DateTime start, DateTime end)
        {

            int totalEarlyShiftCompletionCount = 0;
            int totalShiftCount = 0;
            var staffList = db.UserProfiles;

            foreach (UserProfile s in staffList)
            {
                totalEarlyShiftCompletionCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double staffShiftCancellationRateForTeam(int teamID, DateTime start, DateTime end)
        {

            int totalEarlyShiftCompletionCount = 0;
            int totalShiftCount = 0;
            var staffList = db.StaffInTeams.Where(r => r.TeamID == teamID);

            foreach (StaffInTeam s in staffList)
            {
                totalEarlyShiftCompletionCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.StaffID).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.StaffID).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double staffShiftCancellationRateForSubordinates(int managerID, DateTime start, DateTime end)
        {

            List<UserProfile> users = getAllSubordinates(managerID);
            int totalEarlyShiftCompletionCount = 0;
            int totalShiftCount = 0;

            foreach (UserProfile s in users)
            {
                totalEarlyShiftCompletionCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
                totalShiftCount += db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == s.UserId).Count();
            }

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

        public double staffShiftCancellationRateForStaff(int staffID, DateTime start, DateTime end)
        {

            int totalEarlyShiftCompletionCount = db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.leaveBeforeShiftCompleteExceptionGenerated && r.Start >= start && r.Start <= end && r.StaffID == staffID).Count();
            int totalShiftCount = db.Rosters.Where(r => r.Deleted != true && !r.CancelledByStaff && r.Start >= start && r.Start <= end && r.StaffID == staffID).Count();

            if (totalShiftCount > 0)
                return (Convert.ToDouble(totalEarlyShiftCompletionCount) / Convert.ToDouble(totalShiftCount)) * 100.0;
            else
                return 100.0;
        }

    }
}