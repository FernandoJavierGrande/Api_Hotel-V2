using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.Entidades
{
    [Index(nameof(NumAfiliado), Name = "UqNumAfiliado", IsUnique = true)]
    public class Afiliado : Persona
    {
        [Required]
        public string NumAfiliado { get; set; }
        public List<Reserva> Reservas { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
