using iServiceServices.Services;
using iServiceServices.Services.Models;
using iServiceServices.Services.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserInfo>> LoginAsync([FromBody] Login model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = new AuthService(_configuration).Login(model);

                if (result.IsSuccess)
                {
                    //result.Value.Token = TokenService.GenerateToken((result.Value.User, result.Value.UserRole));
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest(new { message = result.ErrorMessage });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro inesperado.");
            }
        }

        [HttpPost("preregister")]
        public async Task<ActionResult<UserInfo>> PreRegisterAsync([FromBody] PreRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = new AuthService(_configuration).PreRegister(model);

                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest(new { message = result.ErrorMessage });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro inesperado.");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserInfo>> RegisterAsync([FromBody] Register model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = new AuthService(_configuration).Register(model);

                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest(new { message = result.ErrorMessage });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro inesperado.");
            }
        }

        [HttpGet("admin")]
        [Authorize(Roles = "admin")]
        public string Employee() => "admin";
    }

}
