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
        public int isActive { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [Display(Name = "State")]
        public string StateName { get; set; }

        [Display(Name = "District")]
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


    //Change password
    public class ChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


}
