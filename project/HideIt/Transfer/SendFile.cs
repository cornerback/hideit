using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HideIt.Transfer
{
    public partial class SendFile_Form : Form
    {
        /// <summary>
        /// Create a new send file form
        /// </summary>
        public SendFile_Form()
        {
            InitializeComponent();

            this.mtxt_port.Text = Options_Form.Port.ToString();
        }

        private void btn_browseFile_Click(object sender, EventArgs e)
        {
            this.txt_file.Text = AppUtil.SelectFile("Open file to send", "All files (*.*)|*.*", "");
        }

        /// <summary>
        /// Send the selected file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_send_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_file.Text))
            {
                UIMessage.Info("Please select a file to send", "Send file");
                return;
            }

            if (string.IsNullOrEmpty(this.txt_pcname.Text))
            {
                UIMessage.Info("Please provide a PC name or IP to which the file is to be sent", "Send file");
                return;
            }

            int port = 0;

            if(!int.TryParse(this.mtxt_port.Text, out port))
            {
                UIMessage.Error("Port specified contains invalid characters", "Send file");
                return;
            }

            FileSender fileSender = null;

            try
            {
                fileSender = new FileSender(this.txt_pcname.Text, port);
                
                ///Open the file in read mode
                Stream stream = new FileStream(this.txt_file.Text, FileMode.Open, FileAccess.Read);

                int length = (int)stream.Length;

                string filename = string.Empty;

                ///Lets extract the file name. First find the last index of
                /// '\' (the file path separator)
                int index = this.txt_file.Text.LastIndexOf('\\');

                ///And then get the substring fron that index
                filename = this.txt_file.Text.Substring(index + 1);

                ///Perform the handshake
                fileSender.Handshake(filename, length);

                ///Handshake successful ... send the file ...
                ///

                ///Setup the progress bar
                this.progress.Value = 0;
                this.progress.Maximum = length;

                int sent = 0;
                ///While there is still some data to send
                while (sent < length)
                {
                    ///We receive file in size of 1024 bytes chunks
                    int bufferSize = 1024;

                    ///If there is less then 1024 bytes to receive
                    if (length - sent < 1024)
                    {
                        bufferSize = length - sent;
                    }

                    byte[] buffer = new byte[bufferSize];

                    ///Read from file
                    stream.Read(buffer, 0, bufferSize);

                    ///Send the file
                    fileSender.Send(buffer);

                    sent += bufferSize;

                    this.progress.Increment(bufferSize);
                }
            }
            catch (Exception exc)
            {
                UIMessage.Error(exc.Message, "Send file");
                return;
            }
            finally
            {
                try
                {
                    if (fileSender != null)
                    {
                        fileSender.Dispose();
                    }
                }
                catch (Exception)
                {
                }
                //this.progress.Value = 0;
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
