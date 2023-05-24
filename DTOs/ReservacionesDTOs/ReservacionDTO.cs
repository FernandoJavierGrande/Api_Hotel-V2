using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.ReservacionesDTOs
{
    public class ReservacionDTO
    {
        public int HabitacionId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        
        public int ReservaId { get; set; }
    }
}
