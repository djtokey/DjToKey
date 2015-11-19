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
using System.Text;
using System.IO.Packaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Ktos.DjToKey.Packaging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ktos.DjToKey.Plugins.Packaging
{
    /// <summary>
    /// Class describing package for a device
    /// 
    /// Device packages have images and control definitions for devices supported by a plugin. Build under
    /// a Open Packaging Conventions with a extension ".dtkpkg".
    /// </summary>
    public class DevicePackage
    {
        /// <summary>
        /// Opens a device package of a given name and loads metadata from it
        /// </summary>
        /// <param name="fileName">Package file name to be opened</param>
        /// <param name="deviceName">Device name in a package to load configuration</param>
        public static DevicePackage Load(string fileName, string deviceName)
        {
            var p = new DevicePackage();

            p.Metadata = PackageHelper.LoadMetadata(fileName);
            p.PackageFileName = fileName;

            // .dtkpkg files may be device descriptors or other categories, if package describes itself
            // as not device package, throw exception
            if (!string.IsNullOrEmpty(p.Metadata.Category) && p.Metadata.Category != "device")
                throw new ArgumentException("Package is not a device descriptor package.");

            p.Device = loadDevice(fileName, deviceName);

            return p;
        }

        private static Models.Device loadDevice(string fileName, string deviceName)
        {
            // TODO: support for mapping file!
            Models.Device result = new Models.Device();            

            deviceName = MakeValidFileName(deviceName).ToLower();

            using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                Uri u = new Uri(string.Format("/{0}/image.png", deviceName), UriKind.Relative);

                if (pack.PartExists(u))
                {
                    var imageStream = new MemoryStream();
                    pack.GetPart(u).GetStream().CopyTo(imageStream);

                    result.Image = new BitmapImage();
                    result.Image.BeginInit();
                    result.Image.StreamSource = imageStream;
                    result.Image.CacheOption = BitmapCacheOption.OnLoad;
                    result.Image.EndInit();
                    result.Image.Freeze();
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
                        result.Controls = JsonConvert.DeserializeObject<ObservableCollection<Device.Control>>(definition);
                    }
                    catch (JsonException)
                    {
                        throw new FileLoadException("Cannot load device configuration file from package");
                    }
                }
                else
                {
                    throw new FileNotFoundException("Device configuration file not found in package.");
                }
            }

            return result;
        }

        /// <summary>
        /// File name of a package file
        /// </summary>
        public string PackageFileName { get; private set; }

        /// <summary>
        /// Device package metadata
        /// </summary>
        public PackageMetadata Metadata;

        /// <summary>
        /// Loaded device from a package
        /// </summary>
        public Models.Device Device { get; set; }

        /// <summary>Replaces characters in <c>text</c> that are not allowed in
        /// file names with the specified replacement character.</summary>
        /// <param name="text">Text to make into a valid filename. The same string is returned if it is valid already.</param>        
        /// <returns>A string that can be used as a filename. If the output string would otherwise be empty, returns "_".</returns>
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
