using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideIt.Stego.HostObject;
using System.IO;

namespace HideIt.Stego.StegoProcessor
{
    internal sealed class WaveStegoProcessor : StegoProcessorBase, IDisposable
    {
        private Wave _waveHostObject = null;
        private Stream _sourceStream = null;

        /// <summary>
        /// Create a new wave stego processor
        /// </summary>
        public WaveStegoProcessor()
        {
            base.MType = HostMediaType.Wave;
        }

        public override double HidingCapacity
        {
            get
            {
                if (this._waveHostObject == null)
                {
                    return 0;
                }
                return (int)Math.Floor((decimal)((this._waveHostObject.CountSamples - 8 - Message.MAX_KEY_LEN) / 8));
            }
            protected set { }
        }

        public override void LoadHost(string path, bool fetchFileInfo)
        {
            if (base.HostObject != null)
            {
                base.HostObject.Dispose();
            }
            if (this._sourceStream != null)
            {
                this._sourceStream.Flush();
                this._sourceStream.Close();
                this._sourceStream.Dispose();
            }

            this._sourceStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            base.HostObject = this._waveHostObject = new Wave(this._sourceStream);
        }

        public override void Hide(Message message, string sinkPath)
        {
            if (this.HostObject == null)
            {
                throw new Exception("Cover object not loaded yet");
            }

            using (FileStream destination = new FileStream(sinkPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                ///Check if the headers are already read, if not then seek
                ///to data part
                if (this._waveHostObject.Header == null ||
                    (this._waveHostObject.Header != null &&
                    this._waveHostObject.Position < this._waveHostObject.Header.Length))
                {
                    this._waveHostObject.Position = this._waveHostObject.Header.Length;
                }

                ///Copy the header to destination stream first
                destination.Write(this._waveHostObject.Header, 0, this._waveHostObject.Header.Length);

                ///Update the progress bar
                UpdateProgressBar(this.HostObject.Header.Length);

                ///Lets proceed with hiding process
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
                    this.HideData(destination, (byte)next);

                    ///Update the progress bar.
                    UpdateProgressBar(8);
                }

                ///Data is hidden, now copy the remaining data from cover object to stego object
                this.CopyRemaining(destination);


                ///Close the save stream
                destination.Flush();
                destination.Close();
            }
        }

        public override Message Extract(string key)
        {
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

        void IDisposable.Dispose()
        {
            if (base.HostObject != null)
            {
                base.HostObject.Dispose();
                base.HostObject = null;
                this._waveHostObject = null;
            }
            if (this._sourceStream != null)
            {
                this._sourceStream.Flush();
                this._sourceStream.Close();
                this._sourceStream.Dispose();
                this._sourceStream = null;
            }
        }

        #endregion
    }
}
