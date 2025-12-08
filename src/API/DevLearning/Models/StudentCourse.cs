using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevLearning.Models
{
    public class StudentCourse
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid StudentId { get; private set; }
        [BsonRepresentation(BsonType.String)]
        public Guid CourseId { get; private set; }
        
        public byte Progress { get; private set; }
        public bool Favorite { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }


        public StudentCourse(
            Guid studentId,
            Guid courseId,
            byte progress,
            bool favorite
        )
        {
            StudentId = studentId;
            CourseId = courseId;
            Progress = progress;
            Favorite = favorite;
            StartDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;
        }
    }
}
