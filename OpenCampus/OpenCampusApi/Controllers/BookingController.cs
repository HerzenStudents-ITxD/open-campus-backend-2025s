using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCampus.Data;
using OpenCampus.Models;
using System.ComponentModel.DataAnnotations;

namespace OpenCampusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class BookingDto
        {
            [Required]
            public DateTime DateStart { get; set; }

            [Required]
            public DateTime DateEnd { get; set; }

            public string Purpose { get; set; } = "";
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var bookings = _context.Bookings
                .Include(b => b.Location)
                .Include(b => b.User)
                .Select(b => new
                {
                    b.Id,
                    Date = $"{b.DateStart:dd.MM.yyyy HH:mm}–{b.DateEnd:HH:mm}",
                    Location = b.Location != null ? b.Location.Name : "",
                    User = b.User != null ? b.User.Name : "",
                    b.Status,
                    b.Purpose
                })
                .ToList();

            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fakeUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var fakeLocationId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var booking = new Booking
            {
                UserId = fakeUserId,
                LocationId = fakeLocationId,
                DateStart = dto.DateStart,
                DateEnd = dto.DateEnd,
                Purpose = dto.Purpose,
                Status = "Ожидает"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Бронирование успешно создано" });
        }
    }
}
