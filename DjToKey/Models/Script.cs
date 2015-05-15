using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DjToKey.Models
{
    class Script
    {
        public string Text { get; set; }

        public void Execute(int val, DjControl ctrl, V8ScriptEngine eng)
        {
            eng.AddHostObject("Keyboard", Simulator.Input.Keyboard);
            eng.AddHostObject("Mouse", Simulator.Input.Mouse);
            eng.AddHostObject("Control", ctrl);
            eng.AddHostObject("val", val);
            eng.AddHostObject("Value", new { Raw = val, Transformed = (val == 127)? 1 : -1 });

            eng.Execute(this.Text);            
        }
    }
}
