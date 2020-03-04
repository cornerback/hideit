using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectShowLib;
using System.Runtime.InteropServices;
//The .NET Framework enables interoperability with unmanaged code through platform invoke services, the System.Runtime.InteropServices namespace, and the CLR and through COM Interoperability (COM interop). 
using System.IO;
using HideIt.Stego.HostObject;
using DirectShowLib.DES;
using System.Drawing;

namespace HideIt.Stego.StegoProcessor
{
    /// <summary>
    /// Transform filter that implement ISampleGrabberCB interface
    /// </summary>
    /// <remarks>
    /// ISampleGrabberCB provides two methods, BufferCB and SampleCB. These
    /// methods are called by directshow.
    /// </remarks>
    internal class AviStegoProcessor : StegoProcessorBase, ISampleGrabberCB, IDisposable
    {
        private IGraphBuilder _graphBuilder;
        private ISampleGrabber _sampleGrabber;
        private IBaseFilter _aviSplitter;
        private IBaseFilter _aviMultiplexer;
        private IBaseFilter _rendererFilter;
        private IBaseFilter _fileReaderFilter;

        private IMediaControl _mediaCtrl;
        private IMediaEvent _mediaEvnt;

        private string _sourcePath;

        protected Message _message;
        protected string _key;

        protected ProcessingType _processing;

        /// <summary>
        /// Get extracted message
        /// </summary>
        internal Message ExtractedMessage { get; private set; }

        /// <summary>
        /// Type of processing to be done on frames
        /// </summary>
        protected enum ProcessingType
        {
            /// <summary>
            /// No action
            /// </summary>
            None,
            /// <summary>
            /// Hide data in frames
            /// </summary>
            Hide,
            /// <summary>
            /// Extract data from frames
            /// </summary>
            Extract,
            /// <summary>
            /// Processing done
            /// </summary>
            Done
        }

        /// <summary>
        /// Create a new instance of StagoTransformFilter
        /// </summary>        
        public AviStegoProcessor()
        {
            this.MType = HostMediaType.Video;
            this._processing = ProcessingType.None;
        }

        /// <summary>
        /// Get the length (in seconds) of video
        /// </summary>
        public virtual double Length { get; private set; }

        /// <summary>
        /// Get the width of video frames
        /// </summary>
        public virtual int Width { get; private set; }

        /// <summary>
        /// Get the height of video frames
        /// </summary>
        public virtual int Height { get; private set; }

        /// <summary>
        /// Get video frame rate
        /// </summary>
        public virtual double FrameRate { get; private set; }

        /// <summary>
        /// Get the hiding capacity of video (in bytes)
        /// </summary>
        public override double HidingCapacity { get; protected set; }

        /// <summary>
        /// Get total number of bytes used by frames in video
        /// </summary>
        private int TotalBytes { get; set; }

        /// <summary>
        /// Load a host object
        /// </summary>
        /// <param name="path">Path of the host object</param>
        /// <param name="fetchFileInfo">Gather file information</param>
        public override void LoadHost(string path, bool fetchFileInfo)
        {
            this._sourcePath = path;

            double videoLen = 0;
            if (fetchFileInfo)
            {
                ///Get basic file information
                Type t = Type.GetTypeFromCLSID(ComGuids.MediaDetGuid);
                IMediaDet mediaDet = (IMediaDet)Activator.CreateInstance(t);

                ///Set the avi file name for which we have to find information
                mediaDet.put_Filename(path);

                ///Check for the current stream. If it is audio, set it up for video
                AMMediaType mediaType = new AMMediaType();
                mediaDet.get_StreamMediaType(mediaType);

                ///If format type is not video
                if (mediaType.formatType != ComGuids.VideoFormatGuid)
                {
                    mediaDet.put_CurrentStream(1);
                    mediaDet.get_StreamMediaType(mediaType);
                }

                double prop = 0;
                ///Get media length
                mediaDet.get_StreamLength(out prop);
                this.Length = Math.Round(prop, 2);

                ///Get frame rate
                mediaDet.get_FrameRate(out prop);
                this.FrameRate = Math.Round(prop, 1);

                ///Get the dimensions
                VideoInfoHeader info = new VideoInfoHeader();
                Marshal.PtrToStructure(mediaType.formatPtr, info);

                this.Width = info.BmiHeader.Width;
                this.Height = info.BmiHeader.Height;

                ///Get absolute height
                if (this.Height < -1)
                {
                    this.Height *= -1;
                }

                ///Calculate the hiding capacity. Approximate number if bytes used by video frames in a video
                ///can be calculated by the size of each frame (width * heigth), multiplied by number of 
                ///frames it shows per seconds (frame rate) multiplied by length of video (seconds)
                ///multipled by number of bytes in each pixel (for RGB24 its 3 bytes).
                ///We divide by 8 to get number of bytes that can be stored in it.
                videoLen = (this.Width * this.Height * this.FrameRate * this.Length * 3);
                this.HidingCapacity = videoLen / 8;

                Marshal.ReleaseComObject(mediaDet);
            }

            this.HostObject = new Avi((int)videoLen);

            SetupProgressBar(0, (int)videoLen);
        }

