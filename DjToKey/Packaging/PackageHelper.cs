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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Packaging;
using Ktos.DjToKey.Plugins.Packaging;
using System.IO;
using Ktos.DjToKey.Helpers;
using Ktos.DjToKey.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Ktos.DjToKey.Packaging
{
    /// <summary>
    /// A helper class for finding, loading, downloading and checking DjToKey packages
    /// </summary>
    class PackageHelper
    {
        private string safeDeviceName;
        private string deviceName;

        /// <summary>
        /// Loads device package for a specified device
        /// </summary>
        /// <param name="deviceName">Name of a device to load description package for</param>
        /// <returns>Loaded package</returns>
        public DevicePackage LoadDevicePackage(string deviceName)
        {
            safeDeviceName = ValidFileName.MakeValidFileName(deviceName).ToLower();
            this.deviceName = deviceName;
            IEnumerable<string> files = listDevicePackages();

            return DevicePackage.Load(findPackage(files), deviceName);            
        }

        private IEnumerable<string> listDevicePackages()
        {
            var appFolderByName = Directory.EnumerateFiles(@".\devices", safeDeviceName + ".dtkpkg");

            IEnumerable<string> localAppDataByName = null;
            try
            {
                localAppDataByName = Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"DjToKey\devices", safeDeviceName + ".dtkpkg"));
            }
            catch (DirectoryNotFoundException)
            {
                localAppDataByName = new List<string>();
            }

            var files = Enumerable.Concat(Enumerable.Concat(appFolderByName, localAppDataByName), listAllPackages());
            return files;
        }

        private static IEnumerable<string> listAllPackages()
        {
            var appFolderAll = Directory.EnumerateFiles(@".\devices", "*.dtkpkg");
            IEnumerable<string> localAppDataAll = null;
            try
            {
                localAppDataAll = Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"DjToKey\devices"), "*.dtkpkg");
            }
            catch (DirectoryNotFoundException)
            {
                localAppDataAll = new List<string>();
            }

            return Enumerable.Concat(appFolderAll, localAppDataAll);
        }

        private string findPackage(IEnumerable<string> filesList)
        {
            if (filesList.Count() > 0)
            {
                foreach (string f in filesList)
                {
                    var mt = loadPackageMetadata(f);
                    if (deviceSupported(mt.Keywords))
                        return f;
                }
            }

            throw new FileNotFoundException("No suitable package file found for this device");
        }

        private DevicePackage loadPackage(string fileName)
        {
            return DevicePackage.Load(fileName, safeDeviceName);
        }

        private bool deviceSupported(string keywords)
        {            
            if (string.IsNullOrEmpty(keywords))
                return false;

            var devices = keywords.Split(';');
            foreach (string d in devices)
            {
                if (d.StartsWith("*") && deviceName.EndsWith(d)) return true;
                if (d.EndsWith("*") && deviceName.StartsWith(d)) return true;
                if (d == deviceName) return true;
                if (d == safeDeviceName) return true;
                if (d.ToLower() == safeDeviceName) return true;
            }

            return false;
        }

        private PackageMetadata loadPackageMetadata(string fileName)
        {
            using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                return new PackageMetadata()
                {
                    Keywords = pack.PackageProperties.Keywords,
                    Title = pack.PackageProperties.Title,
                    Description = pack.PackageProperties.Description,
                    Version = pack.PackageProperties.Version
                };                
            }
        }

        private IEnumerable<Device> loadDevicesFromPackage(string fileName)
        {
            List<Device> devices = new List<Device>();

            using (var pack = Package.Open(fileName, FileMode.Open))
            {
                List<string> devicesInPackage = new List<string>();

                foreach (var p in pack.GetParts())
                {
                    string x = p.Uri.ToString().TrimStart('/');

                    if (x == "map.json")
                        continue;

                    x = x.Remove(x.IndexOf('/'));

                    if (x != "_rels" && x != "package")
                        devicesInPackage.Add(x);
                }

                foreach (var d in devicesInPackage.Distinct())
                {
                    Device x = new Device();
                    x.Name = d;

                    Uri u = new Uri(string.Format("/{0}/definition.json", x.Name), UriKind.Relative);

                    if (pack.PartExists(u))
                    {
                        var f = pack.GetPart(u).GetStream();
                        using (StreamReader reader = new StreamReader(f, Encoding.UTF8))
                        {
                            string json = reader.ReadToEnd();
                            x.Controls = JsonConvert.DeserializeObject<ObservableCollection<Plugins.Device.Control>>(json);
                        }
                    }

                    u = new Uri(string.Format("/{0}/image.png", x.Name), UriKind.Relative);

                    if (pack.PartExists(u))
                    {
                        BitmapImage bitmap;

                        using (var stream = pack.GetPart(u).GetStream())
                        {
                            bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            bitmap.Freeze();
                        }

                        x.Image = bitmap;
                    }

                    devices.Add(x);
                }

                /*
                if (pack.PartExists(new Uri("/map.json", UriKind.Relative)))
                {
                    using (TextReader tr = new StreamReader(pack.GetPart(new Uri("/map.json", UriKind.Relative)).GetStream()))
                    {
                        mapFile.Map = tr.ReadToEnd();
                    }
                }*/              
                
            }

            return devices;
        }
    }
}
