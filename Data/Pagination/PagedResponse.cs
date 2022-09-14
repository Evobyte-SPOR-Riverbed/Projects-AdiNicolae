using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.Pagination;

public class PagedResponse<T>
{
    public PagedResponse(int totalRecords, Uri nextPage, Uri previousPage, T data)
    {
        TotalRecords = totalRecords;
        NextPage = nextPage;
        PreviousPage = previousPage;
        Data = data;
    }

    [Required(ErrorMessage = "Total records are required.")]
    public int TotalRecords { get; set; }

    [Required(ErrorMessage = "Next page is required.")]
    public Uri NextPage { get; set; }

    [Required(ErrorMessage = "Previous page is required.")]
    public Uri PreviousPage { get; set; }

    [Required(ErrorMessage = "Data is required.")]
    public T Data { get; set; }
}