using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Handlers;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private UserHandler _userHandler;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _userHandler = new UserHandler();
        }

        [HttpGet]
        [Route("GetUserByEmail")]
        public IActionResult GetUserByEmail(string email)
        {
            UserModel user = _userHandler.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            else
            {
                return Ok(user);
            }
        }

    }
}
