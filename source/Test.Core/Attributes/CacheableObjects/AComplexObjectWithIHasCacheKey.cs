namespace CacheByAttribute.Test.Attributes.Attributes.CacheableObjects
{
    public class AComplexObjectWithIHasCacheKey : IHasCacheKey
    {
        public string CacheKey { get; set; }
    }
}