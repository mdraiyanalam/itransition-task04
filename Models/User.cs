using Microsoft.AspNetCore.Mvc;

namespace task04UserManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Status { get; set; } = "Unverified"; // Unverified, Active, Blocked
        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginTime { get; set; }
    }
}
