using DevLearning.AuthorAPI.Controllers.Interfaces;
using DevLearning.AuthorAPI.Services.Interfaces;
using DevLearning.Models.DTOs.Author;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.AuthorAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase, IAuthorController
    {
        private IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService, ILogger<AuthorController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<List<AuthorResponseDTO>>> GetAllAuthorsAsync()
        {
            try
            {
                var author = await _authorService.GetAllAuthorsAsync();

                if (author is null)
                    return NotFound();

                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao listar os autores.");
                return Problem(ex.Message);

            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResponseDTO>> GetAuthorByIdAsync(Guid id)
        {
            try
            {
                var authorFound = await _authorService.GetAuthorByIdAsync(id);

                if (authorFound is null)
                    return NotFound();

                return Ok(await _authorService.GetAuthorByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao encontrar o autor.");
                return Problem(ex.Message);

            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthorAsync([FromBody] AuthorRequestDTO author)
        {
            try
            {
                _logger.LogInformation("Ator registrado com sucesso.");
                await _authorService.CreateAuthorAsync(author);
                return Created();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro inesperado ao registrado o autor.");
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthorAsync(AuthorRequestDTO author, Guid id)
        {
            try
            {
                var authorFound = await _authorService.GetAuthorByIdAsync(id);

                if (authorFound is null)
                    return NotFound();

                await _authorService.UpdateAuthorAsync(author, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar o autor.");
                return Problem(ex.Message);

            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthorAsync(Guid id)
        {
            try
            {
                var authorFound = await _authorService.GetAuthorByIdAsync(id);

                if (authorFound is null)
                    return NotFound();

                await _authorService.DeleteAuthorAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar o autor.");
                return Problem(ex.Message);

            }
        }
    }
}
