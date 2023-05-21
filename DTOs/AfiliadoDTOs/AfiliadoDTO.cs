
using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.EntidadesDTOs
{
    public class AfiliadoDTO 
    {

        public int Id { get; set; }
        [Required]
        public string Cuil { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
