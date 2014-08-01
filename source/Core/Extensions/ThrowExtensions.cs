using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace CacheByAttribute.Core.Extensions
{
    //Util class from http://stackoverflow.com/questions/8042596/static-throw-class-good-or-bad-practise
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Throw")]
    public static class Throw
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void IfNullOrEmpty<T>(string value, params object[] parameters) where T : Exception
        {
            If<T>(string.IsNullOrEmpty(value), parameters);
        }


        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "I"),
         SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", MessageId = "T")]
        public static void IfNullOrEmpty<T, I>(IEnumerable<I> enumerable, params object[] parameters) where T : Exception
        {
            If<T>(enumerable == null || enumerable.Count() == 0, parameters);
        }


        public static void IfNullOrEmpty(string value, string argumentName)
        {
            IfNullOrEmpty(value, argumentName, string.Format("Argument '{0}' cannot be null or empty.", argumentName));
        }


        public static void IfNullOrEmpty(string value, string argumentName, string message)
        {
            IfNullOrEmpty<ArgumentNullOrEmptyException>(value, message, argumentName);
        }


        public static void IfNullOrEmpty<T>(IEnumerable<T> enumerable, string argumentName)
        {
            IfNullOrEmpty(enumerable, argumentName, string.Format("Argument '{0}' cannot be null or empty.", argumentName));
        }


        public static void IfNullOrEmpty<T>(IEnumerable<T> enumerable, string argumentName, string message)
        {
            IfNullOrEmpty<ArgumentNullOrEmptyException, T>(enumerable, message, argumentName);
        }


        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void IfNull<T>(object value, params object[] parameters) where T : Exception
        {
            If<T>(value == null, parameters);
        }


        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void If<T>(bool condition, params object[] parameters) where T : Exception
        {
            if (condition)
            {
                List<Type> types = new List<Type>();
                List<object> args = new List<object>();
                foreach (object p in parameters ?? Enumerable.Empty<object>())
                {
                    types.Add(p.GetType());
                    args.Add(p);
                }

                ConstructorInfo constructor = typeof (T).GetConstructor(types.ToArray());
                T exception = constructor.Invoke(args.ToArray()) as T;
                throw exception;
            }
        }


        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object")]
        public static void IfNull(object value, string argumentName)
        {
            if (value == null)
                throw new ArgumentNullOrEmptyException(argumentName);
        }
    }

    [ComVisible(true)]
    [Serializable]
    public class ArgumentNullOrEmptyException : ArgumentException
    {
        public ArgumentNullOrEmptyException() : base("Value cannot be null or empty")
        {
        }


        public ArgumentNullOrEmptyException(string parameterName) : base("Value cannot be null or empty", parameterName)
        {
        }


        public ArgumentNullOrEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }


        public ArgumentNullOrEmptyException(string parameterName, string message) : base(parameterName, message)
        {
        }


        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries"), SecurityCritical]
        protected ArgumentNullOrEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}