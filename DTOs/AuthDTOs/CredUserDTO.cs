using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.Auth
{
    public class CredUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string role { get; set; }
    }
}
