using Microsoft.AspNetCore.Mvc;
using OpenCampus.Data;
using OpenCampus.Models;
using System;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LocationController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var locations = _context.Locations
            .Select(loc => new
            {
                id = loc.Id,
                name = loc.Name,
                description = loc.Description,
                capacity = loc.Capacity,
                imageUrl = loc.ImagePath
            })
            .ToList();

        return Ok(locations);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public IActionResult Create([FromForm] LocationDto dto)
    {
        var fileName = Path.GetFileName(dto.Image.FileName);
        var path = Path.Combine("wwwroot/uploads", fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            dto.Image.CopyTo(stream);
        }

        var location = new Location
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Capacity = dto.Capacity,
            ImagePath = "/uploads/" + fileName
        };

        _context.Locations.Add(location);
        _context.SaveChanges();

        return Ok(location);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var location = _context.Locations.Find(id);
        if (location == null) return NotFound();

        _context.Locations.Remove(location);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public IActionResult Update(Guid id, [FromForm] LocationDto dto)
    {
        var location = _context.Locations.Find(id);
        if (location == null) return NotFound();

        location.Name = dto.Name;
        location.Description = dto.Description;
        location.Capacity = dto.Capacity;

        if (dto.Image != null)
        {
            var fileName = Path.GetFileName(dto.Image.FileName);
            var path = Path.Combine("wwwroot/uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }
            location.ImagePath = "/uploads/" + fileName;
        }

        _context.SaveChanges();
        return Ok(location);
    }
}




