namespace HideIt
{
    partial class AviDecompressor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
                
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AviDecompressor));
            this.lblVideoPath = new System.Windows.Forms.Label();
            this.txtLoadAvi = new System.Windows.Forms.TextBox();
            this.lblSaveSummary = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSaveAvi = new System.Windows.Forms.TextBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_browseSaveAvi = new System.Windows.Forms.Button();
            this.btn_browseAvi = new System.Windows.Forms.Button();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.lblSummary = new System.Windows.Forms.Label();
            this.lbl_coverSumry = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblVideoPath
            // 
            this.lblVideoPath.AutoSize = true;
            this.lblVideoPath.Location = new System.Drawing.Point(21, 94);
            this.lblVideoPath.Name = "lblVideoPath";
            this.lblVideoPath.Size = new System.Drawing.Size(39, 13);
            this.lblVideoPath.TabIndex = 2;
            this.lblVideoPath.Text = "Video";
            // 
            // txtLoadAvi
            // 
            this.txtLoadAvi.Location = new System.Drawing.Point(85, 91);
            this.txtLoadAvi.Name = "txtLoadAvi";
            this.txtLoadAvi.Size = new System.Drawing.Size(361, 21);
            this.txtLoadAvi.TabIndex = 3;
            // 
            // lblSaveSummary
            // 
            this.lblSaveSummary.AutoSize = true;
            this.lblSaveSummary.Location = new System.Drawing.Point(21, 131);
            this.lblSaveSummary.Name = "lblSaveSummary";
            this.lblSaveSummary.Size = new System.Drawing.Size(280, 13);
            this.lblSaveSummary.TabIndex = 5;
            this.lblSaveSummary.Text = "Select path to write decompressed AVI video to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Video";
            // 
            // txtSaveAvi
            // 
            this.txtSaveAvi.Location = new System.Drawing.Point(85, 164);
            this.txtSaveAvi.Name = "txtSaveAvi";
            this.txtSaveAvi.Size = new System.Drawing.Size(361, 21);
            this.txtSaveAvi.TabIndex = 7;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(24, 244);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(521, 23);
            this.progress.TabIndex = 20;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = global::HideIt.Properties.Resources.Close;
            this.btn_cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_cancel.Location = new System.Drawing.Point(470, 205);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 24);
            this.btn_cancel.TabIndex = 19;
            this.btn_cancel.Text = "Close";
            this.btn_cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_start
            // 
            this.btn_start.Image = global::HideIt.Properties.Resources.Apply1;
            this.btn_start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_start.Location = new System.Drawing.Point(379, 205);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(75, 24);
            this.btn_start.TabIndex = 18;
            this.btn_start.Text = "Start";
            this.btn_start.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_browseSaveAvi
            // 
            this.btn_browseSaveAvi.Image = global::HideIt.Properties.Resources.save_file;
            this.btn_browseSaveAvi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_browseSaveAvi.Location = new System.Drawing.Point(452, 162);
            this.btn_browseSaveAvi.Name = "btn_browseSaveAvi";
            this.btn_browseSaveAvi.Size = new System.Drawing.Size(93, 24);
            this.btn_browseSaveAvi.TabIndex = 17;
            this.btn_browseSaveAvi.Text = "Browse...";
            this.btn_browseSaveAvi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browseSaveAvi.UseVisualStyleBackColor = true;
            this.btn_browseSaveAvi.Click += new System.EventHandler(this.btn_browseSaveAvi_Click);
            // 
            // btn_browseAvi
            // 
            this.btn_browseAvi.Image = global::HideIt.Properties.Resources.Open;
            this.btn_browseAvi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_browseAvi.Location = new System.Drawing.Point(452, 89);
            this.btn_browseAvi.Name = "btn_browseAvi";
            this.btn_browseAvi.Size = new System.Drawing.Size(93, 24);
            this.btn_browseAvi.TabIndex = 4;
            this.btn_browseAvi.Text = "Browse...";
            this.btn_browseAvi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browseAvi.UseVisualStyleBackColor = true;
            this.btn_browseAvi.Click += new System.EventHandler(this.btn_browseAvi_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.Location = new System.Drawing.Point(21, 26);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(524, 29);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "Message can only be hidden in decompressed AVI videos, because any change in comp" +
                "ressed frames can destroy the video.";
            // 
            // lbl_coverSumry
            // 
            this.lbl_coverSumry.Location = new System.Drawing.Point(21, 64);
            this.lbl_coverSumry.Name = "lbl_coverSumry";
            this.lbl_coverSumry.Size = new System.Drawing.Size(280, 20);
            this.lbl_coverSumry.TabIndex = 25;
            this.lbl_coverSumry.Text = "Select a file that you want to zip.";
            // 
            // AviDecompressor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 267);
            this.Controls.Add(this.lbl_coverSumry);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.btn_browseSaveAvi);
            this.Controls.Add(this.txtSaveAvi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSaveSummary);
            this.Controls.Add(this.btn_browseAvi);
            this.Controls.Add(this.txtLoadAvi);
            this.Controls.Add(this.lblVideoPath);
            this.Controls.Add(this.lblSummary);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AviDecompressor";
            this.Text = "Avi Decompressor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVideoPath;
        private System.Windows.Forms.TextBox txtLoadAvi;
        private System.Windows.Forms.Button btn_browseAvi;
        private System.Windows.Forms.Label lblSaveSummary;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSaveAvi;
        private System.Windows.Forms.Button btn_browseSaveAvi;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.ProgressBar progress;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lbl_coverSumry;
    }
}