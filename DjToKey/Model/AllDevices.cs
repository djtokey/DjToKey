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

using Ktos.DjToKey.Plugins;
using Ktos.DjToKey.Plugins.Device;
using System.Collections.Generic;

namespace Ktos.DjToKey.Model
{
    public class AllDevices
    {
        public IEnumerable<string> AvailableDevices
        {
            get
            {
                return availableDevices;
            }
        }

        private List<string> availableDevices;

        private DevicePlugins plugins;

        public AllDevices(DevicePlugins plugins)
        {
            availableDevices = new List<string>();
            this.plugins = plugins;

            if (plugins.DeviceHandlers != null)
                foreach (var dh in plugins.DeviceHandlers)
                    availableDevices.AddRange(dh.AvailableDevices);
        }

        public IDeviceHandler FindHandler(string name)
        {
            if (plugins.DeviceHandlers == null)
                return null;

            foreach (var dh in plugins.DeviceHandlers)
                foreach (var n in dh.AvailableDevices)
                    if (n == name)
                        return dh;

            return null;
        }
    }
}