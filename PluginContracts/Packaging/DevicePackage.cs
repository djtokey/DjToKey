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
        public DevicePackage(string fileName, string deviceName = null)
        {
            using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                // .dtkpkg files may be device descriptors or other categories, if package describes itself
                // as not device package, throw exception
                if (!string.IsNullOrEmpty(pack.PackageProperties.Category) && pack.PackageProperties.Category != "device")
                    throw new ArgumentException("Package is not device descriptor");

                Title = pack.PackageProperties.Title;
                Description = pack.PackageProperties.Description;
                Version = pack.PackageProperties.Version;

                deviceName = MakeValidFileName(deviceName);

                Uri u;
                if (string.IsNullOrEmpty(deviceName))
                    u = new Uri("/image.png", UriKind.Relative);
                else
                    u = new Uri(string.Format("/{0}/image.png", deviceName), UriKind.Relative);

                if (pack.PartExists(u))
                {
                    Image = pack.GetPart(u).GetStream();
                }
                else
                {
                    throw new FileNotFoundException("Device image file not found in package.");
                }
                
                if (string.IsNullOrEmpty(deviceName))
                    u = new Uri("/definition.json", UriKind.Relative);
                else
                    u = new Uri(string.Format("/{0}/definition.json", deviceName), UriKind.Relative);

                if (pack.PartExists(u))
                {
                    var f = pack.GetPart(u).GetStream();
                    using (StreamReader reader = new StreamReader(f, Encoding.UTF8))
                    {
                        Definition = reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new FileNotFoundException("Device configuration file not found in package.");
                }
            }
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

        private static char[] _invalids;

        // Source: http://stackoverflow.com/a/25223884

        /// <summary>Replaces characters in <c>text</c> that are not allowed in
        /// file names with the specified replacement character.</summary>
        /// <param name="text">Text to make into a valid filename. The same string is returned if it is valid already.</param>
        /// <param name="replacement">Replacement character, or null to simply remove bad characters.</param>
        /// <param name="fancy">Whether to replace quotes and slashes with the non-ASCII characters ” and ⁄.</param>
        /// <returns>A string that can be used as a filename. If the output string would otherwise be empty, returns "_".</returns>
        private static string MakeValidFileName(string text, char? replacement = '_', bool fancy = true)
        {
            StringBuilder sb = new StringBuilder(text.Length);
            var invalids = _invalids ?? (_invalids = Path.GetInvalidFileNameChars());

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
                    var repl = replacement ?? '\0';
                    if (fancy)
                    {
                        if (c == '"') repl = '”'; // U+201D right double quotation mark
                        else if (c == '\'') repl = '’'; // U+2019 right single quotation mark
                        else if (c == '/') repl = '⁄'; // U+2044 fraction slash
                    }
                    if (repl != '\0')
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
