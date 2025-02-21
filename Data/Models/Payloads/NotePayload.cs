using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models.Payload
{
    public class NotePayload
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }

        public NoteDTO ToDTO()
        {
            return new NoteDTO
            {
                Title = Title,
                Content = Content,
            };
        }
    }
}
