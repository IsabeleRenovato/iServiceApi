using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [Route("v1")]
    [ApiController]
    public class ViaCepController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ViaCepController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<dynamic>> SearchAsync(string cep)
        {
            var response = await new ViaCepService(_configuration).Search(cep);

            if (response.Cep == null)
            {
                return NotFound(new
                {
                    message = "Endereço não encontrado!"
                });
            }

            return Ok(new
            {
                Address = response
            });
        }
    }
}
