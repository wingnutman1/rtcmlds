using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
   

    public enum escallationState
    {
        Unescallated,
        FirstEscallation,
        SecondEscallation,
        ThirdEscallation,
        FourthEscallation
    }

    public class ExceptionHistory
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int ActionStaffID { get; set; }
        public DateTime ActionDate { get; set; }
        public String ActionDescription { get; set; }
        public int EscallationID { get; set; }
        public exceptionState stateAtHistoryRecordCreation { get; set; }
    }
}