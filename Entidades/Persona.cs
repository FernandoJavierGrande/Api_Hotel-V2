using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.Entidades
{
    [Index(nameof(Cuil), Name = "UqCuil", IsUnique = true)]
    public class Persona: IId
    {
        public int Id { get; set; }
        [MinLength(10, ErrorMessage = "El numero no puede ser menor a 10 caracteres")]
        [MaxLength(11, ErrorMessage = "El numero no puede ser mayor a 11 caracteres")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required]
        public string Cuil { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }

    }
}
