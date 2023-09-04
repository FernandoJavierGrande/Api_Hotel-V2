using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.AfiliadoDTOs;
using Api_Hotel_V2.DTOs.EntidadesDTOs;
using Api_Hotel_V2.Entidades;
using Api_Hotel_V2.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Hotel_V2.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/afiliados")]
    public class AfiliadoController: CustomBaseController
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public AfiliadoController(Context context, IMapper mapper): base(context, mapper)   
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AfiliadoDTO>>> Get([FromQuery] FiltroAfiliadosDTO filtroAfiliadosDTO)
        {
            var queryable = context.Afiliados.AsQueryable(); 

            if (!String.IsNullOrEmpty(filtroAfiliadosDTO.Cuil))
            {
                queryable = queryable.Where(c => c.Cuil == filtroAfiliadosDTO.Cuil);
            }

            if (!String.IsNullOrEmpty(filtroAfiliadosDTO.NumAfiliado))
            {
                queryable = queryable.Where(n => n.NumAfiliado == filtroAfiliadosDTO.NumAfiliado);
            }

            if (!String.IsNullOrEmpty(filtroAfiliadosDTO.Apellido))
            {
                string apellido = filtroAfiliadosDTO.Apellido.Trim(); 

                string[] substring = filtroAfiliadosDTO.Apellido.Split(' ');

                queryable = (substring.Length >= 2) ? 
                    queryable.Where(a => a.Apellido.Contains(substring[0]) && a.Nombre.Contains(substring[1]))
                    : queryable = queryable.Where(a => a.Apellido.Contains(apellido) ||a.Nombre.Contains(apellido));

            }

            await HttpContext.InsertarParamPaginacion(queryable, filtroAfiliadosDTO.CantidadRegistrosPorPagina);

            var listaAfiliados = await queryable.Paginar(filtroAfiliadosDTO.Paginacion).ToListAsync();

            return mapper.Map<List<AfiliadoDTO>>(listaAfiliados);
        }

        [HttpGet("{id:int}" , Name =  "GetAfiliadoById")]
        public async Task<ActionResult<AfiliadoDTO>> Get(int id)
        {
            return await Get<Afiliado,AfiliadoDTO>(id);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AfiliadoCreacionDTO afiliadoCreacionDTO)
        {
            var exist = await context.Afiliados.AnyAsync(a => a.NumAfiliado == afiliadoCreacionDTO.NumAfiliado || a.Cuil == afiliadoCreacionDTO.Cuil);

            if (exist)
            {
                return BadRequest("Cuil o Numero de afiliado existente");
            }

            return await Post<AfiliadoCreacionDTO, Afiliado, AfiliadoDTO>(afiliadoCreacionDTO, "GetAfiliadoById");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,[FromBody] AfiliadoCreacionDTO afiliadoCreacionDTO)
        {
            return await Put<AfiliadoCreacionDTO, Afiliado>(id, afiliadoCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Afiliado>(id);
        }
    }
}
