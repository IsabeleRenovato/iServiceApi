using iServiceRepositories.Repositories.Models;
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

        [HttpGet("{scheduleId}")]
        public ActionResult<Schedule> GetById(int scheduleId)
        {
            var result = _scheduleService.GetScheduleById(scheduleId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<Schedule> Post([FromBody] ScheduleInsert scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _scheduleService.AddSchedule(scheduleModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { scheduleId = result.Value.ScheduleId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{scheduleId}")]
        public ActionResult<Schedule> Put(int scheduleId, [FromBody] ScheduleUpdate schedule)
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

        [HttpPut("{scheduleId}/SetActive")]
        public ActionResult<bool> SetActive(int scheduleId, [FromBody] bool isActive)
        {
            var result = _scheduleService.SetActiveStatus(scheduleId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{scheduleId}/SetDeleted")]
        public ActionResult<bool> SetDeleted(int scheduleId, [FromBody] bool isDeleted)
        {
            var result = _scheduleService.SetDeletedStatus(scheduleId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}
