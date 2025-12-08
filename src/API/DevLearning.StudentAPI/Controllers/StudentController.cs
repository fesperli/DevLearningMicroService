using DevLearning.Models.DTOs.Student;
using DevLearning.Models.DTOs.StudentCourse;
using DevLearning.StudentAPI.Controllers.Interface;
using DevLearning.StudentAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.StudentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase, IStudentController
    {
        private StudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(StudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudentAsync(StudentRequestDTO dto)
        {
            try
            {
                _logger.LogInformation("Criando estudante...");
                await _studentService.CreateStudentAsync(dto);
                return Created();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao criar estudante!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar estudante!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentGetAllResponseDTO>>> GetAllStudentsAsync()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                if (students is null)
                    return NotFound("Não há estudantes cadastrados!");

                _logger.LogInformation("Buscando lista de estudantes...");
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao listar estudantes!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudentAsync(Guid id, StudentUpdateDTO student)
        {
            try
            {
                _logger.LogInformation("Atualizando estudante...");
                await _studentService.UpdateStudentAsync(id, student);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao etualizar estudadnte!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar estudante!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudentAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deletando estudante...");
                await _studentService.DeleteStudentAsync(id);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao deletar estudante!");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar estudante!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{studentId}/Course/{courseId}")]
        public async Task<ActionResult> EnrollingStudentInCourseAsync
        (
            Guid studentId, Guid courseId, StudentCourseRequestDTO dto
        )
        {
            try
            {
                _logger.LogInformation("Matriculando estudante em um novo curso...");
                await _studentService.EnrollingStudentInCourseAsync(studentId, courseId, dto);
                return Created();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao matricular estudante em um curso!");
                if (ex.Message.Contains("não existe!"))
                    return NotFound(ex.Message);
                else
                    return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao matricular estudante em um curso!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{studentId}/Course/{courseId}/Progress")]
        public async Task<ActionResult> UpdateProgressStudentCourseAsync(Guid studentId, Guid courseId, StudentUpdateProgressDTO updateProgressDTO)
        {
            try
            {
                _logger.LogInformation("Atualizando progresso do curso...");
                await _studentService.UpdateProgressStudentCourseAsync(studentId, courseId, updateProgressDTO);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao atualizar progresso do estudante em um curso!");
                if (ex.Message.Contains("não existe!"))
                    return NotFound(ex.Message);
                else
                    return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar progresso do estudante em um curso!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}/Courses")]
        public async Task<ActionResult<StudentWithCoursesResponseDTO>> GetStudentCoursesAsync(Guid id)
        {
            try
            {
                var studentWithCourses = await _studentService.GetStudentCoursesAsync(id);
                if (studentWithCourses is null)
                    return NotFound("Estudante não encontrado");

                if (studentWithCourses.Courses.Count() == 0)
                    return NotFound("Esse estudante não está cadastrado em cursos!");

                _logger.LogInformation("Buscando estudante e curso...");
                return Ok(studentWithCourses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao listar cursos de um estudante!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGetByIdResponseDTO>> GetStudentByIdAsync(Guid id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student is null)
                    return NotFound("Estudante não encontrado!");

                _logger.LogInformation("Buscando estudante por Id...");
                return Ok(student);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao buscar estudante por id!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar estudante por id!");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
