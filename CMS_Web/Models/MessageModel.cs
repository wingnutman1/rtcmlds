using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class Message
    {
        public int ID { get; set; }
        public int SenderStaff { get; set; }
        public int RecipientStaff { get; set; }
        public System.DateTime SendDate { get; set; }
        public Nullable<System.DateTime> ReadDate { get; set; }
        public string MessageText { get; set; }
        public bool MessageRead { get; set; }
        public Nullable<int> TimeToRead { get; set; }
        public bool ReadTimeOut { get; set; }
        public bool MessageDeleted { get; set; }
        public Nullable<System.DateTime> MessageDeleteDate { get; set; }

        public virtual UserProfile UserProfile { get; set; }

    }
}