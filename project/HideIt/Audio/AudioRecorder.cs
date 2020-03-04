using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace HideIt.Audio
{
    /// <summary>Records an audio wave</summary>
    public class AudioRecorder : Form
    {
        /// <summary>How many samples do we need to hide the message using the specified key?</summary>
        private long countSamplesRequired = 0;
        /// <summary>How man samples do we have recorded yet?</summary>
        private long countSamplesRecorded = 0;
        /// <summary>Empty stream to receive the recorded samples</summary>
        private MemoryStream recordedData = new MemoryStream();
        /// <summary>The recorder will do the WaveIn work</summary>
        private WaveInRecorder waveRecorder;
        /// <summary>Format of the new wave: 16 bit, stereo</summary>
        private WaveFormat format = new WaveFormat(11025, BytesPerSample * 8, 2);

        /// <summary>Again: 16 bit</summary>
        private const int BytesPerSample = 2;
        private Label lblSamplesRecorded;
        private Label label1;
        private Label label2;
        private Label lblSamplesRequired;
        internal Button btnStartStop;
        internal Button btnClose;
        private GroupBox gb_audioSummary;
        private PictureBox pictureBox1;

        /// <summary>Header + recorded samples</summary>
        private Stream recordedStream;
        public Stream RecordedStream
        {
            get { return recordedStream; }
        }

        /// <summary>Initialize a new recorder Form to record [countSamplesRequired] or more samples</summary>
        /// <param name="countSamplesRequired">Minimum count of samples to record</param>
        public AudioRecorder(long countSamplesRequired)
        {
            InitializeComponent();
            this.countSamplesRequired = countSamplesRequired;

            lblSamplesRequired.Text = countSamplesRequired.ToString();
            lblSamplesRecorded.Text = countSamplesRecorded.ToString();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioRecorder));
            this.lblSamplesRecorded = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSamplesRequired = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.gb_audioSummary = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gb_audioSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSamplesRecorded
            // 
            this.lblSamplesRecorded.Location = new System.Drawing.Point(131, 8);
            this.lblSamplesRecorded.Name = "lblSamplesRecorded";
            this.lblSamplesRecorded.Size = new System.Drawing.Size(74, 20);
            this.lblSamplesRecorded.TabIndex = 7;
            this.lblSamplesRecorded.Text = "00000000";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Samples Recorded:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Samples Required:";
            // 
            // lblSamplesRequired
            // 
            this.lblSamplesRequired.Location = new System.Drawing.Point(131, 29);
            this.lblSamplesRequired.Name = "lblSamplesRequired";
            this.lblSamplesRequired.Size = new System.Drawing.Size(74, 20);
            this.lblSamplesRequired.TabIndex = 6;
            this.lblSamplesRequired.Text = "00000000";
            // 
            // btnClose
            // 
            this.btnClose.Image = global::HideIt.Properties.Resources.Close;
            this.btnClose.Location = new System.Drawing.Point(132, 204);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(38, 28);
            this.btnClose.TabIndex = 11;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Image = global::HideIt.Properties.Resources.control_play;
            this.btnStartStop.Location = new System.Drawing.Point(88, 204);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(38, 28);
            this.btnStartStop.TabIndex = 8;
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // gb_audioSummary
            // 
            this.gb_audioSummary.Controls.Add(this.label1);
            this.gb_audioSummary.Controls.Add(this.lblSamplesRequired);
            this.gb_audioSummary.Controls.Add(this.label2);
            this.gb_audioSummary.Controls.Add(this.lblSamplesRecorded);
            this.gb_audioSummary.Location = new System.Drawing.Point(18, 141);
            this.gb_audioSummary.Name = "gb_audioSummary";
            this.gb_audioSummary.Size = new System.Drawing.Size(217, 60);
            this.gb_audioSummary.TabIndex = 12;
            this.gb_audioSummary.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HideIt.Properties.Resources.CMYK_256x256;
            this.pictureBox1.Location = new System.Drawing.Point(18, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(217, 147);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // AudioRecorder
            // 
            this.ClientSize = new System.Drawing.Size(254, 234);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gb_audioSummary);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStartStop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AudioRecorder";
            this.gb_audioSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        /// <summary>Start recording</summary>
        private void Start()
        {
            waveRecorder = new WaveInRecorder(-1, format, 16384, 3, new BufferDoneEventHandler(WaveDataArrived));
            btnStartStop.Enabled = false;
            btnStartStop.Image =  global::HideIt.Properties.Resources.control_stop;
        }

        /// <summary>Stop recording, add a header to the sound data</summary>
        private void Stop()
        {
            waveRecorder.Dispose();
            recordedStream = WaveStream.CreateStream(recordedData, format);
            Close();
        }

        /// <summary>Callback method - copy a buffer of recorded audio data</summary>
        /// <param name="data">Pointer to the raw data</param>
        /// <param name="size">Size of the data</param>
        private void WaveDataArrived(IntPtr data, int size)
        {
            byte[] recBuffer = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(data, recBuffer, 0, size);
            recordedData.Write(recBuffer, 0, recBuffer.Length);

            countSamplesRecorded += size / BytesPerSample;
            lblSamplesRecorded.Text = countSamplesRecorded.ToString();

            if (countSamplesRecorded >= countSamplesRequired)
            {
                //enough samples arrived - allow the user to stop recording
                btnStartStop.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            Start();
        }
   }
}
