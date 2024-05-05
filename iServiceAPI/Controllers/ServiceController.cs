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
        public ActionResult<List<Service>> Get()
        {
            var result = _serviceService.GetAllServices();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{serviceId}")]
        public ActionResult<Service> GetById(int serviceId)
        {
            var result = _serviceService.GetServiceById(serviceId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetServiceByUserProfileId/{userProfileId}")]
        public ActionResult<List<Service>> GetServiceByUserProfileId(int userProfileId)
        {
            var result = _serviceService.GetServiceByUserProfileId(userProfileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<Service> Post([FromForm] ServiceInsert serviceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _serviceService.AddService(serviceModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { serviceId = result.Value.ServiceId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{serviceId}")]
        public ActionResult<Service> Put(int serviceId, [FromBody] ServiceUpdate service)
        {
            if (serviceId != service.ServiceId)
            {
                return BadRequest();
            }

            var result = _serviceService.UpdateService(service);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{serviceId}/SetActive")]
        public ActionResult<bool> SetActive(int serviceId, [FromBody] bool isActive)
        {
            var result = _serviceService.SetActiveStatus(serviceId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{serviceId}/SetDeleted")]
        public ActionResult<bool> SetDeleted(int serviceId, [FromBody] bool isDeleted)
        {
            var result = _serviceService.SetDeletedStatus(serviceId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}