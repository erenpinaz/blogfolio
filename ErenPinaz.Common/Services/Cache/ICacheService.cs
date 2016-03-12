using System;

namespace ErenPinaz.Common.Services.Cache
{
    /* Credits: http://stackoverflow.com/questions/343899/how-to-cache-data-in-a-mvc-application */

    public interface ICacheService
    {
        /// <summary>
        ///     Creates or retrieves cache item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getItemCallback"></param>
        /// <returns>
        ///     <see cref="T" />
        /// </returns>
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
    }
}