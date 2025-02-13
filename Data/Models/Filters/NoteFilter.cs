using TechBodiaApi.Services.Models.Filters;

namespace TechBodiaApi.Data.Models.Filters
{
    public class NoteFilter : BaseFilter
    {
        public string SearchText { get; set; } = string.Empty;
    }
}
