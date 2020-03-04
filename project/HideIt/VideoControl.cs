using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using DirectShowLib;
using System.Threading;
using System.IO;
using DirectShowLib.DES;

namespace HideIt
{
    public partial class VideoControl : UserControl
    {
        private const string TITLE = "Compare Video";

        private VideoControl _source;

        // the needed interfaces
        private IGraphBuilder _graphBuilder = null;
        private IMediaControl _mediaControl = null;
        private IMediaEventEx _mediaEvent = null;
        private IVideoWindow _vidWindow = null;
        private IMediaPosition _mediaPosition = null;

        public event EventHandler OnPlay = delegate { };
        public event EventHandler OnStop = delegate { };
        public event EventHandler OnPause = delegate { };

        /// <summary>
        /// Get or set a value specifying whether media controls buttons will
        /// be enabled or not.
        /// </summary>
        private bool _enableControls = true;

        /// <summary>
        /// Get video file name
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public VideoControl(VideoControl source)
        {
            InitializeComponent();

            this._source = source;
            this.EnableButtons(false);

            if (this._source != null)
            {
                ///enable the media control buttons
                this._enableControls = false;
                this.HideControl(true);

                ///Register events with source
                this._source.OnStop += new EventHandler(_source_OnStop);
                this._source.OnPause += new EventHandler(_source_OnPause);
                this._source.OnPlay += new EventHandler(_source_OnPlay);
            }

            this.worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        /// <summary>
        /// Called when source video is played
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _source_OnPlay(object sender, EventArgs e)
        {
            this.btnPlay_Click(sender, e);
        }

        /// <summary>
        /// Called when source video is paused
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _source_OnPause(object sender, EventArgs e)
        {
            this.btnPause_Click(sender, e);
        }

        /// <summary>
        /// Called when source video is stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _source_OnStop(object sender, EventArgs e)
        {
            this.btnStop_Click(sender, e);
        }

        /// <summary>
        /// Called to start background workers job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            EventCode code;
            this._mediaEvent.WaitForCompletion(Timeout.Infinite, out code);
            e.Result = code;
        }

        /// <summary>
        /// The background worker has completed its job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EventCode code = (EventCode)e.Result;
            if (code == EventCode.Complete)
            {
                this._mediaPosition.put_CurrentPosition(0.0);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.FileName = AppUtil.SelectFile("Open video file", "AVI (*.avi)|*.avi", "avi");
            if (this.FileName != string.Empty)
            {
                try
                {
                    this.CloseInterfaces();
                    this.PopulateMediaInformation();
                    this.BuildGraph();

                    ///Get file name from path
                    this.lbl_movieName.Text = Path.GetFileName(this.FileName);
                }
                catch (Exception ex) 
                {
                    UIMessage.Error(ex.Message, TITLE);
                }
            }
        }

        internal void btnPlay_Click(object sender, EventArgs e)
        {
            if (this._mediaControl == null)
            {
                return;
            }

            this._mediaControl.Run();
            this.OnPlay(null, null);

            if (!this.worker.IsBusy)
            {
                this.worker.RunWorkerAsync();
            }
        }

        internal void btnPause_Click(object sender, EventArgs e)
        {
            if (this._mediaControl == null)
            {
                return;
            }

            this._mediaControl.Pause();
            this.OnPause(null, null);
        }

        internal void btnStop_Click(object sender, EventArgs e)
        {
            if (this._mediaControl == null)
            {
                return;
            }

            this._mediaControl.Pause();
            this.OnStop(null, null);
            
            this.Progress(0.0);            
            
            this._mediaPosition.put_CurrentPosition(0.0);
            this._mediaControl.StopWhenReady();
        }

        /// <summary>
        /// Show media information like frames per seconds etc
        /// </summary>
        private void PopulateMediaInformation()
        {
            ///Get basic file information
            Type t = Type.GetTypeFromCLSID(ComGuids.MediaDetGuid);
            IMediaDet mediaDet = (IMediaDet)Activator.CreateInstance(t);

            ///Set the avi file name for which we have to find information            
            mediaDet.put_Filename(this.FileName);

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
            this.lb_val_len.Text = string.Format("{0} sec", Math.Round(prop, 2));

            ///Get frame rate
            mediaDet.get_FrameRate(out prop);
            this.lb_val_fps.Text = Math.Round(prop, 1).ToString();

            ///Get the dimensions
            VideoInfoHeader info = new VideoInfoHeader();
            Marshal.PtrToStructure(mediaType.formatPtr, info);

            int width = info.BmiHeader.Width;
            int height = info.BmiHeader.Height;

            ///Get absolute height
            if (height < -1)
            {
                height *= -1;
            }

            this.lb_val_dimension.Text = string.Format("{0} x {1}", width, height);

            Marshal.ReleaseComObject(mediaDet);
        }

        /// <summary>
        /// Build the graph to play the file
        /// </summary>
        private void BuildGraph()
        {
            // get the interfaces needed
            this._graphBuilder = (IGraphBuilder)new FilterGraph();
            this._mediaControl = (IMediaControl)this._graphBuilder;
            this._mediaEvent = (IMediaEventEx)this._graphBuilder;
            this._mediaPosition = (IMediaPosition)this._graphBuilder;

            // use Intelligent Connect for the rest
            this._graphBuilder.RenderFile(this.FileName, null);

            // and the window
            this._vidWindow = (IVideoWindow)this._graphBuilder;
            this._vidWindow.put_Owner((IntPtr)this.Handle);
            this._vidWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings | WindowStyle.ClipChildren);

            Rectangle rc = this.vidOwner.ClientRectangle;
            this._vidWindow.SetWindowPosition(0, 0, rc.Right, rc.Bottom);

            double duration = 0.0;
            this._mediaPosition.get_Duration(out duration);

            this.SetupProgressBar(duration);

            this.EnableButtons(this._enableControls);            
        }

        /// <summary>
        /// Close all interfaces
        /// </summary>
        private void CloseInterfaces()
        {
            try
            {
                if (this._mediaControl != null)
                {
                    this._mediaControl.StopWhenReady();
                    this._mediaControl = null;
                }
            }
            catch { }

            if (this._mediaEvent != null) this._mediaEvent = null;
            if (this._vidWindow != null) this._vidWindow = null;
            if (this._mediaPosition != null) this._mediaPosition = null;

            try
            {
                if (this._graphBuilder != null)
                {
                    try
                    {
                        Marshal.ReleaseComObject(this._graphBuilder);///error
                    }
                    catch
                    {
                        ///catch and ignore.
                    }

                    this._graphBuilder = null;
                }
            }
            catch (Exception) { }

            this.EnableButtons(false);

            this.SetupProgressBar(0);
        }

        /// <summary>
        /// Set the progress bar
        /// </summary>
        /// <param name="max"></param>
        private void SetupProgressBar(double max)
        {
            //if (this.progress.InvokeRequired)
            //{
            //    this.progress.Invoke((MethodInvoker)(() =>
            //    {
            //        this.progress.Minimum = 0;
            //        this.progress.Maximum = (int)max * 100;
            //    }));
            //}
            //else
            //{
            //    this.progress.Minimum = 0;
            //    this.progress.Maximum = (int)max * 100;
            //}
        }

        private void Progress(double position)
        {
            //if (this.progress.InvokeRequired)
            //{
            //    this.progress.Invoke((MethodInvoker)(() =>
            //    {
            //        this.progress.Value = (int)position * 100;
            //    }));
            //}
            //else
            //{
            //    this.progress.Value = (int)position * 100;
            //}
        }

        /// <summary>
        /// Enable or disable media control buttons
        /// </summary>
        /// <param name="enable"></param>
        private void EnableButtons(bool enable)
        {
            this.btnPause.Enabled = enable;
            this.btnPlay.Enabled = enable;
            this.btnStop.Enabled = enable;
        }

        /// <summary>
        /// Hide controls
        /// </summary>
        /// <param name="hide"></param>
        private void HideControl(bool hide)
        {
            this.btnPlay.Visible = !hide;
            this.btnPause.Visible = !hide;
            this.btnStop.Visible = !hide;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            this.CloseInterfaces();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
