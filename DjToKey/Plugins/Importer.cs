#region Licence
/*
 * DjToKey
 *
 * Copyright (C) Marcin Badurowicz 2015
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
#endregion

using Ktos.DjToKey.Plugins.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

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
        [ImportMany(typeof(IScriptObject))]
        private IEnumerable<IScriptObject> pluginsObjects;

        public IEnumerable<IScriptObject> Objects
        {
            get
            {
                return pluginsObjects;
            }
        }

        [ImportMany(typeof(IScriptType))]
        private IEnumerable<IScriptType> pluginsTypes;

        public IEnumerable<IScriptType> Types
        {
            get
            {
                return pluginsTypes;
            }
        }

        public void Import()
        {            
            var catalog = new AggregateCatalog();

            // adds all the parts found in all assemblies in \plugins subdirectory
            try
            {                
                catalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\plugins"));
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                
                CompositionContainer container = new CompositionContainer(catalog);                
                container.ComposeParts(this);
            }
            catch (DirectoryNotFoundException)
            {
                // if no plugins directory found, silently ignore
            }
        }
    }
}
