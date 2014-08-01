using System;
using CacheByAttribute.Core;
using CacheByAttribute.Core.Extensions;
using CacheByAttribute.Core.KeyResolvers;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace CacheByAttribute
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CacheRemoveAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Removes an entry from cache if it exists. Uses class/method name and parameters as the key. Any parameters which implement IHasCacheKey will have have GetCacheKey called.
        /// </summary>
        public CacheRemoveAttribute()
        {
        }



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

        public override void OnEntry(MethodExecutionArgs args)
        {
            Throw.IfNull(args, "args");
            string cacheKey = string.IsNullOrEmpty(AbsoluteKey)
                                  ? KeyResolver.ResolveKey(KeyPrefix, args.Method, args.Arguments.ToArray())
                                  : AbsoluteKey;
            CacheManager.Remove(cacheKey, Region);
        }
    }
}