using System.ComponentModel.DataAnnotations;
using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models
{
    public class Note : BaseClass
    {
        [Key]
        public Guid NoteId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        public NoteDTO ToDTO()
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
