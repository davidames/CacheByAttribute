using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheByAttribute.Test.Attributes.Attributes.CacheableObjects
{
    public class ObjectWithCachableMethodsByArgs
    {
        [ReturnCacheKey]
        public string AMethodWithParamArray(params object[] parameters)
        {
            return Guid.NewGuid().ToString();
        }


        [ReturnCacheKey]
        public string AMethodWithIListParam(IList<string> strings )
        {
            return Guid.NewGuid().ToString();
        }

        [ReturnCacheKey]
        public string AMethodWithOneStringArg(string arg)
        {
            return Guid.NewGuid().ToString();
        }

         [ReturnCacheKey]
        public string AMethodWithOneGuidArg(Guid theArg)
        {
            return Guid.NewGuid().ToString();
        }

         [ReturnCacheKey]
         public string AMethodWithIHasCacheKey(AComplexObjectWithIHasCacheKey theArg)
         {
             return Guid.NewGuid().ToString();
         }

    }
}
