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
        private Socket socket;     // Provides communication with server 
        private EndPoint server;   // Network address of the server

        // The username of the client
        public string Username { get; private set; }

        // Server is listening on port 11000
        private const int SERVER_PORT = 11000;

        // Buffer for received datagram
        private byte[] buffer;

        public MainForm.AddUserDelegate AddUserDelegate { get; set; }
        public MainForm.RemoveUserDelegate RemoveUserDelegate { get; set; }
        public MainForm.SetUsersDelegate SetUsersDelegate { get; set; }
        public MainForm.AddMessageDelegate AddMessageDelegate { get; set; }

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
        public Client(uint bufferSize = 1024)
        {
            Username = "";
            socket = null;
            server = null;

            // Max size in bytes of each datagram = bufferSize 
            buffer = new byte[bufferSize];
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
            Username = name;

            try
            {
                // Connect to the server 
                Connect(serverIP);

                // Send login request to the server
                Data message = new Data(Username, Command.LogIn, "");
                Send(message, server);
            }
            catch (Exception ex)
            {
                OnExceptionReport(ex);
            }
        }

        // Begins to asynchronously receive data from sender
        private void StartReceiveData()
        {
            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref server,
                    new AsyncCallback(OnReceive), null);
        }

        // Send message to the server
        public void SendMessage(string text)
        {
            Data message = new Data(Username, Command.SendMsg, text);
            Send(message, server);
        }

        // Request the server to send the names of all users of the chat room
        public void RequestUsers()
        {
            Data message = new Data("", Command.GetUsers, "");
            Send(message, server);

            StartReceiveData();
        }

        // Send message to the receiver
        private void Send(Data message, EndPoint receiver)
        {
            byte[] buffer = message.ToBytes();

            try
            {
                // Send datagram to the receiver
                socket.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, receiver,
                    new AsyncCallback(OnSend), receiver);
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

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                // Ends a pending asynchronous read from the server
                socket.EndReceiveFrom(ar, ref server);

                // The message received from the server
                Data message = new Data(buffer);

                switch (message.Command)
                {
                    case Command.LogIn:
                        {
                            AddUserDelegate(message.Username);
                            AddMessageDelegate(message.Message);
                            break;
                        }

                    case Command.LogOut:
                        {
                            RemoveUserDelegate(message.Username);
                            AddMessageDelegate(message.Message);
                            break;
                        }
                    case Command.GetUsers:
                        {
                            SetUsersDelegate(message.Message.Split('*'));
                            AddMessageDelegate($"*** {Username} ***");
                            break;
                        }
                }

                buffer = new byte[1024];
                StartReceiveData();
            }
            catch (Exception ex)
            {
                OnExceptionReport(ex);
            }
        }

        public void LogOut()
        {
            try
            {
                //Send a message to logout of the server
                Data message = new Data(Username, Command.LogOut, "");
                buffer = message.ToBytes();
                Send(message, server);

                socket.Close();
            }
            catch (ObjectDisposedException) {  }
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
