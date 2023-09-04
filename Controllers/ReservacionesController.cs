using Api_Hotel_V2.DTOs.ReservacionesDTOs;
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
    [Route("api/reservaciones")]
    public class ReservacionesController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly Context context;

        public ReservacionesController(IMapper mapper, Context context) : base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
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

        [HttpPost("{id:int}")]
        public async Task<ActionResult> Post(int id,[FromBody] List<ReservacionDTO> LsReservacionDTOs)
        {
            List<DateTime> listaDias = new List<DateTime>();

            if (LsReservacionDTOs.Count <1) return BadRequest();
            

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
                reservacion.ReservaId = id;
                context.Add(reservacion);
            }
            try
            {
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception )
            {
                return StatusCode(500);
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
