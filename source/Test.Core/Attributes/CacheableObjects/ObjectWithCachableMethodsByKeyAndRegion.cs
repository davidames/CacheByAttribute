using System;

namespace CacheByAttribute.Test.Attributes.Attributes.CacheableObjects
{
    public class ObjectWithCachableMethodsByKeyAndRegion
    {
      

        [Cache(KeyPrefix = "CacheTestMethod2", Region = "Davids Region")]
        public virtual string CachableMethodWithExplicitKeyAndRegion(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }


        [CacheRemove(KeyPrefix = "CacheTestMethod2", Region = "Davids Region")]
        public virtual string CachableMethodWithExplicitRemoveKeyAndRegion(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }



        [Cache(Region = "Davids Region")]
        public virtual string CacheableMethodWithMockComplexObjectWithIHasCacheKeyProperty(AComplexObjectWithIHasCacheKey obj, string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }


        [Cache(KeyPrefix = "Multi1", Region = "Bob")]
        public virtual string CacheableMethodWithBobsRegion(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }


        [CacheRemoveAllFromRegion("Bob")]
        public virtual string CacheRemoveAllFromBobsRegion(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }
    }
}