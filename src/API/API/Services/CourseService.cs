using API.Models.DTOs.Course;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Blog.API.Models;

namespace API.Services
{
    public class CourseService : ICourseService
    {
        private readonly CourseRepository _courseRepository;

        public CourseService(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public Task<List<CourseResponseDTO>> GetAllCoursesAsync()
            => _courseRepository.GetAllCoursesAsync();

        public Task<CourseResponseDTO?> GetCourseByIdAsync(Guid id)
            => _courseRepository.GetCourseByIdAsync(id);

        public async Task CreateCourseAsync(CourseRequestDTO dto)
        {
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
