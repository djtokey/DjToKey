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
        /// Opens a device package of a given name and loads a device definition from it
        /// </summary>
        /// <param name="fileName">Package file name to be opened</param>
        /// <param name="deviceName">Device name in a package to load configuration</param>
        public static DevicePackage Load(string fileName, string deviceName = null)
        {
            var p = new DevicePackage();

            using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                // .dtkpkg files may be device descriptors or other categories, if package describes itself
                // as not device package, throw exception
                if (!string.IsNullOrEmpty(pack.PackageProperties.Category) && pack.PackageProperties.Category != "device")
                    throw new ArgumentException("Package is not device descriptor");

                p.Title = pack.PackageProperties.Title;
                p.Description = pack.PackageProperties.Description;
                p.Version = pack.PackageProperties.Version;
                

                Uri u;
                if (string.IsNullOrEmpty(deviceName))
                    u = new Uri("/image.png", UriKind.Relative);
                else
                    u = new Uri(string.Format("/{0}/image.png", MakeValidFileName(deviceName)), UriKind.Relative);

                if (pack.PartExists(u))
                {
                    p.Image = new MemoryStream();
                    pack.GetPart(u).GetStream().CopyTo(p.Image);
                }
                else
                {
                    throw new FileNotFoundException("Device image file not found in package.");
                }
                
                if (string.IsNullOrEmpty(deviceName))
                    u = new Uri("/definition.json", UriKind.Relative);
                else
                    u = new Uri(string.Format("/{0}/definition.json", MakeValidFileName(deviceName)), UriKind.Relative);

                if (pack.PartExists(u))
                {
                    var f = pack.GetPart(u).GetStream();
                    using (StreamReader reader = new StreamReader(f, Encoding.UTF8))
                    {
                        p.Definition = reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new FileNotFoundException("Device configuration file not found in package.");
                }
            }

            return p;
        }

        /// <summary>
        /// A stream to device image file
        /// </summary>
        public Stream Image { get; private set; }

        /// <summary>
        /// A device definition file as a string
        /// </summary>
        public string Definition { get; private set; }

        /// <summary>
        /// Title of device package
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Optional description of device package
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Version of device package
        /// </summary>
        public string Version { get; private set; }        

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
