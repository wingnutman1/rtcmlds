using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class ActivityLogEntry
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public System.DateTime Date { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public string RelatedTableName { get; set; }
        public Nullable<int> RelatedRecordID { get; set; }

        public virtual UserProfile UserProfile { get; set; }

    }
}