using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.AuthDTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation don´t match")]
        public string ConfirmPass { get; set; }

        public string Token { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
