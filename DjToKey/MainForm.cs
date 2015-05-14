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
using Binding = DjToKey.Models.Binding;
using Action = DjToKey.Models.Action;

namespace DjToKey
{


    public partial class MainForm : Form
    {
        private List<Binding> bindings;
        private InputDevice dev;

        public MainForm()
        {
            InitializeComponent();
            bindings = new List<Binding>();

            bindings.Add(new Binding()
            {
                KeyId = "49",
                KeyName = "Deck B",
                Action = new Action() { Command = "MessageBox Hello{VAL}" },
                Type = ControlType.Digital
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var item in InputDevice.InstalledDevices)
            {
                cbMidiDevices.Items.Add(item.Name);
                if (item.Name.Contains("MP3 LE MIDI"))
                    cbMidiDevices.SelectedIndex = cbMidiDevices.Items.Count - 1;
            }

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
                        Text = c.KeyName
                    }, 0, tlpBindings.RowCount -1);

                    tlpBindings.Controls.Add(new TextBox()
                    {
                        Text = c.Action.Command,
                        Tag = c.KeyId
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
            var b = bindings.Find(x => x.KeyId == msg.Control.ToString());
            if (b != null)
                b.Action.Execute(msg.Value);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dev.StopReceiving();
            if (dev.IsOpen) dev.Close();
        }
    }
}
