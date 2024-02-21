using iServiceRepositories.Models.Auth;
using iServiceServices.Models.Auth;
using iServiceServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [Route("v1")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> LoginAsync([FromBody] Login model)
        {
            var auth = new AuthService(_configuration).Get(model);

            if (auth.User == null || auth.UserRole == null)
            {
                return NotFound(new 
                { 
                    message = "Usuário ou senha inválidos" 
                });
            }

            var token = TokenService.GenerateToken((auth.User, auth.UserRole));

            auth.User.Password = "";

            return Ok(new
            {
                auth.User,
                auth.UserRole,
                Token = token
            });
        }

        [HttpPost]
        [Route("preregister")]
        public async Task<ActionResult<dynamic>> PreRegisterAsync([FromBody] PreRegister model)
        {
            var auth = new AuthService(_configuration).PreRegister(model);

            auth.User.Password = "";

            return Ok(new
            {
                auth.User,
                auth.UserRole
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<dynamic>> RegisterAsync([FromBody] Register model)
        {
            var auth = new AuthService(_configuration).Register(model);

            if (auth.User == null)
            {
                return NotFound(new
                {
                    message = "Usuário inválido"
                });
            }

            auth.User.Password = "";

            return Ok(new
            {
                auth.User,
                auth.UserRole,
                auth.ClientProfile,
                auth.EstablishmentProfile,
                auth.Adress
            });
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public string Employee() => "admin";
    }
}
