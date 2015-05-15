namespace Ktos.DjToKey.Models
{
    public enum ControlType
    {
        Analog, Digital
    }

    class DjControl
    {
        public string ControlId { get; set; }
        public string ControlName { get; set; }
        public ControlType Type { get; set; }
    }
}
