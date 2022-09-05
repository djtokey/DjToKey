using Ktos.DjToKey.Models;
using Ktos.DjToKey.Packaging;
using Ktos.DjToKey.Plugins.Device;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ktos.DjToKey.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly string path = "settings.json";

        public void Load()
        {
            if (File.Exists(path))
            {
                var obj = JsonConvert.DeserializeObject<SettingsViewModel>(
                    File.ReadAllText("settings.json")
                );
                ShowMessageWhenMinimized = obj.ShowMessageWhenMinimized;
            }
        }

        public void Save()
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this));
        }

        private bool showMessageWhenMinized;
        public bool ShowMessageWhenMinimized
        {
            get { return showMessageWhenMinized; }
            set
            {
                if (this.showMessageWhenMinized != value)
                {
                    showMessageWhenMinized = value;
                    OnPropertyChanged(nameof(ShowMessageWhenMinimized));
                }
            }
        }

        /// <summary>
        /// Even run when databound property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles when property is changed raising <see
        /// cref="PropertyChanged"/> event.
        ///
        /// Part of <see cref="INotifyPropertyChanged"/> implementation
        /// </summary>
        /// <param name="name">Name of a changed property</param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
