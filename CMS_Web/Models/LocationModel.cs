using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class Location
    {

        public Location()
        {
            this.JournalEntries = new HashSet<JournalEntry>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public string Note { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public Nullable<decimal> Geofence_Radius { get; set; }

        public virtual ICollection<JournalEntry> JournalEntries { get; set; }

    }
}