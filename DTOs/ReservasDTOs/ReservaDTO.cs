namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class ReservaDTO 
    {
        public DateTime Inicio { get; set; }
        
        public DateTime Fin { get; set; }
        public int Id { get; set; }
        public int NumAfiliado { get; set; }
        public int AfiliadoId { get; set; }
        public string EstadoPago { get; set; }
        public bool Activa { get; set; }
        public string Obs { get; set; }
        public string Acompaniantes { get; set; }
        public string UsuarioId { get; set; }
        public DateTime? fechaDeCreacion { get; set; }


    }
}
