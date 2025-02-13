namespace TechBodiaApi.Services.Models.Filters
{
    public class BaseFilter
    {
        private string _orderByColumn = "CreatedAt"; // Default ordering column

        // If OrderByColumn is not set or is empty, default to "CreatedAt" as it is a common base column.
        public string OrderByColumn
        {
            get => string.IsNullOrWhiteSpace(_orderByColumn) ? "CreatedAt" : _orderByColumn;
            set => _orderByColumn = string.IsNullOrWhiteSpace(value) ? "CreatedAt" : value;
        }

        // Default sorting order is descending (newest first).
        public bool OrderByDescending { get; set; } = true;

        // Default to the first page in pagination.
        public int CurrentPage { get; set; } = 1;

        // Default to 1 item per page to ensure at least one result.
        public int ItemsPerPage { get; set; } = 1;
    }
}
