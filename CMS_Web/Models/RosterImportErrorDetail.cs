﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class RosterImportErrorDetail
    {
        public int ID { get; set; }
        public int StaffID { get; set; }
        public DateTime ImportDate { get; set; }
        public String ErrorDetail { get; set; }
    }
}