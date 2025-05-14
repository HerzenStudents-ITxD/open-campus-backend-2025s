using Microsoft.AspNetCore.Http;

namespace OpenCampus.Models
{
    public class EventDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; }
        public string Location { get; set; } = "";
        public string Organizer { get; set; } = "";
        public IFormFile? Image { get; set; }
        public string CreatedBy { get; set; } = "";
    }
}
