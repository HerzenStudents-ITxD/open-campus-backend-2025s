using Microsoft.AspNetCore.Mvc;
using OpenCampus.Data;
using OpenCampus.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        return Ok(new
        {
            user.Id,
            user.Name,
            user.Email,
            user.Bio,
            AvatarUrl = user.AvatarPath
        });
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public IActionResult Update(Guid id, [FromForm] UserDto dto)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Bio = dto.Bio;

        if (dto.Avatar != null)
        {
            var fileName = $"{Guid.NewGuid()}_{dto.Avatar.FileName}";
            var path = Path.Combine("wwwroot/uploads", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            dto.Avatar.CopyTo(stream);
            user.AvatarPath = "/uploads/" + fileName;
        }

        _context.SaveChanges();
        return Ok(user);
    }

    [HttpPost("change-password/{id}")]
    public IActionResult ChangePassword(Guid id, [FromBody] ChangePasswordDto dto)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        if (user.PasswordHash != dto.OldPassword) // упрощённая проверка
            return BadRequest("Неверный текущий пароль");

        if (dto.NewPassword.Length < 6)
            return BadRequest("Новый пароль слишком короткий");

        user.PasswordHash = dto.NewPassword;
        _context.SaveChanges();
        return Ok("Пароль успешно изменён");
    }
}
