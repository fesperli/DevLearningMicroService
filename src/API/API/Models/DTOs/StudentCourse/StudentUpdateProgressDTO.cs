namespace API.Models.DTOs.StudentCourse
{
    public class StudentUpdateProgressDTO
    {
        public byte Progress { get; init; }
        public DateTime LastUpdateDate { get; init; } = DateTime.Now;
    }
}
