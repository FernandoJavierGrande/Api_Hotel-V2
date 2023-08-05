using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.HabitacionDTOs;
using Api_Hotel_V2.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Hotel_V2.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpGet("{id:int}" , Name = "GetHabitacion")]
        public async Task<ActionResult<HabitacionDTO>> Get(int id)
        {
            try
            {
                return await Get<Habitacion, HabitacionDTO>(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] HabitacionDTO habitacionDTO)
        {
            try
            {
                var exist = await context.Habitaciones.AnyAsync(h => h.Id == habitacionDTO.Id);

                if (exist) { return BadRequest("La habitacion ya existe"); }

                return await Post<HabitacionDTO, Habitacion, HabitacionDTO>(habitacionDTO, "GetHabitacion");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] HabitacionDTO habitacionDTO)
        {
            try
            {
                return await Put<HabitacionDTO, Habitacion>(id, habitacionDTO);
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
                return await Delete<Habitacion>(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
