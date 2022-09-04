#region License

/*
 * DjToKey
 *
 * Copyright (C) Marcin Badurowicz 2015-2016
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion License

using Ktos.DjToKey.Extensions;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Assembly = System.Reflection.Assembly;

namespace Ktos.DjToKey.Plugins
{
    /// <summary>
    /// This class is responsible for finding and loading all plugins
    /// from the their subdirectory using MEF.
    ///
    /// Based on: http://dotnetbyexample.blogspot.com/2010/04/very-basic-mef-sample-using-importmany.html
    /// </summary>
    internal class PluginImporter
    {
        /// <summary>
        /// List of all device handling plugins
        /// </summary>
        public DevicePlugins DevicePlugins { get; set; }

        /// <summary>
        /// List of all script plugins
        /// </summary>
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
            get { return loadedPlugins; }
        }

        /// <summary>
        /// Performs loading all plugins, importing them by MEF and
        /// loading their metadata
        /// </summary>
        public PluginImporter()
        {
            DevicePlugins = new DevicePlugins();
            ScriptPlugins = new ScriptPlugins();
            var catalog = new AggregateCatalog();

            // adds all the parts found in all assemblies in \plugins
            // subdirectory and in current assembly
            loadedPlugins = new List<Metadata>();
            DirectoryCatalog dirc = null;
            try
            {
                dirc = new DirectoryCatalog(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\plugins"
                );
                catalog.Catalogs.Add(dirc);
            }
            catch (DirectoryNotFoundException)
            {
                // if no plugins directory found, silently ignore
            }

            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            try
            {
                CompositionContainer container = new CompositionContainer(catalog);
                container.ComposeParts(DevicePlugins);
                container.ComposeParts(ScriptPlugins);
            }
            catch (ReflectionTypeLoadException)
            {
                // plugins cannot be loaded due to type mismatch set
                // dirc to null, so no plugins will be loaded for
                // their metadata
                dirc = null;
            }

            if (dirc != null)
            {
                foreach (var f in dirc.LoadedFiles)
                {
                    // load plugin assemblies again, only for
                    // reflection information, to get their metadata
                    var ass = Assembly.ReflectionOnlyLoadFrom(f);
                    var metadata = ass.GetMetadata();
                    if (metadata != null)
                        loadedPlugins.Add(metadata);
                }
            }
        }
    }
}
