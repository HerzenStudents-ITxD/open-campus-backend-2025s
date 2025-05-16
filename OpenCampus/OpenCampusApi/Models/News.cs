using OpenCampus.Models;
using System.ComponentModel.DataAnnotations;

public class News
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Content { get; set; } = "";

    public string? ImagePath { get; set; }

    public DateTime PublishedAt { get; set; }

    public bool IsPublished { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
