using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstablishmentEmployeeController : ControllerBase
    {
        private readonly EstablishmentEmployeeService _establishmentEmployeeService;

        public EstablishmentEmployeeController(IConfiguration configuration)
        {
            _establishmentEmployeeService = new EstablishmentEmployeeService(configuration);
        }

        [HttpGet]
        public async Task<ActionResult<List<EstablishmentEmployee>>> Get()
        {
            var result = await _establishmentEmployeeService.GetAllEstablishmentEmployees();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{EstablishmentEmployeeId}")]
        public async Task<ActionResult<EstablishmentEmployee>> GetById(int EstablishmentEmployeeId)
        {
            var result = await _establishmentEmployeeService.GetEstablishmentEmployeeById(EstablishmentEmployeeId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public async Task<ActionResult<EstablishmentEmployee>> Post([FromForm] EstablishmentEmployee EstablishmentEmployeeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _establishmentEmployeeService.AddEstablishmentEmployee(EstablishmentEmployeeModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { EstablishmentEmployeeId = result.Value.EstablishmentEmployeeId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{EstablishmentEmployeeId}")]
        public async Task<ActionResult<EstablishmentEmployee>> Put(int EstablishmentEmployeeId, [FromForm] EstablishmentEmployee EstablishmentEmployee)
        {
            if (EstablishmentEmployeeId != EstablishmentEmployee.EstablishmentEmployeeId)
            {
                return BadRequest();
            }

            var result = await _establishmentEmployeeService.UpdateEstablishmentEmployee(EstablishmentEmployee);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{EstablishmentEmployeeId}/SetActive")]
        public async Task<ActionResult<bool>> SetActive(int EstablishmentEmployeeId, [FromBody] bool isActive)
        {
            var result = await _establishmentEmployeeService.SetActiveStatus(EstablishmentEmployeeId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{EstablishmentEmployeeId}/SetDeleted")]
        public async Task<ActionResult<bool>> SetDeleted(int EstablishmentEmployeeId, [FromBody] bool isDeleted)
        {
            var result = await _establishmentEmployeeService.SetDeletedStatus(EstablishmentEmployeeId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}
