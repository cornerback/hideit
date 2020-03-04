using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HideIt.Stego.HostObject
{
    /// <summary>
    /// Provide methods to read file data. This class provide sequential
    /// access to file stream
    /// </summary>
    /// <remarks>This class provide method to read data from stream byte
    /// by byte. It do so by buffering data and then serving from that buffer,
    /// because reading byte by byte from file can be a costly operation </remarks>
    internal class BufferedStream : IDisposable
    {
        /// <summary>
        /// The size of chunk that is read from bitmap stream
        /// </summary>
        private const int _chunkSize = 1024;

        // <summary>
        /// File stream to help read bitmap
        /// </summary>
        private FileStream _stream;

        /// <summary>
        /// An in-memory buffer to hold read data. This buffer will hold a chunk
        /// of data from actual stream. User can retrieve the data
        /// byte by byte. Once all of the data in buffer is read, it loads the next
        /// chunk from stream.
        /// We are not using FileStream's ReadByte method because it may read each
        /// byte from file which can involve a disk-read operation. This is a costly operation.
        /// </summary>
        private MemoryStream _buffer;

        /// <summary>
        /// We save the length of file stream so we dont have to ask the file
        /// stream for the length everytime. As we are opening the stream in read
        /// only mode, and then locking it so no one can alter the file while we
        /// are reading, therefore its pretty safe to assume that the length will
        /// not change
        /// </summary>
        private int _length;

        /// <summary>
        /// Determine whether the file is locked, so to unlock it
        /// </summary>
        private bool _locked;

        /// <summary>
        /// Open a new stream and lock the file if necessary
        /// </summary>
        /// <param name="path">Path of the file to open</param>
        /// <param name="lockFile">Lock the file so other processes cannot alter it</param>
        public BufferedStream(string path, bool lockFile)
        {
            ///Only read access to the bitmap.
            this._stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            ///save the length from base stream
            this._length = (int)this._stream.Length;

            ///Save the user flag for whether to lock file or not
            this._locked = lockFile;

            if ((this._locked))
            {
                ///Lock the base stream. We do not want the file to change while
                ///we are reading the file            
                this._stream.Lock(0, this._length);
            }

            this._buffer = new MemoryStream();
        }

        /// <summary>
        /// Get length of bitmap
        /// </summary>
        public int Length
        {
            get { return (int)this._length; }
        }

        /// <summary>
        /// Gets the current position of this stream.
        /// </summary>
        public int Position
        {
            get { return (int)this._stream.Position; }
            private set { this._stream.Position = value; }
        }

        /// <summary>
        /// Read the next byte from stream
        /// </summary>
        /// <returns>Next byte from stream</returns>
        public int Read()
        {
            ///Reached the end of stream
            if (this.Position == this._length)
            {
                return -1;
            }

            ///The buffer needs refiling with new data.
            if (this._buffer.Position == this._buffer.Length)
            {
                ///Save the orignal position of cover stream
                int orignalPosition = (int)this._stream.Position;

                ///Number of bytes to be read
                int size = _chunkSize;

                ///If the remaining data is less then the chunk size
                ///For e.g. The data to read in only 1000 bytes while the
                ///chunk size is 1024 bytes.
                if ((this._length - this.Position) < _chunkSize)
                {
                    ///New size of chunk
                    size = this._length - this.Position;
                }

                ///Initialize a new chunk
                byte[] chunk = new byte[_chunkSize];

                ///Read next data from chunk
                this._stream.Read(chunk, 0, size);

                ///We have read from the stream, now we change the position
                ///back to its orignal value
                this._stream.Position = orignalPosition;

                ///Bring to start of buffer, so the chunk over-writes the previous
                ///chunk data
                this._buffer.Position = 0;

                ///Update the buffer with new chunk
                this._buffer.Write(chunk, 0, chunk.Length);

                ///Again bring the stream to start so we can start reading bytes
                this._buffer.Position = 0;
            }

            ///Increment the position of cover stream (to simulate that we are reading from it)
            this._stream.Position++;

            ///Read and return the next byte from buffer
            return this._buffer.ReadByte();
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
            return this._stream.Read(buffer, offset, count);
        }

        #region IDisposable Members

        /// <summary>
        ///  Free all resources associated with this stream
        /// </summary>
        public void Dispose()
        {
            if (this._buffer != null)
            {
                this._buffer.Close();
                this._buffer.Dispose();
                this._buffer = null;
            }
            if (this._stream != null)
            {
                if (this._locked)
                {
                    ///Unlock the file first
                    this._stream.Unlock(0, this._length);
                }
                this._stream.Close();
                this._stream.Dispose();
                this._stream = null;
            }
        }

        #endregion
    }
}
