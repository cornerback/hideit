namespace HideIt
{
    partial class AudioPlayerUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioPlayerUI));
            this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
            this.picBoxAudio1 = new System.Windows.Forms.PictureBox();
            this.picBoxAudio2 = new System.Windows.Forms.PictureBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_play = new System.Windows.Forms.Button();
            this.btn_browse = new System.Windows.Forms.Button();
            this.lbl_audioPoemName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAudio1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAudio2)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenDlg
            // 
            this.OpenDlg.DefaultExt = "wav";
            this.OpenDlg.Filter = "WAV files|*.wav";
            // 
            // picBoxAudio1
            // 
            this.picBoxAudio1.Enabled = false;
            this.picBoxAudio1.Image = global::HideIt.Properties.Resources.Equalizer;
            this.picBoxAudio1.Location = new System.Drawing.Point(8, 8);
            this.picBoxAudio1.Name = "picBoxAudio1";
            this.picBoxAudio1.Size = new System.Drawing.Size(272, 176);
            this.picBoxAudio1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxAudio1.TabIndex = 5;
            this.picBoxAudio1.TabStop = false;
            // 
            // picBoxAudio2
            // 
            this.picBoxAudio2.Enabled = false;
            this.picBoxAudio2.Image = global::HideIt.Properties.Resources.Grammaphone_with_notes;
            this.picBoxAudio2.Location = new System.Drawing.Point(8, 187);
            this.picBoxAudio2.Name = "picBoxAudio2";
            this.picBoxAudio2.Size = new System.Drawing.Size(272, 32);
            this.picBoxAudio2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxAudio2.TabIndex = 4;
            this.picBoxAudio2.TabStop = false;
            // 
            // btn_close
            // 
            this.btn_close.Image = global::HideIt.Properties.Resources.Close;
            this.btn_close.Location = new System.Drawing.Point(189, 241);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(38, 28);
            this.btn_close.TabIndex = 3;
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Image = global::HideIt.Properties.Resources.control_stop;
            this.btn_stop.Location = new System.Drawing.Point(145, 241);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(38, 28);
            this.btn_stop.TabIndex = 2;
            this.btn_stop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_play
            // 
            this.btn_play.Image = global::HideIt.Properties.Resources.control_play;
            this.btn_play.Location = new System.Drawing.Point(101, 241);
            this.btn_play.Name = "btn_play";
            this.btn_play.Size = new System.Drawing.Size(38, 28);
            this.btn_play.TabIndex = 1;
            this.btn_play.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_play.UseVisualStyleBackColor = true;
            this.btn_play.Click += new System.EventHandler(this.btn_play_Click);
            // 
            // btn_browse
            // 
            this.btn_browse.Image = global::HideIt.Properties.Resources.Open;
            this.btn_browse.Location = new System.Drawing.Point(57, 241);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(38, 28);
            this.btn_browse.TabIndex = 0;
            this.btn_browse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // lbl_audioPoemName
            // 
            this.lbl_audioPoemName.Location = new System.Drawing.Point(9, 222);
            this.lbl_audioPoemName.Name = "lbl_audioPoemName";
            this.lbl_audioPoemName.Size = new System.Drawing.Size(271, 13);
            this.lbl_audioPoemName.TabIndex = 8;
            // 
            // AudioPlayerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(285, 274);
            this.Controls.Add(this.lbl_audioPoemName);
            this.Controls.Add(this.picBoxAudio1);
            this.Controls.Add(this.picBoxAudio2);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_play);
            this.Controls.Add(this.btn_browse);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AudioPlayerUI";
            this.Text = "Audio Player";
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAudio1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAudio2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.Button btn_play;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.OpenFileDialog OpenDlg;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.PictureBox picBoxAudio2;
        private System.Windows.Forms.PictureBox picBoxAudio1;
        private System.Windows.Forms.Label lbl_audioPoemName;

    }
}