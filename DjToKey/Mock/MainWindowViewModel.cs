using Ktos.DjToKey.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Mock
{
    class MainWindowViewModel
    {
        /// <summary>
        /// List of all devices supported by the application
        /// </summary>
        public ObservableCollection<Device> Devices { get; set; }

        /// <summary>
        /// Current device
        /// </summary>
        public Device CurrentDevice { get; set; }
    }
}
