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

using Ktos.DjToKey.Model;
using Ktos.DjToKey.Plugins.Device;
using Ktos.DjToKey.Plugins.Scripts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ktos.DjToKey
{
    public partial class MainForm : Form
    {
        private IDeviceHandler dev;
        private AllDevices allDevices;

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

            lbAbout.Text = Resources.AppResources.lbAbout;
            lbDevice.Text = Resources.AppResources.lbDevice;
            lbPlugins.Text = Resources.AppResources.lbPlugins;
            gbBindings.Text = Resources.AppResources.gbBindings;
            btnSave.Text = Resources.AppResources.btnSave;
        }

        /// <summary>
        /// Handles errors in script execution and shows proper messages for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScriptError(object sender, ScriptErrorEventArgs e)
        {
            string err = string.Format(Resources.AppResources.ScriptErrorMessage, e.Control, e.Message);
            if ((lastControlError != e.Control) || (DateTime.Now - lastErrorTime > TimeSpan.FromSeconds(ERRORTIME)))
            {
                lastErrorTime = DateTime.Now;
                lastControlError = e.Control;
            }
            MessageBox.Show(err);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            allDevices = new AllDevices(Program.PluginImporter.DevicePlugins.DeviceHandlers);

            // load list of input devices into ComboBox
            foreach (var item in allDevices.AvailableDevices)
            {
                cbMidiDevices.Items.Add(item);
            }

            // if there is none, show message
            // if there are some - set first of them as active
            if (cbMidiDevices.Items.Count == 0)
            {
                MessageBox.Show(Resources.AppResources.NoMidiMessage);
                btnSave.Enabled = false;
            }
            else
                cbMidiDevices.SelectedIndex = cbMidiDevices.Items.Count - 1;
        }

        private void cbMidiDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dev = allDevices.FindHandler(cbMidiDevices.SelectedItem.ToString());
                dev.ScriptEngine = Program.ScriptEngine;
                dev.ScriptErrorOccured += OnScriptError;
                dev.Load(cbMidiDevices.SelectedItem.ToString());

                trayIcon.Text = Resources.AppResources.AppName + " - " + dev.ActiveDevice;
                this.Text = trayIcon.Text;
                createEditor();
            }
            catch (JsonReaderException)
            {
                MessageBox.Show(Resources.AppResources.ControlFileError, Resources.AppResources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(Resources.AppResources.ControlFileNotFound, Resources.AppResources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DeviceException)
            {
                MessageBox.Show(Resources.AppResources.MidiError, Resources.AppResources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                string v = "";

                var b = dev.Bindings.Where(x => x.Control.ControlId == c.ControlId).FirstOrDefault();

                if (b != null)
                {
                    if (b.Script.Text != null)
                        v = b.Script.Text;
                    else
                        v = @"file://" + b.Script.Path;
                }

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
            foreach (var c in tlpBindings.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    var cc = (c as TextBox);

                    var b = dev.Bindings.Where(x => x.Control.ControlId == cc.Tag.ToString()).FirstOrDefault();

                    if (b != null)
                    {
                        if (cc.Text.StartsWith("file://"))
                        {
                            b.Script.Path = cc.Text.Remove(0, "file://".Length);
                            b.Script.Text = null;
                        }
                        else
                        {
                            b.Script.Text = cc.Text;
                            b.Script.Path = null;
                        }
                    }
                    else
                    {
                        if (cc.Text.StartsWith("file://"))
                            dev.Bindings.Add(new ControlBinding() { Control = dev.Controls.Where(x => x.ControlId == cc.Tag.ToString()).First(), Script = new Script() { Text = null, Path = cc.Text.Remove(0, "file://".Length) } });
                        else
                            dev.Bindings.Add(new ControlBinding() { Control = dev.Controls.Where(x => x.ControlId == cc.Tag.ToString()).First(), Script = new Script() { Text = cc.Text, Path = null } });
                    }
                }
            }

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
#if DEBUG
            var version = Build.GitVersion.FullSemVer;
#else
            var version = Build.GitVersion.SemVer;
#endif

            var mess = String.Format(Resources.AppResources.About, Resources.AppResources.AppName, version);

            MessageBox.Show(mess, Resources.AppResources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Resources.AppResources.PluginsMessage);

            foreach (var p in Program.PluginImporter.Plugins)
            {
                sb.Append('\t');
                sb.AppendFormat("{0} {1} - {2}. {3}", p.Title, p.Version, p.Description, p.Copyright);
                sb.AppendLine();
            }

            MessageBox.Show(sb.ToString());
        }
    }
}