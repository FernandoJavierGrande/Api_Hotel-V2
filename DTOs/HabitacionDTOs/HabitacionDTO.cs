using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.HabitacionDTOs
{
    public class HabitacionDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        [StringLength(50)]
        public string Obs { get; set; }

    }
}