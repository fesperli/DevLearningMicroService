using DevLearning.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevLearning.StudentAPI.Data
{
    public class MongoDBConnection
    {
        public readonly IMongoCollection<Student> _studentCollection;
        public readonly IMongoCollection<StudentCourse> _studentCourseCollection;

        public MongoDBConnection(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _studentCollection = database.GetCollection<Student>(mongoDBSettings.Value.StudentCollectionName);
            _studentCourseCollection = database.GetCollection<StudentCourse>(mongoDBSettings.Value.StudentCourseCollectionName);
        }

        public IMongoCollection<Student> GetStudentMongoCollection() 
        {
            return _studentCollection;
        }

        public IMongoCollection<StudentCourse> GetStudentCourseMongoCollection()
        {
            return _studentCourseCollection;
        }

    }
}
