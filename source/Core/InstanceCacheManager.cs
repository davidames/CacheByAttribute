using System;
using System.Collections.Generic;
using System.Linq;
using CacheByAttribute.Core.CacheProviders;
using CacheByAttribute.Core.Statistics;

namespace CacheByAttribute.Core
{
    /// <summary>
    /// I am the private implementation of the faboc caching core.
    /// I am separated from the api of the cache core (ie, cacheManager ) to allow for testability.  
    /// </summary>
    internal class InstanceCacheManager
    {
        private readonly ICacheProvider _CacheProvider;
        private readonly StatisticsTracker _Statistics;


        internal InstanceCacheManager(ICacheProvider cacheProvider, StatisticsTracker statistics)
        {
            _CacheProvider = cacheProvider;
            _Statistics = statistics;
            StatisticsEnabled = true;
        }


        internal bool StatisticsEnabled { get; set; }


        internal void Put(string key, object value, string region = null)
        {
            if (StatisticsEnabled)
                _Statistics.Put(region);

            _CacheProvider.Put(key, value, region: region);
        }


        internal void Put(string key, object value, DateTime? expiresAt = null, TimeSpan? validFor = null, string region = null)
        {
            if (StatisticsEnabled)
                _Statistics.Put(region);

            _CacheProvider.Put(key, value, expiresAt, validFor, region);
        }


        public void Remove(string key, string region = null)
        {
            if (StatisticsEnabled)
                _Statistics.Remove(region);

            _CacheProvider.Remove(key, region);
        }


        public object Get(string key, string region = null)
        {
            object value = _CacheProvider.Get(key, region);
            if (value == null)

            {
                if (StatisticsEnabled)
                    _Statistics.Miss(region);
            }
            else if (StatisticsEnabled)
                _Statistics.Hit(region);

            return value;
        }


        public bool Exists(string key, string region = null)
        {
            return _CacheProvider.Exists(key, region);
        }


        public void ResetStatistics()
        {
            _Statistics.Reset();
        }


        public IEnumerable<RegionStatisticsSnapshot> GetStatisticsSnapshots()
        {
            return ((new List<RegionStatisticsSnapshot> {GetStatisticsSnapshot(null)}).Union(_CacheProvider.RegionNames.Select(GetStatisticsSnapshot))).ToList();
        }


        public RegionStatisticsSnapshot GetStatisticsSnapshot(string region = null)
        {
            return _Statistics.CreateRegionStatisticsSnapshot(region, _CacheProvider.GetRegionProperties(region));
        }


        public void RemoveAllFromRegion(string region)
        {
            if (StatisticsEnabled)
                _Statistics.RemoveAll(region);
            _CacheProvider.RemoveAll(region);
        }
    }
}