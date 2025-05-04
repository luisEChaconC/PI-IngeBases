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
        private UserRepository _userHandler; // Repository to handle database operations for users

        // Constructor to initialize the user repository
        public UserController()
        {
            _userHandler = new UserRepository(); // Initialize the user repository
        }

        // HTTP GET endpoint to retrieve a user by their email
        [HttpGet]
        [Route("GetUserByEmail")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                // Call the repository method to get the user by email
                UserModel user = _userHandler.GetUserByEmail(email);

                // If no user is found, return a 404 Not Found response with a message
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                // If the user is found, return a 200 OK response with the user data
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the user." });
            }
        }

        // HTTP POST endpoint to create a new user
        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser([FromBody] UserModel user)
        {
            try
            {
                // Validate the user model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the user
                string userId = _userHandler.CreateUser(user);

                // Return 201 Created response with a success message
                return Created("", new {id = userId, message = "User created successfully" });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the user." });
            }
        }

        /// <summary>
        /// Creates a user dependency and returns the generated ID.
        /// </summary>
        /// <param name="user">The user model to create.</param>
        /// <returns>The ID of the created user.</returns>
        [NonAction]
        public string CreateUserDependency(UserModel user)
        {
            var userResult = CreateUser(user) as ObjectResult;
            if (userResult == null || userResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create user.");
            }

            var userId = (userResult.Value as dynamic)?.id;
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Failed to retrieve user ID.");
            }

            return userId;
        }
    }
}