        /// <summary>
        /// Hide the message in cover object and write the stego object
        /// on given path
        /// </summary>
        /// <param name="message">Message to be hidden</param>
        /// <param name="sinkPath">Path at which stego object will be written</param>
        public override void Hide(Message message, string sinkPath)
        {
            this._processing = ProcessingType.Hide;
            this._message = message;

            try
            {
                this.BuildGraph(sinkPath);
                this.Run();
            }
            catch (Exception)
            {
                this._processing = ProcessingType.Done;
                throw;
            }
        }

        /// <summary>
        /// Extract the message from stego object
        /// </summary>        
        /// <param name="key">Key used to extract information</param>
        /// <returns>Message that have been extracted from stego object</returns>
        public override Message Extract(string key)
        {
            this._processing = ProcessingType.Extract;
            this._key = key;
            try
            {
                this.BuildGraph(string.Empty);
                this.Run();
            }
            catch (Exception)
            {
                this._processing = ProcessingType.Done;
                throw;
            }

            return this.ExtractedMessage;
        }

        /// <summary>
        /// Run the graph and do desired processing
        /// </summary>
        protected virtual void Run()
        {
            ///Create a media control object to run the graph.
            this._mediaCtrl = this._graphBuilder as IMediaControl;
            ///Create media event object to get certain events from running graph
            this._mediaEvnt = this._graphBuilder as IMediaEvent;
            if (this._mediaCtrl != null)
            {
                ///Run the graph
                this._mediaCtrl.Run();
                if (this._mediaEvnt != null)
                {
                    EventCode code;
                    ///Waite until the graph completes.
                    this._mediaEvnt.WaitForCompletion(System.Threading.Timeout.Infinite, out code);
                }
            }
        }

        /// <summary>
        /// Stop the graph
        /// </summary>
        protected virtual void Stop()
        {
            if (this._mediaCtrl != null)
            {
                this._mediaCtrl.Stop();
            }
        }

        /// <summary>
        /// Build the graph and connect filters
        /// </summary>
        /// <param name="outputPath">Path of stego video file</param>
        private void BuildGraph(string outputPath)
        {
            ///Create a graph builder.
            this._graphBuilder = (IGraphBuilder)new FilterGraph();

            ///Create a sample grabber filter. Sample grabber filter provides callback
            ///for application to grab video frames on the fly. 
            this._sampleGrabber = (ISampleGrabber)new SampleGrabber();
            ///Configure the sample grabber
            this.ConfigureSampleGrabber(this._sampleGrabber, MediaSubType.RGB24);


            ///Create the file reader filter.
            this._fileReaderFilter = ComGuids.GetFilterFromGuid(ComGuids.FileReaderFilterGuid);
            ///Add the filter to graph. This function only adds filter to graph but
            ///do not connect it
            this._graphBuilder.AddFilter(this._fileReaderFilter, "File Source");

            ///Get source filter, this will let you load an AVI file
            IFileSourceFilter sourceFilter = this._fileReaderFilter as IFileSourceFilter;
            ///Load the file
            sourceFilter.Load(this._sourcePath, null);


            ///Create AVI splitter. This filter splits the AVI file into video and audio or other
            ///components that form the video (e.g text)
            this._aviSplitter = ComGuids.GetFilterFromGuid(ComGuids.AviSplitterGuid);
            ///Add the filter to graph
            this._graphBuilder.AddFilter(this._aviSplitter, "AVI Splitter");


            ///Add the sample grabber filter to graph
            this._graphBuilder.AddFilter((IBaseFilter)this._sampleGrabber, "SampleGrabber");


            ///Create AVI mux filter. This filter will combine the audio and video in one stream.
            ///We will use this filter to combine audio and video streams after hiding 
            ///data in ISampleGrabber's BufferCB callback.
            this._aviMultiplexer = ComGuids.GetFilterFromGuid(ComGuids.AviMultiplexerGuid);
            ///Add filter to graph
            this._graphBuilder.AddFilter(this._aviMultiplexer, "AVI multiplexer");

            ///We will add file writer, only then we will add file writer
            if (this._processing == ProcessingType.Hide)
            {
                ///Create a file writer filter. This filter will be used to write the AVI stream
                ///to a file
                this._rendererFilter = ComGuids.GetFilterFromGuid(ComGuids.FileWriterFilterGuid);

                ///Add renderer filter to graph
                this._graphBuilder.AddFilter(this._rendererFilter, "File writer");

                ///Get the file sink filter
                IFileSinkFilter sinkFilter = this._rendererFilter as IFileSinkFilter;
                ///Set the output file name
                sinkFilter.SetFileName(outputPath, null);
            }
            else
            {
                ///Create a null renderer
                this._rendererFilter = ComGuids.GetFilterFromGuid(ComGuids.NullRendererGuid);

                ///Add renderer filter to graph
                this._graphBuilder.AddFilter(this._rendererFilter, "Null renderer");
            }            

            DirectShowUtil.ConnectFilters(this._graphBuilder, this._fileReaderFilter, this._aviSplitter);
            DirectShowUtil.ConnectFilters(this._graphBuilder, this._aviSplitter, (IBaseFilter)this._sampleGrabber);
            DirectShowUtil.ConnectFilters(this._graphBuilder, (IBaseFilter)this._sampleGrabber, this._aviMultiplexer);
            DirectShowUtil.ConnectFilters(this._graphBuilder, this._aviMultiplexer, this._rendererFilter);
        }

