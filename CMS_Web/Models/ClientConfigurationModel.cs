using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class ClientConfigurationModel
    {
        public int ID { get; set; }
        public string ClientName { get; set; }
        public System.DateTime LicenceExpiryDate { get; set; }
        public System.DateTime LicenceReminderDate { get; set; }
        public int MaxLogins { get; set; }
    }
}