namespace HideIt.Stego.SummaryControl
{
    partial class BitmapSummary
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
            this.lbl_sumry_msgSize = new System.Windows.Forms.Label();
            this.lbl_msgSize = new System.Windows.Forms.Label();
            this.lbl_sumry_hidingCapacity = new System.Windows.Forms.Label();
            this.lbl_sumry_coverSize = new System.Windows.Forms.Label();
            this.lbl_capacity = new System.Windows.Forms.Label();
            this.lbl_coverSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_sumry_msgSize
            // 
            this.lbl_sumry_msgSize.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_sumry_msgSize.Location = new System.Drawing.Point(103, 66);
            this.lbl_sumry_msgSize.Name = "lbl_sumry_msgSize";
            this.lbl_sumry_msgSize.Size = new System.Drawing.Size(154, 13);
            this.lbl_sumry_msgSize.TabIndex = 11;
            this.lbl_sumry_msgSize.Text = "0 bytes";
            // 
            // lbl_msgSize
            // 
            this.lbl_msgSize.AutoSize = true;
            this.lbl_msgSize.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_msgSize.Location = new System.Drawing.Point(3, 66);
            this.lbl_msgSize.Name = "lbl_msgSize";
            this.lbl_msgSize.Size = new System.Drawing.Size(82, 13);
            this.lbl_msgSize.TabIndex = 10;
            this.lbl_msgSize.Text = "Message size";
            // 
            // lbl_sumry_hidingCapacity
            // 
            this.lbl_sumry_hidingCapacity.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_sumry_hidingCapacity.Location = new System.Drawing.Point(103, 43);
            this.lbl_sumry_hidingCapacity.Name = "lbl_sumry_hidingCapacity";
            this.lbl_sumry_hidingCapacity.Size = new System.Drawing.Size(154, 13);
            this.lbl_sumry_hidingCapacity.TabIndex = 9;
            this.lbl_sumry_hidingCapacity.Text = "0 bytes";
            // 
            // lbl_sumry_coverSize
            // 
            this.lbl_sumry_coverSize.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_sumry_coverSize.Location = new System.Drawing.Point(103, 21);
            this.lbl_sumry_coverSize.Name = "lbl_sumry_coverSize";
            this.lbl_sumry_coverSize.Size = new System.Drawing.Size(154, 13);
            this.lbl_sumry_coverSize.TabIndex = 8;
            this.lbl_sumry_coverSize.Text = "0 bytes";
            // 
            // lbl_capacity
            // 
            this.lbl_capacity.AutoSize = true;
            this.lbl_capacity.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_capacity.Location = new System.Drawing.Point(3, 43);
            this.lbl_capacity.Name = "lbl_capacity";
            this.lbl_capacity.Size = new System.Drawing.Size(93, 13);
            this.lbl_capacity.TabIndex = 7;
            this.lbl_capacity.Text = "Hiding capacity";
            // 
            // lbl_coverSize
            // 
            this.lbl_coverSize.AutoSize = true;
            this.lbl_coverSize.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbl_coverSize.Location = new System.Drawing.Point(3, 21);
            this.lbl_coverSize.Name = "lbl_coverSize";
            this.lbl_coverSize.Size = new System.Drawing.Size(68, 13);
            this.lbl_coverSize.TabIndex = 6;
            this.lbl_coverSize.Text = "Cover size";
            // 
            // BitmapSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_sumry_msgSize);
            this.Controls.Add(this.lbl_msgSize);
            this.Controls.Add(this.lbl_sumry_hidingCapacity);
            this.Controls.Add(this.lbl_sumry_coverSize);
            this.Controls.Add(this.lbl_capacity);
            this.Controls.Add(this.lbl_coverSize);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BitmapSummary";
            this.Size = new System.Drawing.Size(260, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_sumry_msgSize;
        private System.Windows.Forms.Label lbl_msgSize;
        private System.Windows.Forms.Label lbl_sumry_hidingCapacity;
        private System.Windows.Forms.Label lbl_sumry_coverSize;
        private System.Windows.Forms.Label lbl_capacity;
        private System.Windows.Forms.Label lbl_coverSize;
    }
}
