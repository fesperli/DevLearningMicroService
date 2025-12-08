using DevLearning.CareerAPI.Repositories.Interfaces;
using DevLearning.CareerAPI.Service.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Career;
using DevLearning.Models.DTOs.Course;

namespace DevLearning.Services
{
    public class CareerService : ICareerService
    {
        private readonly ICareerRepository _repository;

        private readonly HttpClient _client;

        public CareerService(ICareerRepository repository, HttpClient client)
        {
            _repository = repository;
            _client = client;
        }

        public async Task<IEnumerable<CareerResponseDTO>> GetAllCareerAsync()
        {
            var careers = await _repository.GetAllCareerAsync();
            return careers.Select(MapToResponse);
        }

        public async Task<CareerResponseDTO?> GetCareerByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.");

            var career = await _repository.GetCareerByIdAsync(id);

            return career == null ? null : MapToResponse(career);
        }

        public async Task<Guid> CreateCareerAsync(CareerRequestDTO careerDto)
        {
            if (careerDto == null) throw new ArgumentException("Dados inválidos.");
            if (string.IsNullOrWhiteSpace(careerDto.Title)) throw new ArgumentException("O título é obrigatório.");

            var careerItems = new List<CareerItem>();
            int totalDuration = 0;

            if (careerDto.Items != null)
            {
                foreach (var itemDto in careerDto.Items)
                {
                    var response = await _client.GetAsync(itemDto.CourseId.ToString());

                    response.EnsureSuccessStatusCode();

                    var course = await response.Content.ReadFromJsonAsync<CourseResponseDTO>();

                    totalDuration += course.DurationInMinutes;

                    careerItems.Add(new CareerItem
                    {
                        CourseId = itemDto.CourseId,
                        Title = string.IsNullOrWhiteSpace(itemDto.Title) ? $"Curso {itemDto.CourseId}" : itemDto.Title,
                        Description = itemDto.Description,
                        Order = itemDto.Order,
                        Duration = course.DurationInMinutes
                    });
                }
            }

            var newCareer = new Career(
                Guid.NewGuid(),
                careerDto.Title,
                careerDto.Summary,
                careerDto.Url,
                totalDuration,
                careerDto.Active,
                careerDto.Featured,
                careerDto.Tags,
                careerItems 
            );

            return await _repository.CreateCareerAsync(newCareer);
        }

        public async Task<bool> UpdateCareerAsync(Guid id, CareerRequestDTO careerDto)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido.");
            if (careerDto == null) throw new ArgumentException("Dados inválidos.");
            if (string.IsNullOrWhiteSpace(careerDto.Title)) throw new ArgumentException("O título é obrigatório.");

            var existingCareer = await _repository.GetCareerByIdAsync(id);
            if (existingCareer == null) return false;

            var careerItems = new List<CareerItem>();
            int totalDuration = 0;

            if (careerDto.Items != null)
            {
                foreach (var itemDto in careerDto.Items)
                {
                    var response = await _client.GetAsync(itemDto.CourseId.ToString());

                    response.EnsureSuccessStatusCode();

                    var course = await response.Content.ReadFromJsonAsync<CourseResponseDTO>();

                    totalDuration += course.DurationInMinutes;

                    careerItems.Add(new CareerItem
                    {
                        CourseId = itemDto.CourseId,
                        Title = string.IsNullOrWhiteSpace(itemDto.Title) ? $"Curso {itemDto.CourseId}" : itemDto.Title,
                        Description = itemDto.Description,
                        Order = itemDto.Order,
                        Duration = course.DurationInMinutes
                    });
                }
            }
            existingCareer.Update(
                careerDto.Title,
                careerDto.Summary,
                careerDto.Url,
                careerDto.Active,
                careerDto.Featured,
                careerDto.Tags
            );
            existingCareer.AddItems(careerItems);
            existingCareer.SetDuration(totalDuration);

            return await _repository.UpdateCareerAsync(existingCareer);
        }

        private CareerResponseDTO MapToResponse(Career entity)
        {
            return new CareerResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Summary = entity.Summary,
                Url = entity.Url,
                DurationInMinutes = entity.DurationInMinutes,
                Active = entity.Active,
                Featured = entity.Featured,
                Tags = entity.Tags,
                Items = entity.Items.Select(i => new CareerItemResponseDTO
                {
                    CourseId = i.CourseId,
                    Title = i.Title,
                    Description = i.Description,
                    Order = i.Order
                }).ToList()
            };
        }
    }
}