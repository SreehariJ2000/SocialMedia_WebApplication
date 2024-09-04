using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaWeb.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } 

        public string ProfilePicture { get; set; }

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
