using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class StaffQualification
    {
        public int ID { get; set; }
        public int StaffID { get; set; }
        public int QualificationID { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool expired { get; set; }
        public bool expiryExceptionRaised { get; set; }
        public int expiryExceptionID { get; set; }
        public bool expiryWarningExceptionRaised { get; set; }
        public int expiryWarningExceptionID { get; set; }

        public StaffQualification()
        {
            expired = false;
            expiryExceptionRaised = false;
            expiryWarningExceptionRaised = false;
        }

    }

}