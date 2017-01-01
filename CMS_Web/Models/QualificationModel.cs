using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class Qualification
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MonthsValidFor { get; set; }
        public string renewalActionMessage { get; set; }
    }
}