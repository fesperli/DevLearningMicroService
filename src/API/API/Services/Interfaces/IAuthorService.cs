using API.Models.DTOs.Author;

namespace API.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorResponseDTO>> GetAllAuthorsAsync();

        Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id);

        Task CreateAuthorAsync(AuthorRequestDTO author);

        Task UpdateAuthorAsync(AuthorRequestDTO author, Guid id);

        Task DeleteAuthorAsync(Guid id);
    }
}
