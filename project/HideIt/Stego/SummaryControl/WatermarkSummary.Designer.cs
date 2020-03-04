namespace HideIt.Stego.SummaryControl
{
    partial class WatermarkSummary
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_sumry_videoDim = new System.Windows.Forms.Label();
            this.lbl_dimensions = new System.Windows.Forms.Label();
            this.lbl_sumry_wmSize = new System.Windows.Forms.Label();
            this.lbl_msgSize = new System.Windows.Forms.Label();
            this.lbl_sumry_videoFps = new System.Windows.Forms.Label();
            this.lbl_sumry_videoLen = new System.Windows.Forms.Label();
            this.lbl_framerate = new System.Windows.Forms.Label();
            this.lbl_len = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_sumry_videoDim
            // 
            this.lbl_sumry_videoDim.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_sumry_videoDim.Location = new System.Drawing.Point(103, 51);
            this.lbl_sumry_videoDim.Name = "lbl_sumry_videoDim";
            this.lbl_sumry_videoDim.Size = new System.Drawing.Size(154, 13);
            this.lbl_sumry_videoDim.TabIndex = 27;
            this.lbl_sumry_videoDim.Text = "0 x 0 px";
            // 
            // lbl_dimensions
            // 
            this.lbl_dimensions.AutoSize = true;
            this.lbl_dimensions.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_dimensions.Location = new System.Drawing.Point(3, 51);
            this.lbl_dimensions.Name = "lbl_dimensions";
            this.lbl_dimensions.Size = new System.Drawing.Size(61, 13);
            this.lbl_dimensions.TabIndex = 26;
            this.lbl_dimensions.Text = "Dimensions";
            // 
            // lbl_sumry_wmSize
            // 
            this.lbl_sumry_wmSize.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_sumry_wmSize.Location = new System.Drawing.Point(103, 70);
            this.lbl_sumry_wmSize.Name = "lbl_sumry_wmSize";
            this.lbl_sumry_wmSize.Size = new System.Drawing.Size(154, 13);
            this.lbl_sumry_wmSize.TabIndex = 25;
            this.lbl_sumry_wmSize.Text = "0 bytes";
            // 
            // lbl_msgSize
            // 
            this.lbl_msgSize.AutoSize = true;
            this.lbl_msgSize.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_msgSize.Location = new System.Drawing.Point(3, 70);
            this.lbl_msgSize.Name = "lbl_msgSize";
            this.lbl_msgSize.Size = new System.Drawing.Size(82, 13);
            this.lbl_msgSize.TabIndex = 24;
            this.lbl_msgSize.Text = "Watermark Size";
            // 
            // lbl_sumry_videoFps
            // 
            this.lbl_sumry_videoFps.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_sumry_videoFps.Location = new System.Drawing.Point(103, 34);
            this.lbl_sumry_videoFps.Name = "lbl_sumry_videoFps";
            this.lbl_sumry_videoFps.Size = new System.Drawing.Size(154, 13);
            this.lbl_sumry_videoFps.TabIndex = 23;
            this.lbl_sumry_videoFps.Text = "0";
            // 
            // lbl_sumry_videoLen
            // 
            this.lbl_sumry_videoLen.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_sumry_videoLen.Location = new System.Drawing.Point(103, 18);
            this.lbl_sumry_videoLen.Name = "lbl_sumry_videoLen";
            this.lbl_sumry_videoLen.Size = new System.Drawing.Size(154, 13);
            this.lbl_sumry_videoLen.TabIndex = 22;
            this.lbl_sumry_videoLen.Text = "0 sec";
            // 
            // lbl_framerate
            // 
            this.lbl_framerate.AutoSize = true;
            this.lbl_framerate.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_framerate.Location = new System.Drawing.Point(3, 34);
            this.lbl_framerate.Name = "lbl_framerate";
            this.lbl_framerate.Size = new System.Drawing.Size(57, 13);
            this.lbl_framerate.TabIndex = 21;
            this.lbl_framerate.Text = "Frame rate";
            // 
            // lbl_len
            // 
            this.lbl_len.AutoSize = true;
            this.lbl_len.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_len.Location = new System.Drawing.Point(3, 18);
            this.lbl_len.Name = "lbl_len";
            this.lbl_len.Size = new System.Drawing.Size(70, 13);
            this.lbl_len.TabIndex = 20;
            this.lbl_len.Text = "Video Length";
            // 
            // WatermarkSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_sumry_videoDim);
            this.Controls.Add(this.lbl_dimensions);
            this.Controls.Add(this.lbl_sumry_wmSize);
            this.Controls.Add(this.lbl_msgSize);
            this.Controls.Add(this.lbl_sumry_videoFps);
            this.Controls.Add(this.lbl_sumry_videoLen);
            this.Controls.Add(this.lbl_framerate);
            this.Controls.Add(this.lbl_len);
            this.Name = "WatermarkSummary";
            this.Size = new System.Drawing.Size(260, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_sumry_videoDim;
        private System.Windows.Forms.Label lbl_dimensions;
        private System.Windows.Forms.Label lbl_sumry_wmSize;
        private System.Windows.Forms.Label lbl_msgSize;
        private System.Windows.Forms.Label lbl_sumry_videoFps;
        private System.Windows.Forms.Label lbl_sumry_videoLen;
        private System.Windows.Forms.Label lbl_framerate;
        private System.Windows.Forms.Label lbl_len;
    }
}
