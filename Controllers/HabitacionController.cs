using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.HabitacionDTOs;
using Api_Hotel_V2.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api_Hotel_V2.Controllers
{
    [ApiController]
    [Route("api/habitaciones")]
    public class HabitacionController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly Context context;

        public HabitacionController( IMapper mapper, Context context): base (context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public  async Task<ActionResult<List<HabitacionDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {

            return await Get<Habitacion, HabitacionDTO>();
        }

        [HttpGet("{id:int}" , Name = "GetHabitacionById")]
        public async Task<ActionResult<HabitacionDTO>> Get(int id)
        {
            return await Get<Habitacion,HabitacionDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] HabitacionCreacionDTO habitacionCreacionDTO)
        {
            return await Post<HabitacionCreacionDTO, Habitacion, HabitacionDTO>(habitacionCreacionDTO, "GetHabitacionById");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] HabitacionCreacionDTO habitacionCreacionDTO)
        {
            return await Put<HabitacionCreacionDTO, Habitacion>(id, habitacionCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
           return await Delete<Habitacion>(id);
        }
    }
}
