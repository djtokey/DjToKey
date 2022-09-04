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

using Ktos.DjToKey.Plugins.Device;
using Ktos.DjToKey.Plugins.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Ktos.DjToKey
{
#if DEBUG

    [Export(typeof(IDeviceHandler))]
    public class MockDevice : IDeviceHandler
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// A list of names of available input devices
        /// </summary>
        public IEnumerable<string> AvailableDevices { get; private set; }

        /// <summary>
        /// The name of a selected device
        /// </summary>
        public string ActiveDevice { get; private set; }

        /// <summary>
        /// List of scripts bound to controls
        /// </summary>
        public IList<ControlBinding> Bindings { get; set; }

        /// <summary>
        /// List of possible controls in connected MIDI device
        /// </summary>
        public IEnumerable<Control> Controls { get; set; }

        /// <summary>
        /// A script engine which will be used when executing scripts
        /// </summary>
        public IScriptEngine ScriptEngine { get; set; }

        /// <summary>
        /// An event invoked when script error occured when handling control
        /// </summary>
        public EventHandler<ScriptErrorEventArgs> ScriptErrorOccured { get; set; }

        private HwndSource source;
        private const int HOTKEY_ID = 9000;
        private const int FsModifiers = 0x0002;

        /// <summary>
        /// A constructor, intializing script engine and setting list
        /// of available input devices
        /// </summary>
        public MockDevice()
        {
            var l = new List<string>();
            l.Add("TEST");

            AvailableDevices = l;
        }

        /// <summary>
        /// Loads information and bindings of a device, sets up
        /// handling device events
        /// </summary>
        /// <param name="deviceName">Name of a device to be loaded</param>
        public void Load(string deviceName)
        {
            ActiveDevice = deviceName;

            IntPtr handle = new WindowInteropHelper(App.Current.MainWindow).Handle;
            source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);

            RegisterHotKey(
                handle,
                HOTKEY_ID,
                FsModifiers,
                (uint)GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.F11
            );
        }

        private IntPtr HwndHook(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled
        )
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            HandleControl("1", 2);
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// Handles MIDI message for buttons or controls
        /// </summary>
        /// <param name="control">
        /// Control ID for searching a script bound to it
        /// </param>
        /// <param name="value">Value sent from MIDI device</param>
        private void HandleControl(string control, int value)
        {
            var binding = Bindings.Where(x => x.Control.ControlId == control).FirstOrDefault();

            if (binding != null)
            {
                try
                {
                    var ctrl = Controls.ToList().Where(x => x.ControlId == control).First();
                    ScriptEngine.Execute(
                        binding.Script,
                        new { Raw = value, Transformed = (value == 127) ? 1 : -1 },
                        ctrl
                    );
                }
                catch (Microsoft.ClearScript.ScriptEngineException e)
                {
                    if (ScriptErrorOccured != null)
                        ScriptErrorOccured.Invoke(
                            this,
                            new ScriptErrorEventArgs() { Control = control, Message = e.Message }
                        );
                }
            }
        }

        /// <summary>
        /// Unloads the device
        /// </summary>
        public void Unload()
        {
            IntPtr handle = new WindowInteropHelper(App.Current.MainWindow).Handle;
            UnregisterHotKey(handle, HOTKEY_ID);
        }
    }
#endif
}
