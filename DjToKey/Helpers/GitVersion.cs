using System.Reflection;

namespace Ktos.Build
{
    /// <summary>
    /// A simple class getting information from assembly built with Ktos.Build.GitVersion
    /// about InformationalVersion attribute covering full semver versioning of an assembly
    /// </summary>
    public class GitVersion
    {
        public static string FullSemVer
        {
            get
            {
                return (Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0] as
                AssemblyInformationalVersionAttribute).InformationalVersion;
            }
        }

        public static string SemVer
        {
            get
            {
                return (Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyVersionAttribute), false)[0] as
                AssemblyVersionAttribute).Version;
            }
        }
    }
}
