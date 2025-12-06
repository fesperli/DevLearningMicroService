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

        public CareerRepository(IMongoClient mongoClient, IOptions<MongoDBSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DataBaseName);
            _collection = database.GetCollection<Career>("Careers");
        }

        public async Task<IEnumerable<Career>> GetAllCareerAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Career?> GetCareerByIdAsync(Guid id)
        {
            return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateCareerAsync(Career career)
        {
            await _collection.InsertOneAsync(career);
            return career.Id;
        }

        public async Task<bool> UpdateCareerAsync(Career career)
        {
            var result = await _collection.ReplaceOneAsync(c => c.Id == career.Id, career);
            return result.IsAcknowledged && result.ModifiedCount >= 0;
        }
    }
}
