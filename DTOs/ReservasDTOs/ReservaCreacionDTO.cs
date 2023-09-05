using Api_Hotel_V2.DTOs.ReservacionesDTOs;
using Api_Hotel_V2.Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class ReservaCreacionDTO
    {
        [Required]
        public int AfiliadoId { get; set; }
        [Required]
        public string EstadoPago { get; set; }
        [StringLength(200)]
        public string Obs { get; set; }
        public string Acompaniantes { get; set; }
        public bool Activa { get; set; } = true;
        public DateTime? fechaDeCreacion { get; set; } = DateTime.Now;
        [Required]
        [MinLength(1, ErrorMessage = "Reservation must have at least one day")]
        public List<ReservacionCreacionDTO> reservaciones { get; set; } 
    }
}
