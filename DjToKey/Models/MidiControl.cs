namespace Ktos.DjToKey.Models
{
    public enum ControlType
    {
        Analog, Digital
    }

    class MidiControl
    {
        public string ControlId { get; set; }
        public string ControlName { get; set; }
        public ControlType Type { get; set; }
    }
}
