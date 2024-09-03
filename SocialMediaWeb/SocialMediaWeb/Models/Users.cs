using System.ComponentModel.DataAnnotations;

namespace SocialMediaWeb.Models
{
    public class Users
    {
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserPassword { get; set; }

        
    }
}
