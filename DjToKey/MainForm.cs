using System;
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

namespace DjToKey
{
    public partial class MainForm : Form
    {
        private Dictionary<DjControl, Script> bindings;
        private InputDevice dev;
        private V8ScriptEngine eng;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                bindings = JsonConvert.DeserializeObject<Dictionary<DjControl, Script>>(File.ReadAllText("bindings.json"));
                eng = new V8ScriptEngine();
            }
            catch (FileNotFoundException)
            {
                bindings = new Dictionary<DjControl, Script>();

                bindings.Add(new DjControl() { ControlId = "48", ControlName = "Deck B", Type = ControlType.Digital }, new Script() { Text = "alert('Here');" });

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

            TrayIcon.Visible = true;

        }

        private void cbMidiDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            dev = InputDevice.InstalledDevices[cbMidiDevices.SelectedIndex];

            if (!dev.Name.Contains("MP3 LE MIDI"))
                MessageBox.Show("Ta aplikacja obsługuje tylko Hercules DJControl MP3 LE MIDI!");
            else
            {
                foreach (var c in bindings)
                {
                    tlpBindings.Controls.Add(new Label()
                    {
                        Text = c.Key.ControlName
                    }, 0, tlpBindings.RowCount - 1);

                    tlpBindings.Controls.Add(new TextBox()
                    {
                        Text = c.Value.Text,
                        Tag = c.Key.ControlId
                    }, 1, tlpBindings.RowCount - 1);

                    tlpBindings.RowCount++;
                }

                dev.ControlChange += dev_ControlChange;
                if (!dev.IsOpen) dev.Open();
                dev.StartReceiving(null);
            }

        }

        void dev_ControlChange(ControlChangeMessage msg)
        {
            var b = bindings.Keys.First(x => x.ControlId == msg.Control.ToString());
            if (b != null)
            {
                try
                {
                    bindings[b].Execute(msg.Value, b, eng);                   
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wystąpił błąd w obsłudze " + b.ControlName + ": " + e.Message);
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
                    var b = bindings.Keys.First(x => x.ControlId == cc.Tag.ToString());
                    bindings[b].Text = cc.Text;                    
                }
            }
            
            File.WriteAllText("bindings.json", JsonConvert.SerializeObject(bindings));
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                TrayIcon.Visible = true;
                this.ShowInTaskbar = false;                
            }
            else
            {
                TrayIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}
