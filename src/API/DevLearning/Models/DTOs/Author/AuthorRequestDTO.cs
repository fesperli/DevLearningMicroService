using System.ComponentModel.DataAnnotations;

namespace DevLearning.Models.DTOs.Author
{
    public class AuthorRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Bio { get; set; }

        public string? Url { get; set; } // Adicionado nullable
        [Required]
        public string Email { get; set; }
        [Required]
        public bool Type { get; set; }
    }
}
