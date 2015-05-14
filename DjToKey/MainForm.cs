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

namespace DjToKey
{
    

    public partial class MainForm : Form
    {
        private List<Binding> controls;

        public MainForm()
        {
            InitializeComponent();
            controls = new List<Binding>();

            controls.Add(new Binding()
            {
                KeyId = 50,
                KeyName = "50",
                Action = "<brak>",
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
            if (!InputDevice.InstalledDevices[cbMidiDevices.SelectedIndex].Name.Contains("MP3 LE MIDI"))
                MessageBox.Show("Ta aplikacja obsługuje tylko Hercules DJControl MP3 LE MIDI!");
            else
            {
                foreach (var c in controls)
                {
                    tlpBindings.Controls.Add(new Label()
                    {
                        Text = c.KeyName
                    }, 0, tlpBindings.RowCount -1);

                    tlpBindings.Controls.Add(new TextBox()
                    {
                        Text = c.Action
                    }, 1, tlpBindings.RowCount - 1);

                    tlpBindings.RowCount++;
                }
            }

        }
    }
}
