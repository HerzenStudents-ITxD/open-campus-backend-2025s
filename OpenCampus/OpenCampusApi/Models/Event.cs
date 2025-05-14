using System.ComponentModel.DataAnnotations;

namespace OpenCampus.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Location { get; set; } = "";

        [Required]
        public string Organizer { get; set; } = "";

        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; } = "";
    }
}
