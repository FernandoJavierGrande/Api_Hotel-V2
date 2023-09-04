using System.ComponentModel.DataAnnotations;

namespace Api_Hotel_V2.DTOs.AfiliadoDTOs
{
    public class FiltroAfiliadosDTO
    {
        public int Pagina { get; set; } = 1;

        public int CantidadRegistrosPorPagina { get; set; } = 10;

        public PaginacionDTO Paginacion 
        {
            get 
            { 
                return new PaginacionDTO() 
                {
                    Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistrosPorPagina 
                };
            }
        }
        [RegularExpression("(^[0-9]{10,11}$)", ErrorMessage = "Solo se permiten números. verifique la cantidad de caracteres")]
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string NumAfiliado { get; set; }

    }
}