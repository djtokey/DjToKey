using Microsoft.ClearScript.V8;
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

        public void Execute(int val, V8ScriptEngine eng)
        {
            string command = Command.Trim();
            command = command.Replace("{VAL}", val.ToString());
            command = command.Replace("{VALD}", (val == 127)? "1" : "-1");

            eng.AddHostObject("Keyboard", Simulator.Input.Keyboard);
            eng.AddHostObject("Mouse", Simulator.Input.Mouse);
            eng.AddHostObject("value", val);

            eng.Execute(command);            
        }
    }
}
