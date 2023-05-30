using Api_Hotel_V2.DTOs.ReservasDTOs;
using Api_Hotel_V2.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<ReservaDTO>> Post([FromBody] ReservaCreacionDTO reservaCreacionDTO)
        { 
            try
            {
                Reservacion reservacion;
                List<Reservacion> ListaReservaciones = new List<Reservacion>();

                int dias = (reservaCreacionDTO.Fin - reservaCreacionDTO.Inicio).Days + 1;
                int habitaciones = reservaCreacionDTO.HabitacionesEnLaReserva.Count();

                var reserva = mapper.Map<Reserva>(reservaCreacionDTO);

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

                return CreatedAtRoute("GetRva", new {id = reserva.Id}, reservaDTO);
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
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ReservaCreacionDTO reservaCreacionDTO)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
