using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.EntidadesDTOs
{
    public class AfiliadoCreacionDTO
    {
        [Required]
        public string Cuil { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
