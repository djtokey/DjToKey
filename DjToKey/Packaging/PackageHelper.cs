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

using Ktos.DjToKey.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;

namespace Ktos.DjToKey.Packaging
{
    /// <summary>
    /// A helper class for finding, loading, downloading and checking
    /// DjToKey packages
    /// </summary>
    internal static class PackageHelper
    {
        /// <summary>
        /// Lists all device package files and directories
        /// </summary>
        /// <returns>Names of all dtkpkg files</returns>
        public static IEnumerable<string> GetAllDevicePackageFileNames()
        {
            List<string> packages = new();

            try
            {
                var appFolderDtks = Directory.EnumerateFiles(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "devices"
                    ),
                    "*.dtkpkg"
                );
                packages.AddRange(appFolderDtks);

                var appFolderUnpackaged = Directory.EnumerateDirectories(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "devices"
                    ),
                    "*"
                );
                packages.AddRange(appFolderUnpackaged);

                var localAppDataDtks = Directory.EnumerateFiles(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"DjToKey\devices"
                    ),
                    "*.dtkpkg"
                );
                packages.AddRange(localAppDataDtks);

                var localAppDataUnpackaged = Directory.EnumerateDirectories(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        @".\devices"
                    ),
                    "*"
                );
                packages.AddRange(localAppDataUnpackaged);
            }
            catch (DirectoryNotFoundException) { }

            return packages;
        }

        /// <summary>
        /// Checks if device is supported by package by checking
        /// keywords from package metadata
        /// </summary>
        /// <param name="keywords">
        /// Keywords extracted from package metadata
        /// </param>
        /// <param name="deviceName">Name of device</param>
        /// <returns>Returns if device is supported</returns>
        public static bool IsDeviceSupported(this DevicePackage package, string deviceName)
        {
            string safeDeviceName = ValidFileName.MakeValidFileName(deviceName).ToLower();

            if (package.SupportedDevices.Length == 0)
                return false;

            foreach (string d in package.SupportedDevices)
            {
                if (d.StartsWith("*") && deviceName.EndsWith(d.Replace("*", "")))
                    return true;
                if (d.EndsWith("*") && deviceName.StartsWith(d.Replace("*", "")))
                    return true;
                if (d == deviceName)
                    return true;
                if (d == safeDeviceName)
                    return true;
                if (d.ToLower() == safeDeviceName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Loads all metadata for a specified package
        /// </summary>
        /// <param name="fileName">File name to a package</param>
        /// <returns>
        /// All package metadata - Keywords, Title, Description,
        /// Version and Category
        /// </returns>
        public static Metadata LoadMetadata(string fileName)
        {
            if (Directory.Exists(fileName))
            {
                // unpackaged device definition
                return JsonConvert.DeserializeObject<Metadata>(
                    File.ReadAllText(Path.Combine(fileName, "./metadata.json"))
                );
            }
            else
            {
                using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    return new Metadata()
                    {
                        Keywords = pack.PackageProperties.Keywords,
                        Title = pack.PackageProperties.Title,
                        Description = pack.PackageProperties.Description,
                        Version = pack.PackageProperties.Version,
                        Category = pack.PackageProperties.Category
                    };
                }
            }
        }
    }
}
