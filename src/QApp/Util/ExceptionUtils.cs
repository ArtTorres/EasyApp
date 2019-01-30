using MagnetArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QApp.Util
{
    public static class ExceptionUtils
    {
        public static IEnumerable<Exception> GetInputExceptions(object obj)
        {
            var output = new List<Exception>();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo propertyInfo = properties[i];
                OptionSetAttribute attribute = GetAttribute<OptionSetAttribute>(propertyInfo);

                if (null != attribute)
                {
                    var property = (IOption)propertyInfo.GetValue(obj, null);

                    if (property.Exceptions.Count() > 0)
                        output.AddRange(property.Exceptions);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets an attribute from a MemberInfo instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        private static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            object[] attributes = member.GetCustomAttributes(true);

            T result;
            for (int i = 0; i < attributes.Length; i++)
            {
                object obj = attributes[i];

                if (obj is T)
                {
                    result = (T)((object)obj);
                    return result;
                }
            }

            return default(T);
        }
    }
}
