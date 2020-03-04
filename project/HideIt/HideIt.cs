using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;

using HideIt.Transfer;
using HideIt.Stego.StegoProcessor;
using HideIt.Stego;
using HideIt.Stego.SummaryControl;
using HideIt.Live;
using HideIt.Audio;
using HideIt.Encryption;
using HideIt.Stego.HostObject;
using System.Data.SqlClient;

namespace HideIt
{
    public partial class HideItUI : Form
    {
        /// <summary>
        /// database connection string
        /// </summary>
       /// public string constr = @"Data Source = SAIRAHBATOOL-PC\SQLEXPRESS ; Initial Catalog = HideitDB ; Integrated Security= true";
        private const string TITLE = "HideIt";

        private StegoProcessorBase _processor;
        private int _hidingCapacity;

        private FileReceiver _receiver;

        private AcceptFileCallback _acceptFile;

        private Summary _currentControl;
        private SelectDevice _selectDevice;

        public const int WATERMARK_LEN = 256;

        /// <summary>
        /// Type of action to take
        /// </summary>
        private enum ActionType
        {
            Hide,
            Extract
        }

        /// <summary>
        /// Called to update the progress bar
        /// </summary>
        /// <param name="increment"></param>
        public delegate void UpdateProgressBarCallback(int increment);

        /// <summary>
        /// Called to setup the progress bar for new operation
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public delegate void SetupProgressBarCallback(int minimum, int maximum);

        /// <summary>
        /// Called when an application request to send file
        /// </summary>
        /// <param name="client"></param>
        public delegate void AcceptFileCallback(Socket client);

        /// <summary>
        /// Called to show the ReceiveFile form
        /// </summary>
        public delegate void OpenReceiveFormCallback(Socket client, HandshakeData data);

        /// <summary>
        /// Hold the variables for hiding and extraction process that is completed in
        /// background worker
        /// </summary>
        private sealed class BackgroundWorkerArgs
        {
            /// <summary>
            /// Get or set the message to hide/ being extracted
            /// </summary>
            public HideIt.Stego.Message Message { get; set; }

            /// <summary>
            /// Get or set the type of action
            /// </summary>
            public ActionType Action { get; set; }

            /// <summary>
            /// Get or set key used for extraction
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Get or set sink path for stego object
            /// </summary>
            public string SinkPath { get; set; }
        }

        public HideItUI()
        {
            InitializeComponent();

            ///Register DoWork and RunWorkerCompleted
            this.backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            ///Set the maximam length of key that is acceptable
            this.txt_key.MaxLength = Stego.Message.MAX_KEY_LEN;

            StegoProcessorBase.SetupProgressBar = new SetupProgressBarCallback(this.SetupProgressBar);
            StegoProcessorBase.UpdateProgressBar = new UpdateProgressBarCallback(this.UpdateProgressBar);

            this._acceptFile = new AcceptFileCallback(this.AcceptFile);

            this._selectDevice = new SelectDevice();
        }

        /// <summary>
        /// Called when new task is registered with background worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorkerArgs args = e.Argument as BackgroundWorkerArgs;

            ///Set the result to args that will be used when operations completes
            e.Result = args;

