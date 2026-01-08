using Microsoft.EntityFrameworkCore;

namespace QuizMaker.Application.Paginations;

public static class PaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
    this IQueryable<T> source,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
            pageNumber = 1;
        if (pageSize <= 0)
            pageSize = 10;

        int totalCount = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }
}
