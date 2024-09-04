using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaWeb.Models
{

    //for login
    public class Users
    {
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserPassword { get; set; }        
    }



    //  view profile of user
    public class ProfileViewModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }

        [Display(Name = "Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
    }

    public class UpdateProfileViewModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
    }

}
