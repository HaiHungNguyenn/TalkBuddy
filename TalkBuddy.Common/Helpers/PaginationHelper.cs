﻿using AutoMapper;
using TalkBuddy.Common.Models;

namespace TalkBuddy.Common.Helpers;

public class PaginationHelper
{
    public static PaginatedResult BuildPaginatedResult<T, TDto>(IMapper? mapper, IQueryable<T> source, int pageSize, int pageIndex)
    {
        var total = source.Count();
        if (total == 0)
        {
            return new PaginatedResult
            {
                PageIndex = 1,
                PageSize = pageSize,
                Data = new List<TDto>(),
                LastPage = 1,
                IsLastPage = true,
                Total = total
            };
        }

        pageSize = Math.Max(1, pageSize);
        var lastPage = (int)Math.Ceiling((decimal)total / pageSize);
        lastPage = Math.Max(1, lastPage);
        pageIndex = Math.Min(pageIndex, lastPage);
        var isLastPage = pageIndex == lastPage;

        var paginatedResult = new PaginatedResult
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            LastPage = lastPage,
            IsLastPage = isLastPage,
            Total = total
        };
        
        if (pageIndex > lastPage / 2)
        {
            var mod = total % pageSize;
            var skip = Math.Max((lastPage - pageIndex - 1) * pageSize + mod, 0);
            var take = isLastPage ? mod : pageSize;
            var reverse = source.Reverse();
            
            var res = reverse.Skip(skip).Take(take);
            paginatedResult.Data = mapper is null ? res.Reverse() : mapper.Map<TDto>(res.Reverse());
            return paginatedResult;
        }
        
        var results = source.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        paginatedResult.Data = results;
        paginatedResult.Data = mapper is null ? results : mapper.Map<TDto>(results);
        return paginatedResult;
    }
    
    public static PaginatedResult BuildPaginatedResult<T>(IQueryable<T> source, int pageSize, int pageIndex)
    {
        return BuildPaginatedResult<T, T>(null, source, pageSize, pageIndex);
    }
}