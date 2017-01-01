using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class ExceptionEscallationDetail
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int Position { set; get; } // Position in the escallation ladder
        public int NextStaffID { get; set; }
        public int HoursDelayBeforeEscallation { get; set; }
        public int MinutesDelayBeforeEscallation { get; set; }
        public TimeSpan DelayBeforeEscalateToNextStaff { get; set; }
        public String EscallationDetailDescription { get; set; }

    }
}
