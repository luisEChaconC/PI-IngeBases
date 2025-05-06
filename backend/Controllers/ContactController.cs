using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactRepository _contactRepository; // Repository to handle database operations for contacts

        // Constructor to initialize the contact repository
        public ContactController()
        {
            _contactRepository = new ContactRepository(); // Initialize the contact repository
        }

        /// <summary>
        /// HTTP POST endpoint to create a new contact.
        /// </summary>
        /// <param name="contact">The contact model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateContact")]
        public IActionResult CreateContact([FromBody] ContactModel contact)
        {
            try
            {
                // Validate the contact model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the contact
                string contactId = _contactRepository.CreateContact(contact);

                // Return 201 Created response with the ID and a success message
                return Created("", new { id = contactId, message = "Contact created successfully" });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the contact." });
            }
        }

        /// <summary>
        /// Creates a contact dependency.
        /// </summary>
        /// <param name="contact">The contact model to create.</param>
        /// <param name="personId">The ID of the associated person.</param>
        [NonAction]
        public void CreateContactDependency(ContactModel contact, string personId)
        {
            contact.PersonId = personId;

            var contactResult = CreateContact(contact) as ObjectResult;
            if (contactResult == null || contactResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create contact.");
            }
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve all contacts by the associated person/company ID.
        /// </summary>
        /// <param name="id">The ID of the person or company whose contacts to retrieve.</param>
        /// <returns>The list of contacts if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet]
        [Route("GetContactsById/{id}")]
        public IActionResult GetContactsById(string id)
        {
            try
            {
                // Call the repository method to get the list of contacts
                var contacts = _contactHandler.GetContactsById(id);

                // Check if any contacts were found
                if (contacts == null || !contacts.Any())
                {
                    // Return 404 Not Found if no contacts were found
                    return NotFound(new { message = "No contacts found for the specified ID." });
                }

                // Return 200 OK response with the list of contacts
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with the error message
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving contacts.", error = ex.Message });
            }
        }
    }
}