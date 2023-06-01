using Api_Hotel_V2.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.EntidadesDTOs
{
    public class AfiliadoDTO 
    {
        public int Id { get; set; }
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NumAfiliado { get; set; }
        public List<Reserva> Reservas { get; set; }
    }
}
