using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideIt
{
    public partial class CompareVideos : Form
    {
        private VideoControl _srcControl;
        private VideoControl _stegoControl;

        public CompareVideos()
        {
            InitializeComponent();
        }

        private void CompareVideo_Load(object sender, EventArgs e)
        {
            this._srcControl = new VideoControl(null);
            this._srcControl.Location = new Point(0, 0);

            this._stegoControl = new VideoControl(this._srcControl);
            this._stegoControl.Location = new Point(this._srcControl.ClientRectangle.Width + 1, 0);

            this.Controls.Add(this._srcControl);
            this.Controls.Add(this._stegoControl);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (this._srcControl != null)
            {
                this._srcControl.Dispose();
                this._srcControl = null;
            }
            if (this._stegoControl != null)
            {
                this._stegoControl.Dispose();
                this._stegoControl = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
