namespace HideIt.Transfer
{
    partial class SendFile_Form
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendFile_Form));
            this.lbl_browseSumry = new System.Windows.Forms.Label();
            this.lbl_filePath = new System.Windows.Forms.Label();
            this.txt_file = new System.Windows.Forms.TextBox();
            this.lbl_destSumry = new System.Windows.Forms.Label();
            this.lbl_pcName = new System.Windows.Forms.Label();
            this.txt_pcname = new System.Windows.Forms.TextBox();
            this.lbl_portSumry = new System.Windows.Forms.Label();
            this.lbl_port = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.lbl_sep = new System.Windows.Forms.Label();
            this.mtxt_port = new System.Windows.Forms.MaskedTextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_send = new System.Windows.Forms.Button();
            this.btn_browseFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_browseSumry
            // 
            this.lbl_browseSumry.Location = new System.Drawing.Point(18, 21);
            this.lbl_browseSumry.Name = "lbl_browseSumry";
            this.lbl_browseSumry.Size = new System.Drawing.Size(125, 13);
            this.lbl_browseSumry.TabIndex = 0;
            this.lbl_browseSumry.Text = "Select a file to send";
            // 
            // lbl_filePath
            // 
            this.lbl_filePath.Location = new System.Drawing.Point(18, 48);
            this.lbl_filePath.Name = "lbl_filePath";
            this.lbl_filePath.Size = new System.Drawing.Size(35, 13);
            this.lbl_filePath.TabIndex = 1;
            this.lbl_filePath.Text = "File";
            // 
            // txt_file
            // 
            this.txt_file.Location = new System.Drawing.Point(118, 45);
            this.txt_file.Name = "txt_file";
            this.txt_file.Size = new System.Drawing.Size(320, 21);
            this.txt_file.TabIndex = 0;
            // 
            // lbl_destSumry
            // 
            this.lbl_destSumry.Location = new System.Drawing.Point(18, 96);
            this.lbl_destSumry.Name = "lbl_destSumry";
            this.lbl_destSumry.Size = new System.Drawing.Size(321, 13);
            this.lbl_destSumry.TabIndex = 4;
            this.lbl_destSumry.Text = "Provide IP Address or name of destination computer";
            // 
            // lbl_pcName
            // 
            this.lbl_pcName.Location = new System.Drawing.Point(18, 119);
            this.lbl_pcName.Name = "lbl_pcName";
            this.lbl_pcName.Size = new System.Drawing.Size(94, 13);
            this.lbl_pcName.TabIndex = 5;
            this.lbl_pcName.Text = "PC Name (IP)";
            // 
            // txt_pcname
            // 
            this.txt_pcname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txt_pcname.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.txt_pcname.Location = new System.Drawing.Point(118, 116);
            this.txt_pcname.Name = "txt_pcname";
            this.txt_pcname.Size = new System.Drawing.Size(235, 21);
            this.txt_pcname.TabIndex = 2;
            // 
            // lbl_portSumry
            // 
            this.lbl_portSumry.Location = new System.Drawing.Point(18, 163);
            this.lbl_portSumry.Name = "lbl_portSumry";
            this.lbl_portSumry.Size = new System.Drawing.Size(458, 13);
            this.lbl_portSumry.TabIndex = 7;
            this.lbl_portSumry.Text = "Provide port number at which destination computer is listening for connections";
            // 
            // lbl_port
            // 
            this.lbl_port.Location = new System.Drawing.Point(18, 191);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(94, 13);
            this.lbl_port.TabIndex = 8;
            this.lbl_port.Text = "Port";
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(1, 270);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(547, 22);
            this.progress.Step = 1;
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progress.TabIndex = 11;
            // 
            // lbl_sep
            // 
            this.lbl_sep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_sep.Location = new System.Drawing.Point(12, 225);
            this.lbl_sep.Name = "lbl_sep";
            this.lbl_sep.Size = new System.Drawing.Size(524, 2);
            this.lbl_sep.TabIndex = 6;
            // 
            // mtxt_port
            // 
            this.mtxt_port.Location = new System.Drawing.Point(118, 188);
            this.mtxt_port.Mask = "00000";
            this.mtxt_port.Name = "mtxt_port";
            this.mtxt_port.Size = new System.Drawing.Size(113, 21);
            this.mtxt_port.TabIndex = 3;
            this.mtxt_port.ValidatingType = typeof(int);
            // 
            // btn_close
            // 
            this.btn_close.Image = global::HideIt.Properties.Resources.Close;
            this.btn_close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_close.Location = new System.Drawing.Point(459, 240);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(77, 24);
            this.btn_close.TabIndex = 5;
            this.btn_close.Text = "Close";
            this.btn_close.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_send
            // 
            this.btn_send.Image = global::HideIt.Properties.Resources.upload_001;
            this.btn_send.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_send.Location = new System.Drawing.Point(376, 240);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(77, 24);
            this.btn_send.TabIndex = 4;
            this.btn_send.Text = "Send";
            this.btn_send.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // btn_browseFile
            // 
            this.btn_browseFile.Image = global::HideIt.Properties.Resources.Open;
            this.btn_browseFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_browseFile.Location = new System.Drawing.Point(444, 44);
            this.btn_browseFile.Name = "btn_browseFile";
            this.btn_browseFile.Size = new System.Drawing.Size(92, 24);
            this.btn_browseFile.TabIndex = 1;
            this.btn_browseFile.Text = "Browse...";
            this.btn_browseFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browseFile.UseVisualStyleBackColor = true;
            this.btn_browseFile.Click += new System.EventHandler(this.btn_browseFile_Click);
            // 
            // SendFile_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(548, 292);
            this.Controls.Add(this.mtxt_port);
            this.Controls.Add(this.lbl_sep);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.lbl_portSumry);
            this.Controls.Add(this.txt_pcname);
            this.Controls.Add(this.lbl_pcName);
            this.Controls.Add(this.lbl_destSumry);
            this.Controls.Add(this.btn_browseFile);
            this.Controls.Add(this.txt_file);
            this.Controls.Add(this.lbl_filePath);
            this.Controls.Add(this.lbl_browseSumry);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SendFile_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send File";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_browseSumry;
        private System.Windows.Forms.Label lbl_filePath;
        private System.Windows.Forms.TextBox txt_file;
        private System.Windows.Forms.Button btn_browseFile;
        private System.Windows.Forms.Label lbl_destSumry;
        private System.Windows.Forms.Label lbl_pcName;
        private System.Windows.Forms.TextBox txt_pcname;
        private System.Windows.Forms.Label lbl_portSumry;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label lbl_sep;
        private System.Windows.Forms.MaskedTextBox mtxt_port;
    }
}