﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Midi;
using Newtonsoft.Json;
using Ktos.DjToKey.Models;
using System.IO;
using Microsoft.ClearScript.V8;

//TODO: https://icons8.com/

namespace Ktos.DjToKey
{
    public partial class MainForm : Form
    {
        private List<MidiControl> controls;
        private Dictionary<string, Script> bindings;

        private InputDevice dev;
        private V8ScriptEngine eng;

        public MainForm()
        {
            InitializeComponent();
            eng = new V8ScriptEngine();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var item in InputDevice.InstalledDevices)
            {
                cbMidiDevices.Items.Add(item.Name);                
            }

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
            dev = InputDevice.InstalledDevices[cbMidiDevices.SelectedIndex];

            try
            {
                loadControls();
                loadBindings();
                createEditor();

                dev.ControlChange += dev_ControlChange;
                if (!dev.IsOpen) dev.Open();
                dev.StartReceiving(null);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Błąd podczas odczytu pliku definiującego kontrolki tego urządzenia MIDI!", "DjToKey", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nie znaleziono pliku definiującego kontrolki tego urządzenia MIDI!", "DjToKey", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DeviceException)
            {
                MessageBox.Show("Błąd urządzenia MIDI!", "DjToKey", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                    Multiline = true,
                    Height = 60,
                    ScrollBars = ScrollBars.Vertical
                }, 1, tlpBindings.RowCount - 1);

                tlpBindings.RowCount++;
            }
        }

        private void loadControls()
        {
            trayIcon.Text = "DJToKey " + dev.Name;
            this.Text = trayIcon.Text;

            string f = ValidFileName.MakeValidFileName(dev.Name) + ".json";
            controls = JsonConvert.DeserializeObject<List<MidiControl>>(File.ReadAllText(f));            
        }

        private void loadBindings()
        {
            string f = "bindings-" + ValidFileName.MakeValidFileName(dev.Name) + ".json";

            try
            {
                bindings = JsonConvert.DeserializeObject<Dictionary<string, Script>>(File.ReadAllText("bindings-" + f));
            }
            catch (FileNotFoundException)
            {
                bindings = new Dictionary<string, Script>();
            }
        }

        void dev_ControlChange(ControlChangeMessage msg)
        {
            handleControl(msg);
        }

        private void handleControl(ControlChangeMessage msg)
        {
            Script s;
            if (bindings.TryGetValue(msg.Control.ToString(), out s))
            {
                try
                {
                    s.Execute(msg.Value, controls.Find(x => x.ControlId == msg.Control.ToString()), eng);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wystąpił błąd w obsłudze zdarzenia: " + e.Message);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dev.StopReceiving();
            if (dev.IsOpen) dev.Close();          
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveBindings();
        }

        private void saveBindings()
        {
            foreach (var c in tlpBindings.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    var cc = (c as TextBox);
                    bindings[cc.Tag.ToString()] = new Script() { Text = cc.Text };
                }
            }

            string f = "bindings-" + ValidFileName.MakeValidFileName(dev.Name) + ".json";
            File.WriteAllText(f, (JsonConvert.SerializeObject(bindings)));
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
    }
}
