using Quartz;
using System.Data;
using CMS_Web.DAL;
using System.Data.Entity;

namespace CMS_Web.Helpers
{
    public class taskEngine : IJob
    {
        private CMS_WebContext db = new CMS_WebContext();

        public void Execute(IJobExecutionContext context)
        {
            CMS_Web.Models.SystemStatus statusRec = db.SystemStatusRecord.Find("ID=0");
             
            if(statusRec == null)
           db.SystemStatusRecord
                        
        }

    }
}