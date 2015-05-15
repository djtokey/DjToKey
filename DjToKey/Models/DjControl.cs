using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjToKey.Models
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
