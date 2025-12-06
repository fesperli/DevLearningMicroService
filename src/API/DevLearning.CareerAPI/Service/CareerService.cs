using DevLearning.Models;
using DevLearning.Models.DTOs.Career;
using DevLearning.Repositories;
using DevLearning.Services;
using DevLearning.CareerAPI.Repositories.Interfaces;
using DevLearning.CareerAPI.Service.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Career;

namespace DevLearning.Services
{
    public class CareerService : ICareerService
    {
        // Usamos a Interface, não a classe concreta (Boas práticas)
        private readonly ICareerRepository _repository;

        // Removemos o CourseRepository pois ele é do SQL e estamos isolados no Mongo
        public CareerService(ICareerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CareerResponseDTO>> GetAllCareerAsync()
        {
            var careers = await _repository.GetAllCareerAsync();

            // Mapeia a lista de Entidades para a lista de DTOs de resposta
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
            // Validações Básicas
            if (careerDto == null) throw new ArgumentException("Dados inválidos.");
            if (string.IsNullOrWhiteSpace(careerDto.Title)) throw new ArgumentException("O título é obrigatório.");

            // 1. Prepara a lista de Itens (Antiga tabela de relacionamento)
            var careerItems = new List<CareerItem>();
            int totalDuration = 0;

            if (careerDto.Items != null)
            {
                foreach (var itemDto in careerDto.Items)
                {
                    // --- LÓGICA DE TESTE STANDALONE ---
                    // Como não temos o CourseRepository aqui, assumimos 60 min ou pegamos do DTO se existisse.
                    // Num cenário real futuro, aqui entraria o HttpClient.
                    int duracaoMock = 60;
                    totalDuration += duracaoMock;

                    careerItems.Add(new CareerItem
                    {
                        CourseId = itemDto.CourseId,
                        // Se não vier título no DTO, criamos um genérico para não quebrar
                        Title = string.IsNullOrWhiteSpace(itemDto.Title) ? $"Curso {itemDto.CourseId}" : itemDto.Title,
                        Description = itemDto.Description,
                        Order = itemDto.Order,
                        Duration = duracaoMock
                    });
                }
            }

            // 2. Cria a Entidade de Domínio (Career)
            // O Mongo vai salvar esse objeto inteiro de uma vez
            var newCareer = new Career(
                Guid.NewGuid(), // Geramos o ID novo
                careerDto.Title,
                careerDto.Summary,
                careerDto.Url,
                totalDuration,
                careerDto.Active,
                careerDto.Featured,
                careerDto.Tags,
                careerItems // Passamos a lista processada
            );

            // 3. Chama o repositório Mongo
            return await _repository.CreateCareerAsync(newCareer);
        }

        public async Task<bool> UpdateCareerAsync(Guid id, CareerRequestDTO careerDto)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido.");
            if (careerDto == null) throw new ArgumentException("Dados inválidos.");
            if (string.IsNullOrWhiteSpace(careerDto.Title)) throw new ArgumentException("O título é obrigatório.");

            // Busca a carreira existente no Mongo
            var existingCareer = await _repository.GetCareerByIdAsync(id);
            if (existingCareer == null) return false;

            // Recalcula os itens (Mesma lógica do Create)
            var careerItems = new List<CareerItem>();
            int totalDuration = 0;

            if (careerDto.Items != null)
            {
                foreach (var itemDto in careerDto.Items)
                {
                    int duracaoMock = 60;
                    totalDuration += duracaoMock;

                    careerItems.Add(new CareerItem
                    {
                        CourseId = itemDto.CourseId,
                        Title = string.IsNullOrWhiteSpace(itemDto.Title) ? $"Curso {itemDto.CourseId}" : itemDto.Title,
                        Description = itemDto.Description,
                        Order = itemDto.Order,
                        Duration = duracaoMock
                    });
                }
            }

            // Atualiza os dados da entidade existente na memória
            // (Assumindo que você tem métodos de Update ou Setters na sua classe Career)
            existingCareer.Update(
                careerDto.Title,
                careerDto.Summary,
                careerDto.Url,
                careerDto.Active,
                careerDto.Featured,
                careerDto.Tags
            );

            // Atualiza lista e duração
            existingCareer.AddItems(careerItems);
            existingCareer.SetDuration(totalDuration);

            // Persiste a alteração no Mongo
            return await _repository.UpdateCareerAsync(existingCareer);
        }

        // --- Método Auxiliar Privado para Mapear Entidade -> DTO ---
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