using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models.Payload
{
    public class NotePayload
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }

        public NoteDTO ToDto()
        {
            return new NoteDTO
            {
                Title = Title,
                Content = Content,
            };
        }
    }
}
