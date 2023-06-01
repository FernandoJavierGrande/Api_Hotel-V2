using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Hotel_V2.Entidades
{
    public class Reserva : IId
    {
        [Required(ErrorMessage = "Las fechas son obligatorias")]
        [Column(TypeName = "date")]
        public DateTime Inicio { get; set; }
        [Required(ErrorMessage = "Las fechas son obligatorias")]
        [Column(TypeName = "date")]
        public DateTime Fin { get; set; }
        public int Id { get; set; }
        [Required]
        public int AfiliadoId { get; set; }
        [Required]
        public string EstadoPago { get; set; }
        [Required]
        public bool Activa { get; set; }
        [StringLength(200)]
        public string Obs { get; set; }
        public string Acompaniantes { get; set; }

        [Required]
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

        public DateTime? fechaDeCreacion { get; set; }
        
        public Afiliado Afiliado { get; set; }
        public List<Reservacion> Reservaciones { get; set; }

    }
}
