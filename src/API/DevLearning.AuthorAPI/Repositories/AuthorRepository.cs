using Dapper;
using DevLearning.AuthorAPI.Database;
using DevLearning.AuthorAPI.Repositories.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Author;
using Microsoft.Data.SqlClient;

namespace DevLearning.AuthorAPI.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly SqlConnection _connection;

        public AuthorRepository(ConnectionDB connectionFactory)
        {
            _connection = connectionFactory.GetConnection();
        }

        public async Task<List<AuthorResponseDTO>> GetAllAuthorsAsync()
        {
            var sqlString =
                @"SELECT Id, Name, Title, Image, Bio, Url, Email, Type 
                FROM Author";

            return (await _connection.QueryAsync<AuthorResponseDTO>(sqlString)).ToList();
        }

        public async Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id)
        {
            var sqlString =
                @"SELECT Id, Name, Title, Image, Bio, Url, Email, Type 
                FROM Author
                WHERE Id = @AuthorId";

            return (await _connection.QueryFirstOrDefaultAsync<AuthorResponseDTO>(sqlString, new { AuthorId = id }))!;
        }

        public async Task CreateAuthorAsync(Author author)
        {
            var sqlString =
                @"INSERT INTO Author (Id, Name, Title, Image, Bio, Url, Email, Type)
                VALUES (@Id, @Name, @Title, @Image, @Bio, @Url, @Email, @Type)";

            await _connection.ExecuteAsync(sqlString, new { author.Id, author.Name, author.Title, author.Image,
                    author.Bio, Url = author.Url is null ? (object)DBNull.Value : author.Url, author.Email, author.Type });  // adicionado nullable no campo Url
        }

        public async Task UpdateAuthorAsync(Author author, Guid id)
        {
            var sqlString =
                @"UPDATE Author SET Name = @Name, Title = @Title, Image = @Image, Bio = @Bio, Url = @Url, Email = @Email, Type = @Type
                WHERE Id = @AuthorId";

            await _connection.ExecuteAsync(sqlString, new { author.Name, author.Title, author.Image, author.Bio, author.Url, author.Email, author.Type, AuthorId = id });
        }

        public async Task DeleteAuthorAsync(Guid id)
        {
            var sqlString =
                @"DELETE FROM Author WHERE Id = @AuthorId";
            await _connection.ExecuteAsync(sqlString, new { AuthorId = id });
        }



        // Adicionado método para registrir o cadastro de um mesmo autor, por e-mail.
        public async Task<AuthorResponseDTO> GetAuthorByEmailAsync(string email)
        {
            var sql = 
                @"SELECT Id, Name, Title, Image, Bio, Url, Email, Type 
                FROM Author WHERE Email = @Email";

            return (await _connection.QueryFirstOrDefaultAsync<AuthorResponseDTO>(sql, new { Email = email }))!;
        }


    }
}
