using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Hotel_V2.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public CustomBaseController(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad : class
        {
            try
            {
                var entidades = await context.Set<TEntidad>().AsNoTracking().ToListAsync();
                var dtos = mapper.Map<List<TDTO>>(entidades);
                return dtos;
            }
            catch (Exception)
            {
                return new List<TDTO>(null); 
            }
            
        }

        //por medio de la interface se puede pasar una entidad que cumpla con IId
        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad : class, IId
        {
            try
            {
                var entidad = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (entidad == null)
                {
                    return NotFound();
                }

                return mapper.Map<TDTO>(entidad);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
        }

        protected async Task<ActionResult> Post<TCreacion, TEntidad, TLectura>(
            TCreacion creacionDTO, String nombreRuta) where TEntidad : class, IId
        {
            try
            {
                var entidad = mapper.Map<TEntidad>(creacionDTO);
                context.Add(entidad);
                await context.SaveChangesAsync();

                var dtoLectura = mapper.Map<TLectura>(entidad);

                
                return new CreatedAtRouteResult(nombreRuta, new { id = entidad.Id }, dtoLectura);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        protected async Task<ActionResult> Put<TCreacion, TEntidad>(int id, TCreacion creacionDTO) where TEntidad : class, IId
        {
            try
            {
                var entidad = mapper.Map<TEntidad>(creacionDTO);

                entidad.Id = id;

                context.Entry(entidad).State = EntityState.Modified; //VER
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
        }
        protected async Task<ActionResult> Delete<TEntidad>(int id) where TEntidad: class, IId, new()
        {
            try
            {
                var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);

                if (!existe)
                {
                    return NotFound();
                }

                context.Remove(new TEntidad() { Id = id });
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception )
            {
                return StatusCode(500);
            }
            
        }
    }
}
