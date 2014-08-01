using System;
using System.Reflection;
using CacheByAttribute.Core;
using CacheByAttribute.Core.Extensions;
using CacheByAttribute.Core.KeyResolvers;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace CacheByAttribute
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CacheAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Caches an item, using the class/method name and parameters as the key. Any parameters which implement IHasCacheKey will have have GetCacheKey called.
        /// </summary>
        public CacheAttribute()
        {
        }


      

        #region HideBasePostSharpProperties

        /* This section contains overrides of the base PostSharp properties which is intended to hide them. It makes the documentation of CacheByAttribute more concise */
        private new int AttributePriority
        {
            get { return base.AttributePriority; }
            set { base.AttributePriority = value; }
        }

        private new int AspectPriority
        {
            get { return base.AspectPriority; }
            set { base.AspectPriority = value; }
        }

        private new bool ApplyToStateMachine
        {
            get { return base.ApplyToStateMachine; }
            set { base.ApplyToStateMachine = value; }
        }

        private new bool AttributeExclude
        {
            get { return base.AttributeExclude; }
            set { base.AttributeExclude = value; }
        }

        private new MulticastInheritance AttributeInheritance
        {
            get { return base.AttributeInheritance; }
            set { base.AttributeInheritance = value; }
        }

        private new MulticastTargets AttributeTargetElements
        {
            get { return base.AttributeTargetElements; }
            set { base.AttributeTargetElements = value; }
        }

        private new MulticastAttributes AttributeTargetExternalMemberAttributes
        {
            get { return base.AttributeTargetExternalMemberAttributes; }
            set { base.AttributeTargetExternalMemberAttributes = value; }
        }

        private new MulticastAttributes AttributeTargetExternalTypeAttributes
        {
            get { return base.AttributeTargetExternalTypeAttributes; }
            set { base.AttributeTargetExternalTypeAttributes = value; }
        }

        private new MulticastAttributes AttributeTargetMemberAttributes
        {
            get { return base.AttributeTargetMemberAttributes; }
            set { base.AttributeTargetMemberAttributes = value; }
        }

        private new MulticastAttributes AttributeTargetParameterAttributes
        {
            get { return base.AttributeTargetParameterAttributes; }
            set { base.AttributeTargetParameterAttributes = value; }
        }

        private new MulticastAttributes AttributeTargetTypeAttributes
        {
            get { return base.AttributeTargetTypeAttributes; }
            set { base.AttributeTargetTypeAttributes = value; }
        }

        private new bool AttributeReplace
        {
            get { return base.AttributeReplace; }
            set { base.AttributeReplace = value; }
        }
        private new string AttributeTargetAssemblies
        {
            get { return base.AttributeTargetAssemblies; }
            set { base.AttributeTargetAssemblies = value; }
        }

        private new string AttributeTargetTypes
        {
            get { return base.AttributeTargetTypes; }
            set { base.AttributeTargetTypes = value; }
        }

        private new string AttributeTargetParameters
        {
            get { return base.AttributeTargetParameters; }
            set { base.AttributeTargetParameters = value; }
        }

        private new string AttributeTargetMembers
        {
            get { return base.AttributeTargetMembers; }
            set { base.AttributeTargetMembers = value; }
        }

        #endregion HideBasePostSharpProperties

        /// <summary>
        /// The name of the region for this cache. If empty, default will be used.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// The prefix for the cache key. If not specified, namespace.class.method will be used as a prefix for cache keys, with the parameter signature used for the rest of the key.
        /// </summary>
        public string KeyPrefix { get; set; }

        /// <summary>
        /// The absolute cache key.  If specified, this is used in preference to a generated cache key. Ie, KeyPrefix and the parameter signature is ignored
        /// </summary>
        public string AbsoluteKey { get;  set; }


        /// <summary>
        /// What is the maximum time this object can live in cache. Expressed in seconds. 
        /// If Expiry/IdleExpiry Hours/Minutes/Seconds are all 0 then this item will be eternal, however could be evicted due to memory presure.
        /// </summary>
        public int ExpirySeconds { get; set; }

        /// <summary>
        /// What is the maximum time this object can live in cache. Expressed in minutes. 
        /// If Expiry/IdleExpiry Hours/Minutes/Seconds are all 0 then this item will be eternal, however could be evicted due to memory presure.
        /// </summary>
        public int ExpiryMinutes { get; set; }

        /// <summary>
        /// What is the maximum time this object can live in cache. Expressed in hours. 
        /// If Expiry/IdleExpiry Hours/Minutes/Seconds are all 0 then this item will be eternal, however could be evicted due to memory presure.
        /// </summary>

        public int ExpiryHours { get; set; }

        /// <summary>
        /// What is the maximum time this object can live in cache since it was last accessed. Expressed in Seconds
        /// If Expiry/IdleExpiry Hours/Minutes/Seconds are all 0 then this item will be eternal, however could be evicted due to memory presure.
        /// </summary>

        public int IdleExpirySeconds { get; set; }
        /// <summary>
        /// What is the maximum time this object can live in cache since it was last accessed. Expressed in Minutes
        /// If Expiry/IdleExpiry Hours/Minutes/Seconds are all 0 then this item will be eternal, however could be evicted due to memory presure.
        /// </summary>
        public int IdleExpiryMinutes { get; set; }
        /// <summary>
        /// What is the maximum time this object can live in cache since it was last accessed. Expressed in Hours
        /// If Expiry/IdleExpiry Hours/Minutes/Seconds are all 0 then this item will be eternal, however could be evicted due to memory presure.
        /// </summary>
        /// 
        public int IdleExpiryHours { get; set; }




        /// <summary>
        /// Entrypoint for caching hooks. Called automatically by PostSharp
        /// </summary>
        /// <param name="args"></param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            Throw.IfNull(args, "args");
            string cacheKey = string.IsNullOrEmpty(AbsoluteKey)
                                  ? KeyResolver.ResolveKey(KeyPrefix, args.Method, args.Arguments.ToArray())
                                  : AbsoluteKey;

            object value = CacheManager.Get(cacheKey, Region);

            if (value != null)
            {
                args.ReturnValue = value;
                args.FlowBehavior = FlowBehavior.Return;
            }
            else
                args.MethodExecutionTag = cacheKey;
        }

        /// <summary>
        /// Automatically called by PostSharp when the cached method has executed.
        /// </summary>
        /// <param name="args"></param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Throw.IfNull(args, "args");
            string cacheKey = (string)args.MethodExecutionTag;

            CacheManager.Put(cacheKey, args.ReturnValue, CreateTimespan(ExpiryHours, ExpiryMinutes, ExpirySeconds), CreateTimespan(IdleExpiryHours, IdleExpiryMinutes, IdleExpirySeconds), Region);
        }


        private static TimeSpan? CreateTimespan(int hours, int minutes, int seconds)
        {
            if (hours == 0 & minutes == 0 && seconds == 0)
                return null;

            return new TimeSpan(hours, minutes, seconds);
        }
    }
}