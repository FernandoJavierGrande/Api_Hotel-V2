using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.AuthDTOs
{
    public class ChangePassDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
