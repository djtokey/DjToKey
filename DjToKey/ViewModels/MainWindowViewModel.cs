#region License

/*
 * DjToKey
 *
 * Copyright (C) Marcin Badurowicz 2015
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion License

using Ktos.DjToKey.Models;
using Ktos.DjToKey.Packaging;
using Ktos.DjToKey.Plugins;
using Ktos.DjToKey.Plugins.Device;
using Ktos.DjToKey.Plugins.Scripts;
using Ktos.DjToKey.Scripts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Ktos.DjToKey.ViewModels
{
    /// <summary>
    /// A ViewModel for a MainWindow of an application
    /// </summary>
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private AllDevices allDevices;
        private IDeviceHandler deviceHandler;

        /// <summary>
        /// A script engine used in application
        /// </summary>
        private ScriptEngine ScriptEngine { get; set; }

        /// <summary>
        /// A class used for importing all possible plugins
        /// </summary>
        private PluginImporter PluginImporter { get; set; }

        /// <summary>
        /// Initializes a new MainWindowViewModel and loads
        /// all devices from all supported handlers
        /// </summary>
        public MainWindowViewModel()
        {
            PluginImporter = new PluginImporter();

            ScriptEngine = new ScriptEngine();
            ScriptEngine.Configure(PluginImporter.ScriptPlugins);

            Devices = new ObservableCollection<Device>();
            allDevices = new AllDevices(PluginImporter.DevicePlugins.DeviceHandlers);

            // lists all devices and loads their device packages automatically
            foreach (var d in allDevices.AvailableDevices)
                Devices.Add(findAndLoadDeviceFromPackage(d));
        }

        private Device findAndLoadDeviceFromPackage(string name)
        {
            var d = PackageHelper.LoadDevicePackage(name);
            return d.Device;
        }

        /// <summary>
        /// Shows About window
        /// </summary>
        public void About()
        {
#if DEBUG
            var version = Build.GitVersion.FullSemVer;
#else
            var version = Build.GitVersion.SemVer;
#endif

            var mess = string.Format(Resources.AppResources.About, Resources.AppResources.AppName, version).Replace("\\n", "\n");

            MessageBox.Show(mess, Resources.AppResources.AppName, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// List of all devices supported by the application
        /// </summary>
        public ObservableCollection<Device> Devices { get; set; }

        private Device currentDevice;

        /// <summary>
        /// Currently active device
        ///
        /// When currently active device is changed, previous one's handler is removed,
        /// current handler is found and loaded, and new bindings for controls are loaded
        /// </summary>
        public Device CurrentDevice
        {
            get
            {
                return currentDevice;
            }

            set
            {
                if (this.currentDevice != value)
                {

                    if (deviceHandler != null)
                    {
                        saveBindings();
                        deviceHandler.Unload();
                    }

                    currentDevice = value;
                    OnPropertyChanged(nameof(CurrentDevice));

                    deviceHandler = allDevices.FindHandler(currentDevice.Name);
                    if (deviceHandler != null) deviceHandler.Load(currentDevice.Name);
                    deviceHandler.Controls = currentDevice.Controls;
                    deviceHandler.ScriptEngine = this.ScriptEngine;

                    loadBindings();
                }
            }
        }

        private void loadBindings()
        {
            // TODO: ładowanie i zapisywanie bindingów z różnych ścieżek
            string f = "bindings-" + Helpers.ValidFileName.MakeValidFileName(currentDevice.Name) + ".json";

            try
            {
                deviceHandler.Bindings = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ControlBinding>>(File.ReadAllText(f));
            }
            catch (FileNotFoundException)
            {
                deviceHandler.Bindings = new List<ControlBinding>();
            }
        }

        /// <summary>
        /// Saves bindings to file
        /// </summary>
        private void saveBindings()
        {
            string f = "bindings-" + Helpers.ValidFileName.MakeValidFileName(currentDevice.Name) + ".json";
            File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(deviceHandler.Bindings));
        }


        public void SetCurrentScript(ViewControl ctrl)
        {
            currentlyEditing = ctrl;
            var binding = deviceHandler.Bindings.Where(x => x.Control.ControlId == ctrl.ControlId).FirstOrDefault();

            if (binding != null)
                CurrentScript = binding.Script.Text;
            else
            {
                CurrentScript = string.Empty;
                deviceHandler.Bindings.Add(new ControlBinding { Control = ctrl, Script = new Script() { Text = CurrentScript } });
            }
        }

        public void UpdateCurrentScript(string script)
        {
            if (currentlyEditing != null)
            {
                var binding = deviceHandler.Bindings.Where(x => x.Control.ControlId == currentlyEditing.ControlId).FirstOrDefault();
                binding.Script.Text = script;
            }
        }

        public void OnClosing()
        {
            saveBindings();
        }

        private ViewControl currentlyEditing;

        private string currentScript;

        public string CurrentScript
        {
            get
            {
                return currentScript;
            }

            private set
            {
                if (this.currentScript != value)
                {
                    currentScript = value;
                    OnPropertyChanged(nameof(CurrentScript));
                }
            }
        }


        /// <summary>
        /// Even run when databound property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles when property is changed raising <see cref="PropertyChanged"/>
        /// event.
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