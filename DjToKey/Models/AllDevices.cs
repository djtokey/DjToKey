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

using Ktos.DjToKey.Plugins.Device;
using System.Collections.Generic;

namespace Ktos.DjToKey.Models
{
    /// <summary>
    /// This class lists all possible devices from all device handlers
    /// and performs routing to get a handler for a device with
    /// specified name
    /// </summary>
    public class AllDevices
    {
        /// <summary>
        /// List of all possible devices from all device handlers
        /// </summary>
        public IEnumerable<string> AvailableDevices
        {
            get { return availableDevices; }
        }

        private List<string> availableDevices;

        private IEnumerable<IDeviceHandler> plugins;

        /// <summary>
        /// Creates a new instance of AllDevices
        /// </summary>
        /// <param name="plugins">
        /// List of all plugins of IDeviceHandler type
        /// </param>
        public AllDevices(IEnumerable<IDeviceHandler> plugins)
        {
            availableDevices = new List<string>();
            this.plugins = plugins;

            if (plugins != null)
                foreach (var dh in plugins)
                {
                    availableDevices.AddRange(dh.AvailableDevices);
                }
        }

        /// <summary>
        /// Finds handler for a specific device name
        /// </summary>
        /// <param name="name">Name of a device</param>
        /// <returns>Handler for this device name</returns>
        public IDeviceHandler FindHandler(string name)
        {
            if (plugins == null)
                return null;

            foreach (var dh in plugins)
                foreach (var n in dh.AvailableDevices)
                    if (n == name)
                        return dh;

            return null;
        }
    }
}
