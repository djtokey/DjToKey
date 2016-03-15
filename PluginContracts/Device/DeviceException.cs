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
using System.Runtime.Serialization;

namespace Ktos.DjToKey.Plugins.Device
{
    /// <summary>
    /// Exception thrown when there is a problem with a device
    /// </summary>
    [Serializable]
    public class DeviceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DeviceException
        /// </summary>
        public DeviceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DeviceException with a message.
        /// 
        /// Use message to signalize what is wrong with your device.
        /// This message may be presented to the user.
        /// </summary>
        /// <param name="message">An error message</param>
        public DeviceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DeviceException with a
        /// message and InnerException
        /// 
        /// Use message to tell what is wrong with your device - this
        /// message may be presented to the user. Use InnerException
        /// to describe any internal exceptions of your device, if any.
        /// </summary>
        /// <param name="message">An error message</param>
        /// <param name="innerException">
        /// An inner exception from the script engine.
        /// </param>
        public DeviceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with
        /// serialized data.
        /// </summary>
        /// <param name="info">
        /// Holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// Contains contextual information about the source or destination.
        /// </param>
        protected DeviceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}