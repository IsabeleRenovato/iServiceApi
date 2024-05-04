using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstablishmentCategoryController : ControllerBase
    {
        private readonly EstablishmentCategoryService _establishmentCategoryService;

        public EstablishmentCategoryController(IConfiguration configuration)
        {
            _establishmentCategoryService = new EstablishmentCategoryService(configuration);
        }

        [HttpGet]
        public ActionResult<List<EstablishmentCategory>> Get()
        {
            var result = _establishmentCategoryService.GetAllEstablishmentCategories();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{categoryId}")]
        public ActionResult<EstablishmentCategory> GetById(int categoryId)
        {
            var result = _establishmentCategoryService.GetEstablishmentCategoryById(categoryId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<EstablishmentCategory> Post([FromBody] EstablishmentCategoryInsert categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _establishmentCategoryService.AddEstablishmentCategory(categoryModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { categoryId = result.Value.EstablishmentCategoryId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{categoryId}")]
        public ActionResult<EstablishmentCategory> Put(int categoryId, [FromBody] EstablishmentCategoryUpdate category)
        {
            if (categoryId != category.EstablishmentCategoryId)
            {
                return BadRequest();
            }

            var result = _establishmentCategoryService.UpdateEstablishmentCategory(category);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{categoryId}/SetActive")]
        public ActionResult<bool> SetActive(int categoryId, [FromBody] bool isActive)
        {
            var result = _establishmentCategoryService.SetActiveStatus(categoryId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{categoryId}/SetDeleted")]
        public ActionResult<bool> SetDeleted(int categoryId, [FromBody] bool isDeleted)
        {
            var result = _establishmentCategoryService.SetDeletedStatus(categoryId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }

}
