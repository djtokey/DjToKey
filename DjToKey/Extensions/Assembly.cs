using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Extensions
{
    public static class Assembly
    {
        public static string GetVersion(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyFileVersionAttribute));
        }

        public static string GetTitle(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyTitleAttribute));
        }

        public static string GetDescription(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyDescriptionAttribute));
        }

        public static string GetCopyright(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyCopyrightAttribute));
        }

        private static string customAttributeToString(System.Reflection.Assembly assembly, Type attributeType)
        {
            string s = null;
            var u = assembly.GetCustomAttributesData().Where(x => x.AttributeType == attributeType).FirstOrDefault();
            if (u != null)
            {
                s = u.ToString();
                s = s.Remove(0, s.IndexOf('"'));
                s = s.Remove(s.IndexOf(')'));
                s = s.Replace("\"", "");
            }

            return s;
        }

        public static Plugins.Metadata GetMetadata(this System.Reflection.Assembly assembly)
        {
            return new Plugins.Metadata()
            {
                Title = assembly.GetTitle(),
                Copyright = assembly.GetCopyright(),
                Description = assembly.GetDescription(),
                Version = assembly.GetVersion()                
            };
        }
    }
}
