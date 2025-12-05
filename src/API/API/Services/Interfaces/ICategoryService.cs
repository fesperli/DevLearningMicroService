using API.Models.DTOs.Category;

namespace API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDTO>> GetAllCategoriesAsync();

        Task<CategoryResponseDTO> GetCategoryByIdAsync(Guid id);

        Task CreateCategoryAsync(CategoryRequestDTO category);

        Task UpdateCategoryAsync(CategoryRequestDTO category, Guid id);

        Task DeleteCategoryAsync(Guid id);
    }
}
