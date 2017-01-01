using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Models
{
    public class OnCallRoster
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string onCallStaffID { get; set; }
        public DateTime onCallDateStart { get; set; }
        public DateTime onCallDateEnd { get; set; }
    }
}
