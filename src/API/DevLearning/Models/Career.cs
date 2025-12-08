using DevLearning.Models.DTOs.Career;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace DevLearning.Models
{
    public class Career
    {
        [BsonId]
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Summary { get; private set; }
        public string Url { get; private set; }
        public int DurationInMinutes { get; private set; }
        public bool Active { get; private set; }
        public bool Featured { get; private set; }
        public string? Tags { get; private set; }

        public List<CareerItem> Items { get; private set; } = new List<CareerItem>();

        public Career() { }

        [BsonConstructor]
        public Career(Guid id, string title, 
                      string? summary, 
                      string url, 
                      int durationInMinutes, 
                      bool active, bool featured, string? tags, 
                      List<CareerItem> items)
        {
            Id = id;
            Title = title;
            Summary = summary;
            Url = url;
            DurationInMinutes = durationInMinutes;
            Active = active;
            Featured = featured;
            Tags = tags;
            Items = items ?? new List<CareerItem>();
        }
        public void Update(string title, string? summary, string url, bool active, bool featured, string? tags)
        {
            Title = title;
            Summary = summary;
            Url = url;
            Active = active;
            Featured = featured;
            Tags = tags;
        }

        public void SetDuration(int minutes)
        {
            DurationInMinutes = minutes;
        }
        public void AddItems(List<CareerItem> items)
        {
            Items = items;
        }
    }
}
