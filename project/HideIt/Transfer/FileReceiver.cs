using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace HideIt.Transfer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FileReceiver : IDisposable
    {
        private Socket _socket;
        private HideItUI.AcceptFileCallback _acceptFile;

        /// <summary>
        /// Create a new file receiver
        /// </summary>
        /// <param name="acceptFile"></param>
        public FileReceiver(HideItUI.AcceptFileCallback acceptFile)
        {
            this._acceptFile = acceptFile;
        }

        /// <summary>
        /// Strat listening on the specified port
        /// </summary>
        /// <param name="port"></param>
        public void Listen(int port)
        {
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ///Communication endpoint. IPAddress.Any means that the socket will listen
            ///on all the available network cards on the machine.
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            ///Now bind with that endpoint
            this._socket.Bind(endPoint);

            ///Now start listening for incomming connections.
            this._socket.Listen(10);

            ///Accept incomming connections request
            this._socket.BeginAccept(new AsyncCallback(Accept), null);
        }

        /// <summary>
        /// Restart the socket on new port
        /// </summary>
        /// <param name="port"></param>
        public void Restart(int port)
        {
            ///Dispoes current socket
            this.Dispose();

            ///Start listening again
            this.Listen(port);
        }

        /// <summary>
        /// Called when socket accepts new connection
        /// </summary>
        /// Accept() is used with connection based sockets such as streams. 
        /// <param name="result"></param>
        private void Accept(IAsyncResult result)
        {
            try
            {
                ///Accept the incomming connections request and get the client socket
                ///for communication
                Socket client = this._socket.EndAccept(result);

                this._acceptFile.BeginInvoke(client, null, null);                
            }
            catch (ObjectDisposedException)///Catch and ignore
            {
            }
        }

        /// <summary>
        /// Perform handshake
        /// </summary>
        /// <remarks>Return true if client is asked to send
        /// more data, false otherwise</remarks>
        public HandshakeData Handshake(Socket client)
        {
            ///Receive the length
            byte[] buffer = new byte[4];

            SocketUtil.Receive(client, buffer);

            int incomingLen = AppUtil.FromByteArray(buffer);

            if (incomingLen <= 0)
            {
                ///Nothing to receive. Might be the connection is closing
                ///
                return null;
            }

            ///Create a buffer to receive data
            buffer = new byte[incomingLen];

            SocketUtil.Receive(client, buffer);

            HandshakeData handshake = null;

            ///Prepeare to deserialize data.
            ///
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                ///Deserialize
                handshake = formatter.Deserialize(stream) as HandshakeData;

                stream.Close();
            }

            ///Cannot deserialize to handshake data 
            if (handshake == null)
            {
                ///Close the connection
                ///
                this.DiconnectClient(client);
            }
            
            return handshake;
        }

        /// <summary>
        /// Send handshake response to client application
        /// </summary>
        /// <param name="continueSending">Instruct the client application whether to
        /// send the file or not</param>
        public void HandshakeResponse(Socket client, bool continueSending)
        {
            HandshakeResponse response = new HandshakeResponse(continueSending);

            ///Serialize the response to sender
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                ///Serialize the response
                formatter.Serialize(stream, response);

                byte[] buffer = new byte[4];

                ///Send the length first
                buffer = AppUtil.ToByteArray((int)stream.Length);

                ///The Send() function is used to send the buffer to the server 
                ///Send the length first
                SocketUtil.Send(client, buffer);

                ///Now send the actual data
                SocketUtil.Send(client, stream.ToArray());

                stream.Close();
            }
        }

        /// <summary>
        /// Disconnect client
        /// </summary>
        /// <param name="client"></param>
        public void DiconnectClient(Socket client)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Disconnect(false);
            client.Close();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this._socket != null)
            {
                if (this._socket.Connected)
                {
                    this._socket.Shutdown(SocketShutdown.Both);
                }
                this._socket.Close();
            }
        }

        #endregion
    }
}
