using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public enum systemLogType
    {
        User,
        Engine,
        Admin,
        Other
    }

    public class SystemLogEntry
    {
        public int ID { get; set; }
        public DateTime EventDate { get; set; }
        public int? StaffID { get; set; }
        public string Description { get; set; }
        public systemLogType LogType { get; set; }
        public string module { get; set; }
        public string function { get; set; }

    }
}