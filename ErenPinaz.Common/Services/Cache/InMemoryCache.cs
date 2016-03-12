using System;
using System.Runtime.Caching;

namespace ErenPinaz.Common.Services.Cache
{
    /* Credits: http://stackoverflow.com/questions/343899/how-to-cache-data-in-a-mvc-application */

    public class InMemoryCache : ICacheService
    {
        /// <summary>
        /// Creates or retreieves <see cref="MemoryCache"/> item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getItemCallback"></param>
        /// <returns><see cref="T"/></returns>
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            var item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(10));
            }
            return item;
        }
    }
}