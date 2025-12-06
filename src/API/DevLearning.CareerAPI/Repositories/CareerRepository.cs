using DevLearning.CareerAPI.Data;
using DevLearning.CareerAPI.Repositories.Interfaces;
using DevLearning.CareerAPI.Service.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Career;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevLearning.Repositories
{
    public class CareerRepository : ICareerRepository
    {
        private readonly IMongoCollection<Career> _collection;

        // Injetamos o Cliente do Mongo e as Configurações (que pegam do appsettings.json)
        public CareerRepository(IMongoClient mongoClient, IOptions<MongoDBSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DataBaseName);

            // Mapeia a coleção "Careers". Se não existir, o Mongo cria no primeiro insert.
            _collection = database.GetCollection<Career>("Careers");
        }

        public async Task<IEnumerable<Career>> GetAllCareerAsync()
        {
            // Find(_ => true) é equivalente a "SELECT * FROM"
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Career?> GetCareerByIdAsync(Guid id)
        {
            return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateCareerAsync(Career career)
        {
            // Salva o objeto Career inteiro (com a lista de Items dentro)
            await _collection.InsertOneAsync(career);
            return career.Id;
        }

        public async Task<bool> UpdateCareerAsync(Career career)
        {
            // Substitui o documento inteiro pelo novo objeto atualizado
            var result = await _collection.ReplaceOneAsync(c => c.Id == career.Id, career);

            // Retorna true se encontrou e modificou (ou deu match)
            return result.IsAcknowledged && result.ModifiedCount >= 0;
        }
    }
}
