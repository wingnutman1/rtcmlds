using CMS_Web.DAL;
using CMS_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Helpers
{
    public class exceptionEngine
    {
        private CMS_WebContext db = new CMS_WebContext();
        private Data dataHelper = new Data();

        // Deflaut constructor
        public exceptionEngine()
        {

        }

        public void processReportMetricExceptions()
        {
            string exceptionReason = "";

           
            GlobalSettingsModel globalSettings = dataHelper.getGlobalSettings();
            DateTime windowStart = DateTime.Now - TimeSpan.FromDays(globalSettings.numberOfDaysDurationPerformanceMetricAnalysis);
            DateTime windowEnd = DateTime.Now;

            int userIDToNotify = dataHelper.checkForDelegateNeeded(globalSettings.staffIDForPerformanceReporting);

            // ToDo list completions fall below target.

            double toDoCompletionRate = dataHelper.toDoCompletionRateForAll(windowStart, windowEnd);
            double minCompletionRate = globalSettings.minimumPercentToDoListCompletions;
            
            if (toDoCompletionRate < minCompletionRate && !dataHelper.getGlobalSetting_todoRateUnderTarget())
            {
                exceptionReason = "ToDo completion rate is below target.  Current rate : " + toDoCompletionRate.ToString("###.#%") + " Target rate :" + minCompletionRate.ToString("###.#%");
                dataHelper.setGlobalSetting_todoRateUnderTarget(true);
                dataHelper.addLog(exceptionReason, systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                if (userIDToNotify != globalSettings.staffIDForPerformanceReporting)
                    dataHelper.addLog("ToDo Rate Target exception reported to delegate " + dataHelper.getStaffFullName(userIDToNotify) + " instead of " + dataHelper.getStaffFullName(globalSettings.staffIDForPerformanceReporting), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                int newExceptionID = addException(exceptionReason, null, exceptionType.OperationalPerformance, null, null, null, null, userIDToNotify);
            }

            if (toDoCompletionRate >= minCompletionRate && dataHelper.getGlobalSetting_todoRateUnderTarget())
            {
                dataHelper.setGlobalSetting_todoRateUnderTarget(false);
                dataHelper.addLog("To Do Rate normal : " + minCompletionRate.ToString("###.#%"), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
            }


            // Staff on time arrivals falls below target.
            double lateArrivalRate = dataHelper.staffLateArrivalRateForAll(windowStart, windowEnd);
            double minOnTimeArrivalRate = globalSettings.minimumPercentOnTimeShiftArrivals;

            if (lateArrivalRate < minOnTimeArrivalRate && !dataHelper.getGlobalSetting_staffShiftArrivalsRateUnderTarget())
            {
                exceptionReason = "On time shift arrival rate is below target.  Current rate : " + lateArrivalRate.ToString("###.#%") + " Target rate :" + minOnTimeArrivalRate.ToString("###.#%");
                dataHelper.setGlobalSetting_staffShiftArrivalsRateUnderTarget(true);
                dataHelper.addLog(exceptionReason, systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                if (userIDToNotify != globalSettings.staffIDForPerformanceReporting)
                    dataHelper.addLog("On Time Arrival Target exception reported to delegate " + dataHelper.getStaffFullName(userIDToNotify) + " instead of " + dataHelper.getStaffFullName(globalSettings.staffIDForPerformanceReporting), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                int newExceptionID = addException(exceptionReason, null, exceptionType.OperationalPerformance, null, null, null, null, userIDToNotify);
            }

            if (lateArrivalRate >= minOnTimeArrivalRate && dataHelper.getGlobalSetting_staffShiftArrivalsRateUnderTarget())
            {
                dataHelper.setGlobalSetting_staffShiftArrivalsRateUnderTarget(false);
                dataHelper.addLog("On time shift arrival rate normal : " + minOnTimeArrivalRate.ToString("###.#%"), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
            }

            // Staff early shift completion exceeds target.
            double earlyShiftCompletionRate = dataHelper.earlyShiftCompletionRateForAll(windowStart, windowEnd);
            double maxEarlyShiftCompletionRate = globalSettings.maximumPercentEarlyShiftLeaving;

            if (earlyShiftCompletionRate > maxEarlyShiftCompletionRate && !dataHelper.getGlobalSetting_staffShiftLeaveRateOverTarget())
            {
                exceptionReason = "Early shift leave rate is above target.  Current rate : " + earlyShiftCompletionRate.ToString("###.#%") + " Target rate :" + maxEarlyShiftCompletionRate.ToString("###.#%");
                dataHelper.setGlobalSetting_staffShiftLeaveRateOverTarget(true);
                dataHelper.addLog(exceptionReason, systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                if (userIDToNotify != globalSettings.staffIDForPerformanceReporting)
                    dataHelper.addLog("Early Shift Leave Target exception reported to delegate " + dataHelper.getStaffFullName(userIDToNotify) + " instead of " + dataHelper.getStaffFullName(globalSettings.staffIDForPerformanceReporting), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                int newExceptionID = addException(exceptionReason, null, exceptionType.OperationalPerformance, null, null, null, null, userIDToNotify);
            }

            if (earlyShiftCompletionRate <= maxEarlyShiftCompletionRate && dataHelper.getGlobalSetting_staffShiftLeaveRateOverTarget())
            {
                dataHelper.setGlobalSetting_staffShiftLeaveRateOverTarget(false);
                dataHelper.addLog("Early Shift Leave rate normal : " + maxEarlyShiftCompletionRate.ToString("###.#%"), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
            }

            // Staff shift cancellations exceed target
            double staffShiftCancellationRate = dataHelper.staffShiftCancellationRateForAll(windowStart, windowEnd);
            double maxStaffShiftCancellationRate = globalSettings.maximumPercentShiftCancellations;

            if (staffShiftCancellationRate > maxStaffShiftCancellationRate && !dataHelper.getGlobalSetting_staffShiftCancelRateOverTarget())
            {
                exceptionReason = "Staff Shift Cancellation rate is above target.  Current rate : " + staffShiftCancellationRate.ToString("###.#%") + " Target rate :" + maxStaffShiftCancellationRate.ToString("###.#%");
                dataHelper.setGlobalSetting_staffShiftCancelRateOverTarget(true);
                dataHelper.addLog(exceptionReason, systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                if (userIDToNotify != globalSettings.staffIDForPerformanceReporting)
                    dataHelper.addLog("Staff Shift Cancellation Target exception reported to delegate " + dataHelper.getStaffFullName(userIDToNotify) + " instead of " + dataHelper.getStaffFullName(globalSettings.staffIDForPerformanceReporting), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
                int newExceptionID = addException(exceptionReason, null, exceptionType.OperationalPerformance, null, null, null, null, userIDToNotify);
            }

            if (staffShiftCancellationRate <= maxStaffShiftCancellationRate && dataHelper.getGlobalSetting_staffShiftCancelRateOverTarget())
            {
                dataHelper.setGlobalSetting_staffShiftCancelRateOverTarget(false);
                dataHelper.addLog("Staff Shift Cancellation rate normal : " + maxStaffShiftCancellationRate.ToString("###.#%"), systemLogType.Engine, "processReportMetricExceptions", "exceptionEngine", null);
            }
        }

        public void processIncidentExceptions()
        {
            string exceptionReason = "";

            // As there can be multiple history records for the same incident, we just need to find the last history record for that incident to check if the incident close time has been exceeded

            var indcidentHistoryRecords = db.IncidentHistoryEntries.Where(r => r.actionByDate <= DateTime.Now &&
                                                                               r.state != incidentState.Closed &&
                                                                               r.actionByDateExceededExceptionRaised == false);

            List<IncidentHistory> latestIncidents = new List<IncidentHistory>();


            foreach (IncidentHistory i in indcidentHistoryRecords)
            {

                var incidentRecord = latestIncidents.Where(r => r.incidentID == i.incidentID).FirstOrDefault();

                if (incidentRecord == null)
                {
                    var newRecordToAdd = indcidentHistoryRecords.Where(r => r.incidentID == i.incidentID)
                                                                .OrderByDescending(t => t.historyEntryCreationDate)
                                                                .FirstOrDefault();
                    if (newRecordToAdd != null)
                        latestIncidents.Add(newRecordToAdd);
                }
         
            }

            // Incident close time exceeded
            foreach (IncidentHistory i in latestIncidents)
            {
                Incident incidentRecord = dataHelper.getIncidentByID(i.incidentID);
                if (incidentRecord != null)
                {
                    exceptionReason = "Time to close incident '" + incidentRecord.Description;
                    if (incidentRecord.LocationID != 0)
                        exceptionReason += "' at location '" + incidentRecord.Location;
                    if (incidentRecord.ClientID != 0)
                        exceptionReason += "' for Client '" + incidentRecord.Client;
                    exceptionReason += "' has expired.";
                    i.actionByDateExceededExceptionRaised = true;
                    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    dataHelper.addLog(exceptionReason, systemLogType.Engine, "processIncidentExceptions", "exceptionEngine", incidentRecord.CurrentManagerID);
                    int newExceptionID = addException(exceptionReason, (int)incidentRecord.CurrentManagerID, exceptionType.ToDo, null, incidentRecord.ID, incidentRecord.ClientID, incidentRecord.LocationID, null);
                    i.actionByDateExceededExceptionID = newExceptionID;
                }
            }

            db.SaveChanges();
        }

        public void processToDoExceptions()
        {
            string exceptionReason = "";

            var notCompletedItems = db.ToDoListItems.Where(r => r.Complete == false &&
                                                                r.itemNotCompleteInAllocatedTimeExceptionRaised == false &&
                                                                r.RequiredCompletionBy <= DateTime.Now);

            // Item not completed within allocated time
            foreach (ToDoListItem i in notCompletedItems)
            {
                exceptionReason = "Incomplete To Do task : '" + i.Description;
                if (i.RelatedLocationID != 0)
                    exceptionReason += "' for location '" + dataHelper.getLocationNameFromID((int)i.RelatedLocationID);
                if (i.RelatedClientID != 0)
                    exceptionReason += "' for Client '" + dataHelper.getClientFullName((int)i.RelatedClientID);
                exceptionReason += "'.";
                i.itemNotCompleteInAllocatedTimeExceptionRaised = true;
                db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                dataHelper.addLog(exceptionReason, systemLogType.Engine, "processToDoExceptions", "exceptionEngine", i.StaffID);
                int newExceptionID = addException(exceptionReason, i.StaffID, exceptionType.ToDo, i.ID, null, i.RelatedClientID, i.RelatedLocationID, null);
                i.itemNotCompleteInAllocatedTimeExceptionID = newExceptionID;
            }
            db.SaveChanges();

            // Item bumped to next staff on shift
        }

        public void processShiftActivityExceptions()
        {
            string exceptionReason = "";

            // Staff not online before shift
            foreach (RosterModel r in db.Rosters.Where(r => r.Start <= DateTime.Now + TimeSpan.FromMinutes(dataHelper.getGlobalSetting_MinutesBeforeShiftToCheckIfStaffOnline()) &&
                                                            !r.notOnlineBeforeShiftExceptionGenerated &&
                                                            (r.StaffID != null) && 
                                                            (r.Deleted == false )))
            {
                if (!dataHelper.staffOnline(r.StaffID))
                {
                    exceptionReason = " Start: " + r.Start.ToString("dd/MM/yyyy HH:mm tt");
                    if (r.SiteID != 0)
                        exceptionReason += " at location " + r.SiteName;
                    if (r.ClientID != 0)
                        exceptionReason += " with Client " + r.ClientFullName;
                    exceptionReason += ".";
                    r.notOnlineBeforeShiftExceptionGenerated = true;
                    db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                    dataHelper.addLog("Not Online Before Shift." + exceptionReason, systemLogType.Engine, "processShiftActivityExceptions", "exceptionEngine", r.StaffID);
                    int newExceptionID = addException("Not Online Before Shift." + exceptionReason, (int)r.StaffID, exceptionType.ShiftTiming, r.ID, null, r.ClientID, r.SiteID, null);
                    r.exceptionID.Add(newExceptionID);
                }
            }


            // Staff not a location in time to start shift
            foreach (RosterModel r in db.Rosters.Where(r => r.Start + TimeSpan.FromMinutes(dataHelper.getGlobalSetting_MinutesAllowedForLateShiftArrival()) <= DateTime.Now   &&
                                                            !r.notArrivedToStartShiftExceptionGenerated &&
                                                            (r.StaffID != null) &&
                                                            (r.Deleted == false )))
            {
                if (!dataHelper.staffOnline(r.StaffID))
                {
                    exceptionReason = " Start: " + r.Start.ToString("dd/MM/yyyy HH:mm tt");
                    if (r.SiteID != 0)
                        exceptionReason += " at location " + r.SiteName;
                    if (r.ClientID != 0)
                        exceptionReason += " with Client " + r.ClientFullName;
                    exceptionReason += ".";
                    r.notOnlineBeforeShiftExceptionGenerated = true;
                    db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                    dataHelper.addLog("Not Online For Shift Start." + exceptionReason, systemLogType.Engine, "processShiftActivityExceptions", "exceptionEngine", r.StaffID);
                    int newExceptionID = addException("Not Online For Shift Start." + exceptionReason, (int)r.StaffID, exceptionType.ShiftTiming, r.ID, null, r.ClientID, r.SiteID, null);
                    r.exceptionID.Add(newExceptionID);
                }
                else
                {
                    if (dataHelper.getStaffLocationID(r.StaffID) != r.SiteID)
                    {
                        exceptionReason = " Start: " + r.Start.ToString("dd/MM/yyyy HH:mm tt");
                        if (r.SiteID != 0)
                            exceptionReason += " at location " + r.SiteName;
                        if (r.ClientID != 0)
                            exceptionReason += " with Client " + r.ClientFullName;                        
                        exceptionReason += " Actual Location was " + dataHelper.getStaffLocationName(r.StaffID);
                        r.notOnlineBeforeShiftExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Was not at required location on time." + exceptionReason, systemLogType.Engine, "processShiftActivityExceptions", "exceptionEngine", r.StaffID);
                        int newExceptionID = addException("Was not at required location on time." + exceptionReason, (int)r.StaffID, exceptionType.ShiftTiming, r.ID, null, r.ClientID, r.SiteID, null);
                        r.exceptionID.Add(newExceptionID);
                    }
                }
            }

            // Staff offline during shift
            foreach (RosterModel r in db.Rosters.Where(r => r.Start <= DateTime.Now &&
                                                         r.End >= DateTime.Now &&
                                                         !r.offlineDuringShiftExceptionGenerated &&
                                                         (r.StaffID != null) &&
                                                         (r.Deleted == false)))
            {
                if (!dataHelper.staffOnline(r.StaffID))
                {
                    DateTime lastOfflineTime = dataHelper.getStaffLastOfflineEventTime(r.StaffID);
                    if (lastOfflineTime > DateTime.MinValue)
                    {
                        TimeSpan offlineDuration = DateTime.Now - lastOfflineTime;
                        if (offlineDuration.Minutes > dataHelper.getGlobalSetting_MinutesAllowedForStaffOfflineDuringShift())
                        {
                            exceptionReason = " Offline during shift ";
                            if (r.SiteID != 0)
                                exceptionReason += " at location " + r.SiteName;
                            if (r.ClientID != 0)
                                exceptionReason += " with Client " + r.ClientFullName;
                            exceptionReason += ".";
                            r.offlineDuringShiftExceptionGenerated = true;
                            db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                            dataHelper.addLog(exceptionReason, systemLogType.Engine, "processShiftActivityExceptions", "exceptionEngine", r.StaffID);
                            int newExceptionID = addException(exceptionReason, (int)r.StaffID, exceptionType.ShiftTiming, r.ID, null, r.ClientID, r.SiteID, null);
                            r.exceptionID.Add(newExceptionID);
                        }
                    }
                }
            }

            // Staff leave site before end of shift
            foreach (RosterModel r in db.Rosters.Where(r => r.Start <= DateTime.Now &&
                                                        r.End - TimeSpan.FromMinutes(dataHelper.getGlobalSetting_MinutesAllowedForEarlyShiftLeave()) >= DateTime.Now &&
                                                        !r.leaveBeforeShiftCompleteExceptionGenerated &&
                                                        (r.StaffID != null) &&
                                                        (r.Deleted == false)))
            {
                if (dataHelper.staffOnline(r.StaffID))
                {
                    int currentLocation = dataHelper.getStaffLocationID(r.StaffID);
                    if (currentLocation != r.SiteID)
                    {
                            exceptionReason = "Leave Shift Early";
                            if (r.SiteID != 0)
                                exceptionReason += " from location " + r.SiteName;
                            if (r.ClientID != 0)
                                exceptionReason += " with Client " + r.ClientFullName;
                            exceptionReason += ".";
                            r.leaveBeforeShiftCompleteExceptionGenerated = true;
                            db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                            dataHelper.addLog(exceptionReason, systemLogType.Engine, "processShiftActivityExceptions", "exceptionEngine", r.StaffID);
                            int newExceptionID = addException(exceptionReason, (int)r.StaffID, exceptionType.ShiftTiming, r.ID, null, r.ClientID, r.SiteID, null);
                            r.exceptionID.Add(newExceptionID);
                    }
                }
            }


            foreach (RosterModel r in db.Rosters.Where(r => r.Start >= DateTime.Now && (r.Deleted == false)))
            {
                // Check for qualifications being valid when shift is allocated i.e. the first time the systems see it.
                if (!r.qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated)
                {
                    if (!isStaffQualifiedToWorkAtLocation(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Shift Allocation: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        int newExceptionID = addException("Shift Allocation: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);
                        r.exceptionID.Add(newExceptionID);

                    }
                    if (!isStaffQualifiedToWorkWithClient(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Shift Allocation: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        int newExceptionID = addException("Shift Allocation: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);
                        r.exceptionID.Add(newExceptionID);
                    }
                }

                // Check for qualifcations being valid when we are within the check value of days before the shift
                if (r.Start - TimeSpan.FromDays(dataHelper.getGlobalSetting_DaysBeforeShiftToCheckQualifications()) < DateTime.Now && !r.qualificationsNotCurrentWarningBeforeShiftExceptionGenerated)
                {
                    if (!isStaffQualifiedToWorkAtLocation(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWarningBeforeShiftExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Early Preshift Warning: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        int newExceptionID = addException("Early Preshift Warning: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);
                        r.exceptionID.Add(newExceptionID);

                    }
                    if (!isStaffQualifiedToWorkWithClient(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWarningBeforeShiftExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Early Preshift Warning: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        int newExceptionID = addException("Early Preshift Warning: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);
                        r.exceptionID.Add(newExceptionID);
                    }

                }


            }

            db.SaveChanges();
        }

        public void processShiftQualificationsExceptions()
        {
            string exceptionReason = "";

            foreach (RosterModel r in db.Rosters.Where(r => r.Start >= DateTime.Now && (r.Deleted == false)))
            {
                // Check for qualifications being valid when shift is allocated i.e. the first time the systems see it.
                if (!r.qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated)
                {
                    if (!isStaffQualifiedToWorkAtLocation(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Shift Allocation: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        addException("Shift Allocation: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);

                    }
                    if (!isStaffQualifiedToWorkWithClient(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Shift Allocation: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        addException("Shift Allocation: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);
                    }
                }

                // Check for qualifcations being valid when we are within the check value of days before the shift
                if (r.Start - TimeSpan.FromDays(dataHelper.getGlobalSetting_DaysBeforeShiftToCheckQualifications()) < DateTime.Now && !r.qualificationsNotCurrentWarningBeforeShiftExceptionGenerated)
                {
                    if (!isStaffQualifiedToWorkAtLocation(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWarningBeforeShiftExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Early Preshift Warning: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        addException("Early Preshift Warning: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);

                    }
                    if (!isStaffQualifiedToWorkWithClient(r, ref exceptionReason))
                    {
                        r.qualificationsNotCurrentWarningBeforeShiftExceptionGenerated = true;
                        db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog("Early Preshift Warning: " + exceptionReason, systemLogType.Engine, "processRosterExceptions", "exceptionEngine", null);
                        addException("Early Preshift Warning: " + exceptionReason, (int)r.StaffID, exceptionType.Qualifications, r.ID, null, r.ClientID, r.SiteID, null);
                    }

                }


            }

            db.SaveChanges();
        }




        public void processQualificationsExpiryExceptions()
        {

            var sq_dataset = db.StaffQualifications.Where(sq => sq.expired != true || sq.expiryExceptionRaised != true || sq.expiryWarningExceptionRaised);

            foreach (StaffQualification sq in sq_dataset)
            {
                if (!sq.expiryExceptionRaised && (sq.ExpiryDate <= DateTime.Now))
                {
                    string warningMessage = "Qualification has expired warning.  Qualification '" + dataHelper.getQualificationName(sq.QualificationID) + "' has expired.";
                    sq.expiryExceptionRaised = true;
                    sq.expiryExceptionID = addException(warningMessage, sq.StaffID, exceptionType.Qualifications, null, null, null, null, null);
                    db.Entry(sq).State = System.Data.Entity.EntityState.Modified;
                    dataHelper.addLog(warningMessage, systemLogType.Engine, "processQualificationsExpiryExceptions", "exceptionEngine", null);
                }
                else
                    if (!sq.expiryWarningExceptionRaised && (sq.ExpiryDate < DateTime.Now + TimeSpan.FromDays(dataHelper.getGlobalSetting_DaysBeforeShiftToCheckQualifications())))
                    {
                        string warningMessage = "Qualification about to expire warning.  Qualification '" + dataHelper.getQualificationName(sq.QualificationID) + "' will expire on " + sq.ExpiryDate.ToString("dd/MM/yyyy") + ".";
                        sq.expiryWarningExceptionRaised = true;
                        sq.expiryWarningExceptionID = addException(warningMessage, sq.StaffID, exceptionType.Qualifications, null, null, null, null, null);
                        db.Entry(sq).State = System.Data.Entity.EntityState.Modified;
                        dataHelper.addLog(warningMessage, systemLogType.Engine, "processQualificationsExpiryExceptions", "exceptionEngine", null);
                    }
                
            }

            db.SaveChanges();
        }


        public int addException(string description, int? relatedStaffID, exceptionType type, int? shiftID, int? incidentID, int? clientID, int? locationID, int? actionByStaff)
        {
            CMS_WebContext db2 = new CMS_WebContext();
            ExceptionDetail newException = new ExceptionDetail();

            newException.Description = description;
            newException.CreationDate = DateTime.Now;
            newException.Active = true;
            newException.relatedStaffID = relatedStaffID;
            newException.realtedLocationID = locationID;
            newException.relatedClientID = clientID;
            newException.relatedIncidentID = incidentID;
            newException.ExceptionType = type;
            newException.state = exceptionState.Created;
            if (relatedStaffID != null)
                newException.CurrentActionByStaff = dataHelper.getIDOfStaffUpline((int)relatedStaffID);
            else
            {
                if (actionByStaff != null)
                    newException.CurrentActionByStaff = (int)actionByStaff;
                else
                    newException.CurrentActionByStaff = 0;
            }
            newException.CurrentActionByDate = DateTime.Now + TimeSpan.FromHours(dataHelper.getGlobalSetting_HoursBetweenExceptionEscallation());
            db2.Exceptions.Add(newException);
            db2.SaveChanges();

            ExceptionHistory newExceptionHistory = new ExceptionHistory();

            newExceptionHistory.ParentID = newException.ID;
            newExceptionHistory.ActionDescription = "Exception created.  Awaiting upline review.";
            newExceptionHistory.ActionDate = DateTime.Now;
            newExceptionHistory.ActionStaffID = newException.CurrentActionByStaff;
            newExceptionHistory.EscallationID = 0;
            newExceptionHistory.stateAtHistoryRecordCreation = exceptionState.Created;

            db2.ExceptionHistory.Add(newExceptionHistory);
            db2.SaveChanges();

            return newException.ID;
        }

        private bool isStaffQualifiedToWorkWithClient(RosterModel r, ref string failureReason)
        {
            Data dataHelper = new Data();
            bool first = true;
            bool qualified = true;

            failureReason = r.StaffFirstName + " " + r.StaffLastName + " does not have the following qualifications required to work with '" + r.ClientFirstName + " " + r.ClientLastName + "' : '";

            int staffID = dataHelper.getStaffIDFromName(r.StaffFirstName, r.StaffLastName);
            int clientID = dataHelper.getClientIDFromName(r.ClientFirstName, r.ClientLastName);

            var staffQuals = db.StaffQualifications.Where(p => p.StaffID == staffID);
            var clientQuals = db.ClientRequiredQualifications.Where(p => p.ClientID == clientID);

            foreach (ClientRequiredQualification qual in clientQuals)
            {
                var staffQual = staffQuals.Where(sq => sq.QualificationID == qual.QualificationID).FirstOrDefault();

                if (staffQual == null)
                {
                    if (first)
                    {
                        first = false;
                        failureReason += dataHelper.getQualificationName(qual.QualificationID) + "'";
                    }
                    else
                        failureReason += ", '" + dataHelper.getQualificationName(qual.QualificationID) + "'";
                    qualified = false;
                }
                else
                    if (staffQual.expired == true)
                {
                    if (first)
                    {
                        first = false;
                        failureReason += dataHelper.getQualificationName(qual.QualificationID) + "' (expired)";
                    }
                    else
                        failureReason += ", '" + dataHelper.getQualificationName(qual.QualificationID) + "' (expired)";
                    qualified = false;

                }
            }

            failureReason += " Shift Start " + r.Start.ToString("dd/MM/yyyy HH:mm tt");
            return qualified;
        }

        private bool isStaffQualifiedToWorkAtLocation(RosterModel r, ref string failureReason)
        {
            Data dataHelper = new Data();
            bool first = true;
            bool qualified = true;

            failureReason = r.StaffFirstName + " " + r.StaffLastName + " does not have the following qualifications required to work at '" + r.SiteName + "' : '";

            int staffID = dataHelper.getStaffIDFromName(r.StaffFirstName, r.StaffLastName);
            int locationID = dataHelper.getLocationIDFromName(r.SiteName);

            var staffQuals = db.StaffQualifications.Where(p => p.StaffID == staffID);
            var locationQuals = db.LocationRequiredQualifications.Where(p => p.LocationID == locationID);

            foreach (LocationRequiredQualification qual in locationQuals)
            {
                var staffQual = staffQuals.Where(sq => sq.QualificationID == qual.QualificationID).FirstOrDefault();

                if (staffQual == null)
                {
                    if (first)
                    {
                        first = false;
                        failureReason += dataHelper.getQualificationName(qual.QualificationID) + "'";
                    }
                    else
                        failureReason += ", '" + dataHelper.getQualificationName(qual.QualificationID) + "'";
                    qualified = false;
                }
                else
                    if (staffQual.expired == true)
                {
                    if (first)
                    {
                        first = false;
                        failureReason += dataHelper.getQualificationName(qual.QualificationID) + "' (expired)";
                    }
                    else
                        failureReason += ", '" + dataHelper.getQualificationName(qual.QualificationID) + "' (expired)";
                    qualified = false;

                }
            }

            failureReason += " Shift Start " + r.Start.ToString("dd/MM/yyyy HH:mm tt");
            return qualified;
        }
    }
}