            if (args != null)
            {
                switch (args.Action)
                {
                    case ActionType.Hide:
                        //User has entered a save path for stego object, start hiding
                        using (Status status = new Status(this, this.lbl_status, "Hidding..."))
                        {
                            RC4 encrypt = new RC4(args.Message.Key, args.Message.SecertMessage);

                            HideIt.Stego.Message message = new HideIt.Stego.Message(
                                args.Message.Key, encrypt.ApplyRC4());

                            using (LockUI lockUi = new LockUI(this))
                            {
                                this._processor.Hide(message, args.SinkPath);
                                //"hide" tab selected                                
                            }
                        }
                        break;
                    case ActionType.Extract:

                        using (Status status = new Status(this, this.lbl_status, "Extracting..."))
                        {
                            using (LockUI lockUi = new LockUI(this))
                            {
                                args.Message = this._processor.Extract(args.Key);
                                //"extract" tab selected
                            }
                        }

                        if (args.Message == null)
                        {
                            UIMessage.Info("Cannot extract message from stego object because of one of the following reasons:\r\n" +
                                            "-Stego object do no contain any message\r\n" +
                                            "-Key provided do not match with the key used to hide message",
                                            "Extract Message");
                            return;
                        }

                        RC4 decrypt = new RC4(args.Key, args.Message.SecertMessage);

                        if (this.rtxt_hiddenMsg.InvokeRequired)
                        {
                            this.rtxt_hiddenMsg.Invoke((MethodInvoker)(() => { this.rtxt_hiddenMsg.Text = decrypt.ApplyRC4(); }));
                        }
                        else
                        {
                            this.rtxt_hiddenMsg.Text = decrypt.ApplyRC4();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Called when background worker completes the task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorkerArgs args = e.Result as BackgroundWorkerArgs;

            ///If an error occured during process
            if (e.Error != null)
            {
                string caption = "HideIt";
                if (args != null)
                {
                    caption = ((args.Action == ActionType.Hide) ? "Hide Message" : "Extract Message");
                }
                UIMessage.Error(e.Error.Message, caption);
            }

            if (args != null)
            {
                try
                {
                    args.Message.Dispose();
                    this._processor.Dispose();
                    this._processor = null;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Called when the form is first shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideIt_Shown(object sender, EventArgs e)
        {
            this._receiver = new FileReceiver(this._acceptFile);
            try
            {
                this._receiver.Listen(Options_Form.Port);
            }
            catch (Exception exc)
            {
                UIMessage.Error("An error occured while starting the listener. " +
                                "Application will not be able to receive files.\r\n" +
                                "Error: " + exc.Message, TITLE);
            }
        }

        /// <summary>
        /// When the text change we update some controls (like summary)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtxt_message_TextChanged(object sender, EventArgs e)
        {
            if (this._currentControl != null)
            {
                this._currentControl.MessageSize = this.ToSize(this.rtxt_message.Text.Length);
            }
        }

        /// <summary>
        /// Select a bitmap file as cover object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_browseCover_Click(object sender, EventArgs e)
        {
            using (Status status = new Status(null, this.lbl_status, "Open cover file"))
            {
                try
                {
                    string path = AppUtil.SelectFile("Open cover file", "Bitmap (*.bmp)|*.bmp|AVI (*.avi)|*.avi|WAV (*.wav)|*.wav", "bmp").ToLower();

                    if (!string.IsNullOrEmpty(path))
                    {
                        this.txt_coverFileName.Text = path;

                        if (this._processor != null)
                        {
                            this._processor.Dispose();
                        }

                        LoadProcessor(path);
                    }
                }
                catch (Exception exc)
                {
                    UIMessage.Error(exc.Message, TITLE);
                    if (this._processor != null)
                    {
                        try
                        {
                            this._processor.Dispose();
                        }
                        catch (Exception)
                        {
                        }
                        this._processor = null;
                    }
                }
            }
        }

        private void LoadProcessor(string path)
        {
            this._processor = StegoProcessorBase.GetProcessor(path, false);
            if (this._processor == null)
            {
                return;
            }

            this._processor.LoadHost(path, true);

            if (this._processor.MType != HostMediaType.Video)
            {
                this.cb_watermark.Enabled = false;
            }

            if (this._processor.MType == HostMediaType.Bitmap)
            {
                ///Show the bitmap summary control
                BitmapSummary summaryControl = new BitmapSummary();
                this._currentControl = summaryControl;
                this.ShowNewSummaryControl(summaryControl);

                ///Display cover file size
                summaryControl.HostSize = this.ToSize(this._processor.HostSize);

                ///Save and display hiding capacity of cover object
                this._hidingCapacity = (int)this._processor.HidingCapacity / 2;
                summaryControl.HidingCapacity = this.ToSize(this._hidingCapacity);

                ///Setup the maximum length of message                    
                this.rtxt_message.MaxLength = this._hidingCapacity - (Stego.Message.MAX_KEY_LEN + 8);

                ///Let show the pic in preview box. Open the image by giving the path
                ///The size mode of this image is set to zoom. So if the image is bigger then the
                ///frame it will automatically adjust it to fit the frame. Same happens if it is smaller
                ///then the frame
                this.pic_coverPreview.Image = new System.Drawing.Bitmap(this.txt_coverFileName.Text);
            }
            else if (this._processor.MType == HostMediaType.Wave)
            {
                ///Show the bitmap summary control
                WaveSummary summaryControl = new WaveSummary();
                this._currentControl = summaryControl;
                this.ShowNewSummaryControl(summaryControl);

                Wave host = (Wave)this._processor.HostObject;

                ///Display number of channels
                summaryControl.Channels = host.Format.nChannels.ToString();

                ///Display samples/sec 
                summaryControl.SamplesPerSec = host.Format.nSamplesPerSec.ToString();

                ///Save and display hiding capacity of cover object
                this._hidingCapacity = (int)this._processor.HidingCapacity / 2;
                summaryControl.HidingCapacity = this.ToSize(this._hidingCapacity);

                ///Setup the maximum length of message                    
                this.rtxt_message.MaxLength = this._hidingCapacity - (Stego.Message.MAX_KEY_LEN + 8);

                ///Setup the Avi icon image in the preview box
                this.pic_coverPreview.Image = global::HideIt.Properties.Resources.wav;
            }
            else
            {
                if (this.cb_watermark.Checked)
                {
                    this.LoadWatermarkingStegoProcessor();
                }
                else
                {
                    this.LoadAviStegoProcessor();
                }
            }
        }

        private void LoadAviStegoProcessor()
        {
            AviSummary summaryControl = new AviSummary();
            this._currentControl = summaryControl;
            this.ShowNewSummaryControl(summaryControl);

            AviStegoProcessor processor = (AviStegoProcessor)this._processor;

            ///Display the length of video
            summaryControl.VideoLength = processor.Length.ToString() + " sec";

            ///Display the framerate
            summaryControl.FrameRate = processor.FrameRate.ToString();

            ///Display the dimensions
            summaryControl.Dimension = processor.Width.ToString() + " x " + processor.Height.ToString() + " px";

            ///Display the hiding capacity
            this._hidingCapacity = (int)processor.HidingCapacity / 2;
            summaryControl.HidingCapacity = this.ToSize(this._hidingCapacity);

            ///Setup the maximum length of message                    
            this.rtxt_message.MaxLength = this._hidingCapacity - (Stego.Message.MAX_KEY_LEN + 8);

            ///Setup the Avi icon image in the preview box
            this.pic_coverPreview.Image = global::HideIt.Properties.Resources.avi;

            this.cb_watermark.Visible = true;
        }

        /// <summary>
        /// Select a bitmap file as stago file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_browseStego_Click(object sender, EventArgs e)
        {
            using (Status status = new Status(null, this.lbl_status, "Open stego file"))
            {
                try
                {
                    string path = AppUtil.SelectFile("Open stego file", "Bitmap (*.bmp)|*.bmp|AVI (*.avi)|*.avi|WAV (*.wav)|*.wav", "bmp").ToLower();

                    if (!string.IsNullOrEmpty(path))
                    {
                        this.txt_stegoObject.Text = path;

                        ///Dispose the previous processor
                        if (this._processor != null)
                        {
                            this._processor.Dispose();
                        }

                        this._processor = StegoProcessorBase.GetProcessor(path, false);
                        this._processor.LoadHost(path, true);

                        if (this._processor.MType == HostMediaType.Bitmap)
                        {
                            ///Let show the pic in preview box. Open the image by giving the path
                            ///The size mode of this image is set to zoom. So if the image is bigger then the
                            ///frame it will automatically adjust it to fit the frame. Same happens if it is smaller
                            ///then the frame
                            this.pic_stegoPreview.Image = new System.Drawing.Bitmap(this.txt_stegoObject.Text);
                        }
                        else if (this._processor.MType == HostMediaType.Wave)
                        {
                            this.pic_stegoPreview.Image = global::HideIt.Properties.Resources.wav;
                        }
                        else
                        {
                            ///setup the Avi icon image
                            this.pic_stegoPreview.Image = global::HideIt.Properties.Resources.avi;
                        }
                    }
                }
                catch (Exception exc)
                {
                    UIMessage.Error(exc.Message, TITLE);

                    if (this._processor != null)
                    {
                        try
                        {
                            this._processor.Dispose();
                        }
                        catch (Exception)
                        {
                        }
                        this._processor = null;
                    }
                }
            }
        }

        /// <summary>
        /// When selecting a location for saving stego object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_browesSaveStego_Click(object sender, EventArgs e)
        {
            using (Status status = new Status(null, this.lbl_status, "Save stego object file"))
            {
                ///Open a save dialog and ask user to save the stego object
                using (SaveFileDialog saveStego = new SaveFileDialog())
                {
                    saveStego.AutoUpgradeEnabled = true;
                    saveStego.Title = "Save stego object";
                    saveStego.Filter = "Bitmap (*.bmp)|*.bmp|AVI (*.avi)|*.avi|WAV (*.wav)|*.wav";
                    if (this._processor != null)
                    {
                        ///FilterIndex 1 = Bitmap
                        ///FilterIndex 2 = Avi
                        ///FilterIndex 3 = Wav
                        switch (this._processor.MType)
                        {
                            case HostMediaType.Bitmap:
                                saveStego.FilterIndex = 1;
                                break;
                            case HostMediaType.Video:
                                saveStego.FilterIndex = 2;
                                break;
                            case HostMediaType.Wave:
                                saveStego.FilterIndex = 3;
                                break;
                        }
                    }
                    if (saveStego.ShowDialog() == DialogResult.OK)
                    {
                        this.txt_saveStego.Text = saveStego.FileName;
                    }
                }
            }
        }

        /// <summary>
        /// When the form is closing (exiting)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideIt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._processor != null)
            {
                try
                {
                    this._processor.Dispose();
                    this._processor = null;
                }
                catch (Exception)
                {
                }
            }

            if (this._liveProcessor != null)
            {
                this._liveProcessor.Dispose();
                this._liveProcessor = null;
            }

            if (this._receiver != null)
            {
                try
                {
                    this._receiver.Dispose();
                }
                catch (Exception)
                {
                }
            }

            if (this.pic_coverPreview.Image != null)
            {
                ///Dispose off the image displayed in preview box
                this.pic_coverPreview.Image.Dispose();
            }

            if (this.pic_stegoPreview.Image != null)
            {
                ///Dipose off the stego object image displayed in preview box
                this.pic_stegoPreview.Image.Dispose();
            }

            if (this._selectDevice != null)
            {
                if (this._selectDevice.CaptureDevice != null)
                {
                    try
                    {
                        this._selectDevice.CaptureDevice.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                    this._selectDevice.CaptureDevice = null;
                }
                this._selectDevice = null;
            }
        }

        /// <summary>
        /// Clear all the data that in form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.backgroundWorker.Dispose();

            if (this._processor != null)
            {
                try
                {
                    this._processor.Dispose();
                }
                catch (Exception)
                {
                }
                this._processor = null;
            }

            if (this._liveProcessor != null)
            {
                try
                {
                    this._liveProcessor.Dispose();
                }
                catch (Exception)
                {
                }
                this._liveProcessor = null;
            }

            this._hidingCapacity = 0;

            ///Clear summary
            ///

            foreach (Control control in this.gb_summary.Controls)
            {
                control.Dispose();
            }
            this.gb_summary.Controls.Clear();
            this.lbl_status.Text = Status.DEFUALT_STATUS;

            ///Clear up all the text box fields
            this.txt_coverFileName.Text = string.Empty;
            this.txt_key.Text = string.Empty;
            this.txt_stegoKey.Text = string.Empty;
            this.txt_saveStego.Text = string.Empty;
            this.txt_stegoObject.Text = string.Empty;

            this.rtxt_message.Text = string.Empty;
            this.rtxt_hiddenMsg.Text = string.Empty;

            this.txt_selected.Text = string.Empty;
            this.rtxt_liveMessage.Text = string.Empty;
            this.txt_keyLive.Text = string.Empty;
            this.txt_saveCapturedPath.Text = string.Empty;

            if (this.pic_coverPreview.Image != null)
            {
                this.pic_coverPreview.Image.Dispose();
                this.pic_coverPreview.Image = null;
            }

            if (this.pic_stegoPreview.Image != null)
            {
                this.pic_stegoPreview.Image.Dispose();
                this.pic_stegoPreview.Image = null;
            }

            this.progress.Value = 0;
        }

        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Hide the message in cover object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_hide_Click(object sender, EventArgs e)
        {
            this.progress.Value = 0;

            ///If no cover object selected yet
            if (this._processor == null)
            {
                UIMessage.Info("Please select a cover object to hide message in", "Hide Message");
                return;
            }

            ///Check the legth of key
            if (this.txt_key.Text.Length < 4 || this.txt_key.Text.Length > Stego.Message.MAX_KEY_LEN)
            {
                UIMessage.Info("Key lenght must be atleast 4 characters and atmost 256 characters",
                                "Hide Message");
                return;
            }

            ///Check if there is enough room in cover object to store secret message and key in it
            if (this._hidingCapacity < (this.rtxt_message.Text.Length + this.txt_key.Text.Length + 8))
            {
                UIMessage.Error("Message is bigger then hiding capacity", "Hide Message");
                return;
            }
            
            ///Initialize the porgress bar         
            this.progress.Minimum = 0;
            this.progress.Maximum = (int)this._processor.HostObject.Length;

            ///See if user hasn't entered the save path for stego object
            if (string.IsNullOrEmpty(this.txt_saveStego.Text))
            {
                UIMessage.Info("Please select a path to save stego object", "Hide Message");
                return;
            }

            ///Run the hiding process in background worker thread
            this.backgroundWorker.RunWorkerAsync(new BackgroundWorkerArgs()
            {
                Action = ActionType.Hide,
                Message = new HideIt.Stego.Message(this.txt_key.Text, this.rtxt_message.Text),
                SinkPath = this.txt_saveStego.Text
            });

            using (new Status(this, this.lbl_status, "Logging information in database..."))
            {
                ///data insertion to maintain log table        
                string _stego_file_name = txt_saveStego.Text;
                string _key = txt_key.Text;
                string _time = DateTime.Now.ToLongTimeString();
                string _date = DateTime.Now.ToShortDateString();
                SqlConnection con = new SqlConnection(DB.DbConnectionStr.conect());
                ///SqlConnection con = new SqlConnection(constr);
                SqlCommand query = new SqlCommand("insert into Log_Table values('" + _stego_file_name + "','" + _key + "', '" + _date + "', '" + _time + "')", con);
                try
                {
                    con.Open();
                    query.ExecuteNonQuery();
                    MessageBox.Show("Record entered successfully.");
                    con.Close();

                }
                catch (SqlException ex)
                {
                    UIMessage.Error(ex.Message, TITLE);
                }
            }
        }

        /// <summary>
        /// Extract the message from stego object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_extract_Click(object sender, EventArgs e)
        {
            this.progress.Value = 0;

            if (this._processor == null)
            {
                UIMessage.Info("No stego object selected to extract data from", "Extract Message");
                return;
            }

            ///Check whether user has entered the key
            if (this.txt_stegoKey.Text.Length == 0)
            {
                UIMessage.Info("Please provide the key used to hide the message", "Extract Message");
                return;
            }

            ///Run the extraction process in background worker thread
            this.backgroundWorker.RunWorkerAsync(new BackgroundWorkerArgs()
            {
                Action = ActionType.Extract,
                Key = this.txt_stegoKey.Text
            });
        }

        /// <summary>
        /// Open file sending dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendFile_Form sendFile = new SendFile_Form();
            sendFile.Show(this);
        }

        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options_Form options = new Options_Form(this._receiver);
            options.Show(this);
        }

        private void compareVideosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompareVideos cVid = new CompareVideos();
            cVid.Show();
        }

        private void aviDecompressorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AviDecompressor aviDecomp = new AviDecompressor();
            aviDecomp.Show();
        }

