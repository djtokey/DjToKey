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
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Ktos.DjToKey.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            Devices = new ObservableCollection<Device>();
            var allDevices = new AllDevices(App.PluginImporter.DevicePlugins.DeviceHandlers);

            // lists all devices as just simply strings, while all loading
            // will be when device is selected
            foreach (var d in allDevices.AvailableDevices)
                Devices.Add(new Device() { Name = d });
        }

        public void LoadDeviceFully(Device device)
        {
            if (device != null)
            {
                var d = PackageHelper.LoadDevicePackage(device.Name);

                for (int i = 0; i < Devices.Count; i++)
                {
                    if (Devices[i].Name == device.Name)
                        Devices[i] = d.Device;
                }
            }        
        }

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

        public ObservableCollection<Device> Devices { get; set; }


        private Device currentDevice;
        public Device CurrentDevice
        {
            get { return currentDevice; }
            set
            {
                if (this.currentDevice != value)
                {
                    currentDevice = value;
                    OnPropertyChanged(nameof(CurrentDevice));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
