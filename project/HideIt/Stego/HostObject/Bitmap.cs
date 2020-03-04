using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HideIt.Stego.HostObject;
using System.Drawing.Imaging;

namespace HideIt.Stego.HostObject
{
    /// <summary>
    /// Provide methods to read bitmap data. This class provide sequential
    /// access to bitmap stream
    /// </summary>
    internal sealed class Bitmap : BufferedStream, IHostObject
    {
        public const int HEADER_SIZE = 128;

        byte[] _header;

        /// <summary>
        /// Open a bitmap image
        /// </summary>
        /// <param name="path">Path of the bitmap image</param>
        public Bitmap(string path)
            : base(path, true)
        {
            ///Copy the bitmap headers.
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);
            int size = (int)(bmp.PhysicalDimension.Height * bmp.PhysicalDimension.Width * (float)this.GetPixelFormat(bmp.PixelFormat) / 8);

            int headerLen = base.Length - size;
            if (headerLen < 0)
            {
                headerLen = HEADER_SIZE;
            }

            this._header = new byte[headerLen];
            this.Read(this._header, 0, headerLen);
        }

        /// <summary>
        /// Get the number of bits per pixel used
        /// </summary>
        /// <param name="format">Pixel format of bitmap</param>
        /// <returns>Number of bits per pixel</returns>
        private int GetPixelFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Canonical:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;

                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;

                case PixelFormat.Format1bppIndexed:
                    return 1;

                case PixelFormat.Format24bppRgb:
                    return 24;

                case PixelFormat.Format48bppRgb:
                    return 48;

                case PixelFormat.Format4bppIndexed:
                    return 4;

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;

                case PixelFormat.Format8bppIndexed:
                    return 8;

                default:
                    return 1;

            }
        }

        #region IHostObject Members

        /// <summary>
        /// Get the host object header
        /// </summary>
        byte[] IHostObject.Header
        {
            get { return this._header; }
        }

        /// <summary>
        /// Get the length of host object
        /// </summary>
        long IHostObject.Length
        {
            get { return base.Length; }
        }

        /// <summary>
        /// Gets the current position of this stream.
        /// Setting the value will throw NotImplementedException
        /// </summary>
        long IHostObject.Position
        {
            get { return base.Position; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Read the next byte from host object
        /// </summary>
        int IHostObject.Read()
        {
            return base.Read();
        }

        /// <summary>
        /// Reads a block of bytes from the host object and writes the data in a given buffer.
        /// </summary>
        /// <param name="buffer">When this method returns, buffer contains the specified byte array with the values
        /// between offset and (offset + count - 1) replaced by the bytes read from the
        /// current source.</param>
        /// <param name="offset">The byte offset in array at which the read bytes will be placed.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns></returns>
        int IHostObject.Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, count);
        }

        /// <summary>
        /// Set the next frame in the buffer
        /// </summary>
        /// <param name="buffer">Next frame</param>
        void IHostObject.SetFrame(byte[] buffer)
        {
        }

        #endregion

        #region IDisposable Members

        public new void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}
