using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public enum incidentState
    {
        Created,
        AwaitingManager1ACK,
        AwaitingManager2ACK,
        AwaitingManager3ACK,
        UploadFileDelete,
        UploadFileAdd,
        Closed
    }

    public class IncidentHistory
    {

        public int ID { get; set; }
        public int incidentID { get; set; }
        public DateTime historyEntryCreationDate { get; set; }
        public DateTime? actionByDate { get; set; }
        public incidentState state { get; set; }
        public string currentActionDescription { get; set; }
        public int? actionByStaffID { get; set; }
        public int currentStaffID { get; set; }
        public bool actionByDateExceededExceptionRaised { get; set; }
        public int actionByDateExceededExceptionID { get; set; }

    }
}