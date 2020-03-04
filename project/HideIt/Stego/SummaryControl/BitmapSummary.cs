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
    public partial class BitmapSummary : Summary
    {
        public BitmapSummary()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the host size label
        /// </summary>
        public string HostSize
        {
            get { return this.lbl_sumry_coverSize.Text; }
            set { this.lbl_sumry_coverSize.Text = value; }
        }

        /// <summary>
        /// Get or set hiding capacity label
        /// </summary>
        public override string HidingCapacity
        {
            get { return this.lbl_sumry_hidingCapacity.Text; }
            set { this.lbl_sumry_hidingCapacity.Text = value; }
        }

        /// <summary>
        /// Get or set message size label
        /// </summary>
        public override string MessageSize
        {
            get { return this.lbl_sumry_msgSize.Text; }
            set { this.lbl_sumry_msgSize.Text = value; }
        }
    }
}
