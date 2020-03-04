using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideIt.Stego.HostObject
{
    /// <summary>
    /// Represents a host object
    /// </summary>
    public interface IHostObject : IDisposable
    {
        /// <summary>
        /// Get the host object header
        /// </summary>
        byte[] Header { get; }

        /// <summary>
        /// Get the length of host object
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets the current position of this stream.
        /// </summary>
        long Position { get; set; }

        /// <summary>
        /// Read the next byte from host object
        /// </summary>
        /// <returns>Next byte from host object</returns>
        int Read();

        /// <summary>
        /// Reads a block of bytes from the host object and writes the data in a given buffer.
        /// </summary>
        /// <param name="buffer">When this method returns, buffer contains the specified byte array with the values
        /// between offset and (offset + count - 1) replaced by the bytes read from the
        /// current source.</param>
        /// <param name="offset">The byte offset in array at which the read bytes will be placed.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns></returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Set the next frame in the buffer
        /// </summary>
        /// <param name="buffer">Next frame</param>
        void SetFrame(byte[] buffer);
    }
}
