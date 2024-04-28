using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(IConfiguration configuration)
        {
            _scheduleService = new ScheduleService(configuration);
        }

        [HttpGet]
        public ActionResult<List<Schedule>> Get()
        {
            var result = _scheduleService.GetAllSchedules();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("GetById/{scheduleId}")]
        public ActionResult<Schedule> GetById(int scheduleId)
        {
            var result = _scheduleService.GetScheduleById(scheduleId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByEstablishmentProfileId/{establishmentProfileId}")]
        public ActionResult<Schedule> GetByEstablishmentProfileId(int establishmentProfileId)
        {
            var result = _scheduleService.GetByEstablishmentProfileId(establishmentProfileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<Schedule> Post([FromBody] ScheduleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _scheduleService.AddSchedule(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { scheduleId = result.Value.ScheduleId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{scheduleId}")]
        public ActionResult<Schedule> Put(int scheduleId, [FromBody] Schedule schedule)
        {
            if (scheduleId != schedule.ScheduleId)
            {
                return BadRequest();
            }

            var result = _scheduleService.UpdateSchedule(schedule);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{scheduleId}")]
        public IActionResult Delete(int scheduleId)
        {
            var result = _scheduleService.DeleteSchedule(scheduleId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
