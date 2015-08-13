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

#endregion Licence

using Ktos.DjToKey.Helpers;
using Ktos.DjToKey.Plugins;
using Ktos.DjToKey.Plugins.Contracts;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ktos.DjToKey.Models
{
    public class ScriptEngine : IScriptEngine
    {
        public IEnumerable<Metadata> Plugins { get; private set; }

        /// <summary>
        /// Instance of script engine
        /// </summary>
        private V8ScriptEngine eng;

        /// <summary>
        /// Configures script engine, adding static, useful object,
        /// debug objects, useful types and objects from additional plugins
        /// </summary>
        public void Configure()
        {
            // adding set of static object which will be available for scripts
            eng = new V8ScriptEngine();
            eng.AddHostObject("Keyboard", ScriptsHelper.Simulator.Keyboard);
            eng.AddHostObject("Mouse", ScriptsHelper.Simulator.Mouse);
            eng.AddHostObject("Document", ScriptsHelper.Document);
            eng.AddHostObject("Console", ScriptsHelper.Console);
            eng.AddHostObject("Global", ScriptsHelper.GlobalDictionary);

            // addind useful types
            eng.AddHostType("KeyCode", typeof(WindowsInput.Native.VirtualKeyCode));

            // adding objects and types coming from additional plugins
            PluginImporter i = new PluginImporter();
            i.Import();

            this.Plugins = i.Plugins;

            if (i.Objects != null)
            {
                foreach (var p in i.Objects)
                {
                    eng.AddHostObject(p.Name, p.Object);
                }
            }

            if (i.Types != null)
            {
                foreach (var p in i.Types)
                {
                    eng.AddHostType(p.Name, p.Type);
                }
            }
        }

        /// <summary>
        /// Executes script giving it also values of Control event which caused it and
        /// its value.
        /// </summary>
        /// <param name="s">Script to be executed</param>
        /// <param name="value">Value of an handled event</param>
        /// <param name="ctrl">Object representing control which caused event</param>
        public void Execute(Script s, object value, object ctrl)
        {
            eng.AddHostObject("Control", ctrl);
            eng.AddHostObject("Value", value);

            string script = s.Text;

            if (s.Path != null)
                script = File.ReadAllText(s.Path);

            eng.Execute(script);
        }
    }
}