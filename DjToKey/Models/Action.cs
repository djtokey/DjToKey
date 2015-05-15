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
            string command = Command.Trim();
            command = command.Replace("{VAL}", val.ToString());
            command = command.Replace("{VALD}", (val == 127)? "1" : "-1");

            var act = command.Split(' ');
            string par0 = (act.Length > 1) ? act[1] : null;
            string par1 = (act.Length > 2) ? act[2] : null;
            var cmd = act[0].ToLower();

            /*
            if (par0 != null && par0.StartsWith("#") && par0.EndsWith("#"))
            {
                par0 = par0.Replace("#", "");
                Expression e = new Expression(par0);
                par0 = e.Evaluate().ToString();
            }

            if (par1 != null && par1.StartsWith("#") && par1.EndsWith("#"))
            {
                par1 = par1.Replace("#", "");
                Expression e = new Expression(par1);
                par1 = e.Evaluate().ToString();
            }*/

            switch (cmd)
            {
                case "verticalwheel":
                    if (par0 != null)
                        Simulator.Input.Mouse.VerticalScroll(int.Parse(par0));
                    else
                        throw new ArgumentException("VerticalWheel wymaga parametru!");
                    break;

                case "horizontalwheel":
                    if (par0 != null)
                        Simulator.Input.Mouse.HorizontalScroll(int.Parse(par0));
                    else
                        throw new ArgumentException("HorizontalWheel wymaga parametru!");
                    break;

                case "mousemoveby":
                    if (par0 != null && par1 != null)
                        Simulator.Input.Mouse.MoveMouseBy(int.Parse(par0), int.Parse(par1));
                    else
                        throw new ArgumentException("MouseMoveBy wymaga parametrów!");
                    break;

                case "messagebox":
                    MessageBox.Show(String.Format("{0} {1}", par0, par1));
                    break;

                case "<brak>":
                    return;

                default:
                    throw new ArgumentException("Nierozpoznane polecenie!");
            }
        }
    }
}
