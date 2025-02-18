using System.Text.Json.Serialization;

namespace LU2Raf.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public User() { }

        public User(string username, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
        }
    }
    public static class UserStore
    {
        public static List<User> Users { get; } = new List<User>();
    }
}
