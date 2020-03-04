using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectShowLib;

namespace HideIt.Stego.StegoProcessor
{	
    // ISampleGrabber interface
    // This interface provides methods for retrieving individual
    // media samples as they move through the filter graph
    
    internal sealed class WatermarkingStegoProcessor : AviStegoProcessor, ISampleGrabberCB, IDisposable
    {
        /// <summary>
        /// Hide data in buffer
        /// </summary>
        /// <param name="buffer">Buffer in which data is to be hidden</param>
        /// <param name="bufferLen">Length of buffer</param>
        protected override unsafe int HideData(byte* buffer, int bufferLen)
        {
            int ret = base.HideData(buffer, bufferLen);
            ///Make sure that you keep on hiding data until the last frame
            this._processing = ProcessingType.Hide;
            this._message.Reset();

            return ret;
        }

        #region ISampleGrabberCB Members
        /// <summary>
        /// Callback method that receives a pointer to the sample buffer.
        /// </summary>
        /// <param name="SampleTime">Starting time of the sample, in seconds.</param>
        /// <param name="pBuffer">Pointer to a buffer that contains the sample data.</param>
        /// <param name="BufferLen">Length of the buffer pointed to by pBuffer, in bytes.</param>
        /// <returns></returns>
        public new int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            return base.BufferCB(SampleTime, pBuffer, BufferLen);
        }
        /// <summary>
        /// SampleCB is a Callback method that receives a pointer to the media sample		
        /// SampleGrabber callback SampleCB receives samples
        /// </summary>
        /// <param name="SampleTime">SampleTime-Starting time of the sample, in seconds.</param>
        /// <param name="pSample">pSample Pointer to the IMediaSample interface of the sample.</param>
        /// <returns></returns>
        public new int SampleCB(double SampleTime, IMediaSample pSample)
        {
            return base.SampleCB(SampleTime, pSample);
        }
        #endregion
    }
}
