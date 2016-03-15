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

using System.Windows.Media;

namespace Ktos.DjToKey.Models
{
    /// <summary>
    /// Extension of a <see cref="Plugins.Device.Control"/> with
    /// position in a window included
    /// </summary>
    public class ViewControl : Plugins.Device.Control
    {
        /// <summary>
        /// Defines position of a control in a window, in pixels from Left
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Defines position of a control in a window, in pixels from Top
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Control width on a screen
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Control height on a screen
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Control background color
        /// </summary>
        public Color Background { get; set; }
    }
}