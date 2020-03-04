using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideIt.Transfer
{
    /// <summary>
    /// Contains the handshake response
    /// </summary>
    [Serializable]
    public sealed class HandshakeResponse
    {
        private bool _continue;

        /// <summary>
        /// Create a new handshake response and set the responsen data
        /// </summary>
        /// <param name="continueSending">Permits the sender to send data</param>
        public HandshakeResponse(bool continueSending)
        {
            this._continue = continueSending;
        }

        /// <summary>
        /// Get the flag to determine whether the sender can continue to send data
        /// or not
        /// </summary>
        public bool Continue
        {
            get { return this._continue; }
        }
    }
}
