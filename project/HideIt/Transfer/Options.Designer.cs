namespace HideIt.Transfer
{
    partial class Options_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options_Form));
            this.lbl_portSumry = new System.Windows.Forms.Label();
            this.lbl_port = new System.Windows.Forms.Label();
            this.mtxt_port = new System.Windows.Forms.MaskedTextBox();
            this.lbl_sep = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_apply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_portSumry
            // 
            this.lbl_portSumry.Location = new System.Drawing.Point(22, 22);
            this.lbl_portSumry.Name = "lbl_portSumry";
            this.lbl_portSumry.Size = new System.Drawing.Size(364, 36);
            this.lbl_portSumry.TabIndex = 0;
            this.lbl_portSumry.Text = "Specify a port at which the program will listen for incomming connections request" +
                ". Port must be between 1000-65535";
            // 
            // lbl_port
            // 
            this.lbl_port.AutoSize = true;
            this.lbl_port.Location = new System.Drawing.Point(22, 69);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(30, 13);
            this.lbl_port.TabIndex = 1;
            this.lbl_port.Text = "Port";
            // 
            // mtxt_port
            // 
            this.mtxt_port.Location = new System.Drawing.Point(79, 66);
            this.mtxt_port.Mask = "00000";
            this.mtxt_port.Name = "mtxt_port";
            this.mtxt_port.Size = new System.Drawing.Size(156, 21);
            this.mtxt_port.TabIndex = 0;
            this.mtxt_port.ValidatingType = typeof(int);
            // 
            // lbl_sep
            // 
            this.lbl_sep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_sep.Location = new System.Drawing.Point(12, 129);
            this.lbl_sep.Name = "lbl_sep";
            this.lbl_sep.Size = new System.Drawing.Size(424, 2);
            this.lbl_sep.TabIndex = 3;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = global::HideIt.Properties.Resources.Cancel;
            this.btn_cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_cancel.Location = new System.Drawing.Point(361, 139);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 24);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_apply
            // 
            this.btn_apply.Image = global::HideIt.Properties.Resources.Apply1;
            this.btn_apply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_apply.Location = new System.Drawing.Point(270, 139);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(75, 24);
            this.btn_apply.TabIndex = 1;
            this.btn_apply.Text = "Apply";
            this.btn_apply.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // Options_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 174);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.lbl_sep);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.mtxt_port);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.lbl_portSumry);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Options_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_portSumry;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.MaskedTextBox mtxt_port;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Label lbl_sep;
        private System.Windows.Forms.Button btn_cancel;
    }
}