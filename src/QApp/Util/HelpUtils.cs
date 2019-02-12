using MagnetArgs;
using QApp.Documentation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace QApp.Util
{
    public static class HelpUtils
    {
        public static IEnumerable<HelpAttribute> GetHelpAttributes(object obj)
        {
            var output = new List<HelpAttribute>();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo propertyInfo = properties[i];
                OptionSetAttribute attribute = GetAttribute<OptionSetAttribute>(propertyInfo);

                if (null != attribute)
                {
                    var o = (IEnumerable<HelpAttribute>)typeof(HelpUtils)
                    .GetMethod("GetHelpFromOptionSet")
                    .MakeGenericMethod(propertyInfo.PropertyType)
                    .Invoke(obj, new object[0]);

                    output.AddRange(o);
                }
            }

            return output;
        }

        public static IEnumerable<HelpAttribute> GetHelpFromOptionSet<T>() where T : IOption, new()
        {
            return GetHelpAttributes(new T());
        }

        private static IEnumerable<HelpAttribute> GetHelpAttributes<T>(T obj) where T : IOption
        {
            var helpItems = new List<HelpAttribute>();

            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                var attribute = GetAttribute<ArgAttribute>(propertyInfo);

                if (null != attribute)
                {
                    // set help description
                    var help = GetAttribute<HelpAttribute>(propertyInfo);
                    var isRequired = GetAttribute<IsRequiredAttribute>(propertyInfo);
                    var ifPresent = GetAttribute<IfPresentAttribute>(propertyInfo);
                    var @default = GetAttribute<DefaultAttribute>(propertyInfo);
                    var @parser = GetAttribute<ParserAttribute>(propertyInfo);

                    if (help != null)
                    {
                        help.SetOption(new OptionInfo()
                        {
                            Name = attribute.Name,
                            Alias = attribute.Alias,
                            IsRequired = (null != isRequired),
                            IfPresent = (null != ifPresent),
                            DefaultValue = @default == null ? null : @default.Value,
                            ParseValues = parser == null ? null : EnumToString(@parser.Type)
                        });
                        helpItems.Add(help);
                    }
                }
            }

            return helpItems;
        }

        private static string EnumToString(Type type)
        {
            var output = new StringBuilder();

            MemberInfo[] memberInfos = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

            for (int i = 0; i < memberInfos.Length; i++)
            {
                output.Append(memberInfos[i].Name);
                output.Append(memberInfos[i].GetType().Name);
            }

            return output.ToString();
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
