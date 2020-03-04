using System;
using System.Collections.Generic; //  Required to content the central directory
using System.IO;
using System.IO.Compression;  // Required to use ZipStorer class
using System.Windows.Forms;

using System.IO.Compression.HideIt;

namespace HideIt
{
    public partial class Zipper : Form
    {
        private const string TITLE = "Zipper";
        public Zipper()
        {
            InitializeComponent();

            this.RadioCreate.Checked = true;
        }
        
        private void btn_browseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.AutoUpgradeEnabled = true;
                dlg.AddExtension = true;
                dlg.CheckFileExists = true;
                dlg.Multiselect = true;
                dlg.Title = "Select files to store/zip";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(dlg.FileNames);
                }
            }
        }

        private void btn_browseSaveZippedFile_Click(object sender, EventArgs e)
        {
            FileDialog dlg;

            if (this.RadioCreate.Checked)
            {
                dlg = new SaveFileDialog();
                ((SaveFileDialog)dlg).OverwritePrompt = true;
            }
            else  // Append checked
            {
                dlg = new OpenFileDialog();
                dlg.CheckFileExists = true;
            }

            dlg.AddExtension = true;
            dlg.Filter = "Zip file|*.zip";
            dlg.Title = "Select filename for storage/zipped file";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSaveZippedFile.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// to zip the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_startZip_Click(object sender, EventArgs e)
        {
            // Previous checkings
            if (this.listBox1.Items.Count <= 0)
            {
                MessageBox.Show("Source files not chosen.", "Zip Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(txtSaveZippedFile.Text))
            {
                MessageBox.Show("Target filename is not defined.", "Zip Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                ZipStorer zip;

                if (this.RadioCreate.Checked)
                    // Creates a new zip file
                    zip = ZipStorer.Create(txtSaveZippedFile.Text, "Zip file will be created now");
                else
                    // Opens existing zip file
                    zip = ZipStorer.Open(txtSaveZippedFile.Text, FileAccess.Write);

                zip.EncodeUTF8 = this.CheckUTF8.Checked;

                // Stores all the files into the zip file
                foreach (string path in listBox1.Items)
                {
                    zip.AddFile(this.checkCompress.Checked ? ZipStorer.Compression.Deflate : ZipStorer.Compression.Store,
                        path, Path.GetFileName(path), "");
                }

                // Creates a memory stream with text
                if (this.RadioCreate.Checked)
                {
                    MemoryStream readme = new MemoryStream(
                        System.Text.Encoding.UTF8.GetBytes(string.Format("{0}\r\nThis file has been {1} using the c# ZipStorer class. It is part of my MCS final project",
                        DateTime.Now, this.RadioCreate.Checked ? "created" : "appended")));

                    // Stores a new file directly from the stream
                    zip.AddStream(ZipStorer.Compression.Store, "readme.txt", readme, DateTime.Now, "Please read");
                    readme.Close();
                }

                // Updates and closes the zip file
                zip.Close();

                lblMessage.Text="Target file processed with success.";

                // Clear controls
                this.listBox1.Items.Clear();
                this.txtSaveZippedFile.Text = "";
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("Error: Invalid or not supported Zip file.", "Zipper Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("Error while processing target file.", "Zipper Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
