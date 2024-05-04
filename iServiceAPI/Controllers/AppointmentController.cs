using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(IConfiguration configuration)
        {
            _appointmentService = new AppointmentService(configuration);
        }

        [HttpGet]
        public ActionResult<List<Appointment>> Get()
        {
            var result = _appointmentService.GetAllAppointments();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{appointmentId}")]
        public ActionResult<Appointment> GetById(int appointmentId)
        {
            var result = _appointmentService.GetAppointmentById(appointmentId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<Appointment> Post([FromBody] AppointmentInsert appointmentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _appointmentService.AddAppointment(appointmentModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { appointmentId = result.Value.AppointmentId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{appointmentId}")]
        public ActionResult<Appointment> Put(int appointmentId, [FromBody] AppointmentUpdate appointment)
        {
            if (appointmentId != appointment.AppointmentId)
            {
                return BadRequest();
            }

            var result = _appointmentService.UpdateAppointment(appointment);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{appointmentId}/SetActive")]
        public ActionResult<bool> SetActive(int appointmentId, [FromBody] bool isActive)
        {
            var result = _appointmentService.SetActiveStatus(appointmentId, isActive);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{appointmentId}/SetDeleted")]
        public ActionResult<bool> SetDeleted(int appointmentId, [FromBody] bool isDeleted)
        {
            var result = _appointmentService.SetDeletedStatus(appointmentId, isDeleted);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}
