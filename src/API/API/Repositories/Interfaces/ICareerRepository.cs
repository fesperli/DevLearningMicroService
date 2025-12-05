using API.Models;
using API.Models.DTOs.Career;

namespace API.Repositories.Interfaces
{
    public interface ICareerRepository
    {
        Task<IEnumerable<CareerResponseDTO>> GetAllCareerAsync();
        Task<CareerResponseDTO> GetCareerByIdAsync(Guid id);
        Task<Guid> CreateCareerAsync(CareerRequestDTO career, int duration);
        Task<bool> UpdateCareerAsync(Guid id, CareerRequestDTO career, int duration);

        //Task<int> SumDurationByCareerIdAsync(Guid careerId);

    }
}
