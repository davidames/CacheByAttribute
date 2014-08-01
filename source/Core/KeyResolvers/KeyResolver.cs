using System;
using System.Collections;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using CacheByAttribute.Core.Extensions;

namespace CacheByAttribute.Core.KeyResolvers
{
    public static class KeyResolver
    {
        /// <summary>
        /// Generates a cache key based on the parameters passed to the method.
        /// </summary>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        public static string ResolveKey(string keyPrefix, MethodBase methodBase, object[] paramValues)
        {
            Throw.IfNull(methodBase, "methodInfo");
            Throw.IfNull(paramValues, "paramValues");

            StringBuilder keyBuilder = new StringBuilder();

            keyBuilder.Append(string.IsNullOrEmpty(keyPrefix)
                                  ? BuildKeyPrefix(methodBase)
                                  : keyPrefix);

            keyBuilder.Append("(");
            keyBuilder.Append(BuildParamSignature(methodBase, paramValues));
            keyBuilder.Append(")");


         
            return keyBuilder.ToString();
        }


     
        private static string BuildParamSignature(MethodBase methodBase, object[] paramValues)
        {
           

            // Format a list with argument names and values.
            ParameterInfo[] paramInfos = methodBase.GetParameters();
            StringBuilder paramBuilder = new StringBuilder();
            bool hasItem = false;
       
            if (paramInfos.Length == paramValues.Length)
            {
                for (int paramIndex = 0; paramIndex < paramInfos.Length; paramIndex++)
                {
                    ParameterInfo paramInfo = paramInfos[paramIndex];

                    string value = GetParamValue(paramInfo, paramValues[paramIndex]) ?? "";

                    if (hasItem)
                        paramBuilder.Append(",");

                    if (paramInfos[paramIndex].IsOptional)
                    {
                        paramBuilder.Append(paramInfo.Name);
                        paramBuilder.Append(":");
                    }
                        
                    hasItem = true;
                    paramBuilder.Append(value);

                }
            }
           
            return paramBuilder.ToString();
            
        }


        private static string BuildKeyPrefix(MethodBase methodBase)
        {
            StringBuilder sb=  new StringBuilder();
            sb.Append( methodBase.DeclaringType == null
                                ? ""
                                : string.Format("{0}.", methodBase.DeclaringType.FullName));

           
            sb.Append( methodBase.Name);

            return sb.ToString();
        }


        private static string GetParamValue(ParameterInfo paramInfo, object paramValue)
        {
            if (paramValue == null)
                return null;

            //If the param has a CacheKeyAttribute, use that to look up the key from the object.
            CacheKeyAttribute cacheKeyAttribute = paramInfo.GetCustomAttribute<CacheKeyAttribute>();
            if (cacheKeyAttribute != null)
                return GetPropertyFromObject(cacheKeyAttribute.PropertyName, paramValue);
               
            
            //If the parameter value is an object that implements IHasCacheKeyProperty then we use that value.
            if (paramValue is IHasCacheKey)
                return ((IHasCacheKey) paramValue).CacheKey;

            if (paramValue is IEnumerable && ! (paramValue is String) )
            {
                StringBuilder concatKeyBuilder= new StringBuilder();
                foreach (var item in (IEnumerable)paramValue)
                {
                    concatKeyBuilder.Append(string.Format("{0},", item.ToString()));
                }
                return concatKeyBuilder.ToString();
            }
            else
            {
                return  paramValue.ToString();
            }
           
        }


        private static string GetPropertyFromObject(string propertyName, object paramValue)
        {
            PropertyInfo property = paramValue.GetType().GetProperty(propertyName);
            if (property == null)
                return null;

            return property.GetValue(paramValue) == null
                       ? null
                       : property.GetValue(paramValue).ToString();
        }


      
    }
}