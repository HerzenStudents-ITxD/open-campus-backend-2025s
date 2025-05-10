public class LocationDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Capacity { get; set; }

    public IFormFile? Image { get; set; } // ✅ теперь необязательно
}
