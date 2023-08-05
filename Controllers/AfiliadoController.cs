using Api_Hotel_V2.DTOs;
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
        public async Task<ActionResult<List<AfiliadoDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Afiliados.AsQueryable();
            await HttpContext.InsertarParamPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);

            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<AfiliadoDTO>>(entidades);
                 
            //return await Get<Afiliado, AfiliadoDTO>();
        }
        [HttpGet("{id:int}" , Name =  "GetAfiliadoById")]
        public async Task<ActionResult<AfiliadoDTO>> Get(int id)
        {
            return await Get<Afiliado,AfiliadoDTO>(id);
        }
        [HttpGet("cuil/{Cuil}")]
        public async Task<ActionResult<AfiliadoDTO>> Get(string Cuil)
        {
            try
            {
                var afiliado = await context.Afiliados.FirstOrDefaultAsync(a => a.Cuil == Cuil);

                if (afiliado == null)
                {
                    return NotFound();
                }

                return mapper.Map<AfiliadoDTO>(afiliado);
            }
            catch (Exception) {throw;}
        }
        [HttpGet("numeroAfiliado/{numAfiliado}")]
        public async Task<ActionResult<AfiliadoDTO>> GetNumAf(string numAfiliado)
        {
            try
            {
                var afiliado = await context.Afiliados.FirstOrDefaultAsync(a => a.NumAfiliado == numAfiliado);

                if (afiliado == null)
                {
                    return NotFound();
                }

                return mapper.Map<AfiliadoDTO>(afiliado);
            }
            catch (Exception) { throw; }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AfiliadoCreacionDTO afiliadoCreacionDTO)
        {
            try
            {
                var exist = await context.Afiliados.AnyAsync(a =>a.NumAfiliado ==afiliadoCreacionDTO.NumAfiliado || a.Cuil == afiliadoCreacionDTO.Cuil);

                if (exist)
                {
                    return BadRequest("Cuil o Numero de afiliado existente");
                }

                return await Post<AfiliadoCreacionDTO, Afiliado, AfiliadoDTO>(afiliadoCreacionDTO, "GetAfiliadoById");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,[FromBody] AfiliadoCreacionDTO afiliadoCreacionDTO)
        {
            try
            {
                return await Put<AfiliadoCreacionDTO, Afiliado>(id, afiliadoCreacionDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                return await Delete<Afiliado>(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
