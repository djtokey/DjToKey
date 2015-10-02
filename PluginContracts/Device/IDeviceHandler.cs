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
using System;
using System.Collections.Generic;

namespace Ktos.DjToKey.Plugins.Device
{
    public interface IDeviceHandler
    {
        /// <summary>
        /// A list of names of available input devices
        /// </summary>
        IEnumerable<string> AvailableDevices { get; }

        /// <summary>
        /// The name of a selected device
        /// </summary>
        string ActiveDevice { get; }

        /// <summary>
        /// List of scripts bound to controls
        /// </summary>
        IList<ControlBinding> Bindings { get; set; }

        /// <summary>
        /// List of possible controls in connected device
        /// </summary>
        IEnumerable<Control> Controls { get; }

        /// <summary>
        /// A script engine which will be used when executing scripts
        /// </summary>
        IScriptEngine ScriptEngine { get; set; }

        /// <summary>
        /// An event invoked when script error occured when handling
        /// control
        /// </summary>
        EventHandler<ScriptErrorEventArgs> ScriptErrorOccured { get; set; }

        /// <summary>
        /// Loads device based on a specified name and starts listening for its events
        /// </summary>
        /// <param name="deviceName"></param>
        void Load(string deviceName);

        /// <summary>
        /// Unloads currently active device and stops listening for its events
        /// </summary>
        void Unload();        

        /// <summary>
        /// Performs saving bindings to a device file
        ///
        /// This is deprecated method available only in Plugin API 0.8 for compatibility before 0.3 DjToKey release. It will be removed after "look & feel" improvements
        /// </summary>
        [Obsolete]
        void SaveBindings();
    }
}