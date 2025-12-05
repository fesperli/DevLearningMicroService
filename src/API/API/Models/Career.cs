using API.Models.DTOs.Career;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Career
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Summary { get; private set; }
        public string Url { get; private set; }
        public int DurationInMinutes { get; private set; }
        public bool Active { get; private set; }
        public bool Featured { get; private set; }
        public string? Tags { get; private set; }

        public Career() { }

        [JsonConstructor]
        public Career(string title, string? summary, string url, int durationInMinutes, bool active, bool featured, string? tags)
        {
            Title = title;
            Summary = summary;
            Url = url;
            DurationInMinutes = durationInMinutes;
            Active = active;
            Featured = featured;
            Tags = tags;
        }

        public void SetDuration(int minutes)
        {
            DurationInMinutes = minutes;
        }
    }
}
