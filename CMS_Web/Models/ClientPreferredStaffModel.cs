using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class ClientPreferredStaff
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int StaffID { get; set; }
    }
}