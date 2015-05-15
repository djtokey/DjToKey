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

using Microsoft.ClearScript.V8;

namespace Ktos.DjToKey.Models
{
    /// <summary>
    /// Class encapsulating Script to be run when event occured
    /// </summary>
    class Script
    {
        /// <summary>
        /// Source code of a script
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Executes script giving it also values of Control event which caused it and
        /// its value.
        /// </summary>
        /// <param name="val">Value in numerical form</param>
        /// <param name="ctrl">Object of MidiControl with name and type of control which caused event</param>
        /// <param name="eng">Instance of script engine</param>
        public void Execute(int val, MidiControl ctrl, V8ScriptEngine eng)
        {
            
            eng.AddHostObject("Control", ctrl);
            eng.AddHostObject("Value", new { Raw = val, Transformed = (val == 127) ? 1 : -1 });

            eng.Execute(Text);
        }
    }
}
