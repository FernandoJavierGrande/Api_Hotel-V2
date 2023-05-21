using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.Entidades
{
    public class Persona: IId
    {
        public int Id { get; set; }
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
