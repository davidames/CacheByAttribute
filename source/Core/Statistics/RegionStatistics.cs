using System;
using System.Threading;
using CacheByAttribute.Core.Extensions;

namespace CacheByAttribute.Core.Statistics
{
    /// <summary>
    /// I track the statistics for an individual cache region.
    /// </summary>
    public class RegionStatistics
    {
        private int _Hits;
        private int _Misses;
        private int _Puts;
        private int _Removals;
        private int _RemoveAlls; //Expired due to time policies


        public RegionStatistics(string name)
        {
            Name = string.IsNullOrEmpty(name)
                       ? "Default"
                       : name;
            Reset();
        }


        public string Name { get; private set; }
        public DateTime LastReset { get; private set; }


        public void Reset()
        {
            LastReset = DateTime.Now;

            _Hits = 0;
            _Puts = 0;
            _Misses = 0;
            _Removals = 0;
            _RemoveAlls = 0;
        }


        public void Hit()
        {
            Interlocked.Increment(ref _Hits);
        }


        public void Put()
        {
            Interlocked.Increment(ref _Puts);
        }


        public void Miss()
        {
            Interlocked.Increment(ref _Misses);
        }


        public void Remove()
        {
            Interlocked.Increment(ref _Removals);
        }


        public void RemoveAll()
        {
            Interlocked.Increment(ref _RemoveAlls);
        }


        public void PopulateSnapshot(RegionStatisticsSnapshot snapshot)
        {
            Throw.IfNull(snapshot, "snapshot");
            snapshot.Name = Name;
            snapshot.Hits = _Hits;
            snapshot.Misses = _Misses;
            snapshot.Removals = _Removals;
            snapshot.LastResetOn = LastReset;
            snapshot.Puts = _Puts;
            snapshot.RemoveAlls = _RemoveAlls;
        }
    }
}