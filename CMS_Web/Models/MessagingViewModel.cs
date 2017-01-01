using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Models
{
    public class MessagingViewModel
    {
        public IEnumerable<string> SelectedUsers { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public string newMessage { get; set; }
        public MessageList displayMessages { get; set; }
        public string newStr { get; set; }
        public string selectedUserName { get; set; }
    }

    public class MessagingViewMessage
    {
        public string senderFullName { get; set; }
        public int senderID { get; set; }
        public string recieverFullName { get; set; }
        public int recieverID { get; set; }
        public DateTime sendDate { get; set; }
        public string messageBody { get; set; }
        public Boolean messageRead { get; set; }
        public int messageID { get; set; }
    }

    public class MessageList
    {
        public List<MessagingViewMessage> Messages { get; set; }
        public String selectedUser { get; set; }
    }

}