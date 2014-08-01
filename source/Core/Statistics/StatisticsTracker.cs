using System;
using System.Collections.Concurrent;
using CacheByAttribute.Core.CacheProviders;
using CacheByAttribute.Core.Extensions;

namespace CacheByAttribute.Core.Statistics
{
    public class StatisticsTracker
    {
        private readonly Lazy<RegionStatistics> DefaultRegionStatistics;
        private readonly ConcurrentDictionary<string, Lazy<RegionStatistics>> RegionStatisticsDictionary;


        public StatisticsTracker()
        {
            DefaultRegionStatistics = new Lazy<RegionStatistics>(() => new RegionStatistics(""));
            RegionStatisticsDictionary = new ConcurrentDictionary<string, Lazy<RegionStatistics>>();
        }


        private RegionStatistics GetRegionStatistics(string region)
        {
            if (string.IsNullOrEmpty(region))
                return DefaultRegionStatistics.Value;

            Lazy<RegionStatistics> result = RegionStatisticsDictionary.GetOrAdd(region, new Lazy<RegionStatistics>(() => new RegionStatistics(region)));
            return result.Value;
        }


        public void Put(string region)
        {
            GetRegionStatistics(region).Put();
        }


        public void Hit(string region)
        {
            GetRegionStatistics(region).Hit();
        }


        public void Miss(string region)
        {
            GetRegionStatistics(region).Miss();
        }


        public void RemoveAll(string region)
        {
            GetRegionStatistics(region).RemoveAll();
        }


        public void Remove(string region)
        {
            GetRegionStatistics(region).Remove();
        }


        public void Reset(string region)
        {
            GetRegionStatistics(region).Reset();
        }


        public void Reset()
        {
            foreach (string region in RegionStatisticsDictionary.Keys)
                Reset(region);
        }


        public RegionStatisticsSnapshot CreateRegionStatisticsSnapshot(string region, RegionProperties regionProperties)
        {
            Throw.IfNull(regionProperties, "regionProperties");

            RegionStatisticsSnapshot snapshot = new RegionStatisticsSnapshot();

            GetRegionStatistics(region).PopulateSnapshot(snapshot);
            snapshot.CountItems = regionProperties.CountItems;
            snapshot.SizeLimitMB = regionProperties.SizeLimitBytes/1024/1024;
            snapshot.SizeLimitPercent = regionProperties.SizeLimitPercent;
            snapshot.PollingInterval = regionProperties.PollingInterval;
            return snapshot;
        }
    }
}