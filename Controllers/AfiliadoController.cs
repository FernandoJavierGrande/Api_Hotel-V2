using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.EntidadesDTOs;
using Api_Hotel_V2.Entidades;
using Api_Hotel_V2.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Hotel_V2.Controllers
{
    [ApiController]
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
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AfiliadoCreacionDTO afiliadoCreacionDTO)
        {
            return await Post<AfiliadoCreacionDTO, Afiliado, AfiliadoDTO>(afiliadoCreacionDTO, "GetAfiliadoById" );
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
