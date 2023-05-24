using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.Entidades
{
    public class Habitacion : IId
    {
        public int Id { get; set; }
        [Required]
        public int NumHab { get; set; } 
        [Required]
        public string Tipo { get; set; }
        [StringLength(50)]
        public string Obs { get; set; }


        public List<Reservacion> Reservaciones { get; set; } // sacar hacia una interface

    }
}
