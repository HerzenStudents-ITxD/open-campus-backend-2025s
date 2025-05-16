using Microsoft.AspNetCore.Mvc;
using OpenCampus.Data;
using Microsoft.EntityFrameworkCore;

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
}