        /// <summary>
        /// Update the progress bar's current position
        /// </summary>
        /// <param name="increment"></param>
        private void UpdateProgressBar(int increment)
        {
            if (this.progress.InvokeRequired)
            {
                this.progress.Invoke((MethodInvoker)(() => { this.progress.Increment(increment); }));
            }
            else
            {
                this.progress.Increment(increment);
            }
        }

        /// <summary>
        /// Setup the progress bar for new operation
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        private void SetupProgressBar(int minimum, int maximum)
        {
            if (this.progress.InvokeRequired)
            {
                this.progress.Invoke((MethodInvoker)(() =>
                {
                    this.progress.Minimum = minimum;
                    this.progress.Maximum = maximum;
                }));
            }
            else
            {
                this.progress.Minimum = minimum;
                this.progress.Maximum = maximum;
            }
        }

        /// <summary>
        /// Lock UI controls
        /// </summary>
        public void LockUIControls()
        {
            this.menu_main.Enabled = false;

            this.tabs.Enabled = false;
            this.btn_browseCover.Enabled = false;
            this.btn_browseSaveStego.Enabled = false;
            this.btn_browseStego.Enabled = false;
            this.btn_hide.Enabled = false;
            this.btn_extract.Enabled = false;

            this.txt_coverFileName.ReadOnly = true;
            this.txt_key.ReadOnly = true;
            this.rtxt_hiddenMsg.ReadOnly = true;
            this.txt_saveStego.ReadOnly = true;
            this.txt_stegoObject.ReadOnly = true;
            this.txt_stegoKey.ReadOnly = true;
        }

