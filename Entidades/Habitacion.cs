using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Hotel_V2.Entidades
{
    public class Habitacion : IId
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Tipo { get; set; }
        [StringLength(50)]
        public string Obs { get; set; }


        public List<Reservacion> Reservaciones { get; set; } 

    }
}
