﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DjToKey.Models;
using Midi;
using Newtonsoft.Json;
using DjControl = DjToKey.Models.DjControl;
using Script = DjToKey.Models.Script;
using System.IO;
using Microsoft.ClearScript.V8;

//TODO: https://icons8.com/

namespace DjToKey
{
    public partial class MainForm : Form
    {
        private List<DjControl> controls;
        private Dictionary<string, Script> bindings;

        private InputDevice dev;
        private V8ScriptEngine eng;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                
                eng = new V8ScriptEngine();
            }
            catch (FileNotFoundException)
            {
                bindings = new Dictionary<string, Script>();

                bindings.Add("48", new Script() { Text = "alert('Here');" });

                MessageBox.Show("Błąd wczytywania pliku z przypisaniami.");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bool select = false;
            foreach (var item in InputDevice.InstalledDevices)
            {
                cbMidiDevices.Items.Add(item.Name);
                if (item.Name.Contains("MP3 LE MIDI"))
                {
                    cbMidiDevices.SelectedIndex = cbMidiDevices.Items.Count - 1;
                    select = true;
                }
            }

            if (!select)
                MessageBox.Show("Nie znaleziono odpowiedniego urządzenia MIDI!");

            trayIcon.Visible = true;

        }

        private void cbMidiDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            dev = InputDevice.InstalledDevices[cbMidiDevices.SelectedIndex];

            try
            {
                trayIcon.Text = "DJToKey " + dev.Name;
                this.Text = trayIcon.Text;

                controls = JsonConvert.DeserializeObject<List<DjControl>>(ValidFileName.MakeValidFileName(dev.Name + ".json"));

                try
                {
                    bindings = JsonConvert.DeserializeObject<Dictionary<string, Script>>(File.ReadAllText("bindings-" + dev.Name + ".json"));
                }
                catch (FileNotFoundException)
                {
                    bindings = new Dictionary<string, Script>();
                }

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
                        Text = v
                    }, 1, tlpBindings.RowCount - 1);

                    tlpBindings.RowCount++;

                    dev.ControlChange += dev_ControlChange;
                    if (!dev.IsOpen) dev.Open();
                    dev.StartReceiving(null);
                }                
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nie znaleziono pliku definiującego kontrolki tego urządzenia MIDI.");
            }
        }

        void dev_ControlChange(ControlChangeMessage msg)
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
            foreach (var c in tlpBindings.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    var cc = (c as TextBox);                    
                    bindings[cc.Tag.ToString()] = new Script() { Text = cc.Text };
                }
            }
            
            File.WriteAllText("bindings - " + dev.Name + ".json", (JsonConvert.SerializeObject(bindings)));
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
