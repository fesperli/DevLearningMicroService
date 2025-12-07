using DevLearning.CategoryAPI.Controllers.Interfaces;
using DevLearning.CategoryAPI.Services.Interfaces;
using DevLearning.Models.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.CategoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase, ICategoryController
    {
        private ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var category = await _categoryService.GetAllCategoriesAsync();

                if (category is null)
                    return NotFound();

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing categories.");
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var categoryFound = await _categoryService.GetCategoryByIdAsync(id);

                if (categoryFound is null)
                    return NotFound();

                return Ok(await _categoryService.GetCategoryByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing category.");
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategoryAsync(CategoryRequestDTO category)
        {
            try
            {
                await _categoryService.CreateCategoryAsync(category);
                return Created();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating category.");
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategoryAsync(CategoryRequestDTO category, Guid id)
        {
            try
            {
                var categoryFound = await _categoryService.GetCategoryByIdAsync(id);

                if (categoryFound is null)
                    return NotFound();

                await _categoryService.UpdateCategoryAsync(category, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category.");
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAsync(Guid id)
        {
            try
            {
                var categoryFound = await _categoryService.GetCategoryByIdAsync(id);

                if (categoryFound is null)
                    return NotFound();

                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting category.");
                return Problem(e.Message);
            }
        }
    }
}
