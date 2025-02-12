using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models
{
    public class Note
    {
        [Key]
        public Guid NoteId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(User))]
        public Guid CreatedByUserId { get; set; }

        public NoteDTO ToDto()
        {
            return new NoteDTO
            {
                NoteId = NoteId,
                Title = Title,
                Content = Content,
                CreatedAt = CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = UpdatedAt ?? DateTime.UtcNow,
                CreatedByUserId = CreatedByUserId,
            };
        }
    }
}
