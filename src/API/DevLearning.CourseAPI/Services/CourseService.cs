using DevLearning.CourseAPI.Repositories;
using DevLearning.CourseAPI.Services.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Course;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevLearning.CourseAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly CourseRepository _courseRepository;

        private readonly IHttpClientFactory _httpClientFactory;

        public CourseService(CourseRepository courseRepository, IHttpClientFactory httpClientFactory)
        {
            _courseRepository = courseRepository;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<CourseResponseDTO>> GetAllCoursesAsync()
            => _courseRepository.GetAllCoursesAsync();

        public Task<CourseResponseDTO?> GetCourseByIdAsync(Guid id)
            => _courseRepository.GetCourseByIdAsync(id);

        public async Task CreateCourseAsync(CourseRequestDTO dto)
        {
            var client = _httpClientFactory.CreateClient("Author");

            var response = await client.GetAsync(dto.AuthorId.ToString());

            response.EnsureSuccessStatusCode();


            client = _httpClientFactory.CreateClient("Category");

            response = await client.GetAsync(dto.CategoryId.ToString());

            response.EnsureSuccessStatusCode();


            var course = new Course(
                tag: dto.Tag,
                title: dto.Title,
                summary: dto.Summary,
                url: dto.Url,
                level: dto.Level,
                durationInMinutes: dto.DurationInMinutes,
                createDate: dto.CreateDate,
                lastUpdateDate: dto.LastUpdateDate,
                active: dto.Active,
                free: dto.Free,
                featured: dto.Featured,
                authorId: dto.AuthorId,
                categoryId: dto.CategoryId,
                tags: dto.Tags
            );

            await _courseRepository.CreateCourseAsync(course);
        }

        public async Task<bool> UpdateCourseAsync(Guid id, CourseRequestDTO dto)
        {
            var client = _httpClientFactory.CreateClient("Author");

            var response = await client.GetAsync(dto.AuthorId.ToString());

            response.EnsureSuccessStatusCode();


            client = _httpClientFactory.CreateClient("Category");

            response = await client.GetAsync(dto.CategoryId.ToString());

            response.EnsureSuccessStatusCode();

            var course = new Course(
                tag: dto.Tag,
                title: dto.Title,
                summary: dto.Summary,
                url: dto.Url,
                level: dto.Level,
                durationInMinutes: dto.DurationInMinutes,
                createDate: dto.CreateDate,
                lastUpdateDate: dto.LastUpdateDate,
                active: dto.Active,
                free: dto.Free,
                featured: dto.Featured,
                authorId: dto.AuthorId,
                categoryId: dto.CategoryId,
                tags: dto.Tags
            );

            await _courseRepository.UpdateCourseAsync(id, course);
            return true;
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            await _courseRepository.DeleteCourseAsync(id);
            return true;
        }
    }
}
