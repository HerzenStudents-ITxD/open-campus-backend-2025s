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

        locations.Add(location);
        return Ok(location);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var location = locations.FirstOrDefault(l => l.Id == id);
        if (location == null) return NotFound();

        locations.Remove(location);
        return NoContent();
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public IActionResult Update(Guid id, [FromForm] LocationDto dto)
    {
        var location = locations.FirstOrDefault(l => l.Id == id);
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

        return Ok(location);
    }

}



