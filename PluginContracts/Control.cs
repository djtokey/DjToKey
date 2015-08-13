#region Licence

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

#endregion Licence

namespace Ktos.DjToKey.Plugins.Contracts
{
    /// <summary>
    /// A control which can cause events in application
    /// </summary>
    public class Control
    {
        /// <summary>
        /// Internal Control ID, usually numerical
        /// </summary>
        public string ControlId { get; set; }

        /// <summary>
        /// Control name visible for user for fast identification
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// Type of this particular control
        /// </summary>
        public ControlType Type { get; set; }
    }

    /// <summary>
    /// Available control types
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// Analog control (values from 0 to 127)
        /// </summary>
        Analog,

        /// <summary>
        /// Digital control (values in [0;127] or [1;127])
        /// </summary>
        Digital,

        /// <summary>
        /// Button control
        /// </summary>
        Button
    }
}