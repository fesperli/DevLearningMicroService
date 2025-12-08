using DevLearning.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevLearning.CareerAPI.Data
{
    public class ConnectionDB
    {
        public readonly IMongoCollection<Career>MongoCollection;
        public ConnectionDB(IOptions<MongoDBSettings> mongoDbSettings)
        {
            MongoClient client = new(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DataBaseName);
            MongoCollection = database.GetCollection<Career>(mongoDbSettings.Value.CollectionName);
        }

        public IMongoCollection<Career> GetCareerCollection()
        {
            return MongoCollection;
        }
    }
}
