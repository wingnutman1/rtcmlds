using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using CMS_Web.DAL;

namespace CMS_Web
{
    public class signalRMessage
    {
        public string messageType { get; set; }
        public string message { get; set; }

    }


    public class messageHub : Hub
    {
        private CMS_WebContext db = new CMS_WebContext();

        public void SendPMTo(string userID, string message)
        {

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<messageHub>();
            String signalRUID = signalRID(userID);

            signalRMessage sMessage = new signalRMessage();
            sMessage.messageType = "PM";
            sMessage.message = message;

            if (signalRUID != null)
                hubContext.Clients.Client(signalRUID).Send(sMessage);
          
        }

        public override System.Threading.Tasks.Task OnConnected()
        {

            var userName = Context.QueryString["userName"];
            var signalRID = Context.ConnectionId;
            
            updateUserLoggedInStatus(userName, true, signalRID);

            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {

            var userName = Context.QueryString["userName"];

            updateUserLoggedInStatus(userName, false, "");

            return base.OnDisconnected(stopCalled);
        }

        private void updateUserLoggedInStatus(String userName, bool loggedIn, String signalRID)
        {


            var userRecord = (from row in db.UserProfiles where row.UserName == userName select row).FirstOrDefault();

            if (userRecord != null)
            {
                if (loggedIn)
                {
                    userRecord.SignalRID = signalRID;
                    userRecord.LoggedOn = true;
                }
                else
                {
                    userRecord.SignalRID = null;
                    userRecord.LoggedOn = false;
                }

                db.SaveChanges();
            }

        }

        private String signalRID(String userID)
        {
            int UID = Convert.ToInt32(userID);

            var userRecord = (from row in db.UserProfiles where row.UserId == UID select row).FirstOrDefault();

            if (userRecord != null)
                return userRecord.SignalRID;
            else
                return null;

        }

       
    }
}