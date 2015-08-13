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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace Ktos.DjToKey.Models
{
    public class MockMidiDevice : IDevice
    {
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
        public IEnumerable<Control> Controls { get; private set; }

        /// <summary>
        /// A script engine which will be used when executing scripts
        /// </summary>
        public IScriptEngine ScriptEngine { get; set; }

        /// <summary>
        /// An event invoked when script error occured when handling
        /// control
        /// </summary>
        public EventHandler<ScriptErrorEventArgs> ScriptErrorOccured { get; set; }

        private Timer tim;

        /// <summary>
        /// A constructor, intializing script engine and setting list of
        /// available input devices
        /// </summary>
        public MockMidiDevice()
        {
            var l = new List<string>();
            l.Add("TEST");

            AvailableDevices = l;

            tim = new Timer();
            tim.Interval = 3000;
            tim.Elapsed += (s, e) => { handleControl("1", 2); };
            tim.Start();
        }

        /// <summary>
        /// Loads information and bindings of a device, sets up handling device events
        /// </summary>
        /// <param name="deviceName">Name of a device to be loaded</param>
        public void Load(string deviceName)
        {
            ActiveDevice = deviceName;

            loadControls();
            loadBindings();

            //dev.ControlChange += dev_ControlChange;
            //dev.NoteOn += dev_NoteOn;
            //if (!dev.IsOpen) dev.Open();
            //dev.StartReceiving(null);
        }

        private void loadBindings()
        {
            string f = "bindings-" + ValidFileName.MakeValidFileName(ActiveDevice) + ".json";

            try
            {
                Bindings = JsonConvert.DeserializeObject<IList<ControlBinding>>(File.ReadAllText(f));
                //var aa = JsonConvert.DeserializeObject<IList<ControlBinding>>(File.ReadAllText(f)); ;
                //Bindings
                //(IList<IControlBinding>)
            }
            catch (FileNotFoundException)
            {
                Bindings = new List<ControlBinding>();
            }
        }

        /// <summary>
        /// Loads control definitions from file
        /// </summary>
        private void loadControls()
        {
            string f = @"devices\" + ValidFileName.MakeValidFileName(ActiveDevice) + ".json";
            Controls = JsonConvert.DeserializeObject<IEnumerable<Control>>(File.ReadAllText(f));
        }

        /// <summary>
        /// Handles MIDI message for buttons or controls
        /// </summary>
        /// <param name="control">Control ID for searching a script bound to it</param>
        /// <param name="value">Value sent from MIDI device</param>
        private void handleControl(string control, int value)
        {
            tim.Stop();
            var binding = Bindings.Where(x => x.Control.ControlId == control).FirstOrDefault();

            if (binding != null)
            {
                try
                {
                    ScriptEngine.Execute(binding.Script, new { Raw = value, Transformed = (value == 127) ? 1 : -1 }, Controls.Where(x => x.ControlId == control.ToString()).First());
                }
                catch (Microsoft.ClearScript.ScriptEngineException e)
                {
                    if (ScriptErrorOccured != null) ScriptErrorOccured.Invoke(this, new ScriptErrorEventArgs() { Control = control, Message = e.Message });
                }
            }
        }

        /// <summary>
        /// Saves bindings to file
        /// </summary>
        public void SaveBindings()
        {
            string f = "bindings-" + ValidFileName.MakeValidFileName(ActiveDevice) + ".json";
            File.WriteAllText(f, JsonConvert.SerializeObject(Bindings));
        }

        /// <summary>
        /// Unloads the device
        /// </summary>
        public void Unload()
        {
        }
    }
}