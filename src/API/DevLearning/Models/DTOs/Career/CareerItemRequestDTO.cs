namespace API.Models.DTOs.Career
{
    public class CareerItemRequestDTO
    {
        public Guid CourseId { get; set; }  

        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public int Order { get; set; }

    }
}
