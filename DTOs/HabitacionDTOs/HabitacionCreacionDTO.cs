using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.HabitacionDTOs
{
    public class HabitacionCreacionDTO
    {
        [Required]
        public int NumHab { get; set; }
        [Required]
        public string Tipo { get; set; }
        [StringLength(50)]
        public string Obs { get; set; }
    }
}
