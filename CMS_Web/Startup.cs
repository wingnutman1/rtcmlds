
using Microsoft.Owin;
using Owin;
using CMS_Web;

[assembly: OwinStartup(typeof(CMS_Web.Startup))]
namespace CMS_Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CMS_Web.Helpers.Data dataHelper = new Helpers.Data();

            dataHelper.addLog("System Startup", Models.systemLogType.Admin, "Startup", "Configuration", null);

            app.MapSignalR();


        }
    }
}