using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using HideIt.Stego.HostObject;

namespace HideIt.Stego.StegoProcessor
{
    /// <summary>
    /// This class provides methods to hide and extract secret message from cover object
    /// </summary>
    internal abstract class StegoProcessorBase : IDisposable
    {
        /// <summary>
        /// Factory method to get the stego processor for the type of file chosen
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>Stego processor to process hide/extract operation</returns>
        public static StegoProcessorBase GetProcessor(string path, bool watermark)
        {
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }
            if(Path.GetExtension(path).Equals(".bmp", StringComparison.OrdinalIgnoreCase))
            {
                return new BitmapStegoProcessor();
            }
            else if(Path.GetExtension(path).Equals(".avi", StringComparison.OrdinalIgnoreCase))
            {
                return (watermark ? new WatermarkingStegoProcessor() : new AviStegoProcessor());
            }
            else if (Path.GetExtension(path).Equals(".wav", StringComparison.OrdinalIgnoreCase))
            {
                return new WaveStegoProcessor();
            }
            else
            {
                throw new Exception("Unknow cover/stego object selected");
            }
        }

        /// <summary>
        /// Update the progress bar
        /// </summary>
        public static HideIt.HideItUI.UpdateProgressBarCallback UpdateProgressBar { get; set; }

        /// <summary>
        /// Setup the progress bar
        /// </summary>
        public static HideIt.HideItUI.SetupProgressBarCallback SetupProgressBar { get; set; }
            
        /// <summary>
        /// Type of media in which the stego process is running
        /// </summary>
        public HostMediaType MType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Get the size of cover (bytes)
        /// </summary>
        public virtual int HostSize
        {
            ///If cover message is not loaded yet, then return 0, otherwise 
            ///return the length
            get { return (this.HostObject == null ? 0 : (int)this.HostObject.Length); }
        }

        /// <summary>
        /// Get the object that will contain the message
        /// </summary>
        public IHostObject HostObject
        {
            get;
            protected set;
        }

        /// <summary>
        /// Load a host object
        /// </summary>
        /// <param name="path">Path of the host object</param>
        /// <param name="fetchFileInfo">Gather file information</param>
        public abstract void LoadHost(string path, bool fetchFileInfo);

        /// <summary>
        /// Unload the host and free up all resources used by it
        /// </summary>
        protected void UnloadHost()
        {
            if (this.HostObject != null)
            {
                this.HostObject.Dispose();
                this.HostObject = null;
            }
        }

        /// <summary>
        /// Hide the message in cover object and write the stego object
        /// on given path
        /// </summary>
        /// <param name="message">Message to be hidden</param>
        /// <param name="sinkPath">Path at which stego object will be written</param>
        public abstract void Hide(Message message, string sinkPath);

        /// <summary>
        /// Extract the message from stego object
        /// </summary>        
        /// <param name="key">Key used to extract information</param>
        /// <returns>Message that have been extracted from stego object</returns>
        public abstract Message Extract(string key);

        /// <summary>
        /// Get the hiding capacity of video (in bytes)
        /// </summary>
        public virtual double HidingCapacity { get; protected set; }

        /// <summary>
        /// The secret message to be hidden in cover object
        /// </summary>
        /// <param name="save">Stream in which stego object is saved</param>
        ///<param name="data">Data to hide</param>
        ///<returns>Return the number of bytes saved in data. 0 means
        ///the hiding is complete</returns>
        protected int HideData(Stream save, byte data)
        {
            int i = 0;
            ///Get 8 bytes of cover message and 
            ///hide a byte of data in it
            for (; i < 8; i++)
            {
                int next = this.HostObject.Read();                
                ///No more cover bytes to save data in
                if (next == -1)
                {
                    return i;
                }

                byte cover = (byte)next;

                ///Right shift the data to get next byte and AND with 1 = 00000001         
                Substitution.LsbSubstitute(ref cover, (byte)(((data >> i) & 0x1)));

                save.WriteByte(cover);
            }
            return i;
        }

