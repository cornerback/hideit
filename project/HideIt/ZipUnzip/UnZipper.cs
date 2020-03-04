using System;
using System.Collections.Generic; //  Required to content the central directory
using System.IO;
using System.IO.Compression;  // Required to use ZipStorer class
using System.Windows.Forms;

using System.IO.Compression.HideIt;

namespace HideIt
{
    public partial class UnZipper : Form
    {
        private const string TITLE = "UnZip Files";
        public UnZipper()
        {
            InitializeComponent();
        }

        private void btnBrowseZipSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.CheckFileExists = true;
            dlg.Multiselect = false;
            dlg.Title = "Select zipped/storage file";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSourceFile.Text = dlg.FileName;
                if (string.IsNullOrEmpty(txtTargetFolder.Text))
                    txtTargetFolder.Text = Path.GetDirectoryName(txtSourceFile.Text);
                listBox2.Items.Clear();
            }
        }

        private void btn_browseSaveUnZippedFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select target folder for extracted files:";
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtTargetFolder.Text = dlg.SelectedPath;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_startUnZip_Click(object sender, EventArgs e)
        {
            // Previous checkings
            if (string.IsNullOrEmpty(txtSourceFile.Text))
            {
                MessageBox.Show("Storage filename not defined.", "UnZipper Error Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(txtTargetFolder.Text))
            {
                MessageBox.Show("Target folder not defined.", "UnZipper Error Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                // Opens existing zip file
                ZipStorer zip = ZipStorer.Open(txtSourceFile.Text, FileAccess.Read);

                // Read all directory contents
                List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
                this.listBox2.Items.Clear();

                // Extract all files in target directory
                string path;
                bool result;
                foreach (ZipStorer.ZipFileEntry entry in dir)
                {
                    path = Path.Combine(txtTargetFolder.Text, Path.GetFileName(entry.FilenameInZip));
                    result = zip.ExtractFile(entry, path);
                    this.listBox2.Items.Add(path + (result ? "" : " (error)"));
                }
                zip.Close();

                lblMessage.Text = "Source file processed with success.";
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("Error: Invalid or not supported Zip file.", "UnZipper Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("Error while processing source file.", "UnZipper Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
