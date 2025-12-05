using API.Models.DTOs.Course;
using Blog.API.Models;

namespace API.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<CourseResponseDTO>> GetAllCoursesAsync();
        Task<CourseResponseDTO?> GetCourseByIdAsync(Guid id);
        Task CreateCourseAsync(Course course);
        Task UpdateCourseAsync(Guid id, Course course);
        Task DeleteCourseAsync(Guid id);
    }
}
