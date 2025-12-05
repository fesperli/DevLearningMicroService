namespace API.Models.DTOs.Course
{
    public class CourseResponseDTO
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Tag { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public int Level { get; init; } = 0;
        public int DurationInMinutes { get; init; } = 0;
        public DateTime CreateDate { get; init; } = DateTime.Now;
        public DateTime LastUpdateDate { get; init; } = DateTime.Now;
        public bool Active { get; init; } = false;
        public bool Free { get; init; } = false;
        public bool Featured { get; init; } = false;
        public Guid AuthorId { get; init; } 
        public Guid CategoryId { get; init; }
        public string Tags { get; init; } = string.Empty;
    }
}
