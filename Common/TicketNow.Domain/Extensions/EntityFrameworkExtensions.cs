using System.Linq.Dynamic.Core;
using TicketNow.Domain.Filters;

namespace TicketNow.Domain.Extensions;

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

        var lstSort = sort.Split(',');
       
        string completeSortExpression = "";
        foreach (var sortOption in lstSort)
        {            
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

