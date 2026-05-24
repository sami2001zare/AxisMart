namespace AxisMart.Framework.Pagination;

public sealed class PagedQuery(
    int pageNumber = 1,
    int pageSize = 10,
    string? sortBy = null,
    bool sortDescending = false)
{
    public int PageNumber { get; } = pageNumber < 1 ? 1 : pageNumber;
    public int PageSize { get; } = pageSize < 1 ? 10 : (pageSize > 100 ? 100 : pageSize);
    public string? SortBy { get; } = sortBy;
    public bool SortDescending { get; } = sortDescending;

    public int Skip => (PageNumber - 1) * PageSize;
}
