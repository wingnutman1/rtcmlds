using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class IncidentType
    {
        public int ID { get; set; }
        public string Type { get; set; }

        public string templateFileLocation { get; set; }

        public bool useOnCallProcessing { get; set; }
        public int? onCallCalendarID { get; set; }

        public int? firstStaffEscID { get; set; }
        public int? firstEscDays { get; set; }
        public int? firstEscHours { get; set; }
        public int? firstEscMinutes { get; set; }

        public int? secondStaffEscID { get; set; }
        public int? secondEscDays { get; set; }
        public int? secondEscHours { get; set; }
        public int? secondEscMinutes { get; set; }

        public int? thirdStaffEscID { get; set; }
        public int? thirdEscDays { get; set; }
        public int? thirdEscHours { get; set; }
        public int? thirdEscMinutes { get; set; }

    }
}