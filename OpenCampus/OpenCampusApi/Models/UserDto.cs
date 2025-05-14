namespace OpenCampus.Models
{
    public class UserDto
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Bio { get; set; } = "";
        public IFormFile? Avatar { get; set; }
    }
}

