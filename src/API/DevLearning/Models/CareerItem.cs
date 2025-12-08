namespace DevLearning.Models
{
    public class CareerItem
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public int Duration { get; set; }
    }
}