        /// <summary>
        /// Unlock UI controls
        /// </summary>
        public void UnLockUIControls()
        {
            this.menu_main.Enabled = true;

            this.tabs.Enabled = true;
            this.btn_browseCover.Enabled = true;
            this.btn_browseSaveStego.Enabled = true;
            this.btn_browseStego.Enabled = true;
            this.btn_hide.Enabled = true;
            this.btn_extract.Enabled = true;

            this.txt_coverFileName.ReadOnly = false;
            this.txt_key.ReadOnly = false;
            this.rtxt_hiddenMsg.ReadOnly = false;
            this.txt_saveStego.ReadOnly = false;
            this.txt_stegoObject.ReadOnly = false;
            this.txt_stegoKey.ReadOnly = false;
        }

        /// <summary>
        /// Called when handshake is completed
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        private void ShowReceiveFileForm(Socket client, HandshakeData data)
        {
            ReceiveFile_Form receiveForm = new ReceiveFile_Form(this._receiver, client, data);
            receiveForm.Show(this);
        }

        /// <summary>
        /// Show new summary control and dispose the previous one
        /// </summary>
        /// <param name="newControl">New summary control</param>
        private void ShowNewSummaryControl(UserControl newControl)
        {
            if (newControl != null)
            {
                foreach (Control control in this.gb_summary.Controls)
                {
                    control.Dispose();
                }
                this.gb_summary.Controls.Clear();

                this.gb_summary.Controls.Add(newControl);
                newControl.Location = new System.Drawing.Point(5, 5);
            }
        }

