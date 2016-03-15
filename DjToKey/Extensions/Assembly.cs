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

using System;
using System.IO;
using System.Linq;

/// <summary>
/// Contains extension methods
/// </summary>
namespace Ktos.DjToKey.Extensions
{
    /// <summary>
    /// Contains extension methods for Assembly class to get access to
    /// their custom attributes
    /// </summary>
    public static class Assembly
    {
        /// <summary>
        /// Gets FileVersion attribute from assembly loaded with ReflectionOnlyLoadFrom
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed</param>
        /// <returns>Returns assembly file version as a string</returns>
        public static string GetVersion(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyFileVersionAttribute));
        }

        /// <summary>
        /// Gets Title attribute from assembly loaded with ReflectionOnlyLoadFrom
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed</param>
        /// <returns>Returns assembly title as a string</returns>
        public static string GetTitle(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyTitleAttribute));
        }

        /// <summary>
        /// Gets Description attribute from assembly loaded with ReflectionOnlyLoadFrom
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed</param>
        /// <returns>Returns assembly description as a string</returns>
        public static string GetDescription(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyDescriptionAttribute));
        }

        /// <summary>
        /// Gets Copyright attribute from assembly loaded with ReflectionOnlyLoadFrom
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed</param>
        /// <returns>Returns assembly copyright as a string</returns>
        public static string GetCopyright(this System.Reflection.Assembly assembly)
        {
            return customAttributeToString(assembly, typeof(System.Reflection.AssemblyCopyrightAttribute));
        }

        /// <summary>
        /// Gets desired assembly attribute as a string, parsed from
        /// custom attributes when assembly is loaded with ReflectionOnlyLoadFrom
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed</param>
        /// <param name="attributeType">
        /// Type of custom attribute to return
        /// </param>
        /// <returns>
        /// Value of desired assembly attribute as a string
        /// </returns>
        private static string customAttributeToString(System.Reflection.Assembly assembly, Type attributeType)
        {
            string s = null;
            var u = assembly.GetCustomAttributesData().Where(x => x.AttributeType == attributeType).FirstOrDefault();
            if (u != null)
            {
                // attributes are being converted to string, so they
                // are in the form of [Name(\"value\")], and we need
                // to get rid of unnecessary chars
                s = u.ToString();
                s = s.Remove(0, s.IndexOf('"'));
                s = s.Remove(s.IndexOf(')'));
                s = s.Replace("\"", string.Empty);
            }

            return s;
        }

        /// <summary>
        /// Converts assembly class into set of metadata for a plugin
        /// </summary>
        /// <param name="assembly">
        /// Loaded plugin assembly using ReflectionOnlyLoadFrom
        /// </param>
        /// <returns>
        /// Plugin metadata - title, description, version and
        /// copyright information for a plugin
        /// </returns>
        public static Plugins.Metadata GetMetadata(this System.Reflection.Assembly assembly)
        {
            try
            {
                return new Plugins.Metadata()
                {
                    Title = assembly.GetTitle(),
                    Copyright = assembly.GetCopyright(),
                    Description = assembly.GetDescription(),
                    Version = assembly.GetVersion()
                };
            }
            catch (FileLoadException)
            {
                return null;
            }
        }
    }
}