using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceService _serviceService;

        public ServiceController(IConfiguration configuration)
        {
            _serviceService = new ServiceService(configuration);
        }

        [HttpGet]
        public async Task<ActionResult<List<Service>>> Get()
        {
            var result = await _serviceService.GetAllServices();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{serviceId}")]
        public async Task<ActionResult<Service>> GetById(int serviceId)
        {
            var result = await _serviceService.GetServiceById(serviceId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetServiceByUserProfileId/{userProfileId}")]
        public async Task<ActionResult<List<Service>>> GetServiceByUserProfileId(int userProfileId)
        {
            var result = await _serviceService.GetServiceByUserProfileId(userProfileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public async Task<ActionResult<Service>> Post([FromForm] Service serviceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceService.AddService(serviceModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { serviceId = result.Value.ServiceId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{serviceId}")]
        public async Task<ActionResult<Service>> Put(int serviceId, [FromBody] Service service)
        {
            if (serviceId != service.ServiceId)
            {
                return BadRequest();
            }

            var result = await _serviceService.UpdateService(service);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{serviceId}/SetActive")]
        public async Task<ActionResult<bool>> SetActive(int serviceId, [FromBody] bool isActive)
        {
            var result = await _serviceService.SetActiveStatus(serviceId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{serviceId}/SetDeleted")]
        public async Task<ActionResult<bool>> SetDeleted(int serviceId, [FromBody] bool isDeleted)
        {
            var result = await _serviceService.SetDeletedStatus(serviceId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}