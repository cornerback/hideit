using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HideIt.Stego.HostObject
{
    /// <summary>
    /// Represent an AVI file host object
    /// </summary>
    internal sealed class Avi : IHostObject
    {
        private byte[] _header = new byte[0];
        private MemoryStream _currentFrame = null;

        private int _length;
        private int _position;

        /// <summary>
        /// Create a new AVI host object
        /// </summary>
        /// <param name="length">Length of avi file</param>
        public Avi(int length)
        {
            this._length = length;
            this._position = 0;
        }

        /// <summary>
        /// Set the next frame in the buffer
        /// </summary>
        /// <param name="buffer">Next frame</param>
        public void SetFrame(byte[] buffer)
        {
            if (this._currentFrame != null)
            {
                try
                {
                    this._currentFrame.Close();
                    this._currentFrame.Dispose();
                }
                catch (Exception)
                {
                }
            }

            this._currentFrame = new MemoryStream(buffer);
        }

        #region IHostObject Members

        /// <summary>
        /// Get the host object header
        /// </summary>
        public byte[] Header
        {
            get { return this._header; }
        }

        /// <summary>
        /// Get length of avi file
        /// </summary>
        public long Length
        {
            get { return this._length; }
        }

        /// <summary>
        /// Gets the current position of this stream.
        /// Setting the value will throw NotImplementedException
        /// </summary>
        public long Position
        {
            get { return this._position; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Read the next byte from stream
        /// </summary>
        /// <returns>Next byte from stream</returns>
        public int Read()
        {
            if (this._currentFrame == null)
            {
                return -1;
            }

            int next = this._currentFrame.ReadByte();
            if (next != -1)
            {
                this._position++;
            }

            return next;
        }

        /// <summary>
        /// Reads a block of bytes from the stream and writes the data in a given buffer.
        /// </summary>
        /// <param name="buffer">When this method returns, contains the specified byte array with the values
        /// between offset and (offset + count - 1) replaced by the bytes read from the
        /// current source.</param>
        /// <param name="offset">The byte offset in array at which the read bytes will be placed.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (this._currentFrame == null)
            {
                return 0;
            }

            int read = this._currentFrame.Read(buffer, offset, count);
            this._position += read;

            return read;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        ///  Free all resources associated with this stream
        /// </summary>
        public void Dispose()
        {
            if (this._currentFrame != null)
            {
                this._currentFrame.Close();
                this._currentFrame.Dispose();
                this._currentFrame = null;
            }
        }

        #endregion
    }
}
