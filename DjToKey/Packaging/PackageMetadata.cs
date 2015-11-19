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

using System.ComponentModel;

namespace Ktos.DjToKey.Packaging
{
    /// <summary>
    /// Class describing all metadata available for the package
    /// </summary>
    public class PackageMetadata : INotifyPropertyChanged
    {
        private string title;

        /// <summary>
        /// Title of device package
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string description;

        /// <summary>
        /// Optional description of device package
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string version;

        /// <summary>
        /// Version of device package
        /// </summary>
        public string Version
        {
            get { return version; }
            set
            {
                if (version != value)
                {
                    version = value;
                    OnPropertyChanged(nameof(Version));
                }
            }
        }

        private string keywords;

        /// <summary>
        /// Keywords of device package - list of device names supported
        ///
        /// Separated by ";", may use * wildcard to support many devices with same prefix
        /// </summary>
        public string Keywords
        {
            get { return keywords; }
            set
            {
                if (keywords != value)
                {
                    keywords = value;
                    OnPropertyChanged(nameof(Keywords));
                }
            }
        }

        private string category;

        /// <summary>
        /// Category of a package
        ///
        /// May be "device" is this package is a device description package with images and controls definitions
        /// </summary>
        public string Category
        {
            get { return category; }
            set
            {
                if (this.category != value)
                {
                    category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        /// <summary>
        /// Raised when any property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}