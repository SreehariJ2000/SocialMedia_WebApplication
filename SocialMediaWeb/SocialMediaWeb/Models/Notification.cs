using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaWeb.Models
{
    public class Notification
    {
        public int  NotificationId {  get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageUrl { get; set; }
        public string messageTitle { get; set; }
    }
}