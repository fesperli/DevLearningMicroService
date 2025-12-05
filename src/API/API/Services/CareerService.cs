using API.Models;
using API.Models.DTOs.Career;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services
{
    public class CareerService : ICareerService
    {
        private readonly CareerRepository _repository;
        private readonly CourseRepository _courseRepository;

        public CareerService(CareerRepository repository, CourseRepository courseRepository)
        {
            _repository = repository;
            _courseRepository = courseRepository;
        }
        public async Task<IEnumerable<CareerResponseDTO>> GetAllCareerAsync()
        {
            var careers = await _repository.GetAllCareerAsync();

            return careers ?? Enumerable.Empty<CareerResponseDTO>();
        }

        public async Task<CareerResponseDTO?> GetCareerByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.");

            var career = await _repository.GetCareerByIdAsync(id);

            return career;
        }

        public async Task<Guid> CreateCareerAsync(CareerRequestDTO career)
        {
            if (career == null)
                throw new ArgumentException("Dados inválidos.");

            if (string.IsNullOrWhiteSpace(career.Title))
                throw new ArgumentException("O título é obrigatório.");

            int totalDuration = 0;

            foreach (var item in career.Items)
            {
                var course = await _courseRepository.GetCourseByIdAsync(item.CourseId);

                if (course == null)
                    throw new ArgumentException("O curso não existe.");

                totalDuration += course.DurationInMinutes;
            }

            return await _repository.CreateCareerAsync(career, totalDuration);
        }

        public async Task<bool> UpdateCareerAsync(Guid id, CareerRequestDTO career)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.");

            if (career == null)
                throw new ArgumentException("Dados inválidos.");

            if (string.IsNullOrWhiteSpace(career.Title))
                throw new ArgumentException("O título é obrigatório.");


            int totalDuration = 0;

            foreach (var item in career.Items)
            {
                var course = await _courseRepository.GetCourseByIdAsync(item.CourseId);

                if (course == null)
                    throw new ArgumentException("O curso não existe.");

                totalDuration += course.DurationInMinutes;
            }

            return await _repository.UpdateCareerAsync(id, career, totalDuration);
        }
    }
}

