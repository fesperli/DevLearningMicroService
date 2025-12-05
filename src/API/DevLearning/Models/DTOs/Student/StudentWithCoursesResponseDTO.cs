namespace API.Models.DTOs.Student
{
    public class StudentWithCoursesResponseDTO
    {
        public Guid StudentId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public List<CourseOfStudentDTO> Courses { get; init; } = new();
    }
}
