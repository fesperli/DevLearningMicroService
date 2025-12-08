using DevLearning.Models.DTOs.Career;

namespace DevLearning.CareerAPI.Service.Interfaces
{
    public interface ICareerService
    {
        Task<IEnumerable<CareerResponseDTO>> GetAllCareerAsync();
        Task<CareerResponseDTO?> GetCareerByIdAsync(Guid id);
        Task<Guid> CreateCareerAsync(CareerRequestDTO career);
        Task<bool> UpdateCareerAsync(Guid id, CareerRequestDTO career);
    }
}
