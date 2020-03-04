using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectShowLib;
using DirectShowLib.DES;
using System.Runtime.InteropServices;

namespace HideIt
{
    public partial class AviDecompressor : Form, ISampleGrabberCB
    {
        private const string TITLE = "Avi Decompressor";

        private IGraphBuilder _graphBuilder;
        private ISampleGrabber _sampleGrabber;
        private IBaseFilter _aviSplitter;
        private IBaseFilter _aviMultiplexer;
        private IBaseFilter _rendererFilter;
        private IBaseFilter _fileReaderFilter;

        private IMediaControl _mediaCtrl;
        private IMediaEvent _mediaEvnt;

        private string _srcFile;
        private string _dcFile;
        private double _totalFrames;

        public AviDecompressor()
        {
            InitializeComponent();
         
            this.worker.DoWork += new DoWorkEventHandler(worker_DoWork);            
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                this.Cleanup();
            }
            catch (Exception) { }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ///Build the gragh
            this.BuildGraph();
            
            ///Run it
            this.Run();

            ///Cleanup resources
            this.Cleanup();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ///If any exception occured while running worker_DoWork method
            if (e.Error != null)
            {
                UIMessage.Error(e.Error.Message, TITLE);
            }
        }

        private void btn_browseAvi_Click(object sender, EventArgs e)
        {
            this._srcFile = AppUtil.SelectFile("Open video file", "AVI (*.avi)|*.avi", "avi");
            this.txtLoadAvi.Text = this._srcFile;
            
            try
            {
                if (this._srcFile != string.Empty)
                {
                    ///Get basic file information
                    Type t = Type.GetTypeFromCLSID(ComGuids.MediaDetGuid);
                    IMediaDet mediaDet = (IMediaDet)Activator.CreateInstance(t);

                    ///Set the avi file name for which we have to find information            
                    mediaDet.put_Filename(this._srcFile);

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
                    this._totalFrames = prop;

                    ///Get frame rate
                    mediaDet.get_FrameRate(out prop);
                    this._totalFrames = Math.Floor(this._totalFrames * prop);

                    AppUtil.SetupProgressBar(this.progress, 0, (int)this._totalFrames);
                }
            }
            catch (Exception exc)
            {
                UIMessage.Error(exc.Message, TITLE);
            }
        }

        private void btn_browseSaveAvi_Click(object sender, EventArgs e)
        {
            ///Open a save dialog and ask user to save the video file
            using (SaveFileDialog saveVideo = new SaveFileDialog())
            {
                saveVideo.AutoUpgradeEnabled = true;
                saveVideo.Title = "Save video file";
                saveVideo.Filter = "AVI (*.avi)|*.avi";

                if (saveVideo.ShowDialog() == DialogResult.OK)
                {
                    this._dcFile = saveVideo.FileName;
                    this.txtSaveAvi.Text = this._dcFile;
                }
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this._srcFile))
            {
                UIMessage.Info("Please select source AVI file for decompression.", TITLE);
                return;
            }

            if (string.IsNullOrEmpty(this._dcFile))
            {
                UIMessage.Info("Please specify save path for decompressed AVI file.", TITLE);
                return;
            }

            if (this.worker.IsBusy)
            {
                return;
            }

