using API.Database;
using API.Models.DTOs.Course;
using API.Repositories.Interfaces;
using Blog.API.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace API.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<CourseRepository> _logger;

        public CourseRepository(DbConnectionFactory connection, ILogger<CourseRepository> logger)
        {
            _connection = connection.GetConnection();
            _logger = logger;
        }

        public async Task<List<CourseResponseDTO>> GetAllCoursesAsync()
        {
            const string sql = @"
                SELECT Id, Tag, Title, Summary, Url, [Level],
                       DurationInMinutes, CreateDate, LastUpdateDate,
                       Active, Free, Featured, AuthorId, CategoryId, Tags
                FROM Course";

            var courses = await _connection.QueryAsync<CourseResponseDTO>(sql);
            return courses.ToList();
        }

        public async Task<CourseResponseDTO?> GetCourseByIdAsync(Guid id)
        {
            const string sql = @"
                SELECT Id, Tag, Title, Summary, Url, [Level],
                       DurationInMinutes, CreateDate, LastUpdateDate,
                       Active, Free, Featured, AuthorId, CategoryId, Tags
                FROM Course
                WHERE Id = @Id";

            return await _connection.QueryFirstOrDefaultAsync<CourseResponseDTO>(sql, new { Id = id });
        }

        public async Task CreateCourseAsync(Course course)
        {
            const string sql = @"
                INSERT INTO Course
                (Id, Tag, Title, Summary, Url, [Level], DurationInMinutes,
                 CreateDate, LastUpdateDate, Active, Free, Featured,
                 AuthorId, CategoryId, Tags)
                VALUES
                (@Id, @Tag, @Title, @Summary, @Url, @Level, @DurationInMinutes,
                 @CreateDate, @LastUpdateDate, @Active, @Free, @Featured,
                 @AuthorId, @CategoryId, @Tags)";

            await _connection.ExecuteAsync(sql, course);
        }

        public async Task UpdateCourseAsync(Guid id, Course course)
        {
            const string sql = @"
                UPDATE Course SET
                    Tag = @Tag,
                    Title = @Title,
                    Summary = @Summary,
                    Url = @Url,
                    [Level] = @Level,
                    DurationInMinutes = @DurationInMinutes,
                    LastUpdateDate = @LastUpdateDate,
                    Active = @Active,
                    Free = @Free,
                    Featured = @Featured,
                    AuthorId = @AuthorId,
                    CategoryId = @CategoryId,
                    Tags = @Tags
                WHERE Id = @Id";

            await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                course.Tag,
                course.Title,
                course.Summary,
                course.Url,
                course.Level,
                course.DurationInMinutes,
                LastUpdateDate = DateTime.UtcNow,
                course.Active,
                course.Free,
                course.Featured,
                course.AuthorId,
                course.CategoryId,
                course.Tags
            });
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            const string sql = "DELETE FROM Course WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