        /// <summary>
        /// Called when an application request to send a file
        /// </summary>
        /// <param name="client"></param>
        private void AcceptFile(Socket client)
        {
            try
            {
                HandshakeData data = this._receiver.Handshake(client);

                if (data == null)
                {
                    return;
                }

                DialogResult result = UIMessage.Ask(this, "A computer " + data.MachineName +
                                   " wishes to transfer a file " + data.FileName +
                                   " of size" + this.ToSize(data.FileLen) +
                                   "\r\nAccept?", "Receive file");

                this._receiver.HandshakeResponse(client, result == DialogResult.Yes);

                ///If user decided to not receive the file
                if (result == DialogResult.No)
                {
                    return;
                }

                ///Fire the delegate that will call the ShowReceiveFileForm method
                this.BeginInvoke(new OpenReceiveFormCallback(this.ShowReceiveFileForm), client, data);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string ToSize(int size)
        {
            int kb = 1024;
            int mb = kb * kb;
            if (size > mb)
                return (Math.Round((float)size / mb, 3).ToString() + " MB");
            else if (size > kb)
                return (Math.Round((float)size / kb, 3).ToString() + " KB");
            else return (size.ToString() + " Bytes");
        }

        private void audioWavePlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AudioPlayerUI audioPly = new AudioPlayerUI();
            audioPly.Show();
        }

        private void toolStripButtonSendFile_Click(object sender, EventArgs e)
        {
            SendFile_Form sendFile = new SendFile_Form();
            sendFile.Show(this);
        }

        private void toolStripButtonOptiions_Click(object sender, EventArgs e)
        {
            Options_Form options = new Options_Form(this._receiver);
            options.Show(this);
        }

        private void toolStripButtonAviDecompressor_Click(object sender, EventArgs e)
        {
            AviDecompressor aviDecomp = new AviDecompressor();
            aviDecomp.Show();
        }

        private void toolStripButtonCompareVideos_Click(object sender, EventArgs e)
        {
            CompareVideos cVid = new CompareVideos();
            cVid.Show();
        }

        private void toolStripButtonWaveAudioPlayer_Click(object sender, EventArgs e)
        {
            AudioPlayerUI audioPly = new AudioPlayerUI();
            audioPly.Show();
        }
       /// <summary>
       /// title & description for help message
       /// </summary>      
       private const string _title = "MY MCS FINAL PROJECT";
       private const string _description = @"
            It is an application to hide some text message into a cover file 
            (i.e. avi video, wav audio, or bmp image) & then extracting that 
            secret message from that cover file. 
            It is capabel of getting avi video at the spot & hide the text 
            message  into it, video will be captured according to user's 
            requirements (If one wants to capture video that is sufficient
            for text massege then check the check-box i.e. Stop capturing 
            when message hiding  is complete. 
            Then only that video is captured that is enough for hiding the 
            text message). 
            It is also capable of decompressing/uncompressing the avi video 
            because I will hide text message in it otherwise the size 
            of original video & stego video( i.e. in which text message is 
            hidden ) is different (i.e. increased after hiding text message).
            It is also capable of video comparison, so that user can 
            compare  both videos (i.e. video before & after hiding text 
            into it), user can  even compare frame-per-second, dimensions, 
            & length of them. 
            It is capable of playing simple wave audio files. 
            It is capable of recording wav audio then hiding text message 
            into it & extracting text message from it.";
            
        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_description, _title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_description, _title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            this.backgroundWorker.Dispose();

            if (this._processor != null)
            {
                try
                {
                    this._processor.Dispose();
                }
                catch (Exception)
                {
                }
                this._processor = null;
            }

            if (this._liveProcessor != null)
            {
                try
                {
                    this._liveProcessor.Dispose();
                }
                catch (Exception)
                {
                }
                this._liveProcessor = null;
            }

            this._hidingCapacity = 0;

            ///Clear summary
            ///

            foreach (Control control in this.gb_summary.Controls)
            {
                control.Dispose();
            }
            this.gb_summary.Controls.Clear();
            this.lbl_status.Text = Status.DEFUALT_STATUS;

            ///Clear up all the text box fields
            this.txt_coverFileName.Text = string.Empty;
            this.txt_key.Text = string.Empty;
            this.txt_stegoKey.Text = string.Empty;
            this.txt_saveStego.Text = string.Empty;
            this.txt_stegoObject.Text = string.Empty;

            this.rtxt_message.Text = string.Empty;
            this.rtxt_hiddenMsg.Text = string.Empty;

            this.txt_selected.Text = string.Empty;
            this.rtxt_liveMessage.Text = string.Empty;
            this.txt_keyLive.Text = string.Empty;
            this.txt_saveCapturedPath.Text = string.Empty;

            if (this.pic_coverPreview.Image != null)
            {
                this.pic_coverPreview.Image.Dispose();
                this.pic_coverPreview.Image = null;
            }

            if (this.pic_stegoPreview.Image != null)
            {
                this.pic_stegoPreview.Image.Dispose();
                this.pic_stegoPreview.Image = null;
            }

            this.progress.Value = 0;
        }

