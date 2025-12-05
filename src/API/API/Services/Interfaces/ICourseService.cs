using API.Models.DTOs.Course;

namespace API.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseResponseDTO>> GetAllCoursesAsync();
        Task<CourseResponseDTO?> GetCourseByIdAsync(Guid id);
        Task CreateCourseAsync(CourseRequestDTO dto);
        Task<bool> UpdateCourseAsync(Guid id, CourseRequestDTO dto);
        Task<bool> DeleteCourseAsync(Guid id);
    }
}
