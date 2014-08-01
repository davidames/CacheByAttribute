using CacheByAttribute.Core;
using CacheByAttribute.Core.CacheProviders;
using CacheByAttribute.Core.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CacheByAttribute.Test.Attributes.CacheManager
{
    [TestClass]
    public class StatisticsTests
    {
        private const string TestRegionName = "TestRegion";
        private const string TestKeyName = "TestKey";


        [TestMethod]
        public void CanRestStatistics()
        {
            //Arrange
            InstanceCacheManager cacheManager = GetCacheManagerWithMockedCache(true);

            //Act
            cacheManager.Put(TestKeyName, "SomeValue", TestRegionName);
            cacheManager.Get(TestKeyName, TestRegionName);
            cacheManager.ResetStatistics();
            RegionStatisticsSnapshot snapshot = cacheManager.GetStatisticsSnapshot(TestRegionName);

            //Assert
            Assert.IsNotNull(snapshot);
            Assert.AreEqual(0, snapshot.Puts);
            Assert.AreEqual(0, snapshot.Hits);
            Assert.AreEqual(0, snapshot.Misses);
        }


        [TestMethod]
        public void CanSetIncrementPuts()
        {
            //Arrange
            InstanceCacheManager cacheManager = GetCacheManagerWithMockedCache(true);

            //Act
            cacheManager.Put(TestKeyName, "SomeValue", TestRegionName);
            RegionStatisticsSnapshot snapshot = cacheManager.GetStatisticsSnapshot(TestRegionName);

            //Assert
            Assert.IsNotNull(snapshot);
            Assert.AreEqual(1, snapshot.Puts);
        }


        [TestMethod]
        public void CanRemoveIncrementRemoves()
        {
            //Arrange
            InstanceCacheManager cacheManager = GetCacheManagerWithMockedCache(true);

            //Act
            cacheManager.Remove(TestKeyName, TestRegionName);
            RegionStatisticsSnapshot snapshot = cacheManager.GetStatisticsSnapshot(TestRegionName);

            //Assert
            Assert.IsNotNull(snapshot);
            Assert.AreEqual(1, snapshot.Removals);
        }


        [TestMethod]
        public void CanGetWithHitIncrementHits()
        {
            //Arrange
            InstanceCacheManager cacheManager = GetCacheManagerWithMockedCache(true);
            cacheManager.StatisticsEnabled = true;
            //Act
            cacheManager.Get(TestKeyName, TestRegionName);
            RegionStatisticsSnapshot snapshot = cacheManager.GetStatisticsSnapshot(TestRegionName);

            //Assert
            Assert.IsNotNull(snapshot);
            Assert.AreEqual(1, snapshot.Hits);
        }


        [TestMethod]
        public void CanGetWithMissIncrementMisses()
        {
            //Arrange
            InstanceCacheManager cacheManager = GetCacheManagerWithMockedCache(false);

            //Act
            cacheManager.Get(TestKeyName, TestRegionName);
            RegionStatisticsSnapshot snapshot = cacheManager.GetStatisticsSnapshot(TestRegionName);

            //Assert
            Assert.IsNotNull(snapshot);
            Assert.AreEqual(1, snapshot.Misses);
        }


        private InstanceCacheManager GetCacheManagerWithMockedCache(bool getIsHit)
        {
            RegionProperties regionProperties = new RegionProperties();

            Mock<ICacheProvider> mock = new Mock<ICacheProvider>();
            mock.Setup(x => x.GetRegionProperties(TestRegionName)).Returns(regionProperties);

            if (getIsHit)
                mock.Setup(x => x.Get(TestKeyName, TestRegionName)).Returns("ANonNullValue");
            else
                mock.Setup(x => x.Get(TestKeyName, TestRegionName)).Returns(null);

            InstanceCacheManager cacheManager = new InstanceCacheManager(mock.Object, new StatisticsTracker());
            cacheManager.StatisticsEnabled = true;
            return cacheManager;
        }
    }
}