using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DirectShowLib;


namespace HideIt
{
    /// <summary>
    /// Application utility class containing utility methods
    /// </summary>
    internal static class AppUtil
    {
        /// <summary>
        /// Convert a 32bit integer to its byte array equivalent
        /// </summary>
        /// <param name="data">32bit integer to be converted to byte</param>
        /// <returns>Byte array representing the 32-int</returns>
        public static byte[] ToByteArray(int data)
        {
            byte[] eqInt = new byte[4];

            ///An integet is 32bit. we first read the least significant 8 bits and 
            ///then shift right 8 bits to read the next 8 bits. Then we shift right 16 and 24 bit
            ///to read the next 8 bits
            eqInt[0] = (byte)data;
            eqInt[1] = (byte)(data >> 0x8);  //0x8 = 8
            eqInt[2] = (byte)(data >> 0x10); //0x10 = 16
            eqInt[3] = (byte)(data >> 0x18); //0x18 = 24

            return eqInt;
        }

        /// <summary>
        /// Convert a array of 4 bytes to 32bit integer
        /// </summary>
        /// <param name="data">Bytes to convert</param>
        /// <returns>32bit integer</returns>
        public static int FromByteArray(byte[] data)
        {
            int eqByt = 0;

            ///         An integer = 4 bytes (32 bits)
            ///         0 Integer = 00000000 00000000 00000000 00000000 bits
            ///         The left most are most significant and the right side bits are
            ///         least significant
            ///         
            ///         The last byte in data will containt the most significant byte and
            ///         the first byte will contain the most significant data
            ///         
            ///         Therefore we will place the most significant byte first for eq if
            ///         byte MSB = 00000101
            ///         eqByt = 00000000 00000000 00000000 00000101

            ///OR the most significant byte and then shift left 24 places. That will make
            ///the first part of 4byte integer
            eqByt = eqByt | data[3];
            eqByt <<= 0x8;

            ///Or the second most significant byte and shift then 16 places back
            eqByt = eqByt | data[2];
            eqByt <<= 0x8;

            ///Place the second least significant byte and shift them 8 places back
            eqByt = eqByt | data[1];
            eqByt <<= 0x8;

            ///The least significant byte
            eqByt = eqByt | data[0];

            return eqByt;
        }

