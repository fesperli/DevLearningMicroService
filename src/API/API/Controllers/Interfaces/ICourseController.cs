using API.Models.DTOs.Course;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Interfaces
{
    public interface ICourseController
    {
        Task<ActionResult<List<CourseResponseDTO>>> GetAllCourses();
        Task<ActionResult<CourseResponseDTO>> GetCourseById(Guid id);
        Task<ActionResult> CreateCourse(CourseRequestDTO dto);
        Task<ActionResult> UpdateCourse(Guid id, CourseRequestDTO dto);
        Task<ActionResult> DeleteCourse(Guid id);
    }
}
