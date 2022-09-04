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

using GregsStack.InputSimulatorStandard;
using Ktos.DjToKey.Plugins.Scripts;
using System;
using System.ComponentModel.Composition;

namespace Ktos.DjToKey.Scripts
{
    /// <summary>
    /// Class representing Keyboard object available for scripts
    /// </summary>
    [Export(typeof(IScriptObject))]
    public class Keyboard : IScriptObject
    {
        private const string NAME = "Keyboard";

        /// <summary>
        /// Name of the object in a script engine
        /// </summary>
        public string Name
        {
            get
            {
                return NAME;
            }
        }

        /// <summary>
        /// Object available for a script engine
        /// </summary>
        public object Object
        {
            get
            {
                return simulator.Keyboard;
            }
        }

        private InputSimulator simulator;

        /// <summary>
        /// Initializes new object for a script engine
        /// </summary>
        public Keyboard()
        {
            simulator = new InputSimulator();
        }
    }

    /// <summary>
    /// Class representing Mouse object available for scripts
    /// </summary>
    [Export(typeof(IScriptObject))]
    public class Mouse : IScriptObject
    {
        private const string NAME = "Mouse";

        /// <summary>
        /// Name of the object in a script engine
        /// </summary>
        public string Name
        {
            get
            {
                return NAME;
            }
        }

        /// <summary>
        /// Object available for a script engine
        /// </summary>
        public object Object
        {
            get
            {
                return simulator.Mouse;
            }
        }

        private InputSimulator simulator;

        /// <summary>
        /// Initializes new object for a script engine
        /// </summary>
        public Mouse()
        {
            simulator = new InputSimulator();
        }
    }

    /// <summary>
    /// Class representing KeyCode type available for scripts
    /// </summary>
    [Export(typeof(IScriptType))]
    public class KeyboardCodes : IScriptType
    {
        private const string NAME = "KeyCode";

        /// <summary>
        /// Name of the object in a script engine
        /// </summary>
        public string Name
        {
            get
            {
                return NAME;
            }
        }

        /// <summary>
        /// A Type available in a script engine
        /// </summary>
        public Type Type
        {
            get
            {
                return typeof(GregsStack.InputSimulatorStandard.Native.VirtualKeyCode);
            }
        }
    }
}