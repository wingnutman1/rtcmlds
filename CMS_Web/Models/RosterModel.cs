using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using System.Data.Entity;

namespace CMS_Web.Models
{

    public class RosterModel : ISchedulerEvent  
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public string Recurrence { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public string EndTimezone { get; set; }
        public string StartTimezone { get; set; }
        public string StaffFullName { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string ClientFullName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string SiteName { get; set; }
        public int? StaffID { get; set; }
        public int? ClientID { get; set; }
        public int? SiteID { get; set; }
        public DateTime? ActualArrivalTime { get; set; }
        public DateTime? ActualLeaveTime { get; set; }
        public bool notOnlineBeforeShiftExceptionGenerated { get; set; }
        public bool notArrivedToStartShiftExceptionGenerated { get; set; }
        public bool offlineDuringShiftExceptionGenerated { get; set; }
        public bool leaveBeforeShiftCompleteExceptionGenerated { get; set; }
        public bool qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated { get; set; }
        public bool qualificationsNotCurrentWhenArrivingAtShiftExceptionGenerated { get; set; }
        public bool qualificationsNotCurrentWarningBeforeShiftExceptionGenerated { get; set; }
        public List<int> exceptionID { get; set; }
        public bool Deleted { get; set; }
        public bool CancelledByStaff { get; set; } 

        public RosterModel()
        {
            notOnlineBeforeShiftExceptionGenerated = false;
            notArrivedToStartShiftExceptionGenerated = false;
            offlineDuringShiftExceptionGenerated = false;
            leaveBeforeShiftCompleteExceptionGenerated = false;
            qualificationsNotCurrentWhenArrivingAtShiftExceptionGenerated = false;
            qualificationsNotCurrentWarningBeforeShiftExceptionGenerated = false;
            qualificationsNotCurrentWhenShiftAllocatedExceptionGenerated = false;
            exceptionID = new List<int>();
            Deleted = false;
            CancelledByStaff = false;
        }

    }

    public class LDSRosterEntry
    {
        public int ID { get; set; }
        public String StaffFullName { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public String ClientName { get; set; }
        public String SiteName { get; set; }
        public bool? Deleted { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }


    }

    public class LDS_RosterContext : DbContext
    {
        public DbSet<LDSRosterEntry> Roster { get; set; }
    }



}