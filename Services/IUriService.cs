using Drinktionary.Data.Pagination;

namespace Drinktionary.Services;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter paginationFilter, string route);
}