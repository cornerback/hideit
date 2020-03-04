using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

namespace HideIt.Live
{
    /// <summary>
    /// Contains the capture devices information
    /// </summary>
    public sealed class Device : IDisposable
    {
        /// <summary>
        /// Create an instance of Device
        /// </summary>
        /// <param name="name"></param>
        /// <param name="moniker"></param>
        public Device(string name, IMoniker moniker)
        {
            this.Name = name;
            this.Moniker = moniker;
        }

        /// <summary>
        /// Get name of the device
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Get moniker instance
        /// </summary>
        public IMoniker Moniker { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.Moniker != null)
            {
                Marshal.ReleaseComObject(this.Moniker);
                this.Moniker = null;
            }
        }

        #endregion
    }
}
