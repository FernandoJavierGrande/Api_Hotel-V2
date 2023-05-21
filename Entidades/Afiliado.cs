using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.Entidades
{
    public class Afiliado : Persona
    {

        public string NumAfiliado { get; set; }
        public List<Reserva> Reservas { get; set; }
    }
}
