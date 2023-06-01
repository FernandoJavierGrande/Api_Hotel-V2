using Api_Hotel_V2.Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class ReservaCreacionDTO
    {
        [Required(ErrorMessage = "Las fechas son obligatorias")]
        [Column(TypeName = "date")]
        public DateTime Inicio { get; set; }
        [Required(ErrorMessage = "Las fechas son obligatorias")]
        [Column(TypeName = "date")]
        public DateTime Fin { get; set; }
        [Required]
        public int AfiliadoId { get; set; }
        [Required]
        public string EstadoPago { get; set; }
        [StringLength(200)]
        public string Obs { get; set; }
        public string Acompaniantes { get; set; }
        public DateTime? fechaDeCreacion { get; set; } = DateTime.Now; //updatedb
        public List<int> HabitacionesEnLaReserva { get; set; } 
    }
}
