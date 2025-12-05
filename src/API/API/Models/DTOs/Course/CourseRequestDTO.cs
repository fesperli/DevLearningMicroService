namespace API.Models.DTOs.Course
{
    public class CourseRequestDTO
    {
        public string Tag { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public int Level { get; init; } = 0;
        public int DurationInMinutes { get; init; } = 0;
        public DateTime CreateDate { get; init; }
        public DateTime LastUpdateDate { get; init; }
        public bool Active { get; init; } = false;
        public bool Free { get; init; } = false;
        public bool Featured { get; init; } = false;
        public Guid AuthorId { get; init; }
        public Guid CategoryId { get; init; }
        public string Tags { get; init; } = string.Empty;
    }
}
