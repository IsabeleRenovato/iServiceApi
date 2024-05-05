using iServiceRepositories.Repositories.Models;
using iServiceServices.Services;
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

        [HttpGet("{userId}")]
        public ActionResult<User> GetById(int userId)
        {
            var result = _userService.GetUserById(userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public ActionResult<User> Post([FromBody] UserInsert userModel)
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
        public ActionResult<User> Put(int userId, [FromBody] UserUpdate user)
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
