using Microsoft.AspNetCore.Mvc;
using OpenCampus.Data;
using OpenCampus.Models;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private static List<Location> locations = new List<Location>();

    [HttpGet]
    public IActionResult GetAll() => Ok(locations);

    [HttpPost]
    public IActionResult Create([FromForm] LocationDto dto, IFormFile image)
    {
        var fileName = Path.GetFileName(image.FileName);
        var path = Path.Combine("wwwroot/uploads", fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        var location = new Location
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Capacity = dto.Capacity,
            ImagePath = "/uploads/" + fileName
        };

        locations.Add(location);
        return Ok(location);
    }
}
