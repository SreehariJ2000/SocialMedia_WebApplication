using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SocialMediaWeb.Models
{
    public class Signup
    {
        public int UserID { get; set; }
        
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

     
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }

      
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

       
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "State")]
        public int StateID { get; set; }

       
        [Display(Name = "District")]
        public int DistrictID { get; set; }
    }
}
