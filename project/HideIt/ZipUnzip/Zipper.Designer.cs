namespace HideIt
{
    partial class Zipper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        ///private System.ComponentModel.IContainer components = null;
                
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Zipper));
            this.txtSaveZippedFile = new System.Windows.Forms.TextBox();
            this.lblZippedFilePath = new System.Windows.Forms.Label();
            this.lblSaveSummary = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblLoadSummary = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.CheckUTF8 = new System.Windows.Forms.CheckBox();
            this.checkCompress = new System.Windows.Forms.CheckBox();
            this.radioAppend = new System.Windows.Forms.RadioButton();
            this.RadioCreate = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_startZip = new System.Windows.Forms.Button();
            this.btn_browseSaveZippedFile = new System.Windows.Forms.Button();
            this.btn_browseFile = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtSaveZippedFile
            // 
            this.txtSaveZippedFile.Location = new System.Drawing.Point(47, 176);
            this.txtSaveZippedFile.Name = "txtSaveZippedFile";
            this.txtSaveZippedFile.Size = new System.Drawing.Size(339, 21);
            this.txtSaveZippedFile.TabIndex = 28;
            // 
            // lblZippedFilePath
            // 
            this.lblZippedFilePath.AutoSize = true;
            this.lblZippedFilePath.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblZippedFilePath.Location = new System.Drawing.Point(15, 179);
            this.lblZippedFilePath.Name = "lblZippedFilePath";
            this.lblZippedFilePath.Size = new System.Drawing.Size(26, 13);
            this.lblZippedFilePath.TabIndex = 27;
            this.lblZippedFilePath.Text = "File";
            // 
            // lblSaveSummary
            // 
            this.lblSaveSummary.AutoSize = true;
            this.lblSaveSummary.Location = new System.Drawing.Point(15, 151);
            this.lblSaveSummary.Name = "lblSaveSummary";
            this.lblSaveSummary.Size = new System.Drawing.Size(387, 13);
            this.lblSaveSummary.TabIndex = 26;
            this.lblSaveSummary.Text = "Select path to write compressed/zipped file to required destination";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblFilePath.Location = new System.Drawing.Point(15, 94);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(26, 13);
            this.lblFilePath.TabIndex = 23;
            this.lblFilePath.Text = "File";
            // 
            // lblLoadSummary
            // 
            this.lblLoadSummary.AutoSize = true;
            this.lblLoadSummary.Location = new System.Drawing.Point(15, 70);
            this.lblLoadSummary.Name = "lblLoadSummary";
            this.lblLoadSummary.Size = new System.Drawing.Size(288, 13);
            this.lblLoadSummary.TabIndex = 22;
            this.lblLoadSummary.Text = "Select a file to start compression/zipping process";
            // 
            // lblSummary
            // 
            this.lblSummary.Location = new System.Drawing.Point(15, 17);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(479, 40);
            this.lblSummary.TabIndex = 21;
            this.lblSummary.Text = resources.GetString("lblSummary.Text");
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(47, 91);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(339, 43);
            this.listBox1.TabIndex = 33;
            // 
            // CheckUTF8
            // 
            this.CheckUTF8.AutoSize = true;
            this.CheckUTF8.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.CheckUTF8.Location = new System.Drawing.Point(315, 248);
            this.CheckUTF8.Name = "CheckUTF8";
            this.CheckUTF8.Size = new System.Drawing.Size(54, 17);
            this.CheckUTF8.TabIndex = 38;
            this.CheckUTF8.Text = "UTF8";
            this.CheckUTF8.UseVisualStyleBackColor = true;
            // 
            // checkCompress
            // 
            this.checkCompress.AutoSize = true;
            this.checkCompress.Checked = true;
            this.checkCompress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkCompress.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.checkCompress.Location = new System.Drawing.Point(228, 247);
            this.checkCompress.Name = "checkCompress";
            this.checkCompress.Size = new System.Drawing.Size(84, 17);
            this.checkCompress.TabIndex = 37;
            this.checkCompress.Text = "Compress";
            this.checkCompress.UseVisualStyleBackColor = true;
            // 
            // radioAppend
            // 
            this.radioAppend.AutoSize = true;
            this.radioAppend.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.radioAppend.Location = new System.Drawing.Point(147, 246);
            this.radioAppend.Name = "radioAppend";
            this.radioAppend.Size = new System.Drawing.Size(68, 17);
            this.radioAppend.TabIndex = 36;
            this.radioAppend.Text = "Append";
            this.radioAppend.UseVisualStyleBackColor = true;
            // 
            // RadioCreate
            // 
            this.RadioCreate.AutoSize = true;
            this.RadioCreate.Checked = true;
            this.RadioCreate.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.RadioCreate.Location = new System.Drawing.Point(85, 246);
            this.RadioCreate.Name = "RadioCreate";
            this.RadioCreate.Size = new System.Drawing.Size(64, 17);
            this.RadioCreate.TabIndex = 35;
            this.RadioCreate.TabStop = true;
            this.RadioCreate.Text = "Create";
            this.RadioCreate.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(15, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Operation:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(470, 26);
            this.label1.TabIndex = 39;
            this.label1.Text = "Select Operation that you want to perform. If you want to zip then select Create " +
                "\r\noperation, Append is for adding some unzipped file to an existed zipped-folder" +
                ". ";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = global::HideIt.Properties.Resources.Close;
            this.btn_cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_cancel.Location = new System.Drawing.Point(410, 277);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 24);
            this.btn_cancel.TabIndex = 31;
            this.btn_cancel.Text = "Close";
            this.btn_cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_startZip
            // 
            this.btn_startZip.Image = global::HideIt.Properties.Resources.Apply1;
            this.btn_startZip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_startZip.Location = new System.Drawing.Point(323, 277);
            this.btn_startZip.Name = "btn_startZip";
            this.btn_startZip.Size = new System.Drawing.Size(75, 24);
            this.btn_startZip.TabIndex = 30;
            this.btn_startZip.Text = "Start";
            this.btn_startZip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_startZip.UseVisualStyleBackColor = true;
            this.btn_startZip.Click += new System.EventHandler(this.btn_startZip_Click);
            // 
            // btn_browseSaveZippedFile
            // 
            this.btn_browseSaveZippedFile.Image = global::HideIt.Properties.Resources.save_file;
            this.btn_browseSaveZippedFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_browseSaveZippedFile.Location = new System.Drawing.Point(392, 175);
            this.btn_browseSaveZippedFile.Name = "btn_browseSaveZippedFile";
            this.btn_browseSaveZippedFile.Size = new System.Drawing.Size(93, 24);
            this.btn_browseSaveZippedFile.TabIndex = 29;
            this.btn_browseSaveZippedFile.Text = "Browse...";
            this.btn_browseSaveZippedFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browseSaveZippedFile.UseVisualStyleBackColor = true;
            this.btn_browseSaveZippedFile.Click += new System.EventHandler(this.btn_browseSaveZippedFile_Click);
            // 
            // btn_browseFile
            // 
            this.btn_browseFile.Image = global::HideIt.Properties.Resources.Open;
            this.btn_browseFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_browseFile.Location = new System.Drawing.Point(392, 102);
            this.btn_browseFile.Name = "btn_browseFile";
            this.btn_browseFile.Size = new System.Drawing.Size(93, 24);
            this.btn_browseFile.TabIndex = 25;
            this.btn_browseFile.Text = "Choose...";
            this.btn_browseFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browseFile.UseVisualStyleBackColor = true;
            this.btn_browseFile.Click += new System.EventHandler(this.btn_browseFile_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblMessage.Location = new System.Drawing.Point(15, 277);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 40;
            // 
            // Zipper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 312);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckUTF8);
            this.Controls.Add(this.checkCompress);
            this.Controls.Add(this.radioAppend);
            this.Controls.Add(this.RadioCreate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_startZip);
            this.Controls.Add(this.btn_browseSaveZippedFile);
            this.Controls.Add(this.txtSaveZippedFile);
            this.Controls.Add(this.lblZippedFilePath);
            this.Controls.Add(this.lblSaveSummary);
            this.Controls.Add(this.btn_browseFile);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.lblLoadSummary);
            this.Controls.Add(this.lblSummary);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Zipper";
            this.Text = "Zipper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_startZip;
        private System.Windows.Forms.Button btn_browseSaveZippedFile;
        private System.Windows.Forms.TextBox txtSaveZippedFile;
        private System.Windows.Forms.Label lblZippedFilePath;
        private System.Windows.Forms.Label lblSaveSummary;
        private System.Windows.Forms.Button btn_browseFile;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label lblLoadSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox CheckUTF8;
        private System.Windows.Forms.CheckBox checkCompress;
        private System.Windows.Forms.RadioButton radioAppend;
        private System.Windows.Forms.RadioButton RadioCreate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage;

    }
}