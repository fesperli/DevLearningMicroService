using DevLearning.Models.DTOs.Student;
using DevLearning.Models.DTOs.StudentCourse;

namespace DevLearning.StudentAPI.Services.Interface
{
    public interface IStudentService
    {
        public Task CreateStudentAsync(StudentRequestDTO dto);
        public Task<List<StudentGetAllResponseDTO>> GetAllStudentsAsync();
        public Task<StudentGetByIdResponseDTO?> GetStudentByIdAsync(Guid id);
        public Task<StudentWithCoursesResponseDTO?> GetStudentCoursesAsync(Guid studentId);
        public Task UpdateStudentAsync(Guid id, StudentUpdateDTO student);
        public Task DeleteStudentAsync(Guid id);
        public Task EnrollingStudentInCourseAsync(Guid studentId, Guid courseId, StudentCourseRequestDTO dto);
        public Task UpdateProgressStudentCourseAsync(Guid studentId, Guid courseId, StudentUpdateProgressDTO updateProgressDTO);
    }
}
