using DevLearning.Models;
using DevLearning.Models.DTOs.Category;

namespace DevLearning.CategoryAPI.Repositories.Interfaces
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
