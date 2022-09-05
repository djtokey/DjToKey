#region License

/*
 * DjToKey
 *
 * Copyright (C) Marcin Badurowicz 2015-2016
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Ktos.DjToKey.Packaging
{
    /// <summary>
    /// Class describing package for a device
    ///
    /// Device packages have images and control definitions for
    /// devices supported by a plugin. Build under a Open Packaging
    /// Conventions with a extension ".dtkpkg".
    /// </summary>
    public class DevicePackage
    {
        /// <summary>
        /// Loads a device definition and image from a specified
        /// device package
        /// </summary>
        /// <param name="fileName">Name of a device package file</param>
        /// <param name="deviceName">Name of a device</param>
        /// <returns>
        /// A device object with controls, image, name and other parameters
        /// </returns>
        public static Models.Device LoadDeviceDefinitionFromPackage(
            DevicePackage package,
            string deviceName
        )
        {
            Models.Device result = new Models.Device();
            result.Name = deviceName;

            if (package.IsPackaged)
                LoadFromPackage(package, result);
            else
                LoadUnpackaged(package, result);

            return result;
        }

        private static void LoadUnpackaged(DevicePackage package, Device result)
        {
            var definitionPath = System.IO.Path.Combine(package.Path, "definition.json");
            var imagePath = System.IO.Path.Combine(package.Path, "image.png");

            if (System.IO.File.Exists(imagePath))
            {
                var imageStream = new MemoryStream();
                File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                    .CopyTo(imageStream);

                try
                {
                    result.Image = new BitmapImage();
                    result.Image.BeginInit();
                    result.Image.StreamSource = imageStream;
                    result.Image.CacheOption = BitmapCacheOption.OnLoad;
                    result.Image.EndInit();
                    result.Image.Freeze();
                }
                catch
                {
                    throw new FileLoadException("Error when loading image file for a device");
                }
            }
            else
            {
                throw new FileNotFoundException("Device image file not found in package.");
            }

            if (File.Exists(definitionPath))
            {
                var definition = File.ReadAllText(definitionPath);

                try
                {
                    result.Controls = JsonConvert.DeserializeObject<
                        ObservableCollection<Models.ViewControl>
                    >(definition);
                }
                catch (JsonException)
                {
                    throw new FileLoadException(
                        "Cannot load device configuration file from package"
                    );
                }
            }
            else
            {
                throw new FileNotFoundException("Device configuration file not found in package.");
            }
        }

        private static void LoadFromPackage(DevicePackage package, Device result)
        {
            using (var pack = Package.Open(package.Path, FileMode.Open, FileAccess.Read))
            {
                Uri u = new Uri("/device/image.png", UriKind.Relative);

                if (pack.PartExists(u))
                {
                    var imageStream = new MemoryStream();
                    pack.GetPart(u).GetStream().CopyTo(imageStream);

                    try
                    {
                        result.Image = new BitmapImage();
                        result.Image.BeginInit();
                        result.Image.StreamSource = imageStream;
                        result.Image.CacheOption = BitmapCacheOption.OnLoad;
                        result.Image.EndInit();
                        result.Image.Freeze();
                    }
                    catch
                    {
                        throw new FileLoadException("Error when loading image file for a device");
                    }
                }
                else
                {
                    throw new FileNotFoundException("Device image file not found in package.");
                }

                u = new Uri("/device/definition.json", UriKind.Relative);

                if (pack.PartExists(u))
                {
                    var f = pack.GetPart(u).GetStream();
                    string definition;
                    using (StreamReader reader = new StreamReader(f, Encoding.UTF8))
                    {
                        definition = reader.ReadToEnd();
                    }

                    try
                    {
                        result.Controls = JsonConvert.DeserializeObject<
                            ObservableCollection<Models.ViewControl>
                        >(definition);
                    }
                    catch (JsonException e)
                    {
                        throw new FileLoadException(
                            "Cannot load device configuration file from package"
                        );
                    }
                }
                else
                {
                    throw new FileNotFoundException(
                        "Device configuration file not found in package."
                    );
                }
            }
        }

        public static DevicePackage LoadMetadata(string fileName)
        {
            var result = new DevicePackage();

            if (Directory.Exists(fileName))
            {
                // unpackaged device definition
                result.Metadata = JsonConvert.DeserializeObject<Metadata>(
                    File.ReadAllText(System.IO.Path.Combine(fileName, "./metadata.json"))
                );

                result.IsPackaged = false;
            }
            else
            {
                result.IsPackaged = true;
                using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    result.Metadata = new Metadata()
                    {
                        Keywords = pack.PackageProperties.Keywords,
                        Title = pack.PackageProperties.Title,
                        Description = pack.PackageProperties.Description,
                        Version = pack.PackageProperties.Version,
                        Category = pack.PackageProperties.Category
                    };
                }
            }

            result.Path = fileName;

            if (result.Metadata.Category != "device")
                throw new InvalidOperationException("Invalid package, not device package.");

            return result;
        }

        /// <summary>
        /// File name of a package file
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Is the package as a file or a loose directory
        /// </summary>
        public bool IsPackaged { get; set; }

        /// <summary>
        /// Device package metadata
        /// </summary>
        public Metadata Metadata { get; set; }

        /// <summary>
        /// List of all supported devices by this device package
        /// (may use wildcards)
        /// </summary>
        public string[] SupportedDevices => Metadata.Keywords.Split(';');
    }
}
