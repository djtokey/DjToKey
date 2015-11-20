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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Ktos.DjToKey.Models
{
    /// <summary>
    /// Representation of a device, with a name, list of all controls
    /// and image of a controller shown in UI.
    /// </summary>
    public class Device : INotifyPropertyChanged
    {
        private string _name;

        /// <summary>
        /// Name of a device, used to show in UI
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        
        /// <summary>
        /// Collection of all controls supported by the device
        /// </summary>
        public ObservableCollection<Plugins.Device.Control> Controls { get; set; }

        private BitmapImage _image;

        /// <summary>
        /// Image of a device, shown in UI
        /// </summary>
        public BitmapImage Image
        {
            get
            {
                return _image;
            }

            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        /// <summary>
        /// Returns name of a device
        /// </summary>
        /// <returns>Device name</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Event run when any property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles when property is changed raising <see cref="PropertyChanged"/>
        /// event.
        /// 
        /// Part of <see cref="INotifyPropertyChanged"/> implementation
        /// </summary>
        /// <param name="name">Name of a changed property</param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
