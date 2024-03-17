using Order.Domain.Filters;
using System.Linq.Dynamic.Core;

namespace Order.Domain.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> entities, _BaseFilter filter)
        {
            entities = entities.ApplySort(filter.Sort);

            filter.TotalRecords = entities.Count();

            if (filter.IsPaginated)
                entities = entities.Skip(filter.Skip).Take(filter.Take);

            return entities;
        }

        private static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (sort == null)
                return source;

            // split the sort string
            var lstSort = sort.Split(',');

            // run through the sorting options and create a sort expression string from them

            string completeSortExpression = "";
            foreach (var sortOption in lstSort)
            {
                // if the sort option starts with "-", we order
                // descending, otherwise ascending

                if (sortOption.StartsWith("-"))
                    completeSortExpression = completeSortExpression + sortOption.Remove(0, 1) + " descending,";
                else
                    completeSortExpression = completeSortExpression + sortOption + ",";

            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
                source = source.OrderBy(completeSortExpression.Remove(completeSortExpression.Count() - 1));

            return source;
        }
    }
}
