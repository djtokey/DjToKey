using Ktos.DjToKey.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Assembly = System.Reflection.Assembly;

namespace Ktos.DjToKey.Plugins
{
    /// <summary>
    /// This class is responsible for finding and loading all plugins from the their subdirectory
    /// using MEF.
    ///
    /// Based on: http://dotnetbyexample.blogspot.com/2010/04/very-basic-mef-sample-using-importmany.html
    /// </summary>
    class PluginImporter
    {
        public DevicePlugins DevicePlugins { get; set; }
        public ScriptPlugins ScriptPlugins { get; set; }

        /// <summary>
        /// List of metadata for loaded plugins
        /// </summary>
        private List<Metadata> loadedPlugins;

        /// <summary>
        /// List of metadata for loaded plugins
        /// </summary>
        public IEnumerable<Metadata> Plugins
        {
            get
            {
                return loadedPlugins;
            }
        }

        /// <summary>
        /// Performs loading all plugins, importing them by MEF and loading their metadata
        /// </summary>
        public PluginImporter()
        {
            DevicePlugins = new DevicePlugins();
            ScriptPlugins = new ScriptPlugins();
            var catalog = new AggregateCatalog();

            // adds all the parts found in all assemblies in \plugins subdirectory and in current assembly
            try
            {
                loadedPlugins = new List<Metadata>();
                var dirc = new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\plugins");
                catalog.Catalogs.Add(dirc);
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

                CompositionContainer container = new CompositionContainer(catalog);
                container.ComposeParts(DevicePlugins);
                container.ComposeParts(ScriptPlugins);

                foreach (var f in dirc.LoadedFiles)
                {
                    // load plugin assemblies again, only for reflection information, to get their metadata
                    var ass = Assembly.ReflectionOnlyLoadFrom(f);
                    var metadata = ass.GetMetadata();
                    if (metadata != null)
                        loadedPlugins.Add(metadata);
                }
            }
            catch (DirectoryNotFoundException)
            {
                // if no plugins directory found, silently ignore
            }
        }
    }
}
