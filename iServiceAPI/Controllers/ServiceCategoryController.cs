using iServiceRepositories.Repositories.Models;
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

        [HttpGet("{categoryId}")]
        public ActionResult<ServiceCategory> GetById(int categoryId)
        {
            var result = _serviceCategoryService.GetServiceCategoryById(categoryId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByUserProfileId/{userProfileId}")]
        public ActionResult<List<ServiceCategory>> GetByUserProfileId(int userProfileId)
        {
            var result = _serviceCategoryService.GetByUserProfileId(userProfileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<ServiceCategory> Post([FromBody] ServiceCategoryInsert categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _serviceCategoryService.AddServiceCategory(categoryModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { categoryId = result.Value.ServiceCategoryId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{categoryId}")]
        public ActionResult<ServiceCategory> Put(int categoryId, [FromBody] ServiceCategoryUpdate category)
        {
            if (categoryId != category.ServiceCategoryId)
            {
                return BadRequest();
            }

            var result = _serviceCategoryService.UpdateServiceCategory(category);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{categoryId}/SetActive")]
        public ActionResult<bool> SetActive(int categoryId, [FromBody] bool isActive)
        {
            var result = _serviceCategoryService.SetActiveStatus(categoryId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{categoryId}/SetDeleted")]
        public ActionResult<bool> SetDeleted(int categoryId, [FromBody] bool isDeleted)
        {
            var result = _serviceCategoryService.SetDeletedStatus(categoryId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}