        /// <summary>
        /// Extract the message from stego object
        /// </summary>        
        /// <param name="key">Key used to extract information</param>
        /// <returns>Message that have been extracted from stego object</returns>
        protected Message ExtractData(string key)
        {
            ///Size of stego object too small to contain key information
            if (this.HostObject.Length - this.HostObject.Position < 32)
            {
                return null;
            }

            int keyLen = 0, messageLen = 0;

            string sKey = string.Empty;
            string message = string.Empty;

            ///Read key length
            keyLen = this.ExtractLength(this.HostObject);

            ///This means that the host object does not contain valid key.
            if (keyLen < Message.MIN_KEY_LEN || keyLen > Message.MAX_KEY_LEN)
            {
                return null;
            }

            ///Size of stego object too small to contain key
            if (this.HostObject.Length - this.HostObject.Position < (keyLen * 8))
            {
                return null;
            }

            ///Prepare a buffer to read key
            byte[] data = new byte[keyLen];

            for (int i = 0; i < keyLen; i++)
            {
                byte[] keyBytes = new byte[8];
                this.HostObject.Read(keyBytes, 0, keyBytes.Length);

                data[i] = this.ExtractEx(keyBytes);
            }

            ///Convert bytes to key string
            sKey = Encoding.UTF8.GetString(data);

            ///Keys do not compare
            if (sKey.CompareTo(key) != 0)
            {
                return null;
            }

            ///Size of stego object too small to contain message information
            if (this.HostObject.Length - this.HostObject.Position < 32)
            {
                return null;
            }

            ///Now read the message length
            messageLen = this.ExtractLength(this.HostObject);

            ///Size of stego object too small to contain message
            if (this.HostObject.Length - this.HostObject.Position < (messageLen * 8))
            {
                return null;
            }

            ///At this point we will prepare the progress object that will keep
            ///the calling method informed about the current progress
            SetupProgressBar(0, messageLen);

            if (messageLen == 0)
            {
                UpdateProgressBar(1);
                return new Message(sKey, string.Empty);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                for (int i = 0; i < messageLen; i++)
                {
                    byte[] messageBytes = new byte[8];
                    this.HostObject.Read(messageBytes, 0, messageBytes.Length);

                    stream.WriteByte(this.ExtractEx(messageBytes));
                    UpdateProgressBar(1);
                }

                message = Encoding.UTF8.GetString(stream.ToArray());
                stream.Close();
            }

            return new Message(sKey, message);
        }

        /// <summary>
        /// Extract length from host object
        /// </summary>
        /// <param name="stegoObject"></param>
        /// <returns></returns>
        protected int ExtractLength(IHostObject hostObject)
        {
            ///32 bytes of stego object will contain 4 bytes of legth information.
            ///We will read it byte by byte
            byte[] lengthInfo = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                byte[] lengthBytes = new byte[8];
                hostObject.Read(lengthBytes, 0, lengthBytes.Length);

                ///The first byte in this array will contain LSB information. We will have to
                ///read MSB first
                for (int j = lengthBytes.Length; j > 0; j--)
                {
                    ///Shift one bit to right side so the next bit can be set
                    lengthInfo[i] <<= 0x1;
                    lengthInfo[i] |= Substitution.LsbValue(lengthBytes[j - 1]);                    
                }
            }

            return AppUtil.FromByteArray(lengthInfo);
        }

        /// <summary>
        /// Extracts a byte from data
        /// </summary>
        /// <param name="data">Data containg bits</param>
        /// <returns>The byte hidden in data</returns>
        protected byte ExtractEx(byte[] data)
        {
            byte info = 0;
            
            ///We will read the LSB value from data and make it part of info
            ///As we hide LSB of message byte first therefore we start the loop from
            ///then i.e. end of the array to read MSB and then right shift it to store 
            ///the next bit
            for (int i = data.Length; i > 0; i--)
            {
                ///Shift one bit to right side so the next bit can be set
                info <<= 0x1;
                info |= Substitution.LsbValue(data[i - 1]);                
            }

            return info;
        }

        #region IDisposable Members

        /// <summary>
        /// Free all resources associated with this process
        /// </summary>
        public virtual void Dispose()
        {
            
        }

        #endregion
    }
}
