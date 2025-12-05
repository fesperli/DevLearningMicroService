using API.Controllers.Interfaces;
using API.Models.DTOs.Category;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase, ICategoryController
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

            }
        }
    }
}
