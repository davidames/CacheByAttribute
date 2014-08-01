using System;

namespace CacheByAttribute.Test.Attributes.Attributes.CacheableObjects
{
    public class ObjectWithCachableMethodsByKeyOnly
    {
        [Cache(AbsoluteKey = "CacheTestKey1")]

        public virtual string CachableMethodWithExplicitKey(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }


        [CacheRemove(AbsoluteKey = "CacheTestKey1")]
        public virtual string CachableMethodWithExplicitRemoveKey(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }


        [Cache(KeyPrefix = "CacheTestMethod2", Region = "Davids Region")]
        public virtual string CachableMethodWithExplicitKeyAndRegion(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }



        [CacheRemove(AbsoluteKey = "Multi1")]
        [CacheRemove(AbsoluteKey = "Multi2")]
        public virtual string CachableMethodWithMultipleRemove(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }


        [Cache(AbsoluteKey = "Multi1")]
        public virtual string CachableMethodWithMultiple1(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }


        [Cache(AbsoluteKey = "Multi2")]
        public virtual string CachableMethodWithMultiple2(string arg1, string arg2)
        {
            return string.Format("{0}.{1}-{2}", arg1, arg2, Guid.NewGuid());
        }



    

     
    }
}