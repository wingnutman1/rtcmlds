using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class JournalEntry
    {
        public int ID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> StaffID { get; set; }
        public Nullable<int> lastActionStaffID { get; set; }
        public System.DateTime lastActionDate { get; set; }
        public string Note { get; set; }
        public System.DateTime creationDate { get; set; }

        public virtual JournalEntryType JournalEntryType { get; set; }

    }
}