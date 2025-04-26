namespace backend.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserModel()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public UserModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
