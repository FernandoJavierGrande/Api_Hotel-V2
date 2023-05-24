using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class ReservaDTO
    {
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
        public IdentityUser Usuario { get; set; }//ver 
    }
}
