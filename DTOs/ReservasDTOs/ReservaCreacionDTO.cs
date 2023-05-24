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
        public int AfiliadoId { get; set; }
        public string EstadoPago { get; set; }
        public bool Activa { get; set; }
        [StringLength(200)]
        public string Obs { get; set; }
        public string Acompaniantes { get; set; }
        public string UsuarioId { get; set; }
        //sirven para construir una reserva con reservaciones
        //multiples de forma automatica

        public List<int> HabitacionesEnLaReserva { get; set; } 
        public List<int> PaxPorHabitacion { get; set; }
    }
}
