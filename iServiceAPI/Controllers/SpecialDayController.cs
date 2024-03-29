using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialDayController : ControllerBase
    {
        private readonly SpecialDayService _specialDayService;

        public SpecialDayController(IConfiguration configuration)
        {
            _specialDayService = new SpecialDayService(configuration);
        }

        [HttpGet]
        public ActionResult<List<SpecialDay>> Get()
        {
            var result = _specialDayService.GetAllSpecialDays();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("GetById/{specialDayId}")]
        public ActionResult<SpecialDay> GetById(int specialDayId)
        {
            var result = _specialDayService.GetSpecialDayById(specialDayId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<SpecialDay> Post([FromBody] SpecialDayModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _specialDayService.AddSpecialDay(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { specialDayId = result.Value.SpecialDayId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{specialDayId}")]
        public ActionResult<SpecialDay> Put(int specialDayId, [FromBody] SpecialDay specialDay)
        {
            if (specialDayId != specialDay.SpecialDayId)
            {
                return BadRequest();
            }

            var result = _specialDayService.UpdateSpecialDay(specialDay);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{specialDayId}")]
        public IActionResult Delete(int specialDayId)
        {
            var result = _specialDayService.DeleteSpecialDay(specialDayId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }

}
