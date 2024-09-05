﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaWeb.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageType { get; set; }
    }


    // Handle data in post form
    public class PostViewModel
    {
        public string Content { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }

    //display post
    public class PostDisplayViewModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
    }

    //report the post
    public class ReportPostViewModel
    {
        public int PostId { get; set; }          
        public string Content { get; set; }      
        public string ImageUrl { get; set; }     
        public string Reason { get; set; }      
        public int ReportedBy { get; set; }      
    }


}