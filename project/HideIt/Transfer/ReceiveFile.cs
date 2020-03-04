using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;

namespace HideIt.Transfer
{
    public partial class ReceiveFile_Form : Form
    {
        private FileReceiver _receiver;
        private Socket _client;
        private HandshakeData _data;

        public ReceiveFile_Form(FileReceiver receiver, Socket client, HandshakeData data)
        {
            InitializeComponent();

            this._receiver = receiver;
            this._client = client;
            this._data = data;
        }

        private void btn_savePath_Click(object sender, EventArgs e)
        {
            try
            {
                ///Ask user for a directory to save the file in
                using (SaveFileDialog saveFileDlg = new SaveFileDialog())
                {
                    saveFileDlg.AutoUpgradeEnabled = true;
                    saveFileDlg.Title = "Save file";
                    saveFileDlg.DefaultExt = "";
                    saveFileDlg.Filter = "All files (*.*)|*.*";

                    ///Set the file name, so the file is saved with same name
                    ///as the sender is sending
                    saveFileDlg.FileName = this._data.FileName;

                    if (saveFileDlg.ShowDialog() == DialogResult.OK)
                    {
                        using (Stream saveStream = saveFileDlg.OpenFile())
                        {
                            ///Setup the progress bar
                            this.progress.Maximum = this._data.FileLen;
                            this.progress.Value = 0;

                            int received = 0;
                            ///While there is still some data to receive
                            while (received < this._data.FileLen)
                            {
                                ///We receive file in size of 1024 bytes chunks
                                int bufferSize = 1024;

                                ///If there is less then 1024 bytes to receive
                                if (this._data.FileLen - received < 1024)
                                {
                                    bufferSize = this._data.FileLen - received;
                                }

                                byte[] buffer = new byte[bufferSize];

                                ///function is used to receive the buffer from the server, look that it is used both in server and client.
                                ///Receive data from client
                                SocketUtil.Receive(this._client, buffer);
                                
                                received += bufferSize;

                                ///Write that data to file
                                saveStream.Write(buffer, 0, buffer.Length);

                                ///Update the progress bar
                                this.progress.Increment(bufferSize);
                            }

                            saveStream.Close();
                        }

                        UIMessage.Info("File receive successful", "File receive");
                    }
                    else
                    {
                        ///Disconnect the client
                        this._receiver.DiconnectClient(this._client);
                    }
                }
            }
            catch (Exception exc)
            {
                UIMessage.Error(exc.Message, "Receive file");
            }

            this.Close();
        }
    }
}
