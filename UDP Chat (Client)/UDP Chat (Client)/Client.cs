using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace UDP_Chat__Client_
{
    public class Client
    {
        private string username;   // The username of the client
        private Socket socket;     // Provides communication with server 
        private EndPoint server;   // Network address of the server

        // Server is listening on port 11000
        private const int SERVER_PORT = 11000;

        // True if user is connected to the server
        public bool IsConnected
        {
            get
            {
                // Socket.Poll returns true if connection isn't active or
                // connection is active and there is data available for writing
                bool isActive = socket.Poll(11000, SelectMode.SelectWrite);

                // Socket.Available returns number of bytes available for reading
                bool isAvailable = (socket.Available == 0);
                
                if (isActive && isAvailable) 
                    return true;
                else
                    return false;
            }
        }

        // Default constructor
        public Client()
        {
            username = "";
            socket = null;
            server = null;
        }

        // Connection to server
        private void Connect(string address)
        {
            // Use UDP protocol
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // Network address of the server
            IPAddress serverIP;
            if (IPAddress.TryParse(address, out serverIP))
            {
                server = new IPEndPoint(IPAddress.Parse(address), SERVER_PORT);
            }
            else
            {
                OnExceptionReport(new ArgumentException($"Cannot login to the chat with IP address {address}."));
            }
        }

        // Connect to the server and send login request
        public void Login(string name, string serverIP)
        {
            // Set username of the client
            username = name;

            try
            {
                // Connect to the server 
                Connect(serverIP);

                // Send login request to the server
                Data message = new Data(username, Command.LogIn, "");
                SendMessage(message, server);
            }
            catch (Exception ex)
            {
                OnExceptionReport(ex);
            }
        }


        // Send message to the server
        private void SendMessage(Data message, EndPoint receiver)
        {
            byte[] buffer = message.ToBytes();

            try
            {
                // Send datagram to the server
                socket.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, server,
                    new AsyncCallback(OnSend), server);
            }
            catch (SocketException socketException)
            {
                OnExceptionReport(socketException);
            }
            catch (Exception ex)
            {
                OnExceptionReport(ex);
            }
        }

        public event EventHandler<ExceptionReportEventArgs> ExceptionReport;

        private void OnExceptionReport(Exception exception)
        {
            EventHandler<ExceptionReportEventArgs> handler = ExceptionReport;
            handler(this, new ExceptionReportEventArgs(exception));
        }

        private void OnSend(IAsyncResult result)
        {
            try
            {
                socket.EndSend(result);
            }
            catch (SocketException socketEx)
            {
                // if the server isn't running, we'll get a socket exception here
                OnExceptionReport(socketEx);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                OnExceptionReport(ex);
            }
        }
    }

    public class ExceptionReportEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public ExceptionReportEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
