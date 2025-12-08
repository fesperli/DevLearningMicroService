using DevLearning.Models;

namespace DevLearning.CareerAPI.Repositories.Interfaces
{
    public interface ICareerRepository
    {
        Task<IEnumerable<Career>> GetAllCareerAsync();
        Task<Career?> GetCareerByIdAsync(Guid id);
        Task<Guid> CreateCareerAsync(Career career);
        Task<bool> UpdateCareerAsync(Career career);
    }
}