        /// <summary>
        /// Configure sample grabber filter
        /// </summary>
        /// <param name="grabber">Instance of sample grabber</param>
        /// <param name="mediaSubType">Capturing media subtype</param>
        protected virtual void ConfigureSampleGrabber(ISampleGrabber grabber, Guid mediaSubType)
        {
            ///Create a media type struct. This will identify the
            ///type of media acceptable by this filter
            AMMediaType mediaType = new AMMediaType();

            ///The filter will accept video files
            mediaType.majorType = MediaType.Video;
                        
            mediaType.subType = mediaSubType;
            mediaType.formatType = FormatType.VideoInfo;

            grabber.SetMediaType(mediaType);

            DsUtils.FreeAMMediaType(mediaType);
            mediaType = null;

            ///As our class derives from ISampleGrabber, we will pass the 'this'
            ///instance to the filter. When a sample is grabbed by the filter
            ///it will call ISampleGrabber.BufferCB() method of this class.
            grabber.SetCallback(this, 1);
        }
        static int iter = 0;
        /// <summary>
        /// Hide data in buffer
        /// </summary>
        /// <param name="buffer">Buffer in which data is to be hidden</param>
        /// <param name="bufferLen">Length of buffer</param>
        protected virtual unsafe int HideData(byte* buffer, int bufferLen)
        {
            int offset = 0;
            
            while ((offset + 8) < bufferLen)
            {
                iter++;

                int next = this._message.NextByte();
                if (next == -1)
                {
                    this._processing = ProcessingType.Done;
                    ///No more data to hide
                    break;
                }
                byte data = (byte)next;

                ///Get 8 bytes of cover message and 
                ///hide a byte of data in it
                for (int i = 0; i < 8; i++)
                {
                    byte cover = buffer[offset];

                    ///Right shift the data to get next byte and AND with 1 = 00000001         
                    Substitution.LsbSubstitute(ref cover, (byte)(((data >> i) & 0x1)));

                    buffer[offset] = cover;
                    offset++;
                }
            }

            return offset;
        }

