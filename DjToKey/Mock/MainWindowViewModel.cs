﻿#region License

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
using Ktos.DjToKey.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ktos.DjToKey.Mock
{
    /// <summary>
    /// Mocking ViewModel for MainWindow, with only necessary properties
    /// </summary>
    internal class MainWindowViewModel
    {
        public IEnumerable<Plugins.Metadata> LoadedPlugins =>
            new List<Plugins.Metadata>
            {
                new Plugins.Metadata { Title = "Mock plugin", Version = "1.0" }
            };

        private List<Device> devices = new List<Device>
        {
            new Device { Name = "Mock", Controls = new ObservableCollection<ViewControl>() }
        };
        public IList<Device> AvailableDevices => devices;
        public Device CurrentDevice => devices[0];

        public string Version => "1.0.0-mock";

        public SettingsViewModel Settings =>
            new SettingsViewModel { ShowMessageWhenMinimized = true };
    }
}
