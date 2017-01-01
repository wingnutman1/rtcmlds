using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.ViewModels
{
    public class journalViewModel
    {
        public int ID { get; set; }
        public int locationID { get; set; }
        public int clientID { get; set; }
        public int typeID { get; set; }
        public int staffID { get; set; }
        public string note { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public int lastModifiedStaffID { get; set; }

    }
}