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
using System.Windows.Forms;
using Ktos.DjToKey.Models;
using System.IO;
using Newtonsoft.Json;

namespace Ktos.DjToKey
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Application name
        /// </summary>
        private const string APPNAME = "DjToKey";

        private MidiDevice dev;

        /// <summary>
        /// Time to ignore errors for the same control
        /// </summary>
        private const int ERRORTIME = 10;

        /// <summary>
        /// When last control error was shown
        /// </summary>
        private DateTime lastErrorTime = DateTime.MinValue;

        /// <summary>
        /// Which control ID was last mentioned in error message
        /// </summary>
        private string lastControlError = "";

        /// <summary>
        /// Form constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            dev = new MidiDevice();
            dev.ScriptErrorOccured += OnScriptError;
        }

        /// <summary>
        /// Handles errors in script execution and shows proper messages for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScriptError(object sender, ScriptErrorEventArgs e)
        {
            string err = string.Format("Wystąpił błąd w obsłudze zdarzenia dla kontrolki {0}: {1}", e.Control, e.Message);
            if ((lastControlError != e.Control) || (DateTime.Now - lastErrorTime > TimeSpan.FromSeconds(ERRORTIME)))
            {
                lastErrorTime = DateTime.Now;
                lastControlError = e.Control;

            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // load list of input devices into ComboBox
            foreach (var item in dev.AvailableDevices)
            {
                cbMidiDevices.Items.Add(item);
            }

            // if there is none, show message
            // if there are some - set first of them as active
            if (dev.AvailableDevices.Count == 0)
            {
                MessageBox.Show("Nie znaleziono urządzeń MIDI!");
                btnSave.Enabled = false;
            }
            else
                cbMidiDevices.SelectedIndex = cbMidiDevices.Items.Count - 1;

        }

        private void cbMidiDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dev.Load(cbMidiDevices.SelectedText);
                trayIcon.Text = APPNAME + " - " + dev.Name;
                this.Text = trayIcon.Text;
                createEditor();
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Błąd podczas odczytu pliku definiującego kontrolki tego urządzenia MIDI!", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nie znaleziono pliku definiującego kontrolki tego urządzenia MIDI!", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Midi.DeviceException)
            {
                MessageBox.Show("Błąd urządzenia MIDI!", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Creates a set of TextBox based on all possible controls for device
        /// </summary>
        private void createEditor()
        {
            foreach (var c in dev.Controls)
            {
                tlpBindings.Controls.Add(new Label()
                {
                    Text = c.ControlName
                }, 0, tlpBindings.RowCount - 1);

                Script s;
                string v = "";
                if (dev.Bindings.TryGetValue(c.ControlId, out s))
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dev.Unload();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dev.SaveBindings();
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
                btnSave_Click(null, null);
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            var version = Build.GitVersion.FullSemVer;

            var mess = String.Format("{0} {1}\n\nThis is a very basic MIDI-controller to script mapper. It allows you to prepare custom scripts for moving mouse, pressing keys and similar things, fired every time some action on your MIDI device occurs. For example, you can bind your Deck from DJ console to a mouse wheel.\n\nCopyright (C) Marcin Badurowicz 2015\nIcon used from: https://icons8.com/", APPNAME, version);

            MessageBox.Show(mess, APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
