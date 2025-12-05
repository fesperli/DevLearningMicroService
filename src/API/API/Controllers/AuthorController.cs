using API.Controllers.Interfaces;
using API.Models.DTOs.Author;
using API.Models.DTOs.Category;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase, IAuthorController
    {

        private IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthorAsync(AuthorRequestDTO author)
        {
            try
            {
                await _authorService.CreateAuthorAsync(author);
                return Created();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");

            }
        }

    }
}
