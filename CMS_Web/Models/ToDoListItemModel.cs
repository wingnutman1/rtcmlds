using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class ToDoListItem
    {
        public int ID { get; set; }
        public int StaffID { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public bool Complete { get; set; }
        public bool BumpedOut { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public System.String CompletedNote { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> BumpOutDate { get; set; }
        public string BumpOutNote { get; set; }
        public Nullable<int> RelatedLocationID { get; set; }
        public Nullable<int> RelatedClientID { get; set; }
        public Nullable<System.DateTime> BumpInDate { get; set; }
        public string BumpInNote { get; set; }
        public Nullable<int> BumpInID { get; set; }
        public bool BumpOutWithoutAuthorisation { get; set; }
        public Nullable<int> AuthorisingStaffID { get; set; }
        public bool AwaitingAuthorisation { get; set; }
        public System.DateTime RequiredCompletionBy { get; set; }
        public string RelatedTo { get; set; }
        public bool itemNotCompleteInAllocatedTimeExceptionRaised { get; set; }
        public int itemNotCompleteInAllocatedTimeExceptionID { get; set; }
        public bool itemBumpedOutExceptionRaised { get; set; }
        public int itemBumpedOutExceptionID { get; set; }


        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile UserProfile1 { get; set; }

        public ToDoListItem()
        {
            itemBumpedOutExceptionID = 0;
            itemBumpedOutExceptionRaised = false;
            itemNotCompleteInAllocatedTimeExceptionID = 0;
            itemNotCompleteInAllocatedTimeExceptionRaised = false;
            Deleted = false;
        }

    }
}