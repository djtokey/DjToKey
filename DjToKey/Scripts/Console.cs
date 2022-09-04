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
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Ktos.DjToKey.Scripts
{
    /// <summary>
    /// Class representing Console object available for scripts
    /// </summary>
    [Export(typeof(IScriptObject))]
    public class Console : IScriptObject
    {
        private const string NAME = "Console";

        /// <summary>
        /// Name of the object in a script engine
        /// </summary>
        public string Name
        {
            get { return NAME; }
        }

        /// <summary>
        /// Object available for a script engine
        /// </summary>
        public object Object
        {
            get { return cimpl; }
        }

        private ConsoleImpl cimpl;

        /// <summary>
        /// Initializes new object for a script engine
        /// </summary>
        public Console()
        {
            cimpl = new ConsoleImpl();
        }
    }

    /// <summary>
    /// Console class has methods for debugging purposes
    /// </summary>
    public class ConsoleImpl
    {
        /// <summary>
        /// The Log method shows message in a debug area
        /// </summary>
        /// <param name="message">A message to be shown</param>
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
