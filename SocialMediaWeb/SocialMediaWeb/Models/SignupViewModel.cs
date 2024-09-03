using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SocialMediaWeb.Models
{
    public class SignupViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } 

        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int StateID { get; set; }

        [Required]
        public int DistrictID { get; set; }
    }
}
