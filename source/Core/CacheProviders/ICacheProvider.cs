using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CacheByAttribute.Core.CacheProviders
{
    public interface ICacheProvider
    {
        IEnumerable<string> RegionNames { get; }


        /// <summary>
        /// Insert or update a cache value with an expiry date and/or time to live.
        /// </summary>
        /// param name="region">The name of the cache region</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="region">The name of the region</param>
        /// <param name="expiresAt">The date/time this item is removed from cache at</param>
        /// <param name="validFor">How long this item will last in the cache if it is not accessed</param>
        void Put(string key, object value, DateTime? expiresAt = null, TimeSpan? validFor = null, string region = null);


        /// <summary>
        /// Retrieve a value from cache
        /// </summary>
        /// <param name="region">The name of the cache region</param>
        /// <param name="key"></param>
        /// <returns>Cached value or null</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
        object Get(string key, string region = null);


        /// <summary>
        /// Removes the value for the given key from the cache
        /// </summary>
        /// <param name="region">The name of the cache region</param> 
        /// <param name="key"></param>
        void Remove(string key, string region = null);


        /// <summary>
        /// Returns whether the cache contains a value for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="region">The name of the cache region</param>
        /// <returns></returns>
        bool Exists(string key, string region = null);


        RegionProperties GetRegionProperties(string region);


        /// <summary>
        /// Removes all entries from the given region.  
        /// </summary>
        /// <param name="region">Name of region, cannot be null or empty</param>
        void RemoveAll(string region);
    }
}