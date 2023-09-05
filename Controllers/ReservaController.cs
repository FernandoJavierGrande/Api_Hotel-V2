using Api_Hotel_V2.DTOs.ReservasDTOs;
using Api_Hotel_V2.Entidades;
using Api_Hotel_V2.Servicios;
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
        private readonly IEmailReservasService _emailReservasService;

        public ReservaController(IMapper mapper, Context context, IEmailReservasService emailReservasService) : base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
            _emailReservasService = emailReservasService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ReservaDTOconReservaciones>> Post([FromBody] ReservaCreacionDTO reservaCreacionDTO)
        {
            Reserva reserva = mapper.Map<Reserva>(reservaCreacionDTO);
            try
            {
                var id = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                reserva.Afiliado = await context.Afiliados.Where(a => a.Id == reserva.AfiliadoId).FirstOrDefaultAsync();

                if (reserva.Afiliado == null)
                {
                    throw new Exception();
                }

                List<DateTime> listaDias = new List<DateTime>();
                List<int> listaHab = new List<int>();

                foreach (var res in reserva.Reservaciones)
                {
                    if (!listaDias.Contains(res.Fecha)) listaDias.Add(res.Fecha);
                    if (!listaHab.Contains(res.HabitacionId)) listaHab.Add(res.HabitacionId);
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
                var resp = await context.SaveChangesAsync();

                if (resp == 0)
                {
                    throw new Exception();
                }
                var reservaDTOMail = mapper.Map<ReservaDTOMail>(reserva);
                reservaDTOMail.Habitaciones = listaHab;

                _emailReservasService.SendEmailNuevaReserva(reservaDTOMail);

                var reservaDTO = mapper.Map<ReservaDTOconReservaciones>(reserva);

                return CreatedAtRoute("GetRva", new { id = reserva.Id }, reservaDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
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
                .Include(u => u.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);

                if (reserva == null)
                {
                    return NotFound();
                }
                return mapper.Map<ReservaDTOconReservaciones>(reserva);
            }
            catch (Exception)
            {
                return BadRequest();
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
                return StatusCode(StatusCodes.Status500InternalServerError);
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
