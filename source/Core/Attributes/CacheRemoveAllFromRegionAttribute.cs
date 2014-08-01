using System;
using System.Reflection;
using CacheByAttribute.Core;
using CacheByAttribute.Core.Extensions;
using PostSharp.Aspects;
using PostSharp.Extensibility;


namespace CacheByAttribute
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CacheRemoveAllFromRegionAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Removes all entries from the specified region.
        /// </summary>
        /// <param name="region">The name of the region to remove all entries from. Cannot be null/empty string</param>
        public CacheRemoveAllFromRegionAttribute(string region)
        {
            Throw.IfNullOrEmpty(region, "Region");
            Region = region;
        }

        /// <summary>
        /// The name of the region to remove all entries from.
        /// </summary>
        public string Region { get; private set; }


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
            CacheManager.RemoveAllFromRegion(Region);
        }
    }
}