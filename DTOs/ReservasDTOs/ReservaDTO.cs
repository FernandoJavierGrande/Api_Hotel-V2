using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class ReservaDTO
    {
        [Column(TypeName = "date")]
        public DateTime Inicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime Fin { get; set; }
        public int Id { get; set; }
        
        public int AfiliadoId { get; set; }

        public string NumAfiliado { get; set; }

        public string EstadoPago { get; set; }
        
        public bool Activa { get; set; }
        [StringLength(200)]
        public string Obs { get; set; }
        public string Acompaniantes { get; set; }

        
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }//ver 
    }
}
