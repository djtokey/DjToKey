﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ktos.DjToKey.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ktos.DjToKey.Resources.AppResources", typeof(AppResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} {1}\n\nThis is a very basic MIDI-controller to script mapper. It allows you to prepare custom scripts for moving mouse, pressing keys and similar things, fired every time some action on your MIDI device occurs. For example, you can bind your Deck from DJ console to a mouse wheel.\n\nCopyright (C) Marcin Badurowicz 2015\nIcon used from: https://icons8.com/&quot;.
        /// </summary>
        internal static string About {
            get {
                return ResourceManager.GetString("About", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DjToKey.
        /// </summary>
        internal static string AppName {
            get {
                return ResourceManager.GetString("AppName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Błąd podczas odczytu pliku definiującego kontrolki tego urządzenia MIDI!.
        /// </summary>
        internal static string ControlFileError {
            get {
                return ResourceManager.GetString("ControlFileError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nie znaleziono pliku definiującego kontrolki tego urządzenia MIDI!.
        /// </summary>
        internal static string ControlFileNotFound {
            get {
                return ResourceManager.GetString("ControlFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Błąd urządzenia MIDI!.
        /// </summary>
        internal static string MidiError {
            get {
                return ResourceManager.GetString("MidiError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nie znaleziono urządzeń MIDI!.
        /// </summary>
        internal static string NoMidiMessage {
            get {
                return ResourceManager.GetString("NoMidiMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugins loaded:.
        /// </summary>
        internal static string PluginsMessage {
            get {
                return ResourceManager.GetString("PluginsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wystąpił błąd w obsłudze zdarzenia dla kontrolki {0}: {1}.
        /// </summary>
        internal static string ScriptErrorMessage {
            get {
                return ResourceManager.GetString("ScriptErrorMessage", resourceCulture);
            }
        }
    }
}
