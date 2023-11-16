namespace NetConf2023.Pagination.Handlers;

public class PaginatorHangler
{
    readonly int TotalElements;
    protected readonly int PageSize;

    public PaginatorHangler(int totalElements, int pageSize)
    {
        TotalElements = totalElements;
        PageSize = pageSize;
    }

    public int ActualPage { get; private set; } = 1;
    public virtual int TotalPages => TotalElements / PageSize;
    public virtual void ToPage(int page) => ActualPage = page;

}
