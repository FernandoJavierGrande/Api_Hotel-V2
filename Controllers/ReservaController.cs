using Api_Hotel_V2.DTOs.ReservasDTOs;
using Api_Hotel_V2.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api_Hotel_V2.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/reserva")]
    public class ReservaController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly Context context;
        public ReservaController(IMapper mapper, Context context) : base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ReservaDTOconReservaciones>> Post([FromBody] ReservaCreacionDTO reservaCreacionDTO)
        {
            try
            {
                var id = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                Reserva reserva = mapper.Map<Reserva>(reservaCreacionDTO);
                List<DateTime> listaDias = new List<DateTime>();

                foreach (var res in reserva.Reservaciones)
                {
                    if (!listaDias.Contains(res.Fecha)) listaDias.Add(res.Fecha);
                }

                var reservaciones = await context.Reservaciones.Where(r => listaDias.Contains(r.Fecha)).ToListAsync();

                for (int i = 0; i < reservaciones.Count; i++)
                {
                    for (int j = 0; j < reserva.Reservaciones.Count; j++)
                    {
                        if (reservaciones[i].HabitacionId == reserva.Reservaciones[j].HabitacionId && reservaciones[i].Fecha == reserva.Reservaciones[j].Fecha)
                        {
                            return BadRequest($"ocupado");
                        }
                    }
                }
                reserva.UsuarioId = id;
                context.Add(reserva);
                await context.SaveChangesAsync();

                var reservaDTO = mapper.Map<ReservaDTO>(reserva);
                return CreatedAtRoute("GetRva", new { id = reserva.Id }, reservaDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpGet("{id:int}", Name = "GetRva")]
        public async Task<ActionResult<ReservaDTOconReservaciones>>Get(int id)
        {
            try
            {
                var reserva = await context.Reservas
                .Include(r => r.Reservaciones)
                .ThenInclude(h => h.Habitacion)
                .Include(r => r.Afiliado)
                .FirstOrDefaultAsync(r => r.Id == id);

                if (reserva == null){
                    return NotFound();}
                return mapper.Map<ReservaDTOconReservaciones>(reserva);
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
                var exist = await context.Reservas.AnyAsync(r => r.Id == id);

                if ( !exist) { return NotFound(); }

                context.Remove(new Reserva { Id = id });

                await context.SaveChangesAsync();

                return NoContent();
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPut("activa/{id:int}")]
        public async Task<ActionResult> Put(int id)
        {
            try
            {
                var reserva = await context.Reservas.FirstOrDefaultAsync(r => r.Id == id);

                if (reserva == null)
                {
                    return NotFound();
                }
                reserva.Activa = !reserva.Activa;

                context.Update(reserva);

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PatchReservaEstadoDTO>jsonPatchDocument )
        {
            if(jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var reserva = await context.Reservas.FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            var RvaDTO = mapper.Map<PatchReservaEstadoDTO>(reserva);

            jsonPatchDocument.ApplyTo(RvaDTO, ModelState);

            var isValid = TryValidateModel(RvaDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(RvaDTO, reserva);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
