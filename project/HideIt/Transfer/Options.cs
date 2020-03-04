using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideIt.Transfer
{
    public partial class Options_Form : Form
    {
        /// <summary>
        /// Default port
        /// </summary>
        private static int _port = 19800;

        private FileReceiver _fileReceiver = null;

        public Options_Form(FileReceiver receiver)
        {
            InitializeComponent();

            this._fileReceiver = receiver;
            this.mtxt_port.Text = _port.ToString();
        }

        /// <summary>
        /// Get the port on which the application will listen
        /// </summary>
        public static int Port
        {
            get { return _port; }
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            int port = _port;
            if (!int.TryParse(this.mtxt_port.Text, out port))
            {
                UIMessage.Error("Port specified contains invalid characters", "Options");
                return;
            }

            if (port < 1000 || port > 65535)
            {
                UIMessage.Error("Port must be between 1000-65535", "Options");
                return;
            }

            ///if port specified is different from the previous one
            if (port != _port)
            {
                try
                {
                    this._fileReceiver.Restart(port);
                }
                catch (Exception exc)
                {
                    UIMessage.Error(exc.Message, "Options");
                    return;
                }

                _port = port;
            }

            ///Close the form
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
