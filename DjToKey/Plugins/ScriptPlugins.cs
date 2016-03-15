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

using Ktos.DjToKey.Plugins.Scripts;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ktos.DjToKey.Plugins
{
    /// <summary>
    /// A collection of plugins added to scripts, consists of two
    /// Lists internally - list of IScriptObject and IScriptType,
    /// automatically imported by MEF
    /// </summary>
    internal class ScriptPlugins : IScriptPlugins
    {
#pragma warning disable 0649

        /// <summary>
        /// List of objects from plugins to be included into script
        /// engine when loading. MEF will automatically satisfy this
        /// list with every class implementing <see cref="IScriptObject"/>
        /// </summary>
        [ImportMany(typeof(IScriptObject))]
        private IEnumerable<IScriptObject> pluginsObjects;

        /// <summary>
        /// List of objects from plugins to be included into script
        /// engine when loading
        /// </summary>
        public IEnumerable<IScriptObject> Objects
        {
            get
            {
                return pluginsObjects;
            }
        }

        /// <summary>
        /// List of types from plugins to be included into script
        /// engine when loading. MEF will automatically satisfy this
        /// list with every class implementing <see cref="IScriptType"/>
        /// </summary>
        [ImportMany(typeof(IScriptType))]
        private IEnumerable<IScriptType> pluginsTypes;

        /// <summary>
        /// List of types from plugins to be included into script
        /// engine when loading
        /// </summary>
        public IEnumerable<IScriptType> Types
        {
            get
            {
                return pluginsTypes;
            }
        }
    }

#pragma warning restore 0649
}