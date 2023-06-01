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
        public async Task<ActionResult<ReservaDTO>> Post([FromBody] ReservaCreacionDTO reservaCreacionDTO)
        { 
            try
            {
                //pasar a middle

                if(reservaCreacionDTO.Fin.Date == reservaCreacionDTO.Inicio.Date)
                {
                    return BadRequest();
                }

                var id = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;


                Reservacion reservacion;
                List<Reservacion> ListaReservaciones = new List<Reservacion>();

                int dias = (reservaCreacionDTO.Fin - reservaCreacionDTO.Inicio).Days;
                int habitaciones = reservaCreacionDTO.HabitacionesEnLaReserva.Count();

                var reserva = mapper.Map<Reserva>(reservaCreacionDTO);
                reserva.UsuarioId = id;
                reserva.Activa = true;

                for (int i = 0; i < habitaciones; i++)
                {
                    for (int j = 0; j < dias; j++)
                    {
                        reservacion = new Reservacion();
                        reservacion.HabitacionId = reservaCreacionDTO.HabitacionesEnLaReserva[i];

                        reservacion.Fecha = reservaCreacionDTO.Inicio.AddDays(j);

                        ListaReservaciones.Add(reservacion);

                        reserva.Reservaciones = ListaReservaciones;
                    }
                }
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
