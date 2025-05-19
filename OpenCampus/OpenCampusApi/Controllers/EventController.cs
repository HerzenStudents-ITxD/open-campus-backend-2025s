using Microsoft.AspNetCore.Mvc;
using OpenCampus.Data;
using OpenCampus.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EventController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Получить мероприятие по ID
    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var eventItem = _context.Events.Find(id);
        if (eventItem == null) return NotFound();

        return Ok(new
        {
            eventItem.Id,
            eventItem.Title,
            eventItem.Description,
            eventItem.Date,
            eventItem.Location,
            eventItem.Organizer,
            ImageUrl = eventItem.ImagePath,
            eventItem.CreatedAt,
            eventItem.CreatedBy
        });
    }

    // Обновить мероприятие
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public IActionResult Update(Guid id, [FromForm] EventDto dto)
    {
        var eventItem = _context.Events.Find(id);
        if (eventItem == null) return NotFound();

        eventItem.Title = dto.Title;
        eventItem.Description = dto.Description;
        eventItem.Date = dto.Date;
        eventItem.Location = dto.Location;
        eventItem.Organizer = dto.Organizer;

        if (dto.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
            var path = Path.Combine("wwwroot/uploads", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            dto.Image.CopyTo(stream);
            eventItem.ImagePath = "/uploads/" + fileName;
        }

        _context.SaveChanges();
        return Ok(eventItem);
    }

    // Создать новое мероприятие
    [HttpPost]
    [Consumes("multipart/form-data")]
    public IActionResult Create([FromForm] EventDto dto)
    {
        var eventItem = new Event
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            Location = dto.Location,
            Organizer = dto.Organizer,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = dto.CreatedBy,
        };

        if (dto.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
            var path = Path.Combine("wwwroot/uploads", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            dto.Image.CopyTo(stream);
            eventItem.ImagePath = "/uploads/" + fileName;
        }

        _context.Events.Add(eventItem);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = eventItem.Id }, eventItem);
    }

    public class DeleteEventDto
    {
        public string Reason { get; set; }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id, [FromBody] DeleteEventDto dto)
    {
        var eventItem = _context.Events.Find(id);
        if (eventItem == null) return NotFound();

        // Можно логировать dto.Reason, если нужно
        Console.WriteLine($"Удаление мероприятия: {id}, причина: {dto.Reason}");

        _context.Events.Remove(eventItem);
        _context.SaveChanges();
        return NoContent();
    }

    // Получить список всех мероприятий
    [HttpGet]
    public IActionResult GetAll()
    {
        var events = _context.Events
            .OrderByDescending(e => e.Date)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Description,
                e.Date,
                e.Location,
                e.Organizer,
                Image = e.ImagePath,
                e.CreatedAt,
                e.CreatedBy
            })
            .ToList();

        return Ok(events);
    }

}

