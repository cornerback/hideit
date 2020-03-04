using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HideIt;

namespace HideIt.Stego
{
    /// <summary>
    /// Secret message along with private key that will be used to hide data
    /// </summary>
    /// <remarks>The message along with key will be converted
    /// in stream the can be written to cover object</remarks>
    public class Message : IDisposable
    {
        public const int MAX_KEY_LEN = 256;
        public const int MIN_KEY_LEN = 4;

        private MemoryStream _stream;
        private StringBuilder _message;

        /// <summary>
        /// Create a new message
        /// </summary>
        /// <param name="actualLength">Total length of message</param>
        public Message(int totalLength)
        {
            this.TotalLength = totalLength;
        }

        /// <summary>
        /// Create a new message
        /// </summary>
        /// <param name="key">Private key used to hide message</param>
        /// <param name="message">The actual message to hide</param>
        public Message(string key, string message)
        {
            this.Key = key;
            this._message = new StringBuilder(message);
        }

        /// <summary>
        /// Total length of message hidden
        /// </summary>
        public int TotalLength { get; private set; }

        /// <summary>
        /// Current length of message
        /// </summary>
        public virtual int CurrentLength { get; private set; }
        
        /// <summary>
        /// Get the key used to hide message
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Get the secret message
        /// </summary>
        public virtual string SecertMessage
        {
            get { return this._message.ToString(); }
        }

        /// <summary>
        /// Write a byte to message stream
        /// </summary>
        /// <param name="b">Byte to write</param>
        public virtual void WriteByte(byte b)
        {
        }

        /// <summary>
        /// Return next byte of the secrete message
        /// </summary>
        /// <returns>Next byte of secret message</returns>
        public virtual int NextByte()
        {
            ///If the key and message are not already converted to byte stream
            if (this._stream == null)
            {
                string key = (this.Key == null ? string.Empty : this.Key);
                string message = (this._message == null ? string.Empty : this._message.ToString());

                this._stream = new MemoryStream();

                ///We now convert the lengths and strings to byte array.
                ///So they can be hidden in cover message

                ///At first we convert the length of key to byte array
                byte[] data = AppUtil.ToByteArray(key.Length);
                ///Write it in stream
                this._stream.Write(data, 0, data.Length);

                ///Convert the key to byte array. UTF8 encoding will
                ///preserve any special characters that fall in UTF8 range, so
                ///they can be reproduced when extracting message from cover
                data = Encoding.UTF8.GetBytes(key);
                ///Write it to stream
                this._stream.Write(data, 0, data.Length);

                ///Covert message to byte array. Again UTF8 encoding is used
                byte[] messageData = Encoding.UTF8.GetBytes(message);                

                ///Convert the length of message to byte array
                data = AppUtil.ToByteArray(messageData.Length);
                
                ///Write it in stream
                this._stream.Write(data, 0, data.Length);
                
                ///Write it to stream
                this._stream.Write(messageData, 0, messageData.Length);

                this._stream.Position = 0;
            }

            return this._stream.ReadByte();
        }

        /// <summary>
        /// Reset the message stream and start over again
        /// </summary>
        public virtual void Reset()
        {
            this.Dispose();
        }

        #region IDisposable Members

        /// <summary>
        /// Free all resources associated
        /// </summary>
        public void Dispose()
        {            
            if (this._stream != null)
            {
                this._stream.Close();
                this._stream.Dispose();
                this._stream = null;
            }
        }

        #endregion
    }
}
