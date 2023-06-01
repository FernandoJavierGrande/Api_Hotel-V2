using Api_Hotel_V2.DTOs.ReservacionesDTOs;
using Api_Hotel_V2.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Hotel_V2.Controllers
{

    [ApiController]
    [Route("api/reservaciones")]
    public class ReservacionesController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly Context context;

        public ReservacionesController(IMapper mapper, Context context): base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }
        [HttpPost("{id:int}")]
        public async Task<ActionResult> Post(int id,[FromBody] List<ReservacionDTO> LsReservacionDTOs)
        {
            List<DateTime> listDays = new List<DateTime>();
            List<int> habs = new List<int>();   
            if (LsReservacionDTOs.Count <1)
            {
                return BadRequest();
            }

            
            foreach (var reservacionDTO in LsReservacionDTOs)
            {
                if (!listDays.Contains(reservacionDTO.Fecha))
                {
                    listDays.Add(reservacionDTO.Fecha);
                }
                if (!habs.Contains(reservacionDTO.HabitacionId))
                {
                    habs.Add(reservacionDTO.HabitacionId);
                }
            }

           var reservacionesDB = await context.Reservaciones.Where(r => listDays.Contains(r.Fecha)).Include(h =>h.Habitacion).ToListAsync();

           if(reservacionesDB.Count > 0)
           {
                foreach (var res in reservacionesDB)
                {
                    if (habs.Contains(res.Habitacion.Id))
                    {
                        return BadRequest("ocupado");
                    }
                }
            }


            var reserva = await context.Reservas.FirstOrDefaultAsync(r => r.Id == id);

            reserva = cambiarFechasReserva(reserva, LsReservacionDTOs);


            Reservacion reservacion;
            foreach (var resDto in LsReservacionDTOs)
            {
                reservacion = new Reservacion();
                reservacion = mapper.Map<Reservacion>(resDto);
                reservacion.ReservaId = id;
                context.Add(reservacion);
            }
            try
            {
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] ReservacionCreacionDTO reservacionCreacionDTO)
        {
            try
            {
                var reserva = await context.Reservas.AsNoTracking().Include(x => x.Reservaciones).FirstOrDefaultAsync(r => r.Id == reservacionCreacionDTO.ReservaId);

                if (reserva == null)
                {
                    return NotFound("La reserva no existe");
                }

                if (reserva.Reservaciones.Count > 0)
                {
                    foreach (var reservacion in reserva.Reservaciones)
                    {
                        if (reservacion.HabitacionId == reservacionCreacionDTO.HabitacionId && reservacion.Fecha == reservacionCreacionDTO.Fecha)
                        {
                            var res = mapper.Map<Reservacion>(reservacionCreacionDTO);

                            context.Remove(res);


                            //Console.WriteLine($"inicio {reserva.Inicio} fin {reserva.Fin}");
                            await context.SaveChangesAsync();
                            return NoContent();
                        }
                    }
                }
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Reserva cambiarFechasReserva(Reserva reserva, List<ReservacionDTO> LsRes)
        {
            foreach (var res in LsRes)
            {
                if (reserva.Inicio > res.Fecha)
                {
                    reserva.Inicio = res.Fecha;
                }
                if (reserva.Fin < res.Fecha.AddDays(1))
                {
                    reserva.Fin = res.Fecha.AddDays(1);
                }
            }
            return reserva;
        }
    }
}
