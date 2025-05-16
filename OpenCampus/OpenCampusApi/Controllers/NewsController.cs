using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCampus.Data;
using OpenCampus.Models;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public NewsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var news = _context.News
            .Include(n => n.User)
            .OrderByDescending(n => n.PublishedAt)
            .Select(n => new
            {
                n.Id,
                n.Title,
                n.Content,
                n.ImagePath,
                n.PublishedAt,
                n.IsPublished,
                Author = n.User != null ? n.User.Name : "Неизвестный"
            })
            .ToList();

        return Ok(news);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public IActionResult Create([FromForm] NewsDto dto)
    {
        string? imagePath = null;

        if (dto.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
            var fullPath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            dto.Image.CopyTo(stream);
            imagePath = "/uploads/" + fileName;
        }

        var news = new News
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            ImagePath = imagePath,
            PublishedAt = DateTime.UtcNow,
            IsPublished = dto.IsPublished,
            UserId = dto.UserId
        };

        _context.News.Add(news);
        _context.SaveChanges();
        return Ok(news);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public IActionResult Update(Guid id, [FromForm] NewsDto dto)
    {
        var news = _context.News.Find(id);
        if (news == null) return NotFound();

        news.Title = dto.Title;
        news.Content = dto.Content;
        news.IsPublished = dto.IsPublished;

        if (dto.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
            var fullPath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            dto.Image.CopyTo(stream);
            news.ImagePath = "/uploads/" + fileName;
        }

        _context.SaveChanges();
        return Ok(news);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var news = _context.News.Find(id);
        if (news == null) return NotFound();

        _context.News.Remove(news);
        _context.SaveChanges();
        return NoContent();
    }
}

