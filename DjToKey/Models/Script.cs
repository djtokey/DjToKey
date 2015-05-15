using Microsoft.ClearScript.V8;

namespace Ktos.DjToKey.Models
{
    class Script
    {
        public string Text { get; set; }

        public void Execute(int val, MidiControl ctrl, V8ScriptEngine eng)
        {
            eng.AddHostObject("Keyboard", ScriptsHelper.Simulator.Keyboard);
            eng.AddHostObject("Mouse", ScriptsHelper.Simulator.Mouse);
            eng.AddHostObject("Document", ScriptsHelper.Document);
            eng.AddHostObject("Console", ScriptsHelper.Console);
            eng.AddHostObject("Control", ctrl);
            eng.AddHostObject("Value", new { Raw = val, Transformed = (val == 127) ? 1 : -1 });

            eng.Execute(Text);
        }
    }
}
