using Api_Hotel_V2.DTOs;

namespace Api_Hotel_V2.Utils
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T> (this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.CantidadRegistrosPorPagina)
                .Take(paginacionDTO.CantidadRegistrosPorPagina);
        }
    }
}
