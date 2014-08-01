using System;
using System.Collections.Generic;
using CacheByAttribute.Core.CacheProviders;
using CacheByAttribute.Core.Extensions;
using CacheByAttribute.Core.Statistics;


namespace CacheByAttribute.Core
{
    /// <summary>
    /// I am a singleton cache manager that provides a public API into the fabac caching system.    /// </summary>
    public sealed class CacheManager
    {
        private static readonly Lazy<InstanceCacheManager> _InstanceHolder = new Lazy<InstanceCacheManager>(CreateInstanceCacheManager);


        private CacheManager()
        {
            /*
             * Used as part of the singleton pattern from  
             * http://csharpindepth.com/Articles/General/Singleton.aspx
             * */
        }


        /// <summary>
        /// Are statistics for hits/misses/removals tracked?  There is a small overhead in tracking statistics. Statistics are enabled by default.
        /// </summary>
        public static bool StatisticsEnabled
        {
            get { return Instance.StatisticsEnabled; }
            set { Instance.StatisticsEnabled = value; }
        }

        private static InstanceCacheManager Instance
        {
            get { return _InstanceHolder.Value; }
        }

        /// <summary>
        /// Returns a readonly snapshot of the cache statistics for all regions
        /// </summary>
        public static IEnumerable<RegionStatisticsSnapshot> StatisticsSnapshots
        {
            get { return Instance.GetStatisticsSnapshots(); }
        }


        private static InstanceCacheManager CreateInstanceCacheManager()
        {
            return new InstanceCacheManager(new MemoryCacheProvider(), new StatisticsTracker());
        }


        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="key">The key to use </param>
        /// <param name="value">The value to cache</param>
        /// <param name="expiresIn">How long until the item will expire regardless of usage</param>
        /// <param name="validFor">How long the item will remain in cache if unused.</param>
        /// <param name="region">The name of the region, if null, the default region will be used.</param>
        public static void Put(string key,object value, TimeSpan? expiresIn, TimeSpan? validFor, string region = null)
        {
            Throw.IfNullOrEmpty(key, "key");
            Throw.IfNull(value, "value");
            Instance.Put(key, value, CalculateAbsoluteExpiry(expiresIn), validFor, region);
        }


        private static DateTime? CalculateAbsoluteExpiry(TimeSpan? expiresIn)
        {
            if (expiresIn == null)
                return null;

            return DateTime.Now.Add(expiresIn.Value);
        }

        /// <summary>
        /// Remove a single item from the cache.
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <param name="region">The name of the region, if null, the default region will be used.</param>
        public static void Remove(string key, string region = null)
        {
            Throw.IfNullOrEmpty(key, "key");
            Instance.Remove(key, region);
        }

        /// <summary>
        /// Retrieve an item from the cache.  If the item does not exist, null will be returned.
        /// </summary>
        /// <param name="key">The key of the item to retrieve</param>
        /// <param name="region">The name of the region, if null, the default region will be used.</param>
        /// <returns>The object if it exists in cache, otherwise, Null.</returns>
        public static object Get(string key, string region = null)
        {
            Throw.IfNullOrEmpty(key, "key");
            return Instance.Get(key, region);
        }


        /// <summary>
        /// Get a statistics snapshot for the specified region. .
        /// </summary>
        /// <param name="region">The name of the region, if null, the default region will be used.</param>
        /// <returns>A readonly statistics snapshot</returns>
        public static RegionStatisticsSnapshot GetStatisticsSnapshot(string region = null)
        {
            return Instance.GetStatisticsSnapshot(region);
        }

        /// <summary>
        /// Check to see if an item is in cache
        /// </summary>
        /// <param name="key">The key of the item to retrieve</param>
        /// <param name="region">The name of the region, if null, the default region will be used.</param>
        /// <returns>True if an item exists in cache</returns>
        public static bool Exists(string key, string region = null)
        {
            Throw.IfNull(key, "key");
            return Instance.Exists(key, region);
        }

        /// <summary>
        /// Removes all items from the specified region. Cannot be used on the default region.
        /// </summary>
        /// <param name="region">The name of the region. This must be supplied</param>
        public static void RemoveAllFromRegion(string region)
        {
            Throw.IfNullOrEmpty(region, "region");
            Instance.RemoveAllFromRegion(region);
        }
    }
}