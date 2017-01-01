using System;

namespace CMS_Web.Models
{

    public enum exceptionType
    {
        Qualifications,
        ShiftTiming,
        ToDo,
        Incident,
        OperationalPerformance
    }

    public enum exceptionState
    {
        Created,
        Escalated,
        Closed
    }

    public class ExceptionDetail
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CurrentActionByDate { get; set; }
        public int CurrentActionByStaff { get; set; }
        public bool Active { get; set; }
        public exceptionType ExceptionType { get; set; }
        public int? relatedIncidentID { get; set; }
        public int? EscalationParentID { get; set; }
        public int? EscalationChildID { get; set; }
        public exceptionState state { get; set; }
        public int? relatedStaffID {get; set;}
        public int? relatedClientID { get; set; }
        public int? realtedLocationID { get; set; }


    }
}