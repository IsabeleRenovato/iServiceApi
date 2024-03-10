using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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
            var result = _establishmentCategoryService.GetAllCategories();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("GetById/{categoryId}")]
        public ActionResult<EstablishmentCategory> GetById(int categoryId)
        {
            var result = _establishmentCategoryService.GetCategoryById(categoryId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<EstablishmentCategory> Post([FromBody] EstablishmentCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _establishmentCategoryService.AddCategory(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { categoryId = result.Value.EstablishmentCategoryID }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{categoryId}")]
        public ActionResult<EstablishmentCategory> Put(int categoryId, [FromBody] EstablishmentCategory category)
        {
            if (categoryId != category.EstablishmentCategoryID)
            {
                return BadRequest();
            }

            var result = _establishmentCategoryService.UpdateCategory(category);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{categoryId}")]
        public IActionResult Delete(int categoryId)
        {
            var result = _establishmentCategoryService.DeleteCategory(categoryId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
