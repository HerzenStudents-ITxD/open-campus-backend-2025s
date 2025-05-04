namespace OpenCampus.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
