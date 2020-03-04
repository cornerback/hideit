namespace HideIt
{
    partial class UnZipper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
       /// private System.ComponentModel.IContainer components = null;
                
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnZipper));
            this.txtTargetFolder = new System.Windows.Forms.TextBox();
            this.lblUnZippedFilePath = new System.Windows.Forms.Label();
            this.lblSaveSummary = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblLoadSummary = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowseZipSource = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_startUnZip = new System.Windows.Forms.Button();
            this.btn_browseSaveUnZippedFile = new System.Windows.Forms.Button();
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTargetFolder
            // 
            this.txtTargetFolder.Location = new System.Drawing.Point(104, 104);
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.Size = new System.Drawing.Size(282, 21);
            this.txtTargetFolder.TabIndex = 28;
            // 
            // lblUnZippedFilePath
            // 
            this.lblUnZippedFilePath.AutoSize = true;
            this.lblUnZippedFilePath.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblUnZippedFilePath.Location = new System.Drawing.Point(15, 107);
            this.lblUnZippedFilePath.Name = "lblUnZippedFilePath";
            this.lblUnZippedFilePath.Size = new System.Drawing.Size(83, 13);
            this.lblUnZippedFilePath.TabIndex = 27;
            this.lblUnZippedFilePath.Text = "Target Folder";
            // 
            // lblSaveSummary
            // 
            this.lblSaveSummary.AutoSize = true;
            this.lblSaveSummary.Location = new System.Drawing.Point(15, 79);
            this.lblSaveSummary.Name = "lblSaveSummary";
            this.lblSaveSummary.Size = new System.Drawing.Size(468, 13);
            this.lblSaveSummary.TabIndex = 26;
            this.lblSaveSummary.Text = "Select path to write un-compressed/un-zipped files to required destination folder" +
                "\r\n";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblFilePath.Location = new System.Drawing.Point(15, 156);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(90, 13);
            this.lblFilePath.TabIndex = 23;
            this.lblFilePath.Text = "Extracted Files";
            // 
            // lblLoadSummary
            // 
            this.lblLoadSummary.AutoSize = true;
            this.lblLoadSummary.Location = new System.Drawing.Point(15, 25);
            this.lblLoadSummary.Name = "lblLoadSummary";
            this.lblLoadSummary.Size = new System.Drawing.Size(367, 13);
            this.lblLoadSummary.TabIndex = 22;
            this.lblLoadSummary.Text = "Select a zipped file to start un-compression/un-zipping process";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(104, 136);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(282, 56);
            this.listBox2.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(15, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Source";
            // 
            // btnBrowseZipSource
            // 
            this.btnBrowseZipSource.Image = global::HideIt.Properties.Resources.save_file;
            this.btnBrowseZipSource.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowseZipSource.Location = new System.Drawing.Point(392, 44);
            this.btnBrowseZipSource.Name = "btnBrowseZipSource";
            this.btnBrowseZipSource.Size = new System.Drawing.Size(93, 24);
            this.btnBrowseZipSource.TabIndex = 36;
            this.btnBrowseZipSource.Text = "Browse...";
            this.btnBrowseZipSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowseZipSource.UseVisualStyleBackColor = true;
            this.btnBrowseZipSource.Click += new System.EventHandler(this.btnBrowseZipSource_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = global::HideIt.Properties.Resources.Close;
            this.btn_cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_cancel.Location = new System.Drawing.Point(410, 208);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 24);
            this.btn_cancel.TabIndex = 31;
            this.btn_cancel.Text = "Close";
            this.btn_cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_startUnZip
            // 
            this.btn_startUnZip.Image = global::HideIt.Properties.Resources.Apply1;
            this.btn_startUnZip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_startUnZip.Location = new System.Drawing.Point(323, 208);
            this.btn_startUnZip.Name = "btn_startUnZip";
            this.btn_startUnZip.Size = new System.Drawing.Size(75, 24);
            this.btn_startUnZip.TabIndex = 30;
            this.btn_startUnZip.Text = "Start";
            this.btn_startUnZip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_startUnZip.UseVisualStyleBackColor = true;
            this.btn_startUnZip.Click += new System.EventHandler(this.btn_startUnZip_Click);
            // 
            // btn_browseSaveUnZippedFile
            // 
            this.btn_browseSaveUnZippedFile.Image = global::HideIt.Properties.Resources.save_file;
            this.btn_browseSaveUnZippedFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_browseSaveUnZippedFile.Location = new System.Drawing.Point(392, 102);
            this.btn_browseSaveUnZippedFile.Name = "btn_browseSaveUnZippedFile";
            this.btn_browseSaveUnZippedFile.Size = new System.Drawing.Size(93, 24);
            this.btn_browseSaveUnZippedFile.TabIndex = 29;
            this.btn_browseSaveUnZippedFile.Text = "Browse...";
            this.btn_browseSaveUnZippedFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browseSaveUnZippedFile.UseVisualStyleBackColor = true;
            this.btn_browseSaveUnZippedFile.Click += new System.EventHandler(this.btn_browseSaveUnZippedFile_Click);
            // 
            // txtSourceFile
            // 
            this.txtSourceFile.Location = new System.Drawing.Point(104, 46);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.Size = new System.Drawing.Size(282, 21);
            this.txtSourceFile.TabIndex = 37;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblMessage.Location = new System.Drawing.Point(15, 208);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 38;
            // 
            // UnZipper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 248);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtSourceFile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnBrowseZipSource);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_startUnZip);
            this.Controls.Add(this.btn_browseSaveUnZippedFile);
            this.Controls.Add(this.txtTargetFolder);
            this.Controls.Add(this.lblUnZippedFilePath);
            this.Controls.Add(this.lblSaveSummary);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.lblLoadSummary);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UnZipper";
            this.Text = "UnZipper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_startUnZip;
        private System.Windows.Forms.Button btn_browseSaveUnZippedFile;
        private System.Windows.Forms.TextBox txtTargetFolder;
        private System.Windows.Forms.Label lblUnZippedFilePath;
        private System.Windows.Forms.Label lblSaveSummary;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label lblLoadSummary;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowseZipSource;
        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.Label lblMessage;

    }
}