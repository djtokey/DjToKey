using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Midi;

namespace DjToKey
{
    public enum ControlType
    {
        Analog, Digital
    }

    public partial class MainForm : Form
    {
        private Dictionary<int, ControlType> controls;

        public MainForm()
        {
            InitializeComponent();
            controls = new Dictionary<int, ControlType>();

            controls.Add(50, ControlType.Digital);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var item in InputDevice.InstalledDevices)
            {
                cbMidiDevices.Items.Add(item.Name);
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
                        Text = c.Key.ToString()
                    }, 0, tlpBindings.RowCount -1);

                    tlpBindings.Controls.Add(new TextBox()
                    {
                        Text = "<brak>"
                    }, 1, tlpBindings.RowCount - 1);

                    tlpBindings.RowCount++;
                }
            }

        }
    }
}
