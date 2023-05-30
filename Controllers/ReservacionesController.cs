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
                if (!habs.Contains(reservacionDTO.HabitacionNum))
                {
                    habs.Add(reservacionDTO.HabitacionNum);
                }
            }

            var reservacionesDB = await context.Reservaciones.Where(r => listDays.Contains(r.Fecha)).Include(h =>h.Habitacion).ToListAsync();

           if(reservacionesDB.Count > 0)
           {
                foreach (var res in reservacionesDB)
                {
                    if (habs.Contains(res.Habitacion.NumHab))
                    {
                        return BadRequest("ocupado");
                    }
                }
           }

            List<Reservacion> reservaciones = new List<Reservacion>();

            foreach (var resDto in LsReservacionDTOs)
            {
                
                reservaciones.Add(mapper.Map<Reservacion>(resDto));
            }

            return Ok(reservaciones);

        }
    }
}
