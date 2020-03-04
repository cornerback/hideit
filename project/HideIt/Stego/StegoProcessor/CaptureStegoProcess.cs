using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectShowLib;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace HideIt.Stego.StegoProcessor
{
    internal sealed class CaptureStegoProcess : AviStegoProcessor, ISampleGrabberCB, IDisposable
    {
        private IGraphBuilder _graphBuilder;
        private ICaptureGraphBuilder2 _captureBuilder;
        private ISampleGrabber _sampleGrabber;

        private IBaseFilter _captureFilter;
        private IBaseFilter _mux;

        private IMediaControl _mediaCtrl;
        private IMediaEvent _mediaEvent;

        private bool _stopWhenComplete;

        private delegate void StopCapture();
        private StopCapture OnStopCapture;

        /// <summary>
        /// Get instance of IVideoWindow to show video on a control
        /// </summary>
        private IVideoWindow _videoWindow;

        public CaptureStegoProcess(IMoniker moniker)
        {
            Guid baseFilterGuid = typeof(IBaseFilter).GUID;
            object o;

            ///Get our device ready
            moniker.BindToObject(null, null, ref baseFilterGuid, out o);
            this._captureFilter = (IBaseFilter)o;

            this._graphBuilder = (IGraphBuilder)new FilterGraph();

            this._captureBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();

            ///Tell capture graph about filter graph builder
            this._captureBuilder.SetFiltergraph(this._graphBuilder);

            this._mediaCtrl = (IMediaControl)this._graphBuilder;
            this._mediaEvent = (IMediaEvent)this._graphBuilder;
            this._videoWindow = (IVideoWindow)this._graphBuilder;

            ///Code for stoping capturing
            this.OnStopCapture = delegate()
            {
                if (this._mediaCtrl != null)
                {
                    int hr = this._mediaCtrl.StopWhenReady();
                }
                base._processing = ProcessingType.Done;
                this._videoWindow.put_Visible(OABool.False);
            };
        }

        /// <summary>
        /// Setup live capturing and previewing
        /// </summary>
        /// <param name="sinkPath">Output file path for captured video</param>
        /// <param name="videoOwner">Video preview control</param>
        public void Setup(string sinkPath, Control videoOwner)
        {
            this.BuildGraph(sinkPath);

            int hr = this._videoWindow.put_Owner(videoOwner.Handle);
            this._videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);

            Rectangle rc = videoOwner.ClientRectangle;
            this._videoWindow.SetWindowPosition(0, 0, rc.Right, rc.Bottom);

            this._videoWindow.put_Visible(OABool.True);
        }

        /// <summary>
        /// Build a graph for capturing video
        /// </summary>
        /// <param name="sinkPath">Save path for the captured video</param>
        private void BuildGraph(string sinkPath)
        {
            ///Add the capture filter in graph
            this._graphBuilder.AddFilter(this._captureFilter, "Video Capture");

            IFileSinkFilter sink = null;
            int hr = this._captureBuilder.SetOutputFileName(MediaSubType.Avi, sinkPath, out this._mux, out sink);

            ///Create a sample grabber filter. Sample grabber filter provides callback
            ///for application to grab video frames on the fly. 
            this._sampleGrabber = (ISampleGrabber)new SampleGrabber();
            ///Configure the sample grabber
            base.ConfigureSampleGrabber(this._sampleGrabber, MediaSubType.RGB24);

            this._graphBuilder.AddFilter((IBaseFilter)this._sampleGrabber, "SampleGrabber");

            ///Stream to file
            hr = this._captureBuilder.RenderStream(PinCategory.Capture, MediaType.Video, this._captureFilter, (IBaseFilter)this._sampleGrabber, this._mux);

            ///preview window
            hr = this._captureBuilder.RenderStream(PinCategory.Preview, MediaType.Video, this._captureFilter, null, null);
        }

        public void StartCapturing(Message message, bool stopWhenComplete)
        {
            base._processing = ProcessingType.Hide;
            base._message = message;
            this._stopWhenComplete = stopWhenComplete;

            SetupProgressBar(0, message.Key.Length + message.SecertMessage.Length);

            try
            {
                if (this._mediaCtrl != null)
                {
                    int hr = this._mediaCtrl.Run();
                }
            }
            catch (Exception)
            {
                base._processing = ProcessingType.Done;
                throw;
            }
        }

        public void StopCapturing()
        {
            if (this._mediaCtrl != null)
            {
                int hr = this._mediaCtrl.Stop();
            }
            base._processing = ProcessingType.Done;
            this._videoWindow.put_Visible(OABool.False);
        }

        public void PauseCapturing()
        {
            if (this._mediaCtrl != null)
            {
                this._mediaCtrl.Pause();
            }
        }

        #region ISampleGrabberCB Members

        public new unsafe int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            if (base._processing == ProcessingType.None)
            {
                return 0;
            }            

            if (base._processing == ProcessingType.Done)
            {
                return 0;
            }
            if (pBuffer == null || BufferLen == 0)
            {
                return 0;
            }

            if (base._processing == ProcessingType.Hide)
            {
                byte* buffer = (byte*)pBuffer;
                int bytesHidden = base.HideData(buffer, BufferLen);

                UpdateProgressBar(bytesHidden);

                ///when we are done hiding
                if (this._stopWhenComplete && base._processing == ProcessingType.Done)
                {
                    this.OnStopCapture.BeginInvoke(null, null);
                }
            }
            return 0;
        }

        #endregion

        #region IDisposable Members

        public new void Dispose()
        {
            if (this._mediaCtrl != null)
            {
                this._mediaCtrl = null;
            }

            if (this._captureFilter != null)
            {
                Marshal.ReleaseComObject(this._captureFilter);
                this._captureFilter = null;
            }

            if (this._captureBuilder != null)
            {
                Marshal.ReleaseComObject(this._captureBuilder);
                this._captureBuilder = null;
            }

            if (this._sampleGrabber != null)
            {
                Marshal.ReleaseComObject(this._sampleGrabber);
                this._sampleGrabber = null;
            }

            if (this._mux != null)
            {
                Marshal.ReleaseComObject(this._mux);
                this._mux = null;
            }

            if (this._message != null)
            {
                this._message.Dispose();
                this._message = null;
            }
        }

        #endregion
    }
}
