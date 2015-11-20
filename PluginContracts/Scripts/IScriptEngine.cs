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

namespace Ktos.DjToKey.Plugins.Scripts
{
    /// <summary>
    /// A generic interface for a script handler.
    /// 
    /// Script handler is configured with possible script plugins, adding new
    /// object, types or other functions.
    /// 
    /// It also provides <see cref="Execute(Script, object, object)"/> method,
    /// which runs script with a value and a control.
    /// </summary>
    public interface IScriptEngine
    {
        /// <summary>
        /// Configures script engine adding possible plugins for scripts
        /// </summary>
        /// <param name="plugins">A collection of additional objects and types for a ScriptEngine</param>
        void Configure(IScriptPlugins plugins);

        /// <summary>
        /// Executes a script within a script engine.
        /// </summary>
        /// <param name="s">A script to be executed as a string</param>
        /// <param name="value">Value from the input device</param>
        /// <param name="ctrl">Object defining control from the input device</param>
        void Execute(Script s, object value, object ctrl);
    }
}