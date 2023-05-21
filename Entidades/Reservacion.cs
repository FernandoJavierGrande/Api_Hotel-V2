using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.Entidades
{
    public class Reservacion
    {
        [Required]
        public int HabitacionId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        [Required]
        public int ReservaId { get; set; }
    }
}
