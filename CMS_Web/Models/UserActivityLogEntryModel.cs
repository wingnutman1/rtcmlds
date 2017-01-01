using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class UserActivityLogEntry
    {
        public int ID { get; set; }
        public DateTime EventDate { get; set; }
        public int StaffID { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}