using Drinktionary.Data.Pagination;
using Drinktionary.Services;
using System.Collections.Generic;

namespace Drinktionary.Misc;

public static class Helpers
{
    public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
    {
        int totalPages = ((totalRecords - 1) / validFilter.PageSize) + 1;
        return new PagedResponse<List<T>>(totalRecords,
                                    validFilter.PageNumber > 0 && validFilter.PageNumber < totalPages
                                        ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                                        : null,
                                    validFilter.PageNumber - 1 > 0 && validFilter.PageNumber <= totalPages
                                        ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                                        : null,
                                    pagedData);
    }

    public static string ToCamelCase(this string str)
    {
        if (!string.IsNullOrEmpty(str) && str.Length > 1)
        {
            return char.ToLowerInvariant(str[0]) + str[1..];
        }

        return str.ToLowerInvariant();
    }
}