        /// <summary>
        /// Show an open file dialog for user to open a bitmap file
        /// </summary>
        /// <param name="title">Title of file dialog box</param>
        /// <param name="defaultExt">Default filename extension</param>
        /// <param name="filter">Name filter string</param>
        /// <returns>Path of selected file</returns>
        public static string SelectFile(string title, string filter, string defaultExt)
        {
            ///We open a file dialog and setup some options for user
            ///to select the cover object (bitmap).
            ///We save the path of cover object
            using (OpenFileDialog fileDlg = new OpenFileDialog())
            {
                fileDlg.AutoUpgradeEnabled = true;
                fileDlg.CheckPathExists = true;
                fileDlg.CheckFileExists = true;
                fileDlg.Multiselect = false;
                fileDlg.Title = title;
                fileDlg.DefaultExt = defaultExt;
                fileDlg.Filter = filter;
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    return fileDlg.FileName;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Setup progress bar by setting its minimum and maximum range.
        /// </summary>
        /// <param name="bar">Instance of progress bar</param>
        /// <param name="min">Minimum value of range of progress bar</param>
        /// <param name="max">Maximum value of range of progress bar</param>
        public static void SetupProgressBar(ProgressBar bar, int min, int max)
        {
            if (bar == null)
            {
                return;
            }
            ///If it requires invocation
            if (bar.InvokeRequired)
            {
                bar.Invoke((MethodInvoker)(() =>
                    {
                        bar.Minimum = min;
                        bar.Maximum = max;
                        bar.Value = 0;
                    }));
            }
            else
            {
                bar.Minimum = min;
                bar.Maximum = max;
                bar.Value = 0;
            }
        }

        /// <summary>
        /// Increment the value of progress bar by specified value
        /// </summary>
        /// <param name="bar">Instance of progress bar</param>
        /// <param name="value">Amount to increment</param>
        public static void IncrementProgressBar(ProgressBar bar, int value)
        {
            if (bar == null)
            {
                return;
            }
            ///If it requires invocation
            if (bar.InvokeRequired)
            {
                bar.Invoke((MethodInvoker)(() =>
                {
                    bar.Increment(value);
                }));
            }
            else
            {
                bar.Increment(value);
            }
        }
    }

    internal static class DirectShowUtil
    {
        /// <summary>
        /// Connect two filters. If the filters cannot be connect, the method will try to 
        /// find intermediate filters.
        /// </summary>
        /// <param name="grapth">Instance of graph builder</param>
        /// <param name="src">Instance of upstream filter</param>
        /// <param name="dest">Instance of downstream filter</param>
        public static void ConnectFilters(IGraphBuilder grapth, IBaseFilter src, IBaseFilter dest)
        {
            if (src == null)
                throw new Exception("Source filter not found");
            if (dest == null)
                throw new Exception("Destination filter not found");

            IPin pOut = null;

            try
            {
                ///Get any the unconnected pins for upstream filter
                GetUnconnectedPin(src, out pOut, PinDirection.Output);

                ///Connect the upstream filter's pin to downstream filter
                ConnectFilters(grapth, pOut, dest);
            }
            finally
            {
                ///Dispose off the pin
                if (pOut != null)
                {
                    Marshal.ReleaseComObject(pOut);
                }
            }
        }

        /// <summary>
        /// Connect the upstrem filter's pin with downstream filter
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="pOut"></param>
        /// <param name="dest"></param>
        public static void ConnectFilters(IGraphBuilder graph, IPin pOut, IBaseFilter dest)
        {
            if (pOut == null)
                throw new Exception("Unable to find any output pin");

            IPin pIn = null;

            ///Get downstream filter's input pin
            GetUnconnectedPin(dest, out pIn, PinDirection.Input);

            try
            {
                ///Connect the pins
                if (graph.Connect(pOut, pIn) < 0)
                {
                    throw new Exception("Unable to connect selected pins");
                }
            }
            finally
            {
                ///Dispose off the input pin
                Marshal.ReleaseComObject(pIn);
            }
        }

        /// <summary>
        /// Get any unconnected pin of the filter
        /// </summary>
        /// <param name="src">Source filter</param>
        /// <param name="pOut">Unconnected pin</param>
        /// <param name="direction">Desired direction of unconnected pin</param>
        public static void GetUnconnectedPin(IBaseFilter src, out IPin pOut, PinDirection direction)
        {
            pOut = null;

            IEnumPins pEnum = null;
            IPin[] pIn = new IPin[1];

            ///Enumerate the filter's pins
            if (src.EnumPins(out pEnum) != 0)
            {
                throw new Exception("Unable to enumerate source filter");
            }

            try
            {
                while (pEnum.Next(1, pIn, IntPtr.Zero) == 0)
                {
                    PinDirection pInDir;
                    ///Query the direction of the pin (input or output)
                    pIn[0].QueryDirection(out pInDir);

                    ///If the direction matches the desired direction
                    if (pInDir == direction)
                    {
                        IPin temp = null;
                        ///See if the pin is already connected, skip it
                        if (pIn[0].ConnectedTo(out temp) >= 0)
                        {
                            Marshal.ReleaseComObject(temp);
                        }
                        else ///Pin is not connected to anyone, return it
                        {
                            pOut = pIn[0];
                            return;
                        }
                    }
                    Marshal.ReleaseComObject(pIn[0]);
                }
            }
            finally
            {
                ///Dispose the enumerator
                Marshal.ReleaseComObject(pEnum);
            }
            throw new Exception("Unable to find any output pin");
        }
    }

    /// <summary>
    /// A helper class to show information/error messages to user
    /// </summary>
    internal static class UIMessage
    {
        /// <summary>
        /// Show an information message box
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="caption">Caption</param>
        public static void Info(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show an error message box
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="caption">Caption</param>
        public static void Error(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show a warning message box
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="caption">Caption</param>
        public static void Warn(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Ask a question
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message">Message to display</param>
        /// <param name="caption">Caption</param>
        /// <returns>Return the result</returns>
        public static DialogResult Ask(IWin32Window owner, string message, string caption)
        {
            return MessageBox.Show(owner, message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }

    /// <summary>
    /// Handle the current status of application
    /// </summary>
    internal sealed class Status : IDisposable
    {
        public const string DEFUALT_STATUS = "Ready";

        /// <summary>
        /// Parent form instance
        /// </summary>
        private Form _parent;

        /// <summary>
        /// The control in which the status is to be shown.
        /// </summary>
        private Control _statusCtrl;

        /// <summary>
        /// Create a new status
        /// </summary>
        /// <param name="parent">Instance of parent form</param>
        /// <param name="ctrl">Control that show status</param>
        /// <param name="status">Status string</param>
        public Status(Form parent, Control ctrl, string status)
        {
            this._statusCtrl = ctrl;
            this._parent = parent;

            if (this._parent != null)
            {
                if (this._parent.InvokeRequired)
                {
                    this._parent.Invoke((MethodInvoker)(() => { this._parent.Cursor = Cursors.WaitCursor; }));
                }
                else
                {
                    this._parent.Cursor = Cursors.WaitCursor;
                }
            }

            ///If invoke is required
            if (this._statusCtrl.InvokeRequired)
            {
                this._statusCtrl.Invoke((MethodInvoker)(() => { this._statusCtrl.Text = status; }));
            }
            else
            {
                this._statusCtrl.Text = status;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Free up all the resources used
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this._parent != null)
                {
                    if (this._parent.InvokeRequired)
                    {
                        this._parent.Invoke((MethodInvoker)(() => { this._parent.Cursor = Cursors.Default; }));
                    }
                    else
                    {
                        this._parent.Cursor = Cursors.Default;
                    }
                }

                if (this._statusCtrl.InvokeRequired)
                {
                    this._statusCtrl.Invoke((MethodInvoker)(() => { this._statusCtrl.Text = DEFUALT_STATUS; }));
                }
                else
                {
                    this._statusCtrl.Text = DEFUALT_STATUS;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        #endregion
    }

    internal sealed class LockUI : IDisposable
    {
        private HideItUI _parent;

        public LockUI(HideItUI parent)
        {
            this._parent = parent;

            if (this._parent != null)
            {
                if (this._parent.InvokeRequired)
                {
                    this._parent.Invoke((MethodInvoker)(() => { this._parent.LockUIControls(); }));
                }
                else
                {
                    this._parent.LockUIControls();
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this._parent != null)
            {
                if (this._parent.InvokeRequired)
                {
                    this._parent.Invoke((MethodInvoker)(() => { this._parent.UnLockUIControls(); }));
                }
                else
                {
                    this._parent.UnLockUIControls();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Socket utility class
    /// </summary>
    internal static class SocketUtil
    {
        /// <summary>
        /// Receive data from socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">Buffer in which the data is received</param>
        public static void Receive(Socket socket, byte[] buffer)
        {
            int received = 0;

            ///While there is still some data to receive
            while (received < buffer.Length)
            {
                try
                {
                    ///Receive data
                    received += socket.Receive(buffer, received, buffer.Length - received, SocketFlags.None);
                }
                catch (ObjectDisposedException)
                {
                    throw new Exception("The socket connection is disposed");
                }
            }

            ///The buffer now contains the data received
        }

        /// <summary>
        /// Send the file
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="stream">Buffer to send</param>
        public static void Send(Socket socket, byte[] buffer)
        {
            int sent = 0;
            while (sent < buffer.Length)
            {
                try
                {
                    ///Send the data
                    sent += socket.Send(buffer, sent, buffer.Length - sent, SocketFlags.None);
                }
                catch (ObjectDisposedException)
                {
                    throw new Exception("The socket connection is disposed");
                }
            }
        }
    }

    /// <summary>
    /// Contains component guids
    /// </summary>
    /// Represents a globally unique identifier (GUID).
    /// A GUID is a 128-bit integer (16 bytes) that can be used across all computers and networks 
    /// wherever a unique identifier is required. Such an identifier has a very low probability of being duplicated.
    internal static class ComGuids
    {
        public static readonly Guid AviSplitterGuid = new Guid("1B544C20-FD0B-11CE-8C63-00AA0044B51E");///{1B544C22-FD0B-11CE-8C63-00AA0044B51E} 
        public static readonly Guid AviMultiplexerGuid = new Guid("E2510970-F137-11CE-8B67-00AA00A3F1A6");
        public static readonly Guid FileReaderFilterGuid = new Guid("e436ebb5-524f-11ce-9f53-0020af0ba770");
        public static readonly Guid FileWriterFilterGuid = new Guid("8596E5F0-0DA5-11D0-BD21-00A0C911CE86");
        public static readonly Guid NullRendererGuid = new Guid("C1F400A4-3F08-11D3-9F0B-006008039E37");
        public static readonly Guid AviDecompressorGuid = new Guid("CF49D4E0-1115-11CE-B03A-0020AF0BA770");

        public static readonly Guid MediaDetGuid = new Guid("65BD0711-24D2-4FF7-9324-ED2E5D3ABAFA");
        public static readonly Guid VideoFormatGuid = new Guid("05589f80-c356-11ce-bf01-00aa0055595a");

        //CLSID_SystemDeviceEnum
        public static readonly Guid DevEnumGuid = new Guid("62BE5D10-60EB-11d0-BD3B-00A0C911CE86");
        //CLSID_VideoInputDeviceCategory
        public static readonly Guid VidCapGuid = new Guid("860BB310-5D01-11d0-BD3B-00A0C911CE86");

        //CLSID for FilterGraph
        public static readonly Guid FilterGraph = new Guid("e436ebb3-524f-11ce-9f53-0020af0ba770");

        //CLSID for CaptureGraph2
        public static readonly Guid CaptureGraph2 = new Guid("BF87B6E1-8C27-11d0-B3F0-00AA003761C5");

        /// <summary>
        /// Get the instance of filter from its GUID
        /// </summary>
        /// <param name="filterGuid">Filter GUID</param>
        /// <returns>Instance of desired filter</returns>
        public static IBaseFilter GetFilterFromGuid(Guid filterGuid)
        {
            Type type = Type.GetTypeFromCLSID(filterGuid);
            object filter = Activator.CreateInstance(type);

            return filter as IBaseFilter;
        }
    }
}