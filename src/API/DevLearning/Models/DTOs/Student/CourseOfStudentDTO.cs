namespace API.Models.DTOs.Student
{
    public class CourseOfStudentDTO
    {
        public Guid CourseId { get; init; }
        public string CourseTitle { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public byte Level { get; init; }
        public int DurationInMinutes { get; init; }
        public string CategoryTitle { get; init; }
        public byte Progress { get; init; }
        public bool Favorite { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime LastUpdateDate { get; init; }
    }
}
