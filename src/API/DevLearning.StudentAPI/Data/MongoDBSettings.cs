namespace DevLearning.StudentAPI.Data
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string StudentCollectionName { get; set; } = null!;
        public string StudentCourseCollectionName { get; set; } = null!;
    }
}
