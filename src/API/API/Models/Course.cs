namespace Blog.API.Models
{
    public class Course
    {
        public Guid Id { get; private set; }
        public string Tag { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public string Url { get; private set; }
        public int Level { get; private set; }
        public int DurationInMinutes { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public bool Active { get; private set; }
        public bool Free { get; private set; }
        public bool Featured { get; private set; }
        public Guid AuthorId { get; private set; }
        public Guid CategoryId { get; private set; }
        public string Tags { get; private set; }

        public Course(
            string tag,
            string title,
            string summary,
            string url,
            int level,
            int durationInMinutes,
            DateTime createDate,
            DateTime lastUpdateDate,
            bool active,
            bool free,
            bool featured,
            Guid authorId,
            Guid categoryId,
            string tags)
        {
            this.Id = Guid.NewGuid();
            this.Tag = tag;
            this.Title = title;
            this.Summary = summary;
            this.Url = url;
            this.Level = level;
            this.DurationInMinutes = durationInMinutes;
            this.CreateDate = createDate;
            this.LastUpdateDate = lastUpdateDate;
            this.Active = active;
            this.Free = free;
            this.Featured = featured;
            this.AuthorId = authorId;
            this.CategoryId = categoryId;
            this.CreateDate = DateTime.Now;
            this.LastUpdateDate = DateTime.Now;
            this.Tags = tags;
        }
    }
}
