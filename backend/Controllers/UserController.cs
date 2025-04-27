using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Repositories;
using backend.Models;

namespace backend.Controllers
{
    // Defines the route for the API controller as "api/User"
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger; // Logger for logging information or errors
        private UserRepository _userHandler; // Repository to handle database operations for users

        // Constructor to initialize the logger and the user repository
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger; // Injected logger instance
            _userHandler = new UserRepository(); // Initialize the user repository
        }

        // HTTP GET endpoint to retrieve a user by their email
        [HttpGet]
        [Route("GetUserByEmail")]
        public IActionResult GetUserByEmail(string email)
        {
            // Call the repository method to get the user by email
            UserModel user = _userHandler.GetUserByEmail(email);

            // If no user is found, return a 404 Not Found response with a message
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            else
            {
                // If the user is found, return a 200 OK response with the user data
                return Ok(user);
            }
        }
    }
}