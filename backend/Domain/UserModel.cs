using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class UserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(320, ErrorMessage = "Email cannot exceed 320 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "IsAdmin is required.")]
        public bool IsAdmin { get; set; }

        public UserModel()
        {
            Id = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            IsAdmin = false;
        }

        public UserModel(string id, string email, string password, bool isAdmin)
        {
            Id = id;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
        }
    }
}