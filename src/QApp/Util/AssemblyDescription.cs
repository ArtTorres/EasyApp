using System;
using System.Collections.Generic;
using System.Reflection;

namespace QApp.Util
{
    internal class AssemblyDescription
    {
        public static string AssemblyFile
        {
            get
            {
                return AppDomain.CurrentDomain.FriendlyName;
            }
        }

        public static string[] Description
        {
            get
            {
                var output = new List<string>();

                Assembly entryAssembly = Assembly.GetEntryAssembly();

                // title
                AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyTitleAttribute));
                output.Add(assemblyTitleAttribute.Title);

                // version
                output.Add(string.Format("{0}.{1}.{2}.{3}", entryAssembly.GetName().Version.Major, entryAssembly.GetName().Version.Minor, entryAssembly.GetName().Version.Build, entryAssembly.GetName().Version.Revision));

                // company
                AssemblyCompanyAttribute assemblyCompanyAttribute = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCompanyAttribute));
                output.Add(assemblyCompanyAttribute.Company);

                // trademark
                AssemblyTrademarkAttribute assemblyTrademarkAttribute = Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyTrademarkAttribute)) as AssemblyTrademarkAttribute;
                if (null != assemblyTrademarkAttribute)
                    output.Add(assemblyTrademarkAttribute.Trademark);

                return output.ToArray();
            }
        }
    }
}
