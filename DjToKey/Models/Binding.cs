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
        public string KeyId { get; set; }
        public string KeyName { get; set; }
        public Action Action { get; set; }
        public ControlType Type { get; set; }
    }
}
