namespace API.Models.DTOs.Career
{
    public class CareerRequestDTO
    {
        public string Title { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        //public int DurationInMinutes { get; set; }
        public bool Active { get; set; }
        public bool Featured { get; set; }
        public string Tags { get; init; } = string.Empty;

        public List<CareerItemRequestDTO> Items { get; set; } = new ();
    }
}
