namespace HideIt
{
    partial class VideoControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.vidOwner = new System.Windows.Forms.Panel();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.lbl_movieName = new System.Windows.Forms.Label();
            this.gb_summary = new System.Windows.Forms.GroupBox();
            this.lb_val_len = new System.Windows.Forms.Label();
            this.lb_val_dimension = new System.Windows.Forms.Label();
            this.lb_val_fps = new System.Windows.Forms.Label();
            this.lb_len = new System.Windows.Forms.Label();
            this.lb_dimension = new System.Windows.Forms.Label();
            this.lb_fps = new System.Windows.Forms.Label();
            this.gb_summary.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::HideIt.Properties.Resources.Open;
            this.btnBrowse.Location = new System.Drawing.Point(285, 287);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(38, 28);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Image = global::HideIt.Properties.Resources.control_play;
            this.btnPlay.Location = new System.Drawing.Point(197, 287);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(38, 28);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Image = global::HideIt.Properties.Resources.control_pause;
            this.btnPause.Location = new System.Drawing.Point(241, 287);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(38, 28);
            this.btnPause.TabIndex = 3;
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Image = global::HideIt.Properties.Resources.control_stop;
            this.btnStop.Location = new System.Drawing.Point(153, 287);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(38, 28);
            this.btnStop.TabIndex = 4;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // vidOwner
            // 
            this.vidOwner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.vidOwner.Location = new System.Drawing.Point(3, 3);
            this.vidOwner.Name = "vidOwner";
            this.vidOwner.Size = new System.Drawing.Size(320, 240);
            this.vidOwner.TabIndex = 6;
            // 
            // lbl_movieName
            // 
            this.lbl_movieName.Location = new System.Drawing.Point(3, 246);
            this.lbl_movieName.Name = "lbl_movieName";
            this.lbl_movieName.Size = new System.Drawing.Size(320, 13);
            this.lbl_movieName.TabIndex = 7;
            // 
            // gb_summary
            // 
            this.gb_summary.Controls.Add(this.lb_val_len);
            this.gb_summary.Controls.Add(this.lb_val_dimension);
            this.gb_summary.Controls.Add(this.lb_val_fps);
            this.gb_summary.Controls.Add(this.lb_len);
            this.gb_summary.Controls.Add(this.lb_dimension);
            this.gb_summary.Controls.Add(this.lb_fps);
            this.gb_summary.Location = new System.Drawing.Point(6, 262);
            this.gb_summary.Name = "gb_summary";
            this.gb_summary.Size = new System.Drawing.Size(141, 53);
            this.gb_summary.TabIndex = 8;
            this.gb_summary.TabStop = false;
            // 
            // lb_val_len
            // 
            this.lb_val_len.Location = new System.Drawing.Point(40, 38);
            this.lb_val_len.Name = "lb_val_len";
            this.lb_val_len.Size = new System.Drawing.Size(95, 13);
            this.lb_val_len.TabIndex = 13;
            // 
            // lb_val_dimension
            // 
            this.lb_val_dimension.Location = new System.Drawing.Point(40, 25);
            this.lb_val_dimension.Name = "lb_val_dimension";
            this.lb_val_dimension.Size = new System.Drawing.Size(95, 13);
            this.lb_val_dimension.TabIndex = 12;
            // 
            // lb_val_fps
            // 
            this.lb_val_fps.Location = new System.Drawing.Point(40, 12);
            this.lb_val_fps.Name = "lb_val_fps";
            this.lb_val_fps.Size = new System.Drawing.Size(95, 13);
            this.lb_val_fps.TabIndex = 9;
            // 
            // lb_len
            // 
            this.lb_len.AutoSize = true;
            this.lb_len.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_len.Location = new System.Drawing.Point(6, 38);
            this.lb_len.Name = "lb_len";
            this.lb_len.Size = new System.Drawing.Size(27, 13);
            this.lb_len.TabIndex = 11;
            this.lb_len.Text = "Len";
            // 
            // lb_dimension
            // 
            this.lb_dimension.AutoSize = true;
            this.lb_dimension.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_dimension.Location = new System.Drawing.Point(6, 25);
            this.lb_dimension.Name = "lb_dimension";
            this.lb_dimension.Size = new System.Drawing.Size(30, 13);
            this.lb_dimension.TabIndex = 10;
            this.lb_dimension.Text = "Dim";
            // 
            // lb_fps
            // 
            this.lb_fps.AutoSize = true;
            this.lb_fps.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_fps.Location = new System.Drawing.Point(6, 12);
            this.lb_fps.Name = "lb_fps";
            this.lb_fps.Size = new System.Drawing.Size(28, 13);
            this.lb_fps.TabIndex = 9;
            this.lb_fps.Text = "FPS";
            // 
            // VideoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb_summary);
            this.Controls.Add(this.lbl_movieName);
            this.Controls.Add(this.vidOwner);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnBrowse);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VideoControl";
            this.Size = new System.Drawing.Size(327, 317);
            this.gb_summary.ResumeLayout(false);
            this.gb_summary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        internal System.Windows.Forms.Button btnPlay;
        internal System.Windows.Forms.Button btnPause;
        internal System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Panel vidOwner;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.Label lbl_movieName;
        private System.Windows.Forms.GroupBox gb_summary;
        private System.Windows.Forms.Label lb_fps;
        private System.Windows.Forms.Label lb_dimension;
        private System.Windows.Forms.Label lb_len;
        private System.Windows.Forms.Label lb_val_len;
        private System.Windows.Forms.Label lb_val_dimension;
        private System.Windows.Forms.Label lb_val_fps;
    }
}
