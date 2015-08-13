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

using System.Diagnostics;
using System.Windows.Forms;
using WindowsInput;

namespace Ktos.DjToKey.Scripts
{
    /// <summary>
    /// Small static class which is a container for a few objects which
    /// will be useful in scripts
    /// </summary>
    public static class ScriptsHelper
    {
        /// <summary>
        /// A Document class instance
        /// </summary>
        public static Document Document = new Document();

        /// <summary>
        /// Reference to a console class
        /// </summary>
        public static Console Console = new Console();

        /// <summary>
        /// Instance of a InputSimulator class
        /// </summary>
        public static InputSimulator Simulator = new InputSimulator();

        /// <summary>
        /// A global dictionary usable by all scripts
        /// </summary>
        public static GlobalDictionary GlobalDictionary = new GlobalDictionary();
    }

    /// <summary>
    /// Document class has methods for showing messages
    /// </summary>
    public class Document
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

    /// <summary>
    /// Console class has methogs for debugging purposes
    /// </summary>
    public class Console
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