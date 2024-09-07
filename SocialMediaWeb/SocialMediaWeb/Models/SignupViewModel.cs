using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SocialMediaWeb.Models
{
    public class Signup
    {
        public int UserID { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Profile picture")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateID { get; set; }

        [Required]
        [Display(Name = "District")]
        public int DistrictID { get; set; }
    }
}
