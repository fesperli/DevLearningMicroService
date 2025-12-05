using API.Controllers.Interfaces;
using API.Models.DTOs.Course;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase, ICourseController
    {
        private readonly CourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(CourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseResponseDTO>>> GetAllCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();

                if (courses == null || !courses.Any())
                    return NotFound();     // 404

                return Ok(courses);         // 200
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de entrada ao listar cursos");
                return BadRequest(new { error = ex.Message }); // 400
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao listar cursos");
                return StatusCode(500, new { error = "Erro interno no servidor." }); // 500
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseResponseDTO>> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);

                if (course == null)
                    return NotFound(new { error = "Curso não encontrado." }); // 404

                return Ok(course); // 200
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de entrada ao obter curso");
                return BadRequest(new { error = ex.Message }); // 400
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter curso");
                return StatusCode(500, new { error = "Erro interno no servidor." }); // 500
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse([FromBody] CourseRequestDTO dto)
        {
            try
            { 

                await _courseService.CreateCourseAsync(dto);
                return StatusCode(201);           // 201 
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de entrada ao criar curso");
                return BadRequest(new { error = ex.Message }); // 400
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar curso");
                return StatusCode(500, new { error = "Erro interno no servidor." }); // 500
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateCourse(Guid id, [FromBody] CourseRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // 400

                var updated = await _courseService.UpdateCourseAsync(id, dto);

                if (!updated)
                    return NotFound(new { error = "Curso não encontrado." }); // 404

                return NoContent(); // 204
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de entrada ao atualizar curso");
                return BadRequest(new { error = ex.Message }); // 400
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar curso");
                return StatusCode(500, new { error = "Erro interno no servidor." }); // 500
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCourse(Guid id)
        {
            try
            {
                var deleted = await _courseService.DeleteCourseAsync(id);

                if (!deleted)
                    return NotFound(new { error = "Curso não encontrado." }); // 404

                return NoContent(); // 204
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de entrada ao excluir curso");
                return BadRequest(new { error = ex.Message }); // 400
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao excluir curso");
                return StatusCode(500, new { error = "Erro interno no servidor." }); // 500
            }
        }
    }
}
