using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class ToDoListRecurringItem
    {
        public int ID { get; set; }
        public Nullable<int> SiteID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> StaffID { get; set; }
        public string AutoGenerateEvent { get; set; }
        public string Task { get; set; }
        public Nullable<DateTime> TimeToComplete { get; set; }
        public Nullable<DayOfWeek> DayOfWeek { get; set; }
    }
}