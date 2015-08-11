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
using Midi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ktos.DjToKey.Models
{
    public class MidiDevice
    {
        /// <summary>
        /// A list of names of available input devices
        /// </summary>
        public List<string> AvailableDevices { get; private set; }

        /// <summary>
        /// The name of a selected device
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Instance of MIDI input device
        /// </summary>
        private InputDevice dev;

        /// <summary>
        /// List of scripts bound to controls
        /// </summary>
        public Dictionary<string, Script> Bindings { get; private set; }

        /// <summary>
        /// List of possible controls in connected MIDI device
        /// </summary>
        public List<MidiControl> Controls { get; private set; }

        /// <summary>
        /// A script engine which will be used when executing scripts
        /// </summary>
        private ScriptEngine eng;

        /// <summary>
        /// An event invoked when script error occured when handling
        /// control
        /// </summary>
        public EventHandler<ScriptErrorEventArgs> ScriptErrorOccured;

        /// <summary>
        /// A constructor, intializing script engine and setting list of
        /// available input devices
        /// </summary>
        public MidiDevice()
        {
            eng = new ScriptEngine();
            eng.Configure();

            AvailableDevices = new List<string>();
            foreach (var d in InputDevice.InstalledDevices)
            {
                AvailableDevices.Add(d.Name);
            }
        }

        /// <summary>
        /// Loads information and bindings of a device, sets up handling device events
        /// </summary>
        /// <param name="deviceName">Name of a device to be loaded</param>
        public void Load(string deviceName)
        {
            var newDevice = InputDevice.InstalledDevices.First(x => x.Name == deviceName);
            if (newDevice != dev)
                dev = newDevice;

            loadControls();
            loadBindings();

            dev.ControlChange += dev_ControlChange;
            dev.NoteOn += dev_NoteOn;
            if (!dev.IsOpen) dev.Open();
            dev.StartReceiving(null);

        }

        /// <summary>
        /// Loads control definitions from file
        /// </summary>
        private void loadControls()
        {
            string f = @"devices\" + ValidFileName.MakeValidFileName(dev.Name) + ".json";
            Controls = JsonConvert.DeserializeObject<List<MidiControl>>(File.ReadAllText(f));
        }

        /// <summary>
        /// Loads bindings for device from file
        /// </summary>
        private void loadBindings()
        {
            string f = "bindings-" + ValidFileName.MakeValidFileName(dev.Name) + ".json";

            try
            {
                Bindings = JsonConvert.DeserializeObject<Dictionary<string, Script>>(File.ReadAllText(f));
            }
            catch (FileNotFoundException)
            {
                Bindings = new Dictionary<string, Script>();
            }
        }

        /// <summary>
        /// Handles ControlChange messages
        /// </summary>
        /// <param name="msg"></param>
        void dev_ControlChange(ControlChangeMessage msg)
        {
            handleControl(msg.Control.ToString(), msg.Value);
        }

        /// <summary>
        /// Handles MIDI message for buttons or controls
        /// </summary>
        /// <param name="control">Control ID for searching a script bound to it</param>
        /// <param name="value">Value sent from MIDI device</param>
        private void handleControl(string control, int value)
        {
            Script s;
            if (Bindings.TryGetValue(control, out s))
            {
                try
                {
                    eng.Execute(s, value, Controls.Find(x => x.ControlId == control.ToString()));
                }
                catch (Microsoft.ClearScript.ScriptEngineException e)
                {
                    if (ScriptErrorOccured != null) ScriptErrorOccured.Invoke(this, new ScriptErrorEventArgs() { Control = control, Message = e.Message });
                }
            }
        }

        /// <summary>
        /// Handles pressing button on a device
        /// </summary>
        /// <param name="msg"></param>
        private void dev_NoteOn(NoteOnMessage msg)
        {
            handleControl(((int)msg.Pitch).ToString(), msg.Velocity);
        }

        /// <summary>
        /// Saves bindings to file
        /// </summary>
        public void SaveBindings()
        {            
            string f = "bindings-" + ValidFileName.MakeValidFileName(dev.Name) + ".json";
            File.WriteAllText(f, JsonConvert.SerializeObject(Bindings));
        }

        /// <summary>
        /// Unloads the device
        /// </summary>
        public void Unload()
        {
            if (dev != null)
            {
                try
                {
                    dev.StopReceiving();
                    if (dev.IsOpen) dev.Close();
                }
                catch (Midi.DeviceException)
                {
                    // device was removed while application was running or cannot be closed
                    // we cannot do much about it, so we are just closing
                }
            }
        }

    }

    /// <summary>
    /// Carries messages about script errors
    /// </summary>
    public class ScriptErrorEventArgs : EventArgs
    {
        public string Control { get; set; }
        public string Message { get; set; }
    }

}
