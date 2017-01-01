using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CMS_Web.ViewModels
{
    public class ClientSiteAndStaffViewModel
    {
        public DbSet allLocations { get; set; }
        public DbSet allStaff { get; set; }
        public int[] selectedLocations { get; set; }
        public int[] selectedStaff { get; set; }

    }
}