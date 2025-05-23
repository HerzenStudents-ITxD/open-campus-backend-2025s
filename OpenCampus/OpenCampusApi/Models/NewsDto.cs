﻿namespace OpenCampus.Models
{
    public class NewsDto
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public bool IsPublished { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime PublishedAt { get; set; }
    }

}

