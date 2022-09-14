using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.Pagination;

public class PaginationFilter
{
    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 10 ? 10 : pageSize;
    }

    public PaginationFilter(PaginationFilter paginationFilter)
    {
        PageNumber = paginationFilter.PageNumber;
        PageSize = paginationFilter.PageSize;
    }

    [Required(ErrorMessage = "Page number is required.")]
    public int PageNumber { get; set; }

    [Required(ErrorMessage = "Page size is required.")]
    public int PageSize { get; set; }
}