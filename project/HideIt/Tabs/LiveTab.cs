using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HideIt.Live;
using System.Runtime.InteropServices.ComTypes;
using DirectShowLib;
using System.Runtime.InteropServices;
using HideIt.Stego.StegoProcessor;
using System.Drawing;
using System.Data.SqlClient;
using HideIt.Encryption;

namespace HideIt
{
    /// <summary>
    /// Contains Live tab methods for HideIt interface
    /// </summary>
    public partial class HideItUI : Form
    {
        private bool _initialized = false;
        private bool _isPlaying = false;
        private CaptureStegoProcess _liveProcessor;
        private HideIt.Stego.Message _message;

        /// <summary>
        /// Open a SelectDevice for user to select a input device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_selected_Click(object sender, EventArgs e)
        {
            if (this._selectDevice.CaptureDevice != null)
            {
                this._selectDevice.CaptureDevice.Dispose();
            }
            this._selectDevice.ShowDialog();            

            Device selectedDevice = this._selectDevice.CaptureDevice;
            if (selectedDevice != null)
            {
                this.txt_selected.Text = selectedDevice.Name;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!this._initialized)
            {                
                if (!this.Initialize())
                {
                    return;
                }
                
                this._initialized = true;                
            }

            if (this._initialized && !this._isPlaying)
            {
                if (this._message != null)
                {
                    this._message.Dispose();
                }

                RC4 rc4 = new RC4(this.txt_keyLive.Text, this.rtxt_liveMessage.Text);

                this._message = new HideIt.Stego.Message(this.txt_keyLive.Text, rc4.ApplyRC4());
                this._liveProcessor.StartCapturing(this._message, this.cb_stopAutomatically.Checked);
                this._isPlaying = true;
            }
            ///data inserrtion to maintain log table        
            string _stego_file_name = txt_saveCapturedPath.Text;
            string _key = txt_keyLive.Text;
            string _time = DateTime.Now.ToLongTimeString();
            string _date = DateTime.Now.ToShortDateString();
            SqlConnection conn = new SqlConnection(DB.DbConnectionStr.conect());
            SqlCommand query = new SqlCommand("insert into Log_Table values('" + _stego_file_name + "','" + _key + "', '" + _date + "', '" + _time + "')", conn);
            try
            {
                conn.Open();
                query.ExecuteNonQuery();
                ///MessageBox.Show("Record entered successfully.");
                conn.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Exception Accured" + ex, "Window Warning");
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (this._initialized)
            {
                this._liveProcessor.PauseCapturing();
                this._isPlaying = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (this._initialized)
            {
                this._liveProcessor.StopCapturing();
                this._isPlaying = false;

                this.txt_saveCapturedPath.Text = string.Empty;

                if (this._message != null)
                {
                    this._message.Dispose();
                }
                try
                {
                    this._liveProcessor.Dispose();
                }
                catch (Exception exc)
                {
                    UIMessage.Error(exc.Message, TITLE);
                }

                this._initialized = false;
            }
        }   
        ///An interface for obtaining an IObject given a moniker string. 
        ///A moniker can be used to obtain a particular object using a human-readable string to describe it, 
        ///rather than having to know the object's UUID. Human-readable strings are, unfortunately, not 
        ///guaranteed to be universally unique, so you might (theoretically) not get the object you want. 
        ///It is an object that implements the IMoniker interface. A moniker acts as a name that uniquely 
        ///identifies a COM object. In the same way that a path identifies a file in the file system, 
        ///a moniker identifies a COM object in the directory namespace. 
        ///A COM object that is used to create instances of other objects. Monikers save programmers time 
        ///when coding various types of COM-based functions such as linking one document to another (OLE). See COM and OLE.
        /// <summary>
        /// Start capturing
        /// </summary>
        private bool Initialize()
        {
            if (this._selectDevice == null)
            {
                return false;
            }

            if (this._selectDevice.CaptureDevice == null)
            {
                return false;
            }

            IMoniker moniker = this._selectDevice.CaptureDevice.Moniker;
            if (moniker == null)
            {
                UIMessage.Info("Please select a capturing device", TITLE);
                return false;
            }
            if (string.IsNullOrEmpty(this.txt_saveCapturedPath.Text))
            {
                UIMessage.Info("Please select a path to save captured video", TITLE);
                return false;
            }
            if (string.IsNullOrEmpty(this.txt_keyLive.Text))
            {
                UIMessage.Info("Key lenght must be atleast 4 characters and atmost 256 characters", TITLE);
                return false;
            }

            this._liveProcessor = new CaptureStegoProcess(moniker);
            this._liveProcessor.Setup(this.txt_saveCapturedPath.Text, this.vidOwner);

            return true;
        }

        private void btn_saveCaptured_Click(object sender, EventArgs e)
        {
            using (Status status = new Status(null, this.lbl_status, "Save captured video"))
            {
                ///Open a save dialog and ask user to save the stego object
                using (SaveFileDialog saveStego = new SaveFileDialog())
                {
                    saveStego.AutoUpgradeEnabled = true;
                    saveStego.Title = "Save captured video";
                    saveStego.Filter = "AVI (*.avi)|*.avi";                    
                    if (saveStego.ShowDialog() == DialogResult.OK)
                    {
                        this.txt_saveCapturedPath.Text = saveStego.FileName;
                    }
                }
            }
        }

        private void rtxt_liveMessage_TextChanged(object sender, EventArgs e)
        {
            if (this.lbl_val_msglen.InvokeRequired)
            {
                this.lbl_val_msglen.Invoke((MethodInvoker)(() =>
                    this.lbl_val_msglen.Text = this.rtxt_liveMessage.Text.Length.ToString()));
            }
            else
            {
                this.lbl_val_msglen.Text = this.rtxt_liveMessage.Text.Length.ToString();
            }
        }
    }
}
