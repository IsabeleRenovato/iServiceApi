using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services;
using iServiceServices.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstablishmentProfileController : ControllerBase
    {
        private readonly EstablishmentProfileService _establishmentProfileService;

        public EstablishmentProfileController(IConfiguration configuration)
        {
            _establishmentProfileService = new EstablishmentProfileService(configuration);
        }

        [HttpGet]
        public ActionResult<List<EstablishmentProfile>> Get()
        {
            var result = _establishmentProfileService.GetAllProfiles();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("GetById/{profileId}")]
        public ActionResult<EstablishmentProfile> GetById(int profileId)
        {
            var result = _establishmentProfileService.GetProfileById(profileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByEstablishmentCategoryId/{establishmentCategoryId}")]
        public ActionResult<List<EstablishmentProfile>> GetByEstablishmentCategoryId(int establishmentCategoryId)
        {
            var result = _establishmentProfileService.GetByEstablishmentCategoryId(establishmentCategoryId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByUserId/{userId}")]
        public ActionResult<EstablishmentProfile> GetByUserId(int userId)
        {
            var result = _establishmentProfileService.GetProfileByUserId(userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<EstablishmentProfile> Post([FromBody] EstablishmentProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _establishmentProfileService.AddProfile(model);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { profileId = result.Value.EstablishmentProfileId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{profileId}")]
        public ActionResult<EstablishmentProfile> Put(int profileId, [FromBody] EstablishmentProfile profile)
        {
            if (profileId != profile.EstablishmentProfileId)
            {
                return BadRequest();
            }

            var result = _establishmentProfileService.UpdateProfile(profile);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost("UploadPhoto")]
        public ActionResult<string> UploadPhoto([FromForm] ImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _establishmentProfileService.UpdatePhoto(model);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{profileId}")]
        public IActionResult Delete(int profileId)
        {
            var result = _establishmentProfileService.DeleteProfile(profileId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
