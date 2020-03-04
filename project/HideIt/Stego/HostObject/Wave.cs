using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HideIt.Audio;

namespace HideIt.Stego.HostObject
{
    internal sealed class Wave : Stream, IDisposable, IHostObject
    {
        private Stream m_Stream;
        private byte[] _header;
        private long m_DataPos;
        private int m_Length;

        private WaveFormat m_Format;

        public WaveFormat Format
        {
            get { return m_Format; }
        }

        private string ReadChunk(BinaryReader reader)
        {
            byte[] ch = new byte[4];
            reader.Read(ch, 0, ch.Length);
            return System.Text.Encoding.UTF8.GetString(ch);
        }

        /// <summary>ReadChunk(reader) - Changed to CopyChunk(reader, writer)</summary>
        /// <param name="reader">source stream</param>
        /// <returns>four characters</returns>
        private string CopyChunk(BinaryReader reader, BinaryWriter writer)
        {
            byte[] ch = new byte[4];
            reader.Read(ch, 0, ch.Length);

            //copy the chunk
            writer.Write(ch);

            return System.Text.Encoding.UTF8.GetString(ch);///return System.Text.Encoding.ASCII.GetString(ch);
        }

        /// <summary>ReadHeader() - Changed to CopyHeader(destination)</summary>
        public void CopyHeader(Stream destinationStream)
        {
            BinaryReader reader = new BinaryReader(m_Stream);
            BinaryWriter writer = new BinaryWriter(destinationStream);

            if (CopyChunk(reader, writer) != "RIFF")
                throw new Exception("Invalid file format");

            writer.Write(reader.ReadInt32()); // File length minus first 8 bytes of RIFF description

            if (CopyChunk(reader, writer) != "WAVE")
                throw new Exception("Invalid file format");

            if (CopyChunk(reader, writer) != "fmt ")
                throw new Exception("Invalid file format");

            int len = reader.ReadInt32();
            if (len < 16)
            { // bad format chunk length
                throw new Exception("Invalid file format");
            }
            else
            {
                writer.Write(len);
            }

            m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
            m_Format.wFormatTag = reader.ReadInt16();
            m_Format.nChannels = reader.ReadInt16();
            m_Format.nSamplesPerSec = reader.ReadInt32();
            m_Format.nAvgBytesPerSec = reader.ReadInt32();
            m_Format.nBlockAlign = reader.ReadInt16();
            m_Format.wBitsPerSample = reader.ReadInt16();

            //copy format information
            writer.Write(m_Format.wFormatTag);
            writer.Write(m_Format.nChannels);
            writer.Write(m_Format.nSamplesPerSec);
            writer.Write(m_Format.nAvgBytesPerSec);
            writer.Write(m_Format.nBlockAlign);
            writer.Write(m_Format.wBitsPerSample);
            
            // advance in the stream to skip the wave format block 
            len -= 16; // minimum format size
            writer.Write(reader.ReadBytes(len));
            len = 0;

            // assume the data chunk is aligned
            while (m_Stream.Position < m_Stream.Length && CopyChunk(reader, writer) != "data")
                ;

            if (m_Stream.Position >= m_Stream.Length)
                throw new Exception("Invalid file format");

            m_Length = reader.ReadInt32();
            writer.Write(m_Length);

            m_DataPos = m_Stream.Position;
            Position = 0;
        }

        public Wave(Stream sourceStream)
        {
            m_Stream = sourceStream;
            using (MemoryStream stream = new MemoryStream())
            {
                this.CopyHeader(stream);
                this._header = stream.ToArray();
            }
            //ReadHeader();
        }

        ~Wave()
        {
            Dispose();
        }

        public new void Dispose()
        {
            if (m_Stream != null)
                m_Stream.Close();
            GC.SuppressFinalize(this);
        }

        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return m_Length; }
        }

        /// <summary>Length of the data (in samples)</summary>
        public long CountSamples
        {
            get
            {
                int bits = m_Format.wBitsPerSample;
                if (bits <= 0)
                {
                    bits = 8;
                }
                return (long)((m_Length - m_DataPos) / (bits / 8));
            }
        }

        public override long Position
        {
            get { return m_Stream.Position - m_DataPos; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public override void Close()
        {
            Dispose();
        }

        public override void Flush()
        {
        }

        public override void SetLength(long len)
        {
            throw new InvalidOperationException();
        }

        public override long Seek(long pos, SeekOrigin o)
        {
            switch (o)
            {
                case SeekOrigin.Begin:
                    m_Stream.Position = pos + m_DataPos;
                    break;
                case SeekOrigin.Current:
                    m_Stream.Seek(pos, SeekOrigin.Current);
                    break;
                case SeekOrigin.End:
                    m_Stream.Position = m_DataPos + m_Length - pos;
                    break;
            }
            return this.Position;
        }

        public override int Read(byte[] buf, int ofs, int count)
        {
            int toread = (int)Math.Min(count, m_Length - Position);
            return m_Stream.Read(buf, ofs, toread);
        }

        /// <summary>Read - Changed to Copy</summary>
        /// <param name="buf">Buffer to receive the data</param>
        /// <param name="ofs">Offset</param>
        /// <param name="count">Count of bytes to read</param>
        /// <param name="destination">Where to copy the buffer</param>
        /// <returns>Count of bytes actually read</returns>
        public int Copy(byte[] buf, int ofs, int count, Stream destination)
        {
            int toread = (int)Math.Min(count, m_Length - Position);
            int read = m_Stream.Read(buf, ofs, toread);
            destination.Write(buf, ofs, read);

            if (m_Stream.Position != destination.Position)
            {
                Console.WriteLine();
            }

            return read;
        }

        public override void Write(byte[] buf, int ofs, int count)
        {
            throw new InvalidOperationException();
        }

        #region IHostObject Members

        public byte[] Header
        {
            get { return this._header; }
        }

        public int Read()
        {
            byte[] next = new byte[1];
            this.Read(next, 0, 1);
            return (int)next[0];
        }

        public void SetFrame(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
