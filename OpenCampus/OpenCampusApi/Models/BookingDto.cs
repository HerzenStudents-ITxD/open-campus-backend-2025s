namespace OpenCampus.Models
{
    public class BookingDto
    {
        public Guid UserId { get; set; }
        public Guid LocationId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Purpose { get; set; } = "";
    }
}
