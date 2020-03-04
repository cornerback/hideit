using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideIt.Stego.SummaryControl
{
    public partial class AviSummary : Summary
    {
        public AviSummary()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set video length label value
        /// </summary>
        public string VideoLength
        {
            get { return this.lbl_sumry_videoLen.Text; }
            set { this.lbl_sumry_videoLen.Text = value; }
        }

        /// <summary>
        /// Get or set video frames per second label value
        /// </summary>
        public string FrameRate
        {
            get { return this.lbl_sumry_videoFps.Text; }
            set { this.lbl_sumry_videoFps.Text = value; }
        }

        /// <summary>
        /// Get or set video dimensions label value
        /// </summary>
        public string Dimension
        {
            get { return this.lbl_sumry_videoDim.Text; }
            set { this.lbl_sumry_videoDim.Text = value; }
        }

        /// <summary>
        /// Get or set hiding capacity label value
        /// </summary>
        public override string HidingCapacity
        {
            get { return this.lbl_sumry_hidingCapacity.Text; }
            set { this.lbl_sumry_hidingCapacity.Text = value; }
        }

        /// <summary>
        /// Get or set message size label value
        /// </summary>
        public override string MessageSize
        {
            get { return this.lbl_sumry_msgSize.Text; }
            set { this.lbl_sumry_msgSize.Text = value; }
        }
    }
}
