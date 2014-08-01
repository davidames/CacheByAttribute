using CacheByAttribute.Test.Attributes.Attributes.CacheableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheByAttribute.Test.Attributes.Attributes
{
    [TestClass]
    public class CachedMethodTest
    {
        [TestMethod]
        public void CanCacheWithExplicitKey()
        {
            //Arrange
            ObjectWithCachableMethodsByKeyOnly objectWithCachableMethods = new ObjectWithCachableMethodsByKeyOnly();
            Core.CacheManager.Remove("CacheTestKey1");

            //Act
            string result1 = objectWithCachableMethods.CachableMethodWithExplicitKey("David", "Ames");

            string result2 = objectWithCachableMethods.CachableMethodWithExplicitKey("David", "Ames");

            //Assert
            Assert.AreEqual(result1, result2);
            Assert.IsTrue(Core.CacheManager.Exists("CacheTestKey1"));
        }


        [TestMethod]
        public void CanRemoveWithExplicitKey()
        {
            //Arrange

            ObjectWithCachableMethodsByKeyOnly objectWithCachableMethods = new ObjectWithCachableMethodsByKeyOnly();

            objectWithCachableMethods.CachableMethodWithExplicitKey("David", "Ames");

            //Assert
            Assert.IsTrue(Core.CacheManager.Exists("CacheTestKey1"));

            //Act
            objectWithCachableMethods.CachableMethodWithExplicitRemoveKey("David", "Ames");

            //Assert

            Assert.IsFalse(Core.CacheManager.Exists("CacheTestKey1"));
        }


        [TestMethod]
        public void CanRemoveWithExplicitKeyAndRegion()
        {
            //Arrange

            ObjectWithCachableMethodsByKeyAndRegion objectWithCachableMethods = new ObjectWithCachableMethodsByKeyAndRegion();

            objectWithCachableMethods.CachableMethodWithExplicitKeyAndRegion("David", "Ames");

            //Act
            objectWithCachableMethods.CachableMethodWithExplicitRemoveKeyAndRegion("David", "Ames");

            //Assert
        }


        [TestMethod]
        public void CanPutWithKeyedObject()
        {
            //Arrange

            ObjectWithCachableMethodsByKeyAndRegion objectWithCachableMethods = new ObjectWithCachableMethodsByKeyAndRegion();
            AComplexObjectWithIHasCacheKey keyedObject = new AComplexObjectWithIHasCacheKey {CacheKey = "Roger"};

            //Act
            objectWithCachableMethods.CacheableMethodWithMockComplexObjectWithIHasCacheKeyProperty(keyedObject, "David", "Ames");

            //Assert

            Assert.IsFalse(Core.CacheManager.Exists("Roger"));
        }


        [TestMethod]
        public void CanMultipleRemove()
        {
            //Arrange

            ObjectWithCachableMethodsByKeyOnly objectWithCachableMethodsByKeyOnly = new ObjectWithCachableMethodsByKeyOnly();

            //Act
            objectWithCachableMethodsByKeyOnly.CachableMethodWithMultiple1("David", "Ames");
            objectWithCachableMethodsByKeyOnly.CachableMethodWithMultiple2("David", "Ames");

            //Assert
            Assert.IsTrue(Core.CacheManager.Exists("Multi1"));
            Assert.IsTrue(Core.CacheManager.Exists("Multi2"));

            //Act
            objectWithCachableMethodsByKeyOnly.CachableMethodWithMultipleRemove("David", "Ames");

            //Assert

            Assert.IsFalse(Core.CacheManager.Exists("Multi1"));
            Assert.IsFalse(Core.CacheManager.Exists("Multi2"));
        }
    }
}