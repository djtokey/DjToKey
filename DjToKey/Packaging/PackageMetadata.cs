using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Packaging
{
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


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
