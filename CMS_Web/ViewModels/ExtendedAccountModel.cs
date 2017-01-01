using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS_Web.Models;

namespace CMS_Web.ViewModels
{
    public class ExtendedAccountModel
    {
        public string[] roles { get; set; }
        public UserProfile user { get; set; }
    }
}