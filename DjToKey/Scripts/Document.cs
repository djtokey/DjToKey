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

using Ktos.DjToKey.Plugins.Scripts;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace Ktos.DjToKey.Scripts
{
    [Export(typeof(IScriptObject))]
    public class Document : IScriptObject
    {
        private const string name = "Document";

        public string Name
        {
            get
            {
                return name;
            }
        }

        public object Object
        {
            get
            {
                return dimpl;
            }
        }

        private DocumentImpl dimpl;

        public Document()
        {
            dimpl = new DocumentImpl();
        }
    }

    /// <summary>
    /// Document class has methods for showing messages
    /// </summary>
    public class DocumentImpl
    {
        /// <summary>
        /// Named for a sake of consistency with browser implementations, Alert shows message box
        /// </summary>
        /// <param name="message">A message to be shown</param>
        public void Alert(string message)
        {
            MessageBox.Show(message);
        }
    }
}