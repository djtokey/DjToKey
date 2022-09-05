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

using Ktos.DjToKey.Packaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Ktos.DjToKey.Plugins.Packaging
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
        /// Opens a device package of a given name and loads metadata
        /// from it
        /// </summary>
        /// <param name="fileName">Package file name to be opened</param>
        /// <param name="deviceName">
        /// Device name in a package to load configuration
        /// </param>
        /// <returns>
        /// A device package with a single device and whole metadata loaded
        /// </returns>
        public static DevicePackage Load(string fileName, string deviceName)
        {
            var p = new DevicePackage();

            p.Metadata = PackageHelper.LoadMetadata(fileName);
            p.PackageFileName = fileName;

            // .dtkpkg files may be device descriptors or other
            // categories, if package describes itself as not device
            // package, throw exception
            if (!string.IsNullOrEmpty(p.Metadata.Category) && p.Metadata.Category != "device")
                throw new ArgumentException("Package is not a device descriptor package.");

            p.Device = LoadDeviceFromPackage(fileName, deviceName);

            return p;
        }

        /// <summary>
        /// Loads a device definition and image from a specified
        /// device package
        /// </summary>
        /// <param name="fileName">Name of a device package file</param>
        /// <param name="deviceName">Name of a device</param>
        /// <returns>
        /// A device object with controls, image, name and other parameters
        /// </returns>
        public static Models.Device LoadDeviceFromPackage(string fileName, string deviceName)
        {
            // TODO: support for mapping file!
            Models.Device result = new Models.Device();
            result.Name = deviceName;

            deviceName = MakeValidFileName(deviceName).ToLower();

            using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                Uri u = new Uri(string.Format("/{0}/image.png", deviceName), UriKind.Relative);

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

                u = new Uri(string.Format("/{0}/definition.json", deviceName), UriKind.Relative);

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

            return result;
        }

        /// <summary>
        /// Loads all devices supported in a device package
        /// </summary>
        /// <param name="fileName">File name of a device package</param>
        /// <returns>
        /// List of all devices supported with their images and
        /// control definitions
        /// </returns>
        public static IEnumerable<Models.Device> LoadDevicesFromPackage(string fileName)
        {
            List<Models.Device> devices = new List<Models.Device>();

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

                // TODO: map file handling
                /*
                if (pack.PartExists(new Uri("/map.json", UriKind.Relative)))
                {
                    using (TextReader tr = new StreamReader(pack.GetPart(new Uri("/map.json", UriKind.Relative)).GetStream()))
                    {
                        mapFile.Map = tr.ReadToEnd();
                    }
                }*/

                foreach (var d in devicesInPackage.Distinct())
                {
                    Models.Device x = new Models.Device();
                    x.Name = d;

                    Uri u = new Uri(
                        string.Format("/{0}/definition.json", x.Name),
                        UriKind.Relative
                    );

                    if (pack.PartExists(u))
                    {
                        var f = pack.GetPart(u).GetStream();
                        using (StreamReader reader = new StreamReader(f, Encoding.UTF8))
                        {
                            string json = reader.ReadToEnd();
                            x.Controls = JsonConvert.DeserializeObject<
                                ObservableCollection<Models.ViewControl>
                            >(json);
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
            }

            return devices;
        }

        /// <summary>
        /// File name of a package file
        /// </summary>
        public string PackageFileName { get; private set; }

        /// <summary>
        /// Device package metadata
        /// </summary>
        public PackageMetadata Metadata { get; set; }

        /// <summary>
        /// Loaded device from a package
        /// </summary>
        public Models.Device Device { get; set; }

        /// <summary>
        /// Replaces characters in <c>text</c> that are not allowed in
        /// file names with the specified replacement character.
        /// </summary>
        /// <param name="text">
        /// Text to make into a valid filename. The same string is
        /// returned if it is valid already.
        /// </param>
        /// <returns>
        /// A string that can be used as a filename. If the output
        /// string would otherwise be empty, returns "_".
        /// </returns>
        private static string MakeValidFileName(string text)
        {
            StringBuilder sb = new StringBuilder(text.Length);
            var invalids = Path.GetInvalidFileNameChars();

            char repl = '_';

            // space is also changed into replacementchar
            Array.Resize(ref invalids, invalids.Length + 1);
            invalids[invalids.Length - 1] = ' ';

            bool changed = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (invalids.Contains(c))
                {
                    changed = true;
                    sb.Append(repl);
                }
                else
                    sb.Append(c);
            }

            if (sb.Length == 0)
                return "_";

            return changed ? sb.ToString() : text;
        }
    }
}
