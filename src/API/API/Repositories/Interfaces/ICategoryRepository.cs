using API.Models;
using API.Models.DTOs.Category;

namespace API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResponseDTO>> GetAllCategoriesAsync();

        Task<CategoryResponseDTO> GetCategoryByIdAsync(Guid id);

        Task CreateCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category, Guid id);

        Task DeleteCategoryAsync(Guid id);
    }
}
