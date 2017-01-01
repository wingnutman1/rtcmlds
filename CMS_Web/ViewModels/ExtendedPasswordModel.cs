using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS_Web.Models;

namespace CMS_Web.ViewModels
{
    public class ExtendedPasswordModel
    {
        public LocalPasswordModel passwordModel { get; set; }
        public string userName { get; set; }
        public int userID { get; set; }
    }
}