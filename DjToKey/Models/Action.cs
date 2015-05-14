using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DjToKey.Models
{
    class Action
    {
        public string Command { get; set; }

        public void Execute(int? val = null)
        {
            var act = Command.Split(' ');
            string par0 = (act.Length > 1) ? act[1] : null;
            string par1 = (act.Length > 2) ? act[2] : null;            

            switch (act[0])
            {
                case "VerticalWheel":
                    if (par0 != null)
                        Simulator.Input.Mouse.VerticalScroll(int.Parse(par0));
                    else
                        throw new ArgumentException("Wheel wymaga parametru!");
                    break;

                case "MessageBox":
                    MessageBox.Show(String.Format("{0} {1} {2}", par0, par1, val));
                    break;

                default:
                    return;
            }
        }
    }
}
