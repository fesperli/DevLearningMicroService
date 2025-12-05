using API.Models.DTOs.Author;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Interfaces
{
    public interface IAuthorController
    {
        Task<ActionResult<List<AuthorResponseDTO>>> GetAllAuthorsAsync();

        Task<ActionResult<AuthorResponseDTO>> GetAuthorByIdAsync(Guid id);

        Task<ActionResult> CreateAuthorAsync(AuthorRequestDTO author);

        Task<ActionResult> UpdateAuthorAsync(AuthorRequestDTO author, Guid id);

        Task<ActionResult> DeleteAuthorAsync(Guid id);
    }
}
