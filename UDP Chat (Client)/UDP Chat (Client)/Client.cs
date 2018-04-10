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

        // Default constructor
        public Client()
        {
            username = "";
            socket   = null;
            server   = null;
        }

        // Connection to server
        private void Connect(string address)
        {
            try
            {
                // Use UDP protocol
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                // Network address of the server
                server = new IPEndPoint(IPAddress.Parse(address), SERVER_PORT);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot login to chat with IP address {address}.", ex);
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
                throw new Exception($"Cannot login to chat with IP address {serverIP}.", ex);
            }
        }


        // Send message to the server
        private void SendMessage(Data message, EndPoint receiver)
        {
            byte[] buffer = message.ToBytes();

            // Send datagram to the server
            socket.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, server,
                new AsyncCallback(OnSend), server);
        }

        private void OnSend(IAsyncResult result)
        {
            try
            {
                socket.EndSend(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "UDP Chat", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
