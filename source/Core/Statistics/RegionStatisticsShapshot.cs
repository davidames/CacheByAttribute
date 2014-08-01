using System;

namespace CacheByAttribute.Core.Statistics
{
    /// <summary>
    /// I am a point-in-time, readonly snapshot of statistics for a cache region
    /// </summary>
    public class RegionStatisticsSnapshot
    {
        internal RegionStatisticsSnapshot()
        {
        }


        public string Name { get; set; }
        public int Hits { get; set; }
        public int Misses { get; set; }
        public int Puts { get; set; }
        public int Removals { get; set; }

        public int RemoveAlls { get; set; }

        public decimal HitRatio
        {
            get
            {
                return (Hits + Misses) == 0
                           ? 0
                           : Math.Round((decimal) Hits/(Hits + Misses)*100, 2);
            }
        }
        public DateTime LastResetOn { get; set; }
        public long CountItems { get; set; }
        public long SizeLimitMB { get; set; }
        public long SizeLimitPercent { get; set; }
        public TimeSpan PollingInterval { get; set; }
    }
}