using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services;
using iServiceServices.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientProfileController : ControllerBase
    {
        private readonly ClientProfileService _clientProfileService;

        public ClientProfileController(IConfiguration configuration)
        {
            _clientProfileService = new ClientProfileService(configuration);
        }

        [HttpGet]
        public ActionResult<List<ClientProfile>> Get()
        {
            var result = _clientProfileService.GetAllProfiles();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("GetById/{profileId}")]
        public ActionResult<ClientProfile> GetById(int profileId)
        {
            var result = _clientProfileService.GetProfileById(profileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByUserId/{userId}")]
        public ActionResult<ClientProfile> GetByUserId(int userId)
        {
            var result = _clientProfileService.GetProfileByUserId(userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<ClientProfile> Post([FromBody] ClientProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _clientProfileService.AddProfile(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { profileId = result.Value.ClientProfileID }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{profileId}")]
        public ActionResult<ClientProfile> Put(int profileId, [FromBody] ClientProfile profile)
        {
            if (profileId != profile.ClientProfileID)
            {
                return BadRequest();
            }

            var result = _clientProfileService.UpdateProfile(profile);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost("UploadPhoto")]
        public ActionResult<bool> UploadPhoto([FromBody] ImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _clientProfileService.UpdatePhoto(model);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{profileId}")]
        public IActionResult Delete(int profileId)
        {
            var result = _clientProfileService.DeleteProfile(profileId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
