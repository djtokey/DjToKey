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

    class Binding
    {
        public int KeyId { get; set; }
        public string KeyName { get; set; }
        public string Action { get; set; }
        public ControlType Type { get; set; }
    }
}
