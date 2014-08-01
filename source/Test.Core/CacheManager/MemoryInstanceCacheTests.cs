using CacheByAttribute.Core;
using CacheByAttribute.Core.CacheProviders;
using CacheByAttribute.Core.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheByAttribute.Test.Attributes.CacheManager
{
    [TestClass]
    public class MemoryInstanceCacheTests
    {
        private const string TestRegionName = "TestRegion";
        private const string TestKeyName = "TestKey";


        [TestMethod]
        public void CanPutAndGetItem()
        {
            //Arrange
            string inputPayload = "This is the payload";
            InstanceCacheManager instanceCacheManager = GetCacheManager();
            instanceCacheManager.Put(TestKeyName, inputPayload, TestRegionName);

            //Act
            string outputPayload = (string) instanceCacheManager.Get(TestKeyName, TestRegionName);

            //Assert
            Assert.AreEqual(inputPayload, outputPayload);
        }


        private InstanceCacheManager GetCacheManager()
        {
            InstanceCacheManager instanceCacheManager = new InstanceCacheManager(new MemoryCacheProvider(), new StatisticsTracker());
            return instanceCacheManager;
        }
    }
}