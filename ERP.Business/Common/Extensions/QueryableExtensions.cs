using ERP.Business.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Business.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query,
        PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(paginationQuery);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
            .Take(paginationQuery.PageSize)
            .ToListAsync(cancellationToken);

        return PaginatedResult<T>.Create(
            items,
            paginationQuery.PageNumber,
            paginationQuery.PageSize,
            totalCount);
    }
}
