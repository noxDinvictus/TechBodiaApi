using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using TechBodiaApi.Data.Models.DTO;
using TechBodiaApi.Services.Models.Filters;

namespace TechBodiaApi.Services.Models.DTO
{
    public class ListResultDTO<ItemType, DtoItemType, FilterType>
        where FilterType : BaseFilter
    {
        [JsonIgnore]
        public FilterType Filter { get; set; }

        [JsonIgnore]
        public IQueryable<ItemType> BaseItems { get; set; }

        public IEnumerable<DtoItemType> Items { get; set; }

        public MetadataDTO Metadata { get; set; }

        public ListResultDTO(FilterType filter)
        {
            Filter = filter;
            Metadata = new MetadataDTO();
        }

        public void SetPageAndOrder(bool ignoreOrdering = false)
        {
            int itemsPerPage;

            if (Filter.ItemsPerPage == 0)
            {
                itemsPerPage = 500;
            }
            else
            {
                itemsPerPage = Filter.ItemsPerPage >= 1 ? Filter.ItemsPerPage : 10;
            }

            Metadata.CurrentPage = Filter.CurrentPage;

            if (!ignoreOrdering)
            {
                // Capitalize first letter
                var orderByColumn =
                    char.ToUpper(Filter.OrderByColumn[0]) + Filter.OrderByColumn.Substring(1);
                BaseItems = ApplyOrdering(BaseItems, orderByColumn, Filter.OrderByDescending);
            }

            Metadata.TotalItems = BaseItems.Count();

            BaseItems = BaseItems.Skip((Metadata.CurrentPage - 1) * itemsPerPage).Take(itemsPerPage);

            Metadata.TotalPages = (int)Math.Ceiling(Metadata.TotalItems / (double)itemsPerPage);

            if (Metadata.TotalPages == 0)
            {
                Metadata.TotalPages = 1;
            }
        }

        private static IQueryable<T> ApplyOrdering<T>(
            IQueryable<T> source,
            string columnName,
            bool descending
        )
        {
            // Return source if column name is null or empty
            if (string.IsNullOrWhiteSpace(columnName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");

            // Get the property and ensure it's not null
            var property =
                typeof(T).GetProperty(
                    columnName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                )
                ?? throw new ArgumentException(
                    $"Property '{columnName}' not found on type '{typeof(T).Name}'."
                );

            // Create the property access expression
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // Determine the correct ordering method
            string methodName = descending ? "OrderByDescending" : "OrderBy";

            // Retrieve the OrderBy/OrderByDescending method safely
            var method =
                typeof(Queryable)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2)
                ?? throw new InvalidOperationException(
                    $"Unable to find method '{methodName}' on Queryable."
                );

            // Make it generic for the specified type
            var genericMethod = method.MakeGenericMethod(typeof(T), property.PropertyType);

            // Invoke the method safely and handle null return
            var result = genericMethod.Invoke(null, new object[] { source, orderByExpression });

            if (result is IQueryable<T> queryableResult)
                return queryableResult;

            throw new InvalidOperationException($"Ordering operation failed on property '{columnName}'.");
        }
    }
}
