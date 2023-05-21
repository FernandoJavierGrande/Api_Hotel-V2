using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.Auth;
using Api_Hotel_V2.DTOs.AuthDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Hotel_V2.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class AuthController : CustomBaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly Context context;

        public AuthController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager, Context context, IMapper mapper)
            :base (context,mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.context = context;
        } 
        
        [HttpPost("registrar")] //api/cuentas/registrar
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<RespAuthDTO>> Registrar(CredUserDTO credencialesUsuariosDTO)
        {
            var usuario = new IdentityUser { UserName = credencialesUsuariosDTO.Email, Email = credencialesUsuariosDTO.Email };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password);
            var role = await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, credencialesUsuariosDTO.role));

            if (resultado.Succeeded && role.Succeeded)
            {

                return await ConstruirToken(credencialesUsuariosDTO);
            }
            else
            {
                return BadRequest("there was an error");
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<RespAuthDTO>> Login(CredUserDTO credencialesUsuariosDTO)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuariosDTO.Email,
                credencialesUsuariosDTO.Password,
                isPersistent: false,
                lockoutOnFailure: false); //lockout si el usuario intenta varias veces fallidas lo bloquea

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuariosDTO);
            }
            else
            {
                return BadRequest("Login Wrong");
            }
        }
        [HttpPost("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespAuthDTO>> Renovar()
        {
            var userInfo = new CredUserDTO
            {
                Email = HttpContext.User.Identity.Name
            };

            return await ConstruirToken(userInfo);
        }
        //[HttpGet("Usuarios")] //Users list
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        //public async Task<ActionResult<List<UsuarioDTO>>> Get([FromQuery] PaginacionDTO paginationDTO)
        //{
        //    var queryable = context.Users.AsQueryable();
        //    queryable = queryable.OrderBy(x => x.Email);
        //    return await Get<IdentityUser, UsuarioDTO>(paginationDTO);
        //}
        [HttpPost("AsignarRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> AsignarRol(EditRoleDTO editarRolDTO)
        {
            try
            {
                var user = await userManager.FindByIdAsync(editarRolDTO.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                //usar para crear admin?
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        [HttpPost("RemoveRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> RemoverRol(EditRoleDTO editarRolDTO)
        {
            try
            {
                var user = await userManager.FindByIdAsync(editarRolDTO.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private async Task<RespAuthDTO> ConstruirToken(CredUserDTO credencialesUsuariosDTO)
        {
            var claims = new List<Claim>() // no informacion sensible
            {
                new Claim("email", credencialesUsuariosDTO.Email)
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);


            claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.Id));

            var claimsDB = await userManager.GetClaimsAsync(usuario); //busca los claims en db

            claims.AddRange(claimsDB);//une con los claims de la bbdd para que se generen en token

            var expiracion = DateTime.UtcNow.AddDays(1);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llaveJwt"]));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: credenciales);

            return new RespAuthDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiracion
            };
        }
    }
}
