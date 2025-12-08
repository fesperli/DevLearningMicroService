using DevLearning.Models;
using DevLearning.Models.DTOs.Course;
using DevLearning.Models.DTOs.Student;
using DevLearning.Models.DTOs.StudentCourse;
using DevLearning.StudentAPI.Data;
using DevLearning.StudentAPI.Repositories.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Net;


namespace DevLearning.StudentAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MongoDBConnection _mongoConnection;
        private readonly IMongoCollection<Student> _studentsCollection;
        private readonly IMongoCollection<StudentCourse> _studentCourseCollection;
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentRepository(MongoDBConnection connection, IHttpClientFactory httpClientFectory)
        {
            _mongoConnection = connection;
            _studentsCollection = _mongoConnection.GetStudentMongoCollection();
            _studentCourseCollection = _mongoConnection.GetStudentCourseMongoCollection();
            _httpClientFactory = httpClientFectory;
        }

        public async Task CreateStudentAsync(Student student)
        {
            await _studentsCollection.InsertOneAsync(student);                  
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            await _studentCourseCollection.DeleteManyAsync(s => s.StudentId == id);
            await _studentsCollection.DeleteOneAsync(s => s.Id == id);
        }

        public async Task EnrollingStudentInCourseAsync(StudentCourse studentCourse)
        {
           await _studentCourseCollection.InsertOneAsync(studentCourse);
        }

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
            var student = (await _studentsCollection.FindAsync(s => s.Id == studentId)).FirstOrDefault();

            if (student is null)
                return null;

            var studentCourses = ( await _studentCourseCollection.FindAsync(sc => sc.StudentId == studentId)).ToList();

            var client = _httpClientFactory.CreateClient("courseClient");

            var newStudentWithCourses = new StudentWithCoursesResponseDTO
            {
                StudentId = student.Id,
                Name = student.Name,
                Email = student.Email
            };

            foreach (var sc in studentCourses) 
            { 
                var response = await client.GetAsync(sc.CourseId.ToString());

                if (!response.IsSuccessStatusCode)
                    continue;

                var courseData = await response.Content
                    .ReadFromJsonAsync<CourseResponseDTO>();

                if (courseData is null)
                    continue;

                newStudentWithCourses.Courses.Add(new CourseOfStudentDTO
                {
                    CourseId = sc.CourseId,
                    CourseTitle = courseData.Title,
                    Summary = courseData.Summary,
                    Url = courseData.Url,
                    Level = (byte)courseData.Level,
                    DurationInMinutes = courseData.DurationInMinutes,
                    Progress = sc.Progress,
                    Favorite = sc.Favorite,
                    StartDate = sc.StartDate,
                    LastUpdateDate = sc.LastUpdateDate
                });
            }

            return newStudentWithCourses;

        }

        public async Task<int> SearchStudentByDocument(string document)
        {
            var student = (await _studentsCollection.FindAsync(d => d.Document == document)).FirstOrDefault();

            int documentExist = 0;

            if (student != null)
                documentExist = 1;

            return documentExist;
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
            var filter = Builders<StudentCourse>.Filter.Eq(s => s.StudentId, studentId) &
                Builders<StudentCourse>.Filter.Eq(c => c.CourseId, courseId);

            var update = Builders<StudentCourse>.Update
                .Set(p => p.Progress, updateProgressDTO.Progress)
                .Set(d => d.LastUpdateDate, updateProgressDTO.LastUpdateDate);

            await _studentCourseCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateStudentAsync(Guid id, StudentUpdateDTO student)
        {

            var filter = Builders<Student>.Filter.Eq(i => i.Id, id);

            var update = Builders<Student>.Update
                .Set(n => n.Name, student.Name)
                .Set(n => n.Email, student.Email)
                .Set(n => n.Phone, student.Phone);


            await _studentsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<bool> VerifyExistCourseAsync(Guid courseId)
        {
            var client =  _httpClientFactory.CreateClient("courseClient");
            var response = await client.GetAsync(courseId.ToString());

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> VerifyExistStudentAsync(Guid studentId)
        {
            var exist = await _studentsCollection.AsQueryable().AnyAsync(i => i.Id == studentId);

            return exist;

        }

        public async Task<byte> VerifyProgressToStudentInCourseAsync(Guid studentId, Guid courseId)
        {

            var studentCourse = (await _studentCourseCollection.FindAsync(i => i.StudentId == studentId && i.CourseId == courseId)).FirstOrDefault();

            var progress = studentCourse.Progress;

            return progress;
        }

        public async Task<bool> VerifyStudentEnrollingInCourseAsync(Guid studentId, Guid courseId)
        {

            var exist = await _studentCourseCollection.AsQueryable().AnyAsync(i => i.StudentId == studentId && i.CourseId == courseId);

            return exist;
        }
    }
}
