using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaWeb.Models
{
    //insert 
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentedAt { get; set; }
    }


    //display the details of the post and user who post it
    public class CommentProfile
    {
        public int PostId { get; set; }
        public string PostContent { get; set; }
        public string PostImageUrl { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePicture { get; set; }
        
    }


    //get all comments of a particular post
    public class CommentData
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentedAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
    }


    public class ViewComments
    {
        public CommentProfile PostDetails { get; set; }
        public List<CommentData> Comments { get; set; }
    }


}