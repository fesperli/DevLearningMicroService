using API.Models;
using API.Models.DTOs.Author;

namespace API.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<AuthorResponseDTO>> GetAllAuthorsAsync();

        Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id);

        Task CreateAuthorAsync(Author author);

        Task UpdateAuthorAsync(Author author, Guid id);

        Task DeleteAuthorAsync(Guid id);
    }
}
