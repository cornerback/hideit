namespace HideIt.Live
{
    partial class SelectDevice
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
            this.components = new System.ComponentModel.Container();
            this.lstDevices = new System.Windows.Forms.ListView();
            this.devices = new System.Windows.Forms.ColumnHeader();
            this.deviceImageList = new System.Windows.Forms.ImageList(this.components);
            this.cancel = new System.Windows.Forms.Button();
            this.select = new System.Windows.Forms.Button();
            this.lbl_sumry = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstDevices
            // 
            this.lstDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.devices});
            this.lstDevices.FullRowSelect = true;
            this.lstDevices.GridLines = true;
            this.lstDevices.Location = new System.Drawing.Point(15, 36);
            this.lstDevices.MultiSelect = false;
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(360, 119);
            this.lstDevices.SmallImageList = this.deviceImageList;
            this.lstDevices.TabIndex = 0;
            this.lstDevices.UseCompatibleStateImageBehavior = false;
            this.lstDevices.View = System.Windows.Forms.View.Details;
            this.lstDevices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstDevices_MouseDoubleClick);
            // 
            // devices
            // 
            this.devices.Text = "Devices";
            this.devices.Width = 350;
            // 
            // deviceImageList
            // 
            this.deviceImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.deviceImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.deviceImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(297, 161);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // select
            // 
            this.select.Location = new System.Drawing.Point(216, 161);
            this.select.Name = "select";
            this.select.Size = new System.Drawing.Size(75, 23);
            this.select.TabIndex = 2;
            this.select.Text = "Select";
            this.select.UseVisualStyleBackColor = true;
            this.select.Click += new System.EventHandler(this.select_Click);
            // 
            // lbl_sumry
            // 
            this.lbl_sumry.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbl_sumry.Location = new System.Drawing.Point(12, 18);
            this.lbl_sumry.Name = "lbl_sumry";
            this.lbl_sumry.Size = new System.Drawing.Size(360, 15);
            this.lbl_sumry.TabIndex = 3;
            this.lbl_sumry.Text = "Select a capture device from the list of available devices";
            // 
            // SelectDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 195);
            this.Controls.Add(this.lbl_sumry);
            this.Controls.Add(this.select);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.lstDevices);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDevice";
            this.Text = "Capture Devices";
            this.Load += new System.EventHandler(this.SelectDevice_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstDevices;
        private System.Windows.Forms.ImageList deviceImageList;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button select;
        private System.Windows.Forms.Label lbl_sumry;
        private System.Windows.Forms.ColumnHeader devices;
    }
}