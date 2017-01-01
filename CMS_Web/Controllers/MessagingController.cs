using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Web.Models;
using CMS_Web.DAL;
using WebMatrix.WebData;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;
using Microsoft.AspNet.SignalR;

namespace CMS_Web.Controllers
{
    public class MessagingController : Controller
    {
        private CMS_WebContext db = new CMS_WebContext();

        //
        // GET: /Messaging/

        [HttpGet]
        public ActionResult Index()
        {
            MessagingViewModel messagesViewModel = new MessagingViewModel();


            int UserID = WebSecurity.CurrentUserId;
            var userRecord = (from row in db.UserProfiles where row.UserId == UserID select row).FirstOrDefault();
            string currentUserName = "";

            currentUserName = userRecord.FirstName + " " + userRecord.LastName;

            List<SelectListItem> listSelectListItems = new List<SelectListItem>();

            foreach (UserProfile user in db.UserProfiles)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = user.FirstName + " " + user.LastName,
                    Value = user.UserId.ToString()
                };

                listSelectListItems.Add(selectList);

            }

            messagesViewModel.Users = listSelectListItems;

            return View(messagesViewModel);

        }



        [HttpPost]
        public PartialViewResult Index(int[] selectedUsers, string command, string newMessage)
        {
            Helpers.Data dataHelper = new Helpers.Data();

            HttpRequestBase r = Request;

            int selectedUser = selectedUsers[0];            

            if (newMessage != null && newMessage != "" && command == "Send")
            {
                Message newMessageRecord = new Message();

                int recipientId = Convert.ToInt16(selectedUser);
                newMessageRecord.RecipientStaff = recipientId;
                newMessageRecord.SenderStaff = WebSecurity.CurrentUserId;
                newMessageRecord.SendDate = DateTime.Now;
                newMessageRecord.TimeToRead = 5;
                newMessageRecord.MessageText = newMessage;
                db.Messages.Add(newMessageRecord);
                db.SaveChanges();

                messageHub hub = new messageHub();
               
                hub.SendPMTo(recipientId.ToString(), "Message from : " + dataHelper.getStaffFullName(WebSecurity.CurrentUserId) + "<p>" +  newMessage + "</p>");
            }

            return messages(selectedUser);

        }

        public PartialViewResult MessageRead(int messageID, int senderID)
        {
            
            Message m = db.Messages.Find(messageID);

            m.MessageRead = true;
            m.ReadDate = DateTime.Now;

            db.SaveChanges();

            return messages(senderID);

        }

        public PartialViewResult MessageDelete(int messageID, int recipientID)
        {

            Message m = db.Messages.Find(messageID);

            m.MessageDeleted = true;
            m.MessageDeleteDate = DateTime.Now;

            db.SaveChanges();

            return messages(recipientID);

        }

        private PartialViewResult messages(int selectedUser)
        {

             MessagingViewModel messagesViewModel = new MessagingViewModel();

            int UserID = WebSecurity.CurrentUserId;
            var userRecord = (from row in db.UserProfiles where row.UserId == UserID select row).FirstOrDefault();
            string currentUserName = "";

            currentUserName = userRecord.FirstName + " " + userRecord.LastName;

            //List<SelectListItem> listSelectListItems = new List<SelectListItem>();

            //foreach (UserProfile user in db.USERPROFILEs)
            //{
            //    SelectListItem selectList = new SelectListItem()
            //    {
            //        Text = user.FirstName + " " + user.LastName,
            //        Value = user.UserId.ToString()
            //    };

            //    listSelectListItems.Add(selectList);

            //}

            if (selectedUser != 0)
            {

                int currentRecieveUserID = Convert.ToInt32(selectedUser);
                
                if (selectedUser != 0)
                {
                    var reciveUserRecord = (from row in db.UserProfiles where row.UserId == currentRecieveUserID select row).FirstOrDefault();
                    messagesViewModel.selectedUserName = reciveUserRecord.FirstName + " " + reciveUserRecord.LastName;
                }



                messagesViewModel.displayMessages = new MessageList();

                var unorderedMessages = from row in db.Messages where ((row.RecipientStaff == UserID && row.SenderStaff == currentRecieveUserID) || (row.RecipientStaff == currentRecieveUserID && row.SenderStaff == UserID)) && !row.MessageDeleted orderby row.SendDate select row;
                var messages = unorderedMessages.OrderByDescending(s => s.SendDate);

                messagesViewModel.displayMessages.Messages = new List<MessagingViewMessage>();

                foreach (var m in messages)
                {
                    MessagingViewMessage newMessageView = new MessagingViewMessage();

                    if (m.ReadDate == null)
                        newMessageView.messageRead = false;
                    else
                        newMessageView.messageRead = true;

                    newMessageView.messageBody = m.MessageText;
                    newMessageView.sendDate = m.SendDate;

                    var sendUserRecord = (from row in db.UserProfiles where row.UserId == m.SenderStaff select row).FirstOrDefault();
                    newMessageView.senderFullName = sendUserRecord.FirstName;
                    newMessageView.senderID = m.SenderStaff;

                    var recipientUserRecord = (from row in db.UserProfiles where row.UserId == m.RecipientStaff select row).FirstOrDefault();
                    newMessageView.recieverFullName = recipientUserRecord.FirstName + " " + recipientUserRecord.LastName;
                    newMessageView.recieverID = m.RecipientStaff;

                    newMessageView.messageID = m.ID;

                    messagesViewModel.displayMessages.Messages.Add(newMessageView);

                }
            }

            return PartialView("_Messages", messagesViewModel.displayMessages);

        }


        public ActionResult getStaff([DataSourceRequest]DataSourceRequest request)
        {

            var staff = new CMS_WebContext().UserProfiles;

            DataTable Users = new DataTable();
            Users.Columns.Add("UserID");
            Users.Columns.Add("FirstName");
            Users.Columns.Add("LastName");
            Users.Columns.Add("UnreadMessages");
            Users.Columns.Add("Online");


            foreach (var staffMember in db.UserProfiles)
            {
                if (staffMember.UserId != WebSecurity.CurrentUserId)
                {
                    var count = db.Messages
                        .AsEnumerable()
                        .Where(p => p.SenderStaff == staffMember.UserId && p.RecipientStaff == WebSecurity.CurrentUserId && !p.MessageRead && !p.MessageDeleted).Count();

                    var onLine = "offline";

                    if (staffMember.SignalRID != null)
                        onLine = "online";

                    Users.Rows.Add(staffMember.UserId, staffMember.FirstName, staffMember.LastName, count, onLine);
                }
            }

            DataView dv = Users.DefaultView;
            dv.Sort = "UnreadMessages DESC";
            DataTable sorted = dv.ToTable();

            DataSourceResult result = sorted.ToDataSourceResult(request);

            return Json(result);

        }
    }
}