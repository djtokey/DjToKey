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
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ktos.DjToKey.Scripts
{
    /// <summary>
    /// Class representing Global object available to scripts
    /// </summary>
    [Export(typeof(IScriptObject))]
    public class Global : IScriptObject
    {
        private const string name = "Global";

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
                return gimpl;
            }
        }

        private GlobalDictionary gimpl;

        public Global()
        {
            gimpl = new GlobalDictionary();
        }
    }

    /// <summary>
    /// A global object accessible for all scripts on a page. It wraps
    /// a typical Dictionary of string as a key and object as a value.
    /// </summary>
    public class GlobalDictionary
    {
        private Dictionary<string, object> dict;

        /// <summary>
        /// Initializes a new instance of GlobalDictionary
        /// </summary>
        public GlobalDictionary()
        {
            dict = new Dictionary<string, object>();
        }

        /// <summary>
        /// Sets a value for a given key. If key does not exist, it is added.
        /// </summary>
        /// <param name="key">A key</param>
        /// <param name="value">Any value for this key</param>
        public void Set(string key, object value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }

        /// <summary>
        /// Gets a value for a given key
        /// </summary>
        /// <param name="key">A key to be retrieved value for</param>
        /// <returns></returns>
        public object Get(string key)
        {
            try
            {
                return dict[key];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}