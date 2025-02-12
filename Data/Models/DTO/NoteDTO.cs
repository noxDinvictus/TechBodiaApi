namespace TechBodiaApi.Data.Models.DTO
{
    public class NoteDTO
    {
        public Guid NoteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }

        public Note ToModel()
        {
            return new Note
            {
                NoteId = NoteId,
                Title = Title,
                Content = Content,
                CreatedAt = CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = UpdatedAt ?? DateTime.UtcNow,
            };
        }
    }
}
