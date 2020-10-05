using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class MemoryAccess : IMemoryAccess
    {
        private readonly IMemoryCache _cache;
        private readonly IDataAccessService _dataAccess;

        public MemoryAccess(IMemoryCache memoryCache, IDataAccessService dataAccess)
        {
            _cache = memoryCache;
            _dataAccess = dataAccess;
        }

        public IEnumerable<SeaModel> GetSeaAreas()
        {
            IEnumerable<SeaModel> result;
            if (!_cache.TryGetValue(CacheKeys.SeaAreas, out result))
            {
                result = _dataAccess.GetSeaAreas();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));

                _cache.Set(CacheKeys.SeaAreas, result, cacheEntryOptions);
            }

            return result;
        }
    }

    public class CacheKeys
    {
        public static string SeaAreas { get { return "_SeaAreas"; } }
    }
}
