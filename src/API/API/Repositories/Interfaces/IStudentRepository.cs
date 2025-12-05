using API.Models;
using API.Models.DTOs.Student;
using API.Models.DTOs.StudentCourse;

namespace API.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        public Task CreateStudentAsync(Student student);
        public Task<List<StudentGetAllResponseDTO>> GetAllStudentsAsync();
        public Task<StudentGetByIdResponseDTO?> GetStudentByIdAsync(Guid id);
        public Task<StudentWithCoursesResponseDTO?> GetStudentCoursesAsync(Guid studentId);
        public Task<StudentUpdateDTO?> SearchStudentToUpdateAsync(Guid id);
        public Task UpdateStudentAsync(Guid id, StudentUpdateDTO student);
        public Task DeleteStudentAsync(Guid id);
        public Task<bool> VerifyExistCourseAsync(Guid courseId);
        public Task<bool> VerifyExistStudentAsync(Guid studentId);
        public Task EnrollingStudentInCourseAsync(StudentCourse studentCourse);
        public Task<byte> VerifyProgressToStudentInCourseAsync(Guid studentId, Guid courseId);
        public Task<bool> VerifyStudentEnrollingInCourseAsync(Guid studentId, Guid courseId);
        public Task UpdateProgressStudentCourseAsync(Guid studentId, Guid courseId, StudentUpdateProgressDTO updateProgressDTO);
        public Task<int> SearchStudentByDocument(string document);
    }
}
