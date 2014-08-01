using System;

namespace CacheByAttribute.Core.CacheProviders
{
    public class RegionProperties
    {
        public long CountItems { get; set; }
        public long SizeLimitBytes { get; set; }
        public long SizeLimitPercent { get; set; }
        public TimeSpan PollingInterval { get; set; }
    }
}