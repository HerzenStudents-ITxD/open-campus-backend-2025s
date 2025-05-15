using System.ComponentModel.DataAnnotations;

namespace OpenCampus.Models
{
    public enum UserRole
    {
        Visitor,
        Editor
    }
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string Bio { get; set; } = "";

        public string? AvatarPath { get; set; }

        [Required]
        public string PasswordHash { get; set; } = "";
        
        [Required]
        public UserRole Role { get; set; } = UserRole.Visitor;
    }
}

