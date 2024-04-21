using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly ServiceCategoryService _serviceCategoryService;

        public ServiceCategoryController(IConfiguration configuration)
        {
            _serviceCategoryService = new ServiceCategoryService(configuration);
        }

        [HttpGet]
        public ActionResult<List<ServiceCategory>> Get()
        {
            var result = _serviceCategoryService.GetAllServiceCategories();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{serviceCategoryId}")]
        public ActionResult<ServiceCategory> GetById(int serviceCategoryId)
        {
            var result = _serviceCategoryService.GetServiceCategoryById(serviceCategoryId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<ServiceCategory> Post([FromBody] ServiceCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _serviceCategoryService.AddServiceCategory(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { serviceCategoryId = result.Value.ServiceCategoryId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{serviceCategoryId}")]
        public ActionResult<ServiceCategory> Put(int serviceCategoryId, [FromBody] ServiceCategory serviceCategory)
        {
            if (serviceCategoryId != serviceCategory.ServiceCategoryId)
            {
                return BadRequest();
            }

            var result = _serviceCategoryService.UpdateServiceCategory(serviceCategory);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{serviceCategoryId}")]
        IActionResult Delete(int serviceCategoryId)
        {
            var result = _serviceCategoryService.DeleteServiceCategory(serviceCategoryId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
