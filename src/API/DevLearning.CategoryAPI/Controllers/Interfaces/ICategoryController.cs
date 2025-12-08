using DevLearning.Models.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.CategoryAPI.Controllers.Interfaces
{
    public interface ICategoryController
    {
        Task<ActionResult<List<CategoryResponseDTO>>> GetAllCategoriesAsync();

        Task<ActionResult<CategoryResponseDTO>> GetCategoryByIdAsync(Guid id);

        Task<ActionResult> CreateCategoryAsync(CategoryRequestDTO category);

        Task<ActionResult> UpdateCategoryAsync(CategoryRequestDTO category, Guid id);

        Task<ActionResult> DeleteCategoryAsync(Guid id);
    }
}
