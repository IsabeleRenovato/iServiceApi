using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpGet("GetAvailableTimes/{serviceId}/{date}")]
        public ActionResult<List<string>> GetAvailableTimes(int serviceId, DateTime date)
        {
            var result = _serviceService.GetAvailableTimes(serviceId, date);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("GetById/{serviceId}")]
        public ActionResult<Service> GetById(int serviceId)
        {
            var result = _serviceService.GetServiceById(serviceId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByEstablishmentProfileId/{establishmentProfileId}")]
        public ActionResult<List<Service>> GetByEstablishmentProfileId(int establishmentProfileId)
        {
            var result = _serviceService.GetByEstablishmentProfileId(establishmentProfileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByServiceCategoryId/{serviceCategoryId}")]
        public ActionResult<List<Service>> GetByServiceCategoryId(int serviceCategoryId)
        {
            var result = _serviceService.GetByServiceCategoryId(serviceCategoryId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<Service> Post([FromBody] ServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _serviceService.AddService(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { serviceId = result.Value.ServiceId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{serviceId}")]
        public ActionResult<Service> Put(int serviceId, [FromBody] Service service)
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

        [HttpDelete("{serviceId}")]
        public IActionResult Delete(int serviceId)
        {
            var result = _serviceService.DeleteService(serviceId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
