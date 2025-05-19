using Microsoft.AspNetCore.Mvc;
using OpenCampus.Data;
using Microsoft.EntityFrameworkCore;
using OpenCampus.Models;

public class BookingDto
{
    public string UserId { get; set; } = "";
    public int LocationId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Purpose { get; set; } = "";
}

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookingController(ApplicationDbContext context)
    {
        _context = context;
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
        if (dto == null)
            return BadRequest("Данные не переданы");

        var booking = new Booking
        {
            UserId = dto.UserId,
            LocationId = dto.LocationId,
            DateStart = dto.DateStart,
            DateEnd = dto.DateEnd,
            Purpose = dto.Purpose,
            Status = "Ожидает",
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Бронирование успешно создано" });
    }
}
