using Drinktionary.Data.Pagination;
using Drinktionary.Misc;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace Drinktionary.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetPageUri(PaginationFilter paginationFilter, string route)
    {
        var _enpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), nameof(paginationFilter.PageNumber).ToCamelCase(), paginationFilter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, nameof(paginationFilter.PageSize).ToCamelCase(), paginationFilter.PageSize.ToString());
        return new Uri(modifiedUri);
    }
}