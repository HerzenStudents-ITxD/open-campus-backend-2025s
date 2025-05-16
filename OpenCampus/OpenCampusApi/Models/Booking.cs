using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCampus.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public Guid LocationId { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        public string Purpose { get; set; } = "";
        public string Status { get; set; } = "Ожидает";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
