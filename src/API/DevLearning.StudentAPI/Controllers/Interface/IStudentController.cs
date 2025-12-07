using DevLearning.Models.DTOs.Student;
using DevLearning.Models.DTOs.StudentCourse;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.StudentAPI.Controllers.Interface
{
    public interface IStudentController
    {
        public Task<ActionResult> CreateStudentAsync(StudentRequestDTO dto);
        public Task<ActionResult<List<StudentGetAllResponseDTO>>> GetAllStudentsAsync();
        public Task<ActionResult<StudentGetByIdResponseDTO>> GetStudentByIdAsync(Guid id);
        //public Task<ActionResult<StudentWithCoursesResponseDTO>> GetStudentCoursesAsync(Guid id);
        public Task<ActionResult> UpdateStudentAsync(Guid id, StudentUpdateDTO student);
        public Task<ActionResult> DeleteStudentAsync(Guid id);
        public Task<ActionResult> EnrollingStudentInCourseAsync(Guid studentId, Guid courseId, StudentCourseRequestDTO dto);
        public Task<ActionResult> UpdateProgressStudentCourseAsync(Guid studentId, Guid courseId, StudentUpdateProgressDTO updateProgressDTO);
    }
}
