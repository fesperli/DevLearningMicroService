using DevLearning.AuthorAPI.Repositories.Interfaces;
using DevLearning.AuthorAPI.Services.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Author;

namespace DevLearning.AuthorAPI.Services
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
            // Adicionado: Validar entrada de um mesmo autor, por email.
            var authorEmail = await _authorRepository.GetAuthorByEmailAsync(author.Email);
            if (authorEmail is not null) throw new ArgumentException("Erro: Já existe um autor cadastrado com este endereço de email.");

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


        // Adicionado método para registrir o cadastro de um mesmo autor, por e-mail.
        public async Task<AuthorResponseDTO> GetAuthorByEmailAsync(string email)
        {
            var author = await _authorRepository.GetAuthorByEmailAsync(email);
            if (author is null) throw new KeyNotFoundException("Autor não encontrado.");

            return author;
        }

    }
}
