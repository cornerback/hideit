using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HideIt.Transfer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FileSender : IDisposable
    {
        private Socket _socket;

        /// <summary>
        /// Create a new FileSender and connect with receiver
        /// </summary>
        /// When you create a socket there are three main parameters that you have to specify: 
        /// the domain , the type , the protocol 
        /// int socket(int domain, int type, int protocol)
        /// <param name="pc">Pc name or IP address to which the connection is to be made</param>
        /// <param name="port">Port at which the receiver would be listening</param>
        public FileSender(string pc, int port)
        {
            ///Create a new socket instance and set it up for TCP connection
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = null;
            ///Try to parse it. If it as IP address then ok, but if not then we have
            ///to resolve the name and get the ip address;
            if (!IPAddress.TryParse(pc, out address))
            {
                IPAddress[] addresses = Dns.GetHostAddresses(pc);
                if (addresses == null || addresses.Length == 0)
                {
                    throw new Exception("Unable to resolve pc name");
                }
                address = addresses[0];
            }

            ///Create an endpoint with which the connection will be made
            IPEndPoint endPoint = new IPEndPoint(address, port);

            ///Connect with the endpoint
            this._socket.Connect(endPoint);

            ///Set some options
            ///

            ///Nodelay specifies the use of Nagle algorithm. 
            ///When Nagle algorithm is enabled, it will waite for 200ms and 
            ///keep on accumalting all the data that client wants to send
            ///and after 200ms it sends all the data at once.
            ///If disabled the data is sent as soon as you write it on socket. If you
            ///are sending very small data (like 50-100 bytes) then this can slow performance
            ///as everytime it has to go through all the TCP process and send that data,
            ///but if Nagle is enabled, all these small chunks are accumlated and send
            ///after 200ms.
            ///As we are going to send large amount of data, we will disable it so it wont
            ///cause 200ms delay
            this._socket.NoDelay = false;

            ///Receive timeout. Receive operations will timeout after 30 seconds
            this._socket.ReceiveTimeout = 1200 * 1000;
        }

        /// <summary>
        /// Initialize handshake process and transfer initial information.
        /// Actual data will be sent after handshake
        /// </summary>
        /// <param name="filename">Name of file to send</param>
        /// <param name="len">Length of file</param>
        public void Handshake(string filename, int len)
        {
            HandshakeData handshake = new HandshakeData(filename, len);

            ///Lets now serialize the handshake data. Create a binary
            ///formatter which will help convert the serializable object
            ///into binary form
            BinaryFormatter formatter = new BinaryFormatter();

            ///Create a memory string that will hold the serialized data
            using (MemoryStream stream = new MemoryStream())
            {
                ///Now do the serialization of handshake data
                formatter.Serialize(stream, handshake);

                byte[] buffer = new byte[4];

                ///Send the length first
                buffer = AppUtil.ToByteArray((int)stream.Length);

                ///Send the length first
                SocketUtil.Send(this._socket, buffer);

                ///Send the data
                SocketUtil.Send(this._socket, stream.ToArray());
                
                ///Now get the handshake response
                HandshakeResponse response = this.GetHandshakeResponse();

                ///Close the stream as its not longer needed
                stream.Close();

                ///If casting failed
                if (response == null)
                {
                    throw new Exception("The handshake process with receiver failed. Receiver sent an invalid response");
                }

                ///Otherwise we check receivers response. If receiver denied receiving data...
                if (!response.Continue)
                {
                    throw new Exception("The destination computer denied to receive data");
                }
            }

            ///All is well and initial handshake process is completed.
            ///
        }

        /// <summary>
        /// Send data to destination computer
        /// </summary>
        /// <param name="buffer"></param>
        public void Send(byte[] buffer)
        {
            SocketUtil.Send(this._socket, buffer);
        }

        /// <summary>
        /// Get the response from server
        /// </summary>
        /// <returns></returns>
        private HandshakeResponse GetHandshakeResponse()
        {   
            byte[] buffer = new byte[4];

            ///First we receive the 4 bytes of data
            ///
            SocketUtil.Receive(this._socket, buffer);

            int incomingLen = AppUtil.FromByteArray(buffer);

            if (incomingLen <= 0)
            {
                throw new Exception("The handshake process with receiver failed. No response from the server received");
            }

            ///Creat a new buffer to receive handshake response bytes
            buffer = new byte[incomingLen];

            SocketUtil.Receive(this._socket, buffer);

            ///Now we create a binary formatter to deserialize the handshake response
            ///
            BinaryFormatter formatter = new BinaryFormatter();

            ///Create a stream containing the serialized bytes
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                ///Deserialize it and try to cast it in HandshakeResponse
                HandshakeResponse response = formatter.Deserialize(stream) as HandshakeResponse;

                ///Close the stream
                stream.Close();

                return response;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this._socket != null)
            {
                try
                {
                    ///Shutdown the socket after send/receive any pending data
                    this._socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)///catch and ignore
                {
                }

                ///Close the connection and free up resources used by it
                this._socket.Close();
            }
        }

        #endregion
    }
}
