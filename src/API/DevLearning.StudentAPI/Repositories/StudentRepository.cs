using DevLearning.Models;
using DevLearning.Models.DTOs.Student;
using DevLearning.Models.DTOs.StudentCourse;
using DevLearning.StudentAPI.Data;
using DevLearning.StudentAPI.Repositories.Interface;
using MongoDB.Bson;
using MongoDB.Driver;


namespace DevLearning.StudentAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MongoDBConnection _mongoConnection;
        private readonly IMongoCollection<Student> _studentsCollection;
        private readonly IMongoCollection<StudentCourse> _studentCourseCollection;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(MongoDBConnection connection, ILogger<StudentRepository> logger)
        {
            _mongoConnection = connection;
            _studentsCollection = _mongoConnection.GetStudentMongoCollection();
            _studentCourseCollection = _mongoConnection.GetStudentCourseMongoCollection();
            _logger = logger;
        }

        public async Task CreateStudentAsync(Student student)
        {
            await _studentsCollection.InsertOneAsync(student);                  
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            await _studentsCollection.DeleteOneAsync(id.ToString());
        }

        public async Task EnrollingStudentInCourseAsync(StudentCourse studentCourse)
        {
           await _studentCourseCollection.InsertOneAsync(studentCourse);
        }

        //TODO: Foi usado o OrderBy por CreateDate
        public async Task<List<StudentGetAllResponseDTO>> GetAllStudentsAsync()
        {
            var students = (await _studentsCollection.FindAsync(s => true)).ToList();
            var studentDtos = students.Select(student => new StudentGetAllResponseDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                Birthdate = student.Birthdate.ToDateTime(TimeOnly.MinValue),
            }).ToList();

            return studentDtos;
        }

        public async Task<StudentGetByIdResponseDTO?> GetStudentByIdAsync(Guid id)
        {
            var student = (await _studentsCollection.FindAsync(i => i.Id == id)).FirstOrDefault();

            var studentResponse = new StudentGetByIdResponseDTO
            {
                Id = student.Id,
                Name =  student.Name,
                Email = student.Email,
                Document = student.Document,
                Phone = student.Phone,
                Birthdate = student.Birthdate.ToDateTime(TimeOnly.MinValue),
                CreateDate = student.CreateDate,
            };

            return studentResponse;
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
            var student = (await _studentsCollection.FindAsync(d => d.Document == document)).FirstOrDefault();
            var findDocument = int.Parse(student.Document);

            return findDocument;
        }

        public async Task<StudentUpdateDTO?> SearchStudentToUpdateAsync(Guid id)
        {
            var student = (await _studentsCollection.FindAsync(i => i.Id == id)).FirstOrDefault();

            var studentResponse = new StudentUpdateDTO
            {
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
            };

            return studentResponse;
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
