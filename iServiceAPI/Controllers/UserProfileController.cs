using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
using iServiceServices.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;

        public UserProfileController(IConfiguration configuration)
        {
            _userProfileService = new UserProfileService(configuration);
        }

        [HttpGet]
        public ActionResult<List<UserProfile>> Get()
        {
            var result = _userProfileService.GetAllUserProfiles();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("{userProfileId}")]
        public ActionResult<UserProfile> GetById(int userProfileId)
        {
            var result = _userProfileService.GetUserProfileById(userProfileId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<UserProfile> Post([FromBody] UserProfileInsert profileModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userProfileService.AddUserProfile(profileModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { userProfileId = result.Value.UserProfileId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost("UpdateProfileImage")]
        public ActionResult<string> UpdateProfileImage([FromForm] ImageModel profileModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userProfileService.UpdateProfileImage(profileModel);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{userProfileId}")]
        public ActionResult<UserProfile> Put(int userProfileId, [FromBody] UserProfileUpdate profile)
        {
            if (userProfileId != profile.UserProfileId)
            {
                return BadRequest();
            }

            var result = _userProfileService.UpdateUserProfile(profile);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{userProfileId}")]
        public IActionResult Delete(int userProfileId)
        {
            var result = _userProfileService.DeleteUserProfile(userProfileId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
