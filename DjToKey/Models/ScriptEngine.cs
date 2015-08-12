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

using Ktos.DjToKey.Helpers;
using Ktos.DjToKey.Plugins;
using Microsoft.ClearScript.V8;
using System.Collections.Generic;
using System.IO;

namespace Ktos.DjToKey.Models
{
    public class ScriptEngine
    {
        public IEnumerable<string> Plugins { get; private set; }

        /// <summary>
        /// Instance of script engine
        /// </summary>
        private V8ScriptEngine eng;

        /// <summary>
        /// An instance of a script engine
        /// </summary>
        public V8ScriptEngine Engine
        {
            get
            {
                return eng;
            }
        }

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

            var pl = new List<string>();

            if (i.Objects != null)
            {
                foreach (var p in i.Objects)
                {
                    eng.AddHostObject(p.Name, p.Object);
                    pl.Add(p.Description);
                }
            }

            if (i.Types != null)
            {
                foreach (var p in i.Types)
                {
                    eng.AddHostType(p.Name, p.Type);
                    pl.Add(p.Description);
                }
            }

            Plugins = pl;
        }

        /// <summary>
        /// Executes script giving it also values of Control event which caused it and
        /// its value.
        /// </summary>
        /// <param name="val">Value in numerical form</param>
        /// <param name="ctrl">Object of MidiControl with name and type of control which caused event</param>
        /// <param name="eng">Instance of script engine</param>
        public void Execute(Script s, int val, MidiControl ctrl)
        {
            eng.AddHostObject("Control", ctrl);
            eng.AddHostObject("Value", new { Raw = val, Transformed = (val == 127) ? 1 : -1 });

            string script = s.Text;

            if (s.Path != null)
                script = File.ReadAllText(s.Path);

            eng.Execute(script);
        }        
    }
}
