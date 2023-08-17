using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.AuthDTOs
{
    public class ChangePassDTO
    {
        [Required]
        public string MyProperty { get; set; }

    }
}
