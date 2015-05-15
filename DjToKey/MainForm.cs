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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Midi;
using Newtonsoft.Json;
using Ktos.DjToKey.Models;
using System.IO;
using System.Reflection;
using Microsoft.ClearScript.V8;

namespace Ktos.DjToKey
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Application name
        /// </summary>
        private const string APPNAME = "DjToKey";

        /// <summary>
        /// List of possible controls in connected MIDI device
        /// </summary>
        private List<MidiControl> controls;

        /// <summary>
        /// List of scripts bound to controls
        /// </summary>
        private Dictionary<string, Script> bindings;

        /// <summary>
        /// Instance of MIDI input device
        /// </summary>
        private InputDevice dev;

        /// <summary>
        /// Instance of script engine
        /// </summary>
        private V8ScriptEngine eng;

        /// <summary>
        /// Form constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            configureEngine();
        }

        /// <summary>
        /// Configures script engine
        /// </summary>
        private void configureEngine()
        {
            // adding set of static object which will be available for scripts
            eng = new V8ScriptEngine();
            eng.AddHostObject("Keyboard", ScriptsHelper.Simulator.Keyboard);
            eng.AddHostObject("Mouse", ScriptsHelper.Simulator.Mouse);
            eng.AddHostObject("Document", ScriptsHelper.Document);
            eng.AddHostObject("Console", ScriptsHelper.Console);

            // addind useful types
            eng.AddHostType("KeyCode", typeof (WindowsInput.Native.VirtualKeyCode));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // load list of input devices into ComboBox
            foreach (var item in InputDevice.InstalledDevices)
            {
                cbMidiDevices.Items.Add(item.Name);                
            }

            // if there is none, show message
            // if there are some - set first of them as active
            if (InputDevice.InstalledDevices.Count == 0)
            {
                MessageBox.Show("Nie znaleziono urządzeń MIDI!");
                btnSave.Enabled = false;
            }
            else
                cbMidiDevices.SelectedIndex = cbMidiDevices.Items.Count - 1;

        }

        private void cbMidiDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dev != null && InputDevice.InstalledDevices[cbMidiDevices.SelectedIndex].Name == dev.Name)
                return;

            dev = InputDevice.InstalledDevices[cbMidiDevices.SelectedIndex];

            try
            {
                loadControls();
                loadBindings();
                createEditor();

                dev.ControlChange += dev_ControlChange;
                dev.NoteOn += dev_NoteOn;
                if (!dev.IsOpen) dev.Open();
                dev.StartReceiving(null);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Błąd podczas odczytu pliku definiującego kontrolki tego urządzenia MIDI!", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nie znaleziono pliku definiującego kontrolki tego urządzenia MIDI!", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DeviceException)
            {
                MessageBox.Show("Błąd urządzenia MIDI!", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Creates a set of TextBox based on all possible controls for device
        /// </summary>
        private void createEditor()
        {
            foreach (var c in controls)
            {
                tlpBindings.Controls.Add(new Label()
                {
                    Text = c.ControlName
                }, 0, tlpBindings.RowCount - 1);

                Script s;
                string v = "";
                if (bindings.TryGetValue(c.ControlId, out s))
                    v = s.Text;

                tlpBindings.Controls.Add(new TextBox()
                {
                    Tag = c.ControlId,
                    Text = v,
                    Height = 50,
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical
                }, 1, tlpBindings.RowCount - 1);

                tlpBindings.RowCount++;
            }

            tlpBindings.PerformLayout();
        }

        /// <summary>
        /// Loads control definitions from file
        /// </summary>
        private void loadControls()
        {
            trayIcon.Text = APPNAME + " - " + dev.Name;
            this.Text = trayIcon.Text;

            string f = ValidFileName.MakeValidFileName(dev.Name) + ".json";
            controls = JsonConvert.DeserializeObject<List<MidiControl>>(File.ReadAllText(f));            
        }

        /// <summary>
        /// Loads bindings for device from file
        /// </summary>
        private void loadBindings()
        {
            string f = "bindings-" + ValidFileName.MakeValidFileName(dev.Name) + ".json";

            try
            {
                bindings = JsonConvert.DeserializeObject<Dictionary<string, Script>>(File.ReadAllText(f));
            }
            catch (FileNotFoundException)
            {
                bindings = new Dictionary<string, Script>();
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
            if (bindings.TryGetValue(control, out s))
            {
                try
                {
                    s.Execute(value, controls.Find(x => x.ControlId == control.ToString()), eng);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wystąpił błąd w obsłudze zdarzenia: " + e.Message);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dev != null)
            {
                dev.StopReceiving();
                if (dev.IsOpen) dev.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveBindings();
        }

        /// <summary>
        /// Saves bindings to file
        /// </summary>
        private void saveBindings()
        {
            foreach (var c in tlpBindings.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    var cc = (c as TextBox);
                    if (bindings.ContainsKey(cc.Tag.ToString()))
                        bindings[cc.Tag.ToString()].Text = cc.Text;
                    else
                        bindings.Add(cc.Tag.ToString(), new Script() { Text = cc.Text });
                }
            }

            string f = "bindings-" + ValidFileName.MakeValidFileName(dev.Name) + ".json";
            File.WriteAllText(f, JsonConvert.SerializeObject(bindings));
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                trayIcon.Visible = true;
                this.ShowInTaskbar = false;                
            }
            else
            {
                trayIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                saveBindings();
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof (AssemblyInformationalVersionAttribute), false)[0] as
                AssemblyInformationalVersionAttribute;

            var mess = String.Format("{0} {1}\n\nThis is a very basic MIDI-controller to script mapper. It allows you to prepare custom scripts for moving mouse, pressing keys and similar things, fired every time some action on your MIDI device occurs. For example, you can bind your Deck from DJ console to a mouse wheel.\n\nCopyright (C) Marcin Badurowicz 2015\nIcon used from: https://icons8.com/", APPNAME, version.InformationalVersion);

            MessageBox.Show(mess, APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
