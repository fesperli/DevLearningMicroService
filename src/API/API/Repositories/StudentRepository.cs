using API.Database;
using API.Models;
using API.Models.DTOs.Student;
using API.Models.DTOs.StudentCourse;
using API.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(DbConnectionFactory dbConnection, ILogger<StudentRepository> logger)
        {
            _connection = dbConnection.GetConnection();
            _logger = logger;
        }

        public async Task CreateStudentAsync(Student student)
        {
            var sql = @"INSERT INTO Student (Id, Name, Email, Document, Phone, Birthdate, CreateDate)
                        VALUES (@Id, @Name, @Email, @Document, @Phone, @Birthdate, @CreateDate)";

            await _connection.ExecuteAsync(sql, new
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Document = student.Document,
                Phone = student.Phone,
                Birthdate = student.Birthdate.ToDateTime(new TimeOnly(0, 0)),
                CreateDate = student.CreateDate
            });
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            var sql = "DELETE FROM StudentCourse WHERE StudentId = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });

            sql = "DELETE FROM Student WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task EnrollingStudentInCourseAsync(StudentCourse studentCourse)
        {
            var sql = @"INSERT INTO StudentCourse (CourseId, StudentId, Progress, Favorite, StartDate, LastUpdateDate)
                        VALUES (@CourseId, @StudentId, @Progress, @Favorite, @StartDate, @LastUpdateDate)";

            await _connection.ExecuteAsync(sql, new
            {
                CourseId = studentCourse.CourseId,
                StudentId = studentCourse.StudentId,
                Progress = studentCourse.Progress,
                Favorite = studentCourse.Favorite,
                StartDate = studentCourse.StartDate,
                LastUpdateDate = studentCourse.LastUpdateDate
            });
        }

        public async Task<List<StudentGetAllResponseDTO>> GetAllStudentsAsync()
        {
            var sql = @"SELECT Id, Name, Email, Phone, Birthdate FROM Student ORDER BY CreateDate";

            return (await _connection.QueryAsync<StudentGetAllResponseDTO>(sql)).ToList();
        }

        public async Task<StudentGetByIdResponseDTO?> GetStudentByIdAsync(Guid id)
        {
            var sql = @"SELECT
                            Id, [Name], Email, Document, Phone, Birthdate, CreateDate
                        FROM Student
                        WHERE Id = @Id";

            var student = await _connection.QueryFirstOrDefaultAsync<StudentGetByIdResponseDTO>(sql, new { Id = id });
            return student;
        }

        public async Task<StudentWithCoursesResponseDTO?> GetStudentCoursesAsync(Guid studentId)
        {
            var sql = @"SELECT 
                            s.Id AS StudentId, s.[Name], s.Email,
                            c.Id AS CourseId, c.Title AS CourseTitle, c.Summary, c.[Url], c.[Level], c.DurationInMinutes,
                            ca.Title AS CategoryTitle,
                            sc.Progress, sc.Favorite, sc.StartDate, sc.LastUpdateDate
                        FROM Student s
                        LEFT JOIN StudentCourse sc
                        ON s.Id = sc.StudentId
                        LEFT JOIN Course c
                        ON sc.CourseId = c.Id
                        LEFT JOIN Category ca
                        ON c.CategoryId = ca.Id
                        WHERE s.Id = @Id
                        ORDER BY 
                            sc.Favorite DESC,
                            sc.StartDate";

            var lookup = new Dictionary<Guid, StudentWithCoursesResponseDTO>();
            await _connection.QueryAsync<StudentWithCoursesResponseDTO, CourseOfStudentDTO, StudentWithCoursesResponseDTO>(sql,
                (student, course) =>
                {
                    if (!lookup.TryGetValue(student.StudentId, out var dto))
                    {
                        dto = student;
                        lookup.Add(student.StudentId, dto);
                    }

                    if (course is not null)
                        dto.Courses.Add(course);

                    return student;
                },
                new { Id = studentId },
                splitOn: "CourseId"
            );

            var student = lookup.Values.FirstOrDefault();
            return student;
        }

        public async Task<int> SearchStudentByDocument(string document)
        {
            var sql = @"SELECT COUNT(*) FROM Student WHERE Document = @Document";
            return await _connection.QueryFirstOrDefaultAsync<int>(sql, new { Document = document });
        }

        public async Task<StudentUpdateDTO?> SearchStudentToUpdateAsync(Guid id)
        {
            var sql = @"SELECT Name, Email, Phone FROM Student WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<StudentUpdateDTO>(sql, new { Id = id });
        }

        public async Task UpdateProgressStudentCourseAsync(Guid studentId, Guid courseId, StudentUpdateProgressDTO updateProgressDTO)
        {
            var sql = @"UPDATE StudentCourse SET
                            Progress = @Progress,
                            LastUpdateDate = @LastUpdateDate
                        WHERE StudentId = @StudentId AND CourseId = @CourseId";

            await _connection.ExecuteAsync(sql, new
            {
                Progress = updateProgressDTO.Progress,
                LastUpdateDate = updateProgressDTO.LastUpdateDate,
                StudentId = studentId,
                CourseId = courseId
            });
        }

        public async Task UpdateStudentAsync(Guid id, StudentUpdateDTO student)
        {
            var sql = @"UPDATE Student SET
                            Name = @Name,
                            Email = @Email,
                            Phone = @Phone
                        WHERE Id = @Id";

            await _connection.ExecuteAsync(sql, new
            {
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                Id = id
            });
        }

        public async Task<bool> VerifyExistCourseAsync(Guid courseId)
        {
            var sql = @"SELECT CASE WHEN EXISTS (SELECT 1 FROM Course WHERE Id = @Id) THEN 1 ELSE 0 END";
            var exist = await _connection.QueryFirstOrDefaultAsync<bool>(sql, new { Id = courseId });
            return exist;
        }

        public async Task<bool> VerifyExistStudentAsync(Guid studentId)
        {
            var sql = @"SELECT CASE WHEN EXISTS (SELECT 1 FROM Student WHERE Id = @Id) THEN 1 ELSE 0 END";
            var exist = await _connection.QueryFirstOrDefaultAsync<bool>(sql, new { Id = studentId });
            return exist;
        }

        public async Task<byte> VerifyProgressToStudentInCourseAsync(Guid studentId, Guid courseId)
        {
            var sql = "SELECT Progress FROM StudentCourse WHERE StudentId = @StudentId AND CourseId = @CourseId";

            return await _connection.QueryFirstOrDefaultAsync<byte>(sql, new
            {
                StudentId = studentId,
                CourseId = courseId
            });
        }

        public async Task<bool> VerifyStudentEnrollingInCourseAsync(Guid studentId, Guid courseId)
        {
            var sql = @"SELECT CASE WHEN EXISTS 
                            (SELECT 1 FROM StudentCourse WHERE StudentId = @StudentId AND CourseId = @CourseId)
                        THEN 1 ELSE 0 END";

            return await _connection.QueryFirstOrDefaultAsync<bool>(sql, new
            {
                StudentId = studentId,
                CourseId = courseId
            });
        }
    }
}