            this.worker.RunWorkerAsync();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Build the graph and connect filters
        /// </summary>
        private void BuildGraph()
        {
            ///Create a graph builder.
            this._graphBuilder = (IGraphBuilder)new FilterGraph();

            ///Create a sample grabber filter. Sample grabber filter provides callback
            ///for application to grab video frames on the fly. 
            this._sampleGrabber = (ISampleGrabber)new SampleGrabber();
            ///Configure the sample grabber
            this.ConfigureSampleGrabber(this._sampleGrabber);


            ///Create the file reader filter.
            this._fileReaderFilter = ComGuids.GetFilterFromGuid(ComGuids.FileReaderFilterGuid);
            ///Add the filter to graph. This function only adds filter to graph but
            ///do not connect it
            this._graphBuilder.AddFilter(this._fileReaderFilter, "File Source");

            ///Get source filter, this will let you load an AVI file
            IFileSourceFilter sourceFilter = this._fileReaderFilter as IFileSourceFilter;
            ///Load the file
            sourceFilter.Load(this._srcFile, null);
            
            
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


            ///Create a file writer filter. This filter will be used to write the AVI stream
            ///to a file
            this._rendererFilter = ComGuids.GetFilterFromGuid(ComGuids.FileWriterFilterGuid);

            ///Add renderer filter to graph
            this._graphBuilder.AddFilter(this._rendererFilter, "File writer");

            ///Get the file sink filter
            IFileSinkFilter sinkFilter = this._rendererFilter as IFileSinkFilter;
            ///Set the output file name
            sinkFilter.SetFileName(this._dcFile, null);
                        

            ///Connect the filter that are added to graph
            DirectShowUtil.ConnectFilters(this._graphBuilder, this._fileReaderFilter, this._aviSplitter);
            DirectShowUtil.ConnectFilters(this._graphBuilder, this._aviSplitter, (IBaseFilter)this._sampleGrabber);
            DirectShowUtil.ConnectFilters(this._graphBuilder, (IBaseFilter)this._sampleGrabber, this._aviMultiplexer);
            DirectShowUtil.ConnectFilters(this._graphBuilder, this._aviMultiplexer, this._rendererFilter);

            IEnumFilters enu = null;
            _graphBuilder.EnumFilters(out enu);

            if (enu != null)
            {
                IBaseFilter[] filters = new IBaseFilter[1];
                while (enu.Next(1, filters, IntPtr.Zero) == 0)
                {
                    FilterInfo info;
                    filters[0].QueryFilterInfo(out info);

                    if (info.achName.Equals("AVI multiplexer", StringComparison.OrdinalIgnoreCase))
                    {
                        IBaseFilter temp = filters[0];
                        IEnumPins penum;
                        IPin[] pins = new IPin[1];
                        temp.EnumPins(out penum);
                        while (penum.Next(1, pins, IntPtr.Zero) == 0)
                        {
                            PinDirection direction;
                            PinInfo pinfo;
                            FilterInfo finfo;
                            IEnumMediaTypes enuM;
                            AMMediaType[] types = new AMMediaType[1];
                            pins[0].QueryDirection(out direction);
                            pins[0].EnumMediaTypes(out enuM);
                            while (enuM.Next(1, types, IntPtr.Zero) == 0)
                            {
                            }
                            pins[0].QueryPinInfo(out pinfo);
                            IBaseFilter pinFilter = pinfo.filter;
                            pinFilter.QueryFilterInfo(out finfo);
                        }
                    }

                    string name = info.achName;
                }
            }
        }

        /// <summary>
        /// Run the graph
        /// </summary>
        private void Run()
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
        /// Configure sample grabber filter
        /// </summary>
        /// <param name="grabber">Instance of sample grabber</param>
        private void ConfigureSampleGrabber(ISampleGrabber grabber)
        {
            ///Create a media type struct. This will identify the
            ///type of media acceptable by this filter
            AMMediaType mediaType = new AMMediaType();

            ///The filter will accept video files
            mediaType.majorType = MediaType.Video;

            ///RGB24 as we are expecting decompressed frames
            mediaType.subType = MediaSubType.RGB24;
            mediaType.formatType = FormatType.VideoInfo;

            grabber.SetMediaType(mediaType);

            DsUtils.FreeAMMediaType(mediaType);
            mediaType = null;

            ///Callback ISampleGrabberCB.BufferCB method
            grabber.SetCallback(this, 1);
        }

        /// <summary>
        /// Release any resource used and reset settings for next operation
        /// </summary>
        private void Cleanup()
        {
            this._mediaEvnt = null;

            if (this._mediaCtrl != null)
            {
                this._mediaCtrl.StopWhenReady();
                this._mediaCtrl = null;
            }

            if (this._fileReaderFilter != null)
            {
                Marshal.ReleaseComObject(this._fileReaderFilter);
                this._fileReaderFilter = null;
            }

            if (this._aviMultiplexer != null)
            {
                Marshal.ReleaseComObject(this._aviMultiplexer);
                this._aviMultiplexer = null;
            }

            if (this._aviSplitter != null)
            {
                Marshal.ReleaseComObject(this._aviSplitter);
                this._aviSplitter = null;
            }

            if (this._rendererFilter != null)
            {
                Marshal.ReleaseComObject(this._rendererFilter);
                this._rendererFilter = null;
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

        #region ISampleGrabberCB Members

        public int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            AppUtil.IncrementProgressBar(this.progress, 1);
            return 0;
        }

        public int SampleCB(double SampleTime, IMediaSample pSample)
        {
            return 0;
        }

        #endregion
    }
}
