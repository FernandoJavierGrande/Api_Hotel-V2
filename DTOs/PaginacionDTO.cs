namespace Api_Hotel_V2.DTOs
{
    public class PaginacionDTO
    {
        // contiene la pagina en la que se encuentra el usuario y la cantidad de registros por pagina
        public int Pagina { get; set; } = 1;

        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int CantidadRegistrosPorPagina
        {
            get => cantidadRegistrosPorPagina;
            set { cantidadRegistrosPorPagina = (value > cantidadMaximaRegistrosPorPagina) ? cantidadMaximaRegistrosPorPagina: value ; }
        }
    }
}
