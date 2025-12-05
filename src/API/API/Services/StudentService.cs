using API.Models;
using API.Models.DTOs.Student;
using API.Models.DTOs.StudentCourse;
using API.Repositories;
using API.Services.Interfaces;

namespace API.Services
{
    public class StudentService : IStudentService
    {
        private StudentRepository _studentRepository;

        public StudentService(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task CreateStudentAsync(StudentRequestDTO dto)
        {
            if (await _studentRepository.SearchStudentByDocument(dto.Document) == 1)
                throw new ArgumentException("Este estudante já está cadastrado no sistema!");

            var student = new Student(
                dto.Name,
                dto.Email,
                dto.Document,
                String.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone,
                dto.Birthdate
            );

            await _studentRepository.CreateStudentAsync(student);
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            if (!await _studentRepository.VerifyExistStudentAsync(id))
                throw new ArgumentException("Estudante não encontrado!");

            await _studentRepository.DeleteStudentAsync(id);
        }

        public async Task EnrollingStudentInCourseAsync(Guid studentId, Guid courseId, StudentCourseRequestDTO dto)
        {
            if (!await _studentRepository.VerifyExistStudentAsync(studentId))
                throw new ArgumentException("Esse estudante não existe!");

            if (!await _studentRepository.VerifyExistCourseAsync(courseId))
                throw new ArgumentException("Esse curso não existe!");

            if (await _studentRepository.VerifyStudentEnrollingInCourseAsync(studentId, courseId))
                throw new ArgumentException("O Estudante já está cadastrado nesse curso!");

            var registration = new StudentCourse(
                courseId,
                studentId,
                dto.Progress,
                dto.Favorite
            );

            await _studentRepository.EnrollingStudentInCourseAsync(registration);
        }

        public async Task<List<StudentGetAllResponseDTO>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            if (students is null || students.Count() == 0)
                return null;

            return students;
        }

        public async Task<StudentGetByIdResponseDTO?> GetStudentByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            return student;
        }

        public async Task<StudentWithCoursesResponseDTO?> GetStudentCoursesAsync(Guid studentId)
        {
            var student = await _studentRepository.GetStudentCoursesAsync(studentId);
            
            return student;
        }

        public async Task UpdateProgressStudentCourseAsync(Guid studentId, Guid courseId, StudentUpdateProgressDTO updateProgressDTO)
        {
            if (updateProgressDTO.Progress < 1 || updateProgressDTO.Progress > 100)
                throw new ArgumentException("Insira um valor entre 1 e 100");

            if (!await _studentRepository.VerifyExistStudentAsync(studentId))
                throw new ArgumentException("Esse estudante não existe!");

            if (!await _studentRepository.VerifyExistCourseAsync(courseId))
                throw new ArgumentException("Esse curso não existe!");

            if (!await _studentRepository.VerifyStudentEnrollingInCourseAsync(studentId, courseId))
                throw new ArgumentException("Este Estudante não está matriculado em nenhum curso!");

            var currentProgress = await _studentRepository.VerifyProgressToStudentInCourseAsync(studentId, courseId);

            if (currentProgress == 100)
                throw new ArgumentException("Estudante já concluiu o curso!");
            else if (currentProgress >= updateProgressDTO.Progress)
                throw new ArgumentException($"Não foi possível atualizar o progresso! Insira um valor maior que {currentProgress}%!");

            await _studentRepository.UpdateProgressStudentCourseAsync(studentId, courseId, updateProgressDTO);
        }

        public async Task UpdateStudentAsync(Guid id, StudentUpdateDTO student)
        {
            var studentFromDB = await _studentRepository.SearchStudentToUpdateAsync(id);
            if (studentFromDB is null)
                throw new ArgumentException("Estudante não encontrado para atualização");

            var studentToUpdate = new StudentUpdateDTO
            {
                Name = String.IsNullOrWhiteSpace(student.Name) ? studentFromDB.Name : student.Name,
                Email = String.IsNullOrWhiteSpace(student.Email) ? studentFromDB.Email : student.Email,
                Phone = String.IsNullOrWhiteSpace(student.Phone) ? studentFromDB.Phone : student.Phone
            };

            await _studentRepository.UpdateStudentAsync(id, studentToUpdate);
        }
    }
}
