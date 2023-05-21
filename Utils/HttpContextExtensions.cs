using Microsoft.EntityFrameworkCore;

namespace Api_Hotel_V2.Utils
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParamPaginacion<T>(this HttpContext httpContext, IQueryable<T> queryable,int cantidadRegPorPag)
        {   //obtiene la cantidad total de registros que hay como resultado de la consulta
            double cantidad = await queryable.CountAsync(); 
            
            // devuelve la cantidad de paginas que va haber en funcion de la cantidad de resgistros que se "pidio" por param 
            double cantidadDePaginas = Math.Ceiling(cantidad / cantidadRegPorPag);

            //agrega al res la el resultado
            httpContext.Response.Headers.Add("cantidadDePaginas", cantidadDePaginas.ToString());
        }
    } 
}
