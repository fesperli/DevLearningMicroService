using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevLearning.Models.DTOs.Student
{
    public class StudentGetByIdResponseDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; init; }
        [BsonElement("name")]
        public string Name { get; init; }
        [BsonElement("email")]
        public string Email { get; init; }
        [BsonElement("document")]
        public string Document { get; init; }
        [BsonElement("phone")]
        public string? Phone { get; init; }
        [BsonElement("birthdate")]
        public DateTime Birthdate { get; init; }
        [BsonElement("createdate")]
        public DateTime CreateDate { get; init; }
    }
}
