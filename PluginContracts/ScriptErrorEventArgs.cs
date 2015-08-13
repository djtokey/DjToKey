using System;

namespace Ktos.DjToKey.Plugins
{

    /// <summary>
    /// Carries messages about script errors
    /// </summary>
    public class ScriptErrorEventArgs : EventArgs
    {
        public string Control { get; set; }
        public string Message { get; set; }
    }
}
