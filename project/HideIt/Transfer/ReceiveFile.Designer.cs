namespace HideIt.Transfer
{
    partial class ReceiveFile_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiveFile_Form));
            this.lbl_saveSumry = new System.Windows.Forms.Label();
            this.lbl_path = new System.Windows.Forms.Label();
            this.txt_file = new System.Windows.Forms.TextBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.lbl_sep = new System.Windows.Forms.Label();
            this.btn_savePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_saveSumry
            // 
            this.lbl_saveSumry.Location = new System.Drawing.Point(24, 22);
            this.lbl_saveSumry.Name = "lbl_saveSumry";
            this.lbl_saveSumry.Size = new System.Drawing.Size(213, 13);
            this.lbl_saveSumry.TabIndex = 0;
            this.lbl_saveSumry.Text = "Select a path to save the file";
            // 
            // lbl_path
            // 
            this.lbl_path.AutoSize = true;
            this.lbl_path.Location = new System.Drawing.Point(24, 50);
            this.lbl_path.Name = "lbl_path";
            this.lbl_path.Size = new System.Drawing.Size(57, 13);
            this.lbl_path.TabIndex = 1;
            this.lbl_path.Text = "Save file";
            // 
            // txt_file
            // 
            this.txt_file.Location = new System.Drawing.Point(105, 47);
            this.txt_file.Name = "txt_file";
            this.txt_file.Size = new System.Drawing.Size(290, 21);
            this.txt_file.TabIndex = 0;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(1, 114);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(505, 22);
            this.progress.Step = 1;
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progress.TabIndex = 3;
            // 
            // lbl_sep
            // 
            this.lbl_sep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_sep.Location = new System.Drawing.Point(12, 96);
            this.lbl_sep.Name = "lbl_sep";
            this.lbl_sep.Size = new System.Drawing.Size(482, 2);
            this.lbl_sep.TabIndex = 2;
            // 
            // btn_savePath
            // 
            this.btn_savePath.Image = global::HideIt.Properties.Resources.Open;
            this.btn_savePath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_savePath.Location = new System.Drawing.Point(401, 45);
            this.btn_savePath.Name = "btn_savePath";
            this.btn_savePath.Size = new System.Drawing.Size(93, 24);
            this.btn_savePath.TabIndex = 1;
            this.btn_savePath.Text = "Browse...";
            this.btn_savePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_savePath.UseVisualStyleBackColor = true;
            this.btn_savePath.Click += new System.EventHandler(this.btn_savePath_Click);
            // 
            // ReceiveFile_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 136);
            this.Controls.Add(this.lbl_sep);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.btn_savePath);
            this.Controls.Add(this.txt_file);
            this.Controls.Add(this.lbl_path);
            this.Controls.Add(this.lbl_saveSumry);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ReceiveFile_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receive File";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_saveSumry;
        private System.Windows.Forms.Label lbl_path;
        private System.Windows.Forms.TextBox txt_file;
        private System.Windows.Forms.Button btn_savePath;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label lbl_sep;
    }
}