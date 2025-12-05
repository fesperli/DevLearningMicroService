namespace API.Models.DTOs.Student
{
    public class StudentRequestDTO
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Document { get; init; }
        public string Phone { get; init; }
        public DateOnly Birthdate { get; init; }
    }
}
