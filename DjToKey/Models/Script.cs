using Microsoft.ClearScript.V8;

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
            eng.AddHostObject("Value", new { Raw = val, Transformed = (val == 127) ? 1 : -1 });

            eng.Execute(Text);
        }
    }
}
