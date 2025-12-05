using API.Models;
using API.Models.DTOs.Author;
using API.Models.DTOs.Category;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<AuthorResponseDTO>> GetAllAuthorsAsync()
        {
            return await _authorRepository.GetAllAuthorsAsync();
        }

        public async Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id)
        {
            return await _authorRepository.GetAuthorByIdAsync(id);
        }

        public async Task CreateAuthorAsync(AuthorRequestDTO author)
        {
            Author newAuthor = new(author.Name, author.Title, author.Image, author.Bio, author.Url, author.Email, author.Type);

            await _authorRepository.CreateAuthorAsync(newAuthor);
        }

        public async Task UpdateAuthorAsync(AuthorRequestDTO author, Guid id)
        {
            Author newAuthor = new(author.Name, author.Title, author.Image, author.Bio, author.Url, author.Email, author.Type);

            await _authorRepository.UpdateAuthorAsync(newAuthor, id);
        }

        public async Task DeleteAuthorAsync(Guid id)
        {
            await _authorRepository.DeleteAuthorAsync(id);
        }
    }
}
