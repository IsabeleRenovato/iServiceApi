using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialScheduleController : ControllerBase
    {
        private readonly SpecialScheduleService _specialScheduleService;

        public SpecialScheduleController(IConfiguration configuration)
        {
            _specialScheduleService = new SpecialScheduleService(configuration);
        }

        [HttpGet]
        public ActionResult<List<SpecialSchedule>> Get()
        {
            var result = _specialScheduleService.GetAllSpecialSchedules();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{specialScheduleId}")]
        public ActionResult<SpecialSchedule> GetById(int specialScheduleId)
        {
            var result = _specialScheduleService.GetSpecialScheduleById(specialScheduleId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<SpecialSchedule> Post([FromBody] SpecialScheduleInsert scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _specialScheduleService.AddSpecialSchedule(scheduleModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { specialScheduleId = result.Value.SpecialScheduleId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{specialScheduleId}")]
        public ActionResult<SpecialSchedule> Put(int specialScheduleId, [FromBody] SpecialScheduleUpdate schedule)
        {
            if (specialScheduleId != schedule.SpecialScheduleId)
            {
                return BadRequest();
            }

            var result = _specialScheduleService.UpdateSpecialSchedule(schedule);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{specialScheduleId}/SetActive")]
        public ActionResult<bool> SetActive(int specialScheduleId, [FromBody] bool isActive)
        {
            var result = _specialScheduleService.SetActiveStatus(specialScheduleId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{specialScheduleId}/SetDeleted")]
        public ActionResult<bool> SetDeleted(int specialScheduleId, [FromBody] bool isDeleted)
        {
            var result = _specialScheduleService.SetDeletedStatus(specialScheduleId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}