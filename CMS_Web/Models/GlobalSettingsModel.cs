using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class GlobalSettingsModel
    {
        public int ID { get; set; } 
        public int daysBeforeShiftToCheckQualifications { get; set; }
        public int hoursBetweenExceptionEscallation { get; set; }
        public int daysBeforeQualificaitonExpiryForReminder { get; set; }
        public int minutesBeforeShiftToCheckIfStaffOnline { get; set; }
        public int minutesAllowedForLateShiftArrival { get; set; }
        public int minutesAllowedForEarlyShiftLeave { get; set; }
        public int minutesAllowedForStaffOfflineDuringShift { get; set; }
        public int numberOfDaysDurationPerformanceMetricAnalysis { get; set; }
        public int numberOfDaysDurationToDoPerformanceAnalysis { get; set; }
        public int minimumPercentToDoListCompletions { get; set; }
        public int minimumPercentOnTimeShiftArrivals { get; set; }
        public int maximumPercentEarlyShiftLeaving { get; set; }
        public int maximumPercentShiftCancellations { get; set; }

        public int staffIDForPerformanceReporting { get; set; }

        public bool todoRateUnderTarget { get; set; }
        public bool staffShiftArrivalsRateUnderTarget { get; set; }
        public bool staffShiftLeaveRateOverTarget { get; set; }
        public bool staffShiftCancelRateOverTarget { get; set; }

    }
}