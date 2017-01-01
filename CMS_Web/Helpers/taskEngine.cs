using Quartz;
using System.Data;
using CMS_Web.DAL;
using System.Data.Entity;
using Quartz.Impl;
using CMS_Web.Models;
using System;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Web.ViewModels;
using WebMatrix.WebData;
using System.Text;
using System.IO;

namespace CMS_Web.Helpers
{
    public class taskEngine : IJob
    {
        private CMS_WebContext db = new CMS_WebContext();
        private exceptionEngine ee = new exceptionEngine();

        public void Execute(IJobExecutionContext context)
        {
            System.Diagnostics.Debugger.Launch();

            CMS_Web.Models.SystemStatus statusRec = db.SystemStatusRecord.Find(1);
            CMS_Web.Models.SystemStatus newRec = new Models.SystemStatus();

            if (statusRec == null)
            {
                newRec.exceptionEngineEnabled = false;
                newRec.exceptionEngineStatus = "Created at : " + System.DateTime.Now.ToString();
                newRec.incidentEngineEnabled = false;
                newRec.incidentEngineStatus = "Created at : " + System.DateTime.Now.ToString();
                newRec.todoEngineEnabled = false;
                newRec.todoEngineStatus = "Created at : " + System.DateTime.Now.ToString();
                db.SystemStatusRecord.Add(newRec);
                db.SaveChanges();    
            }
            else
            {
                if (statusRec.exceptionEngineEnabled)
                {
                    //ee.processRosterExceptions();
                    statusRec.exceptionEngineStatus = "Last execution : " + System.DateTime.Now.ToString();
                }

                if (statusRec.incidentEngineEnabled)
                    statusRec.incidentEngineStatus = "Last execution : " + System.DateTime.Now.ToString();

                if (statusRec.todoEngineEnabled)
                    statusRec.todoEngineStatus = "Last execution : " + System.DateTime.Now.ToString();

                db.Entry(statusRec).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

        }

        

    }

    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<taskEngine>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("mainScheduler","main")
                .StartNow()
                .WithSimpleSchedule( x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }

   


}