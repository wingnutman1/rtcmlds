using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class JournalEntryType
    {
        public JournalEntryType()
        {
            this.JournalEntries = new HashSet<JournalEntry>();
        }
    
        public int ID { get; set; }
        public string Type { get; set; }

        public virtual ICollection<JournalEntry> JournalEntries { get; set; }


    }
}