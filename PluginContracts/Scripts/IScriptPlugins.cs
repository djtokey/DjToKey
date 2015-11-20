#region License

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

#endregion License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Plugins.Scripts
{
    /// <summary>
    /// A generic interface for a collection of possible
    /// script object and types to be added to script engine
    /// </summary>
    public interface IScriptPlugins
    {        
        /// <summary>
        /// List of objects from plugins to be included into script engine when loading
        /// </summary>
        IEnumerable<IScriptObject> Objects { get; }
        
        /// <summary>
        /// List of types from plugins to be included into script engine when loading
        /// </summary>
        IEnumerable<IScriptType> Types { get; }        
    }
}
