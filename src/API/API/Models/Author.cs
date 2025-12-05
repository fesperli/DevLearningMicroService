namespace API.Models
{
    public class Author
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Image { get; private set; }
        public string Bio { get; private set; }
        public string Url { get; private set; }
        public string Email { get; private set; }
        public bool Type { get; private set; }

        public Author(string name, string title, string image, string bio, string url, string email, bool type)
        {
            Id = Guid.NewGuid();
            Name = name;
            Title = title;
            Image = image;
            Bio = bio;
            Url = url;
            Email = email;
            Type = type;
        }
    }
}
