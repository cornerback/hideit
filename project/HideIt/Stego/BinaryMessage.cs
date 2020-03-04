using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HideIt.Stego
{
    public class BinaryMessage : Message, IDisposable
    {
        private MemoryStream _messageStream;

        /// <summary>
        /// Create a new binary message
        /// </summary>
        /// <param name="totalLength">Total length of message</param>
        public BinaryMessage(int totalLength)
            : base(totalLength)
        {
            this._messageStream = new MemoryStream(totalLength);
        }

        /// <summary>
        /// Current length of message
        /// </summary>
        public override int CurrentLength
        {
            get
            {
                int currentLen = 0;
                if (this._messageStream != null)
                {
                    currentLen = (int)this._messageStream.Length;
                }
                return currentLen;
            }
        }

        /// <summary>
        /// Get the secret message
        /// </summary>
        public override string SecertMessage
        {
            get 
            {
                string message = string.Empty;
                if (this._messageStream != null)
                {
                    message = Encoding.UTF8.GetString(this._messageStream.GetBuffer());
                }
                return message;
            }
        }

        /// <summary>
        /// Write a byte to message stream
        /// </summary>
        /// <param name="b">Byte to write</param>
        public override void WriteByte(byte b)
        {
            if (this._messageStream != null)
            {
                this._messageStream.WriteByte(b);
            }
        }

        #region IDisposable Members

        public new void Dispose()
        {
            if (this._messageStream != null)
            {
                this._messageStream.Close();
                this._messageStream.Dispose();
                this._messageStream = null;
            }
        }

        #endregion
    }
}
