using iServiceRepositories.Models;
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
        public async Task<ActionResult<UserInfo>> LoginAsync([FromBody] Login model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = new AuthService(_configuration).Login(model);
                result.Value.Token = TokenService.GenerateToken((result.Value.User, result.Value.UserRole));

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

        [HttpPost]
        [Route("preregister")]
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

        [HttpPost]
        [Route("register")]
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

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public string Employee() => "admin";
    }
}
