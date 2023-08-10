using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.Auth;
using Api_Hotel_V2.DTOs.AuthDTOs;
using Api_Hotel_V2.Servicios;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Context context;

        public AuthController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IEmailService emailService,
            SignInManager<IdentityUser> signInManager, Context context, IMapper mapper)
            : base(context, mapper)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._emailService = emailService;
            this._signInManager = signInManager;
            this.context = context;
        }

        [HttpPost("registrar")] //api/cuentas/registrar
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<RespAuthDTO>> Registrar(CredUserDTO credencialesUsuariosDTO)
        {
            try
            {
                var usuario = new IdentityUser { UserName = credencialesUsuariosDTO.Email, Email = credencialesUsuariosDTO.Email };
                var resultado = await _userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password);
                var role = await _userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, credencialesUsuariosDTO.role));

                if (resultado.Succeeded && role.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                    if (!string.IsNullOrEmpty(token))
                    {
                        SendConfirmationEmail(usuario, token);
                        return Ok("chek your email account");
                    }
                    throw new Exception();
                }
                return BadRequest("there was an error");

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult<RespAuthDTO>> ConfirmarEmail(string uid, string token)
        {

            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token)) return BadRequest();

            token = token.Replace(" ", "+");

            var user = await _userManager.FindByIdAsync(uid);

            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded) return StatusCode(500);

            return await ConstruirToken(new CredUserDTO { Email = user.Email });

        }
        [HttpPost("RecoveryPassword")]
        public async Task<ActionResult> OlvidoPassword(EmailForgetPassDTO emailDTO)
        {
            var user = await _userManager.FindByEmailAsync(emailDTO.Email);

            if (user == null || !user.EmailConfirmed )
            {
                return BadRequest();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string resetLink = _configuration.GetSection("Application:ResetPassword").Value;

            string url = string.Format(appDomain + resetLink, user.Email, token);

            var email = new EmailDTO() { Para = user.Email, Asunto = "" +
                ".", Contenido = url};

            _emailService.SendEmail(email);

            return Ok();

        }

        [HttpPost("login")]
        public async Task<ActionResult<RespAuthDTO>> Login(CredUserDTO credencialesUsuariosDTO)
        {
            var resultado = await _signInManager.PasswordSignInAsync(credencialesUsuariosDTO.Email,
                credencialesUsuariosDTO.Password,
                isPersistent: false,
                lockoutOnFailure: false); //lockout si el usuario intenta varias veces fallidas lo bloquea  

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuariosDTO);
            }
            else if (resultado.IsNotAllowed)
            {
                return Unauthorized("Confirm your email");
            }
            return BadRequest("Login Wrong");

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
                var user = await _userManager.FindByIdAsync(editarRolDTO.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                //usar para crear admin?
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
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
                var user = await _userManager.FindByIdAsync(editarRolDTO.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
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

            var usuario = await _userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.Id));

            var claimsDB = await _userManager.GetClaimsAsync(usuario); //busca los claims en db

            claims.AddRange(claimsDB);//une con los claims de la bbdd para que se generen en token

            var expiracion = DateTime.UtcNow.AddDays(15);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llaveJwt"]));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: credenciales);

            return new RespAuthDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiracion
            };
        }
        private void SendConfirmationEmail(IdentityUser user, string token)
        {

            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            var email = new EmailDTO();
            email.Para = user.Email;
            email.Asunto = "Confirm your account.";

            string url = string.Format(appDomain + confirmationLink, user.Id, token);

            email.Contenido = url;

            _emailService.SendEmail(email);
        }
    }
}
