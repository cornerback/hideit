using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HideIt.Transfer
{
    /// <summary>
    /// Holds that data that will be transfered during handshake
    /// </summary>
    /// <remarks>The class is serializable so it can be serialized
    /// and sent to receiver, where it can deserialized.</remarks>
    [Serializable]
    public class HandshakeData
    {
        private string _machinename;
        private int _filelen;
        private string _filename;

        /// <summary>
        /// Create a new Handshake data object
        /// </summary>        
        /// <param name="filename">Name of file to send</param>
        /// <param name="filelen">Length of file to send</param>
        public HandshakeData(string filename, int filelen)
        {
            this._machinename = Environment.MachineName.ToLower();
            this._filelen = filelen;
            this._filename = filename;
        }

        /// <summary>
        /// Get the machine name
        /// </summary>
        public string MachineName
        {
            get { return this._machinename; }
        }

        /// <summary>
        /// Get the length of data
        /// </summary>
        public int FileLen
        {
            get { return this._filelen; }
        }

        /// <summary>
        /// Get the name or id of data
        /// </summary>
        public string FileName
        {
            get { return this._filename; }
        }
    }
}
