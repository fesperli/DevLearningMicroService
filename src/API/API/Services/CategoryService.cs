using API.Models;
using API.Models.DTOs.Category;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponseDTO>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task CreateCategoryAsync(CategoryRequestDTO category)
        {
            Category newCategory = new(category.Title, category.Url, category.Summary, category.Order, category.Description, category.Featured);

            await _categoryRepository.CreateCategoryAsync(newCategory);
        }

        public async Task UpdateCategoryAsync(CategoryRequestDTO category, Guid id)
        {
            Category newCategory = new(category.Title, category.Url, category.Summary, category.Order, category.Description, category.Featured);

            await _categoryRepository.UpdateCategoryAsync(newCategory, id);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}
