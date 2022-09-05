using Ktos.DjToKey.Models;
using Ktos.DjToKey.Packaging;
using Ktos.DjToKey.Plugins.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Devices
{
    internal class DeviceManager : INotifyPropertyChanged
    {
        /// <summary>
        /// List of all device packages available
        /// </summary>
        public List<DevicePackage> AvailableDevicePackages { get; init; }

        /// <summary>
        /// List of all supported and available devices from all device handlers
        /// </summary>
        public List<Device> AvailableDevices { get; init; }

        private IEnumerable<IDeviceHandler> deviceHandlers;

        /// <summary>
        /// Initializes a new instance of DeviceManager
        /// </summary>
        /// <param name="deviceHandlers"></param>
        public DeviceManager(IEnumerable<IDeviceHandler> deviceHandlers)
        {
            // loads all device package metadata
            AvailableDevicePackages = PackageHelper
                .GetAllDevicePackageFileNames()
                .Select(x => DevicePackage.LoadMetadata(x))
                .ToList();

            AvailableDevices = new List<Device>();
            this.deviceHandlers = deviceHandlers;

            if (deviceHandlers != null)
                foreach (var dh in deviceHandlers)
                {
                    foreach (var names in dh.AvailableDeviceNames)
                    {
                        var package = AvailableDevicePackages.FirstOrDefault(
                            x => x.IsDeviceSupported(names)
                        );

                        if (package != null)
                        {
                            AvailableDevices.Add(
                                new Device
                                {
                                    Name = names,
                                    Handler = dh,
                                    Package = package
                                }
                            );
                        }
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
