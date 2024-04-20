using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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

        [HttpGet("GetById/{appointmentId}")]
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
        public ActionResult<Appointment> Post([FromBody] AppointmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _appointmentService.AddAppointment(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { appointmentId = result.Value.AppointmentID }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{appointmentId}")]
        public ActionResult<Appointment> Put(int appointmentId, [FromBody] Appointment appointment)
        {
            if (appointmentId != appointment.AppointmentID)
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

        [HttpDelete("{appointmentId}")]
        public IActionResult Delete(int appointmentId)
        {
            var result = _appointmentService.DeleteAppointment(appointmentId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }

}