        /// <summary>
        /// Extract data from buffer
        /// </summary>
        /// <param name="buffer">Buffer from which data is to be extracted</param>
        /// <param name="bufferLen">Length of buffer</param>
        protected virtual void ExtractData(byte[] buffer, int bufferLen)
        {
            int keyLen = 0, messageLen = 0;

            string sKey = string.Empty;            

            this.HostObject.SetFrame(buffer);

            ///Read key length
            keyLen = this.ExtractLength(this.HostObject);

            ///This means that the host object does not contain valid key.
            if (keyLen < Message.MIN_KEY_LEN || keyLen > Message.MAX_KEY_LEN)
            {
                this._processing = ProcessingType.Done;
                return;
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
            if (sKey.CompareTo(this._key) != 0)
            {
                this._processing = ProcessingType.Done;
                return;
            }

            ///Now read the message length
            messageLen = this.ExtractLength(this.HostObject);

            if (messageLen == 0)
            {                
                this.ExtractedMessage = new Message(sKey, string.Empty);
                this._processing = ProcessingType.Done;
                return;
            }

            this.ExtractedMessage = new BinaryMessage(messageLen);
            this.MessageString(bufferLen);
        }

        /// <summary>
        /// Read message string from media
        /// </summary>
        /// <param name="bufferLen">Length of buffer</param>
        public virtual void MessageString(int bufferLen)
        {
            int offset = 0;
            while ((offset + 8) < bufferLen)
            {
                byte[] messageBytes = new byte[8];
                if (this.HostObject.Read(messageBytes, 0, messageBytes.Length) < messageBytes.Length)
                {
                    break;
                }

                byte byteEx = this.ExtractEx(messageBytes);
                if (byteEx == 0)
                {
                }
                this.ExtractedMessage.WriteByte(byteEx);                

                if (this.ExtractedMessage.TotalLength <= this.ExtractedMessage.CurrentLength)
                {
                    this._processing = ProcessingType.Done;
                    return;
                }

                offset += 8;
            }
        }

        #region ISampleGrabberCB Members
        /// <summary>
        /// Callback method that receives a pointer to the sample buffer.
        /// Here we use "unsafe" code because we want to directly access the bits of our sample, we cast the pBuffer argument to a pointer to a byte.        
        /// </summary>
        /// <param name="SampleTime">Starting time of the sample, in seconds.</param>
        /// <param name="pBuffer">Pointer to a buffer that contains the sample data.</param>
        /// <param name="BufferLen">Length of the buffer pointed to by pBuffer, in bytes.</param>
        /// <returns></returns>
        public unsafe int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            this.TotalBytes += BufferLen;

            if (this._processing == ProcessingType.None)
            {
                return 0;
            }

            UpdateProgressBar(BufferLen);

            if (this._processing == ProcessingType.Done)
            {
                return 0;
            }
            if (pBuffer == null || BufferLen == 0)
            {
                return 0;
            }

            if (this._processing == ProcessingType.Hide)
            {
                byte* buffer = (byte*)pBuffer;
                this.HideData(buffer, BufferLen);
            }
            else if (this._processing == ProcessingType.Extract)
            {
                byte[] buffer = new byte[BufferLen];
                Marshal.Copy(pBuffer, buffer, 0, BufferLen);

                if (this.ExtractedMessage == null)
                {
                    this.ExtractData(buffer, BufferLen);
                }
                else
                {
                    ///Set new frame
                    this.HostObject.SetFrame(buffer);
                    this.MessageString(BufferLen);
                }
            }

            return 0;
        }

        public int SampleCB(double SampleTime, IMediaSample pSample)
        {
            return 0;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Free all resources associated with this process
        /// </summary>
        public override void Dispose()
        {
            if (this._mediaCtrl != null)
            {
                this._mediaCtrl.StopWhenReady();
                Marshal.ReleaseComObject(this._mediaCtrl);
                this._mediaCtrl = null;
            }
            if (this._mediaEvnt != null)
            {
                Marshal.ReleaseComObject(this._mediaEvnt);
                this._mediaEvnt = null;
            }
            if (this._rendererFilter != null)
            {
                Marshal.ReleaseComObject(this._rendererFilter);
                this._rendererFilter = null;
            }
            if (this._fileReaderFilter != null)
            {
                Marshal.ReleaseComObject(this._fileReaderFilter);
                this._fileReaderFilter = null;
            }
            if (this._aviSplitter != null)
            {
                Marshal.ReleaseComObject(this._aviSplitter);
                this._aviSplitter = null;
            }
            if (this._aviMultiplexer != null)
            {
                Marshal.ReleaseComObject(this._aviMultiplexer);
                this._aviMultiplexer = null;
            }
            if (this._sampleGrabber != null)
            {
                Marshal.ReleaseComObject(this._sampleGrabber);
                this._sampleGrabber = null;
            }
            if (this._graphBuilder != null)
            {
                Marshal.ReleaseComObject(this._graphBuilder);
                this._graphBuilder = null;
            }
        }

        #endregion
    }
}
///The ISampleGrabber interface provides a way to peek into the stream
///of data and modify it "on the fly". This is a very powerful mechanism and,
///when the restrictions imposed by this interface are respected, is very easy
///to access from C#