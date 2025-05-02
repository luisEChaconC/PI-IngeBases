namespace backend.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public UserModel()
        {
            Id = 0;
            Email = string.Empty;
            Password = string.Empty;
            IsAdmin = false;
        }

        public UserModel(int id, string email, string password, bool isAdmin)
        {
            Id = id;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
        }
    }
}
