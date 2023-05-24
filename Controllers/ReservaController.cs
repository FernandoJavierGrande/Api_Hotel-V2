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

        //[HttpGet("{id:int}", Name = "GetRva")]
        //public async Task<ActionResult<Reserva>> Get1(int id)
        //{
        //    var resp = await context.Reservas.FirstOrDefaultAsync(r => r.Id == id);

        //    if (resp != null)
        //    {
        //        return resp;
        //    };
        //    return BadRequest();
        //}

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

                return Ok(reserva);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Reserva>>Get(int id)
        {
            var reserva = await context.Reservas.Include(r => r.Reservaciones).ThenInclude(h => h.Habitacion).FirstOrDefaultAsync(r => r.Id == id);
            return reserva;
        }



    }
}
