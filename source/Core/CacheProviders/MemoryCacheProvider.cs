using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace CacheByAttribute.Core.CacheProviders
{
    internal class MemoryCacheProvider : ICacheProvider
    {
        private readonly ConcurrentDictionary<string, Lazy<MemoryCache>> CacheRegions;


        public MemoryCacheProvider()
        {
            //MemoryCache does not support multiple regions - we will implement our own regions using a ConcurrentDictionary.  See
            //http://softwareblog.alcedo.com/post/2012/01/11/Sometimes-being-Lazy-is-a-good-thing.aspx for details of Lazy
            CacheRegions = new ConcurrentDictionary<string, Lazy<MemoryCache>>();
        }


        public void Put(string key, object value, DateTime? expiresAt = null, TimeSpan? validFor = null, string region = null)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (expiresAt != null)
                policy.AbsoluteExpiration = expiresAt.Value;

            if (validFor != null)
                policy.SlidingExpiration = validFor.Value;

            Put(key, value, policy, region);
        }


        public object Get(string key, string region = null)
        {
            return GetCacheRegion(region).Get(key, null);
        }


        public void Remove(string key, string region = null)
        {
            GetCacheRegion(region).Remove(key, null);
        }


        public void RemoveAll(string region = null)
        {
            if (string.IsNullOrEmpty(region))
                throw new ArgumentNullException(region, "You cannot specify the default (null) region");

            Lazy<MemoryCache> retValue = null;
            MemoryCache cacheRegion = GetCacheRegion(region);

            CacheRegions.TryRemove(region, out retValue);

            if (cacheRegion != null)
                cacheRegion.Dispose();
        }


        public bool Exists(string key, string region = null)
        {
            return GetCacheRegion(region).Contains(key, null);
        }


        /// <summary>
        /// Returns the basic properties for the specific region, or default region if not supplied
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public RegionProperties GetRegionProperties(string region = null)
        {
            return new RegionProperties
            {
                CountItems = GetCacheRegion(region).GetCount(null),
                PollingInterval = GetCacheRegion(region).PollingInterval,
                SizeLimitBytes = GetCacheRegion(region).CacheMemoryLimit,
                SizeLimitPercent = GetCacheRegion(region).PhysicalMemoryLimit
            };
        }


        public IEnumerable<string> RegionNames
        {
            get { return CacheRegions.Keys; }
        }


        public bool RegionExists(string region)
        {
            return CacheRegions.ContainsKey(region);
        }


        private MemoryCache GetCacheRegion(string name)
        {
            if (string.IsNullOrEmpty(name))
                return MemoryCache.Default;

            Lazy<MemoryCache> result = CacheRegions.GetOrAdd(name, new Lazy<MemoryCache>(() => new MemoryCache(name)));
            return result.Value;
        }


        private void Put(string key, object value, CacheItemPolicy policy, string region = null)
        {
            GetCacheRegion(region).Set(key, value, policy);
        }
    }
}