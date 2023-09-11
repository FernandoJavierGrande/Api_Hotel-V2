using Api_Hotel_V2.DTOs.ReservacionesDTOs;
using Api_Hotel_V2.DTOs.ReservasDTOs;
using Api_Hotel_V2.Entidades;
using Api_Hotel_V2.Servicios;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Api_Hotel_V2.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/reservaciones")]
    public class ReservacionesController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly Context context;
        private readonly IEmailReservasService _emailReservas;

        public ReservacionesController(IMapper mapper, Context context, IEmailReservasService emailReservas) : base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
            this._emailReservas = emailReservas;
        }

        [HttpGet("{fecha}")]
        public async Task<ActionResult> Get(string fecha, int cantidadDias = 7)
        {
            DateTime date;
            List<ReservacionDTO> listaReservacionesDTO = new List<ReservacionDTO>();

            var ok = DateTime.TryParse(fecha, out date);
            
            if (!ok) return BadRequest();
            try
            {
                var reservaciones = await context.Reservaciones.Where(r => r.Fecha >= date && r.Fecha <= date.AddDays(cantidadDias)).ToListAsync();

                foreach (var res in reservaciones)
                {
                    listaReservacionesDTO.Add(mapper.Map<ReservacionDTO>(res));
                }

            return Ok(listaReservacionesDTO);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("{idReserva:int}")]
        public async Task<ActionResult> Post(int idReserva,[FromBody] List<ReservacionDTO> LsReservacionDTOs)
        {
            try
            {
                if (LsReservacionDTOs.Count < 1) return BadRequest();

                int room = LsReservacionDTOs[0].HabitacionId;
                foreach (var res in LsReservacionDTOs)
                {
                    idReserva = (res.ReservaId == idReserva) ? idReserva : throw new Exception("Error en numero de reserva.");
                    room = (res.HabitacionId == room) ? room : throw new Exception("Solo puede agregar de a una habitacion");
                }

                var reserva = await context.Reservas.Where(r => r.Id == idReserva).Include(r => r.Reservaciones).Include(a=>a.Afiliado).AsNoTracking().FirstOrDefaultAsync();

                reserva = (reserva != null) ? reserva : throw new Exception("El numero de reserva no existe.");
                //Probar con AllAsync() para varias habitacioness
                var exist = (await context.Habitaciones.Where(h => h.Id == room).AnyAsync() == true) ? true : throw new Exception("La habitacion NO EXISTE.");

                List<DateTime> listaDias = new List<DateTime>();
                foreach (var res in LsReservacionDTOs)
                {
                    if (!listaDias.Contains(res.Fecha)) listaDias.Add(res.Fecha);
                }

                var reservacionesDb = await context.Reservaciones.Where(r => listaDias.Contains(r.Fecha)).ToListAsync();

                for (int i = 0; i < reservacionesDb.Count; i++)
                {
                    for (int j = 0; j < LsReservacionDTOs.Count; j++)
                    {
                        if (reservacionesDb[i].HabitacionId == LsReservacionDTOs[j].HabitacionId && reservacionesDb[i].Fecha == LsReservacionDTOs[j].Fecha)
                        {
                            return BadRequest($"ocupado");
                        }
                    }
                }

                Reservacion reservacion;
                foreach (var resDto in LsReservacionDTOs)
                {
                    reservacion = new Reservacion();
                    reservacion = mapper.Map<Reservacion>(resDto);
                    reservacion.ReservaId = idReserva;

                    reserva.Reservaciones.Add(reservacion);
                    context.Add(reservacion);
                }

                await context.SaveChangesAsync();

                var reservaDTOMail = mapper.Map<ReservaDTOMail>(reserva);
                List<int> hab = new List<int>();
                hab.Add(room);
                reservaDTOMail.Habitaciones = hab;
                reservaDTOMail.Dias = listaDias;

                _emailReservas.SendEmailModReserva(reservaDTOMail);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] ReservacionCreacionDTO reservacionDeleteDTO)
        {
            Reservacion reservacionDelete = mapper.Map<Reservacion>(reservacionDeleteDTO);

            try
            {
                var existe = await context.Reservaciones.AnyAsync(c => c == reservacionDelete);

                if (!existe) return NotFound();
                context.Remove(reservacionDelete);

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
