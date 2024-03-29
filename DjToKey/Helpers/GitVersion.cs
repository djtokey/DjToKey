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

using System.Reflection;

namespace Ktos.Build
{
    /// <summary>
    /// A simple class getting information from assembly built with
    /// Ktos.Build.GitVersion about InformationalVersion attribute
    /// covering full semver versioning of an assembly
    /// </summary>
    public class GitVersion
    {
        /// <summary>
        /// Full SemVer from an assembly, in form of similar to "0.3.1-devel.1+39.Branch.devel.Sha.cb1319297409cd056e8126767be2d33206ec86ba"
        /// </summary>
        public static string FullSemVer
        {
            get
            {
                return (
                    Assembly
                        .GetExecutingAssembly()
                        .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[
                        0
                    ] as AssemblyInformationalVersionAttribute
                ).InformationalVersion;
            }
        }

        /// <summary>
        /// Short SemVer version from an assembly, similar to "0.3.1-devel.1"
        /// </summary>
        public static string SemVer
        {
            get
            {
                return (
                    Assembly
                        .GetExecutingAssembly()
                        .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0]
                    as AssemblyFileVersionAttribute
                ).Version;
            }
        }
    }
}
