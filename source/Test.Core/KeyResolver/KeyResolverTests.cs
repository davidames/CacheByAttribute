using System;
using System.Collections.Generic;
using CacheByAttribute.Test.Attributes.Attributes.CacheableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheByAttribute.Test.Attributes.KeyResolver
{
    [TestClass]
    public class KeyResolverTests
    {
        [TestMethod]
        public void CanParamArrayProduceDifferentKeys()
        {
            //Arrange
            ObjectWithCachableMethodsByArgs testClass = new ObjectWithCachableMethodsByArgs();

            //Act
            string cacheKey1 = testClass.AMethodWithParamArray("Param1");
            string cacheKey2 = testClass.AMethodWithParamArray("Param2");

            //Assert
            Assert.AreNotEqual(cacheKey1, cacheKey2);
        }


        [TestMethod]
        public void CanIListParamProduceCorrectKeys()
        {
            //Arrange
            IList<string> strings = new List<string>();
            strings.Add("string1");
            strings.Add("string2");

            ObjectWithCachableMethodsByArgs testClass = new ObjectWithCachableMethodsByArgs();

            //Act
            string cacheKey = testClass.AMethodWithIListParam(strings);

            //Assert
            Assert.IsTrue(cacheKey.Contains("string1"));
            Assert.IsTrue(cacheKey.Contains("string2"));
        }


        [TestMethod]
        public void CanCacheKeyContainClassAndMethodName()
        {
            //Arrange

            ObjectWithCachableMethodsByArgs testClass = new ObjectWithCachableMethodsByArgs();

            //Act
            string cacheKey = testClass.AMethodWithOneStringArg("theArg");

            //Assert
            Assert.IsTrue(cacheKey.Contains("ObjectWithCachableMethodsByArgs"));
            Assert.IsTrue(cacheKey.Contains("AMethodWithOneStringArg"));
        }


        [TestMethod]
        public void CanCacheKeyContainArgValueForSimpleStringArg()
        {
            //Arrange

            ObjectWithCachableMethodsByArgs testClass = new ObjectWithCachableMethodsByArgs();

            //Act
            string cacheKey = testClass.AMethodWithOneStringArg("theArg");

            //Assert
            Assert.IsTrue(cacheKey.Contains("theArg"));
        }


        [TestMethod]
        public void CanCacheKeyContainArgValueForGuidArg()
        {
            //Arrange

            ObjectWithCachableMethodsByArgs testClass = new ObjectWithCachableMethodsByArgs();
            Guid theArg = Guid.NewGuid();
            //Act
            string cacheKey = testClass.AMethodWithOneGuidArg(theArg);

            //Assert
            Assert.IsTrue(cacheKey.Contains(theArg.ToString()));
        }


        [TestMethod]
        public void CanCacheKeyContainValueForIHasCacheKey()
        {
            //Arrange

            ObjectWithCachableMethodsByArgs testClass = new ObjectWithCachableMethodsByArgs();
            AComplexObjectWithIHasCacheKey theArg = new AComplexObjectWithIHasCacheKey {CacheKey = "TheCacheKey"};

            //Act
            string cacheKey = testClass.AMethodWithIHasCacheKey(theArg);

            //Assert
            Assert.IsTrue(cacheKey.Contains(theArg.CacheKey));
        }

        /*
         * Introduce AbsoluteKey
         * Make Key = KeyPrefix
         
         * 
         */
    }
}