namespace CacheByAttribute
{
    /// <summary>
    /// Complex objects that implement this interface will have CacheKey called when they are passed as a parmeter to a method that is decorated with the Cache attribute.
    /// </summary>
    public interface IHasCacheKey
    {
        /// <summary>
        /// The cache key for this object. Returning NULL will result in this object not being cached.
        /// </summary>
        string CacheKey { get; }
    }
}