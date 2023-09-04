using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.Auth;
using Api_Hotel_V2.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api_Hotel_V2.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AdminController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            IEmailService emailService)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._emailService = emailService;
        }

        [HttpPost("registrar")] //api/cuentas/registrar
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
                    
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                Console.WriteLine($" esta en el catch superior {e.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new {message= "User error" });
            }
        }
        [HttpPost("AsignarRol")]
        public async Task<ActionResult> AsignarRol(EditRoleDTO editarRolDTO)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(editarRolDTO.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message= "Can´t assign role" } ); 
            }

        }
        [HttpPost("RemoveRol")]
        public async Task<ActionResult> RemoverRol(EditRoleDTO editarRolDTO )
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Can´t remove role" });
            }
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
