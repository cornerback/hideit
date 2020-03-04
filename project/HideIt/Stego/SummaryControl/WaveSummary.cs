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
    public partial class WaveSummary : Summary
    {
        public WaveSummary()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set wave channels count
        /// </summary>
        public string Channels
        {
            get { return this.lbl_sumry_channel.Text; }
            set { this.lbl_sumry_channel.Text = value; }
        }

        /// <summary>
        /// Get or set wave files samples/sec
        /// </summary>
        public string SamplesPerSec
        {
            get { return this.lbl_sumry_samples.Text; }
            set { this.lbl_sumry_samples.Text = value; }
        }

        /// <summary>
        /// Get or set hiding capacity label value
        /// </summary>
        public override string HidingCapacity
        {
            get { return this.lbl_sumry_hidingcap.Text; }
            set { this.lbl_sumry_hidingcap.Text = value; }
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
