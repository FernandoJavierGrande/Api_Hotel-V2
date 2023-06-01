using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class PatchReservaEstadoDTO
    {
        [Required]
        public string EstadoPago { get; set; }
    }
}
