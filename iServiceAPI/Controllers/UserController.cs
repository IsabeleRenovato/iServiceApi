using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services;
using iServiceServices.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace iServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(IConfiguration configuration)
        {
            _userService = new UserService(configuration);
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            var result = _userService.GetAllUsers();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("GetById/{userId}")]
        public ActionResult<User> GetById(int userId)
        {
            var result = _userService.GetUserById(userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetUserInfoById/{userId}")]
        public ActionResult<UserInfo> GetUserInfoById(int userId)
        {
            var result = _userService.GetUserInfoById(userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpGet("GetByEmail/{email}")]
        public ActionResult<User> GetByEmail(string email)
        {
            var result = _userService.GetUserByEmail(email);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<User> Post([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.AddUser(userModel);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { userId = result.Value.UserId }, result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{userId}")]
        public ActionResult<User> Put(int userId, [FromBody] User user)
        {
            if (userId != user.UserId)
            {
                return BadRequest();
            }

            var result = _userService.UpdateUser(user);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            var result = _userService.DeleteUser(userId);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.ErrorMessage });
        }
    }
}
