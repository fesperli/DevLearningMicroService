using API.Database;
using API.Models;
using API.Models.DTOs.Career;
using API.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace API.Repositories
{
    public class CareerRepository : ICareerRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CareerRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<CareerResponseDTO>> GetAllCareerAsync()
        {
            using var connection = _connectionFactory.GetConnection();

            var sqlCareer = "SELECT * FROM Career";
            var sqlItems = "SELECT * FROM CareerItem WHERE CareerId = @CareerId ORDER BY [Order]";

            var careers = await connection.QueryAsync(sqlCareer);

            var list = new List<CareerResponseDTO>();

            foreach (var career in careers)
            {
                var items = await connection.QueryAsync(sqlItems, new { CareerId = (Guid)career.Id });

                list.Add(new CareerResponseDTO
                {
                    Id = career.Id,
                    Title = career.Title,
                    Summary = career.Summary,
                    Url = career.Url,
                    DurationInMinutes = career.DurationInMinutes,
                    Active = career.Active,
                    Featured = career.Featured,
                    Tags = career.Tags,
                    Items = items.Select(i => new CareerItemResponseDTO
                    {
                        CourseId = i.CourseId,
                        Title = i.Title,
                        Description = i.Description,
                        Order = i.Order
                    }).ToList()
                });
            }
            return list;
        }

        public async Task<CareerResponseDTO> GetCareerByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.GetConnection();

            var sqlCareer = "SELECT * FROM Career WHERE Id = @Id";
            var sqlItems = "SELECT * FROM CareerItem WHERE CareerId = @CareerId ORDER BY [Order]";

            var career = await connection.QueryFirstOrDefaultAsync(sqlCareer, new { Id = id });

            if (career == null)
                return null;

            var items = await connection.QueryAsync(sqlItems, new { CareerId = id });

            return new CareerResponseDTO
            {
                Id = career.Id,
                Title = career.Title,
                Summary = career.Summary,
                Url = career.Url,
                DurationInMinutes = career.DurationInMinutes,
                Active = career.Active,
                Featured = career.Featured,
                Tags = career.Tags,
                Items = items.Select(i => new CareerItemResponseDTO
                {
                    CourseId = i.CourseId,
                    Title = i.Title,
                    Description = i.Description,
                    Order = i.Order
                }).ToList()
            };
        }

        public async Task<Guid> CreateCareerAsync(CareerRequestDTO career, int duration)
        {
            using var connection = _connectionFactory.GetConnection();

            var id = Guid.NewGuid();

            var sqlCareer = @" INSERT INTO Career (Id, Title, Summary, Url, DurationInMinutes, Active, Featured, Tags)
                            VALUES (@Id, @Title, @Summary, @Url,@DurationInMinutes, @Active, @Featured, @Tags)";

            await connection.ExecuteAsync(sqlCareer, new
            {
                Id = id,
                career.Title,
                career.Summary,
                career.Url,
                DurationInMinutes = duration,
                career.Active,
                career.Featured,
                career.Tags
            });

            var sqlItem = @" INSERT INTO CareerItem (CareerId, CourseId, Title, Description, [Order])
                          VALUES (@CareerId, @CourseId, @Title, @Description, @Order)";

            foreach (var item in career.Items)
            {
                await connection.ExecuteAsync(sqlItem, new
                {
                    CareerId = id,
                    item.CourseId,
                    item.Title,
                    item.Description,
                    item.Order
                });
            }

            return id;
        }

        public async Task<bool> UpdateCareerAsync(Guid id, CareerRequestDTO career, int duration)
        {
            using var connection = _connectionFactory.GetConnection();

            var sqlUpdate = @" UPDATE Career SET Title = @Title, Summary = @Summary, Url = @Url, DurationInMinutes = @DurationInMinutes,
                            Active = @Active, Featured = @Featured, Tags = @Tags WHERE Id = @Id";

            var linhasAfetadas = await connection.ExecuteAsync(sqlUpdate, new
            {
                Id = id, career.Title, career.Summary, career.Url,
                DurationInMinutes = duration,
                career.Active, career.Featured, career.Tags
            });

            return linhasAfetadas > 0;
        }

        public async Task<int> SumDurationByCareerIdAsync(Guid careerId)
        {
            using var connection = _connectionFactory.GetConnection();

            var sql = @" SELECT SUM(c.DurationInMinutes)
                      FROM CareerItem ci
                      INNER JOIN Course c ON ci.CourseId = c.Id
                      WHERE ci.CareerId = @CareerId";

            return await connection.ExecuteScalarAsync<int>(sql, new { CareerId = careerId });
        }
    }
}