        private void cb_watermark_CheckedChanged(object sender, EventArgs e)
        {
            this.LoadWatermarkingStegoProcessor();
        }

        private void LoadWatermarkingStegoProcessor()
        {
            try
            {
                ///Reduce the message length to 256 character
                if (this.cb_watermark.Checked)
                {
                    ///Change summary controls
                    WatermarkSummary summaryControl = new WatermarkSummary();
                    this._currentControl = summaryControl;
                    this.ShowNewSummaryControl(summaryControl);

                    if (this._processor != null)
                    {
                        try
                        {
                            this._processor.Dispose();
                        }
                        catch (Exception)
                        {
                        }
                        this._processor = null;
                    }

                    this._processor = StegoProcessorBase.GetProcessor(this.txt_coverFileName.Text, true);
                    if (this._processor == null)
                    {
                        return;
                    }
                    this._processor.LoadHost(this.txt_coverFileName.Text, true);

                    WatermarkingStegoProcessor processor = (WatermarkingStegoProcessor)this._processor;

                    ///Display the length of video
                    summaryControl.VideoLength = processor.Length.ToString() + " sec";

                    ///Display the framerate
                    summaryControl.FrameRate = processor.FrameRate.ToString();

                    ///Display the dimensions
                    summaryControl.Dimension = processor.Width.ToString() + " x " + processor.Height.ToString() + " px";

                    ///Display the hiding capacity
                    this._hidingCapacity = (int)processor.HidingCapacity / 2;

                    this.rtxt_message.MaxLength = WATERMARK_LEN;
                    
                    ///If there is already text in text box
                    if (rtxt_message.Text.Length > WATERMARK_LEN)
                    {
                        DialogResult result = UIMessage.Ask(this, @"Enabling watermarking allows only " + WATERMARK_LEN.ToString() + " characters for hiding.\r\n"
                            + "The current length of message is " + this.rtxt_message.Text.Length.ToString() + ".\r\n"
                            + "Do you wish to automatically strip the mssage to " + WATERMARK_LEN.ToString() + " characters?",
                            TITLE);

                        if (result == DialogResult.Yes)
                        {
                            this.rtxt_message.Text = this.rtxt_message.Text.Substring(0, WATERMARK_LEN - 1);
                        }
                        else
                        {
                            this.cb_watermark.Checked = false;
                        }
                    }
                }
                else
                {
                    this.LoadProcessor(this.txt_coverFileName.Text);
                }
            }
            catch (Exception exc)
            {
                UIMessage.Error(exc.Message, TITLE);
                if (this._processor != null)
                {
                    try
                    {
                        this._processor.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                    this._processor = null;
                }
            }
        }

        private void zipUnZipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Zipper zip = new Zipper();
            zip.Show();
        }

        private void toolStripButtonCompress_Click(object sender, EventArgs e)
        {
            Zipper zip = new Zipper();
            zip.Show();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UnZipper uzip = new UnZipper();
            uzip.Show();
        }

        private void unZipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnZipper uzip = new UnZipper();
            uzip.Show();
        }
   }
}

