using DevLearning.Models;
using DevLearning.Models.DTOs.Author;

namespace DevLearning.AuthorAPI.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<AuthorResponseDTO>> GetAllAuthorsAsync();

        Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id);

        Task CreateAuthorAsync(Author author);

        Task UpdateAuthorAsync(Author author, Guid id);

        Task DeleteAuthorAsync(Guid id);

        // Adicionado método
        Task<AuthorResponseDTO> GetAuthorByEmailAsync(string email);
    }
}
