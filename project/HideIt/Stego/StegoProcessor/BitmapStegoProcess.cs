using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HideIt.Stego.HostObject;

namespace HideIt.Stego.StegoProcessor
{
    internal sealed class BitmapStegoProcessor : StegoProcessorBase, IDisposable
    {
        /// <summary>
        /// Create a new bitmap stego processor
        /// </summary>
        public BitmapStegoProcessor()
        {
            this.MType = HostMediaType.Bitmap;
        }

        public override double HidingCapacity
        {
            get
            {
                if (this.HostObject == null)
                {
                    return 0;
                }
                return (int)Math.Floor((decimal)(this.HostObject.Length - 8 - Message.MAX_KEY_LEN) / 8);
            }
            protected set { }
        }

        /// <summary>
        /// Load a host object
        /// </summary>
        /// <param name="path">Path of the host object</param>
        /// <param name="fetchFileInfo">Gather file information</param>
        public override void LoadHost(string path, bool fetchFileInfo)
        {
            ///Dispose off any previous cover that is being loaded
            if (this.HostObject != null)
            {
                this.HostObject.Dispose();
            }

            this.HostObject = new Bitmap(path);
        }

        /// <summary>
        /// Hide the message in cover object and write the stego object
        /// on given path
        /// </summary>
        /// <param name="message">Message to be hidden</param>
        /// <param name="sinkPath">Path at which stego object will be written</param>
        public override void Hide(Message message, string sinkPath)
        {
            if (this.HostObject == null)
            {
                throw new Exception("Cover object not loaded yet");
            }

            using (FileStream save = new FileStream(sinkPath, FileMode.Create, FileAccess.ReadWrite))
            {
                ///Copy the headers of cover object (bitmap)
                save.Write(this.HostObject.Header, 0, this.HostObject.Header.Length);

                ///Update the progress bar
                UpdateProgressBar(this.HostObject.Header.Length);

                while (true)
                {
                    ///Get a byte from message
                    int next = message.NextByte();

                    ///There is no more bytes in stream
                    if (next == -1)
                    {
                        break;
                    }

                    ///Hide it in cover object
                    this.HideData(save, (byte)next);

                    ///Update the progress bar.
                    UpdateProgressBar(8);
                }

                ///Data is hidden, now copy the remaining data from cover object to stego object
                this.CopyRemaining(save);

                ///Close the save stream
                save.Flush();
                save.Close();
            }
        }

        /// <summary>
        /// Extract the message from stego object
        /// </summary>        
        /// <param name="key">Key used to extract information</param>
        /// <returns>Message that have been extracted from stego object</returns>
        public override Message Extract(string key)
        {
            ///Transfer object is too small to contain any message.
            if (this.HostObject.Length < Bitmap.HEADER_SIZE)
            {
                return null;
            }

            return base.ExtractData(key);
        }

        /// <summary>
        /// Copy remaining data of cover object to save stream
        /// </summary>
        /// <param name="save">Stream to save stego object</param>
        private void CopyRemaining(Stream save)
        {
            int bufferSize = 1024;

            while (this.HostObject.Position < this.HostObject.Length)
            {
                byte[] buffer = null;

                ///If remaining bytes are less then 1024
                if ((this.HostObject.Length - this.HostObject.Position) < 1024)
                {
                    bufferSize = (int)(this.HostObject.Length - this.HostObject.Position);
                }

                ///Create a buffer
                buffer = new byte[bufferSize];

                ///Read from cover object into buffer
                this.HostObject.Read(buffer, 0, bufferSize);

                ///Write that buffer to stego object
                save.Write(buffer, 0, bufferSize);

                ///Update the progress bar
                UpdateProgressBar(bufferSize);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Free all resources associated with this process
        /// </summary>
        public override void Dispose()
        {
            this.UnloadHost();
        }

        #endregion
    }
}
