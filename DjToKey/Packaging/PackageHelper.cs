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
using Ktos.DjToKey.Plugins.Packaging;
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
    internal class PackageHelper
    {
        /// <summary>
        /// Loads device package for a specified device
        /// </summary>
        /// <param name="deviceName">
        /// Name of a device to load description package for
        /// </param>
        /// <returns>Loaded package</returns>
        public static DevicePackage LoadDevicePackage(string deviceName)
        {
            IEnumerable<string> files = ListDevicePackageFileNames(deviceName);

            return DevicePackage.Load(FindDevicePackageName(files, deviceName), deviceName);
        }

        /// <summary>
        /// Lists file names for all packages which are possible to
        /// handle device of a specified name
        /// </summary>
        /// <param name="deviceName">Name of a device to be handled</param>
        /// <returns>A list of packages file names</returns>
        public static IEnumerable<string> ListDevicePackageFileNames(string deviceName)
        {
            string safeDeviceName = ValidFileName.MakeValidFileName(deviceName).ToLower();

            IEnumerable<string> appFolderByName = null;
            try
            {
                appFolderByName = Directory.EnumerateFiles(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        @".\devices"
                    ),
                    safeDeviceName + ".dtkpkg"
                );
            }
            catch (DirectoryNotFoundException)
            {
                appFolderByName = new List<string>();
            }

            IEnumerable<string> localAppDataByName = null;
            try
            {
                localAppDataByName = Directory.EnumerateFiles(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"DjToKey\devices",
                        safeDeviceName + ".dtkpkg"
                    )
                );
            }
            catch (DirectoryNotFoundException)
            {
                localAppDataByName = new List<string>();
            }

            var files = Enumerable.Concat(
                Enumerable.Concat(appFolderByName, localAppDataByName),
                ListAllPackages()
            );
            return files;
        }

        /// <summary>
        /// Lists all dtkpkg files
        /// </summary>
        /// <returns>Names of all dtkpkg files</returns>
        public static IEnumerable<string> ListAllPackages()
        {
            IEnumerable<string> appFolderAll = null;
            try
            {
                appFolderAll = Directory.EnumerateFiles(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        @".\devices"
                    ),
                    "*.dtkpkg"
                );
            }
            catch (DirectoryNotFoundException)
            {
                appFolderAll = new List<string>();
            }

            IEnumerable<string> localAppDataAll = null;
            try
            {
                localAppDataAll = Directory.EnumerateFiles(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"DjToKey\devices"
                    ),
                    "*.dtkpkg"
                );
            }
            catch (DirectoryNotFoundException)
            {
                localAppDataAll = new List<string>();
            }

            return Enumerable.Concat(appFolderAll, localAppDataAll);
        }

        /// <summary>
        /// Finds a first package which can handle a device
        /// </summary>
        /// <param name="filesList">List of all device packages</param>
        /// <param name="deviceName">Name of a device to be handled</param>
        /// <returns>
        /// Filename of a package which can handle this device
        /// </returns>
        public static string FindDevicePackageName(IEnumerable<string> filesList, string deviceName)
        {
            if (filesList.Count() > 0)
            {
                foreach (string f in filesList)
                {
                    var mt = LoadMetadata(f);
                    if (IsDeviceSupported(mt.Keywords, deviceName))
                        return f;
                }
            }

            throw new FileNotFoundException("No suitable package file found for this device");
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
        public static bool IsDeviceSupported(string keywords, string deviceName)
        {
            string safeDeviceName = ValidFileName.MakeValidFileName(deviceName).ToLower();

            if (string.IsNullOrEmpty(keywords))
                return false;

            var devices = keywords.Split(';');
            foreach (string d in devices)
            {
                if (d.StartsWith("*") && deviceName.EndsWith(d))
                    return true;
                if (d.EndsWith("*") && deviceName.StartsWith(d))
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
        public static PackageMetadata LoadMetadata(string fileName)
        {
            using (var pack = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                return new PackageMetadata()
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
