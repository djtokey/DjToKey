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
using Ktos.DjToKey.Views;
using System.ComponentModel.Composition;
using System.Windows;

namespace Ktos.DjToKey.Scripts
{
    /// <summary>
    /// Class representing Document object available for scripts
    /// </summary>
    [Export(typeof(IScriptObject))]
    public class Message : IScriptObject
    {
        private const string NAME = "Message";

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
            get { return dimpl; }
        }

        private MessageImpl dimpl;

        /// <summary>
        /// Initializes new object for a script engine
        /// </summary>
        public Message()
        {
            dimpl = new MessageImpl();
        }
    }

    /// <summary>
    /// Document class has methods for showing messages
    /// </summary>
    public class MessageImpl
    {
        /// <summary>
        /// Shows a simple message box
        /// </summary>
        /// <param name="message">A message to be shown</param>
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Shows a toast message
        /// </summary>
        /// <param name="message">A message to be shown</param>
        public void Toast(string message)
        {
            MainWindow.Toast(message);
        }
    }
}
