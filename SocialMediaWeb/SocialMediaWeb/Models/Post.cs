using System;
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
    public class ReportPost
    {
        public int PostId { get; set; }          
        public string Content { get; set; }      
        public string ImageUrl { get; set; }     
        public string Reason { get; set; }      
        public int ReportedBy { get; set; }      
    }

    //get the reported post to admin
    public class ReportedPostViewModel
    {
        public int ReportId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Reason { get; set; }
    }

    public class FriendsProfile
    {
        public ProfileViewModel Profile { get; set; }
        public List<Post> Posts { get; set; }
        public bool IsFollower { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }




}