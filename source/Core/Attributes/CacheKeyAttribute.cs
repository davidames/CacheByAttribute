using System;

namespace CacheByAttribute
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class CacheKeyAttribute : Attribute
    {
        /// <summary>
        /// For arguments to cachable methods that are objects, this attribute indicates what property of the object forms part of the cache key. 
        ///  EG,  "UserName", would use a property called UserName as the cache key.
        /// </summary>
        /// <param name="propertyName"></param>
        public CacheKeyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }


        public string PropertyName { get; private set; }
    }
}