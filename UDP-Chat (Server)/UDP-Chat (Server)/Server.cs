using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Chat__Server_
{
    // Contains info about client connected to the server
    public struct Client
    {
        public string username;     // Username of the client
        public EndPoint endPoint;   // Network address of the client

        public Client(string name, EndPoint address)
        {
            username = name;
            endPoint = address;
        }
    }

    public class Server
    {
        // Listen for and accept connections from users 
        private Socket serverSocket;

        // Buffer for received datagram
        private byte[] buffer;

        // List of the clients logged into the chat
        private List<Client> clientList;

        // Server is listening on port 11000
        private const int SERVER_PORT = 11000;

        // Dafault constructor
        public Server(int bufferSize = 1024)
        {
            // Max size in bytes of each datagram = bufferSize 
            buffer = new byte[bufferSize];

            // List of the chat members
            clientList = new List<Client>();

            serverSocket = null;
        }

        public void Listen()
        {
            try
            {
                // Use UDP protocol
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // Represents the server endpoint as any local machine IP address and a port number SERVER_PORT
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, SERVER_PORT);

                // Associates the serverSocket with a serverEndPoint
                serverSocket.Bind(serverEndPoint);

                // Server listen for client activity on all network interfaces
                EndPoint sender = new IPEndPoint(IPAddress.Any, 0);

                // Begins to asynchronously receive data
                StartReceiveData(ref sender);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        // Begins to asynchronously receive data from sender
        private void StartReceiveData(ref EndPoint sender)
        {
            serverSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref sender,
                    new AsyncCallback(OnReceive), sender);
        }

        // Process received data
        private void OnReceive(IAsyncResult result)
        {
            try
            {
                // Ends a pending asynchronous read from the client
                EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                serverSocket.EndReceiveFrom(result, ref sender);

                // The message received from the client
                Data message = new Data(buffer);

                switch (message.Command)
                {
                    case Command.LogIn:
                        {
                            // Add the user to the chat room and get the response message
                            Data response = AddUser(message.Username, sender);

                            // Send message to all the users of the chat
                            SendMessage(response);

                            // Begins to asynchronously receive data
                            StartReceiveData(ref sender);

                            break;
                        }

                    case Command.LogOut:
                        {
                            // Remove the user from the chat room and get the response message
                            Data response = RemoveUser(sender);

                            // Send message to all the users of the chat
                            SendMessage(response);

                            break;
                        }
                    case Command.SendMsg:
                        {
                            // Construct the text of the message
                            Data response = SetMessage(message.Username, message.Message);

                            // Send message to all the users of the chat
                            SendMessage(response);

                            // Begins to asynchronously receive data
                            StartReceiveData(ref sender);

                            break;
                        }
                    case Command.GetUsers:
                        {
                            // Collect the names of the users of the chat room
                            Data response = GetUsers();

                            // Send response to the user
                            SendMessage(response, sender);

                            // Begins to asynchronously receive data
                            StartReceiveData(ref sender);

                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        // Ends a pending asynchronous send
        private void OnSend(IAsyncResult result)
        {
            try
            {
                serverSocket.EndSend(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        // Add the user to the clientList and set the response message
        private Data AddUser(string username, EndPoint sender)
        {
            Client client = new Client(username, sender);
            clientList.Add(client);

            // Set the response
            Data response = new Data();
            response.Message = "*** " + client.username + " has joined the chat ***";

            return response;
        }

        // Remove the user from the clientList and set the response message
        private Data RemoveUser(EndPoint sender)
        {
            foreach (Client client in clientList)
            {
                if (client.endPoint == sender)
                {
                    // Set the response
                    Data response = new Data();
                    response.Message = "*** " + client.username + " has left the chat ***";

                    clientList.Remove(client);
                    return response;
                }
            }
            return null;
        }

        // Set the response message
        private Data SetMessage(string username, string message)
        {
            Data response = new Data();
            response.Message = username + ": " + message;

            return response;
        }

        // Set the message with names of all users of the chat room
        private Data GetUsers()
        {
            Data response = new Data();
            response.Command = Command.GetUsers;

            // Asterics separates the usernames
            response.Message = String.Join("*", clientList);

            return response;
        }

        // Send message to the network address
        private void SendMessage(Data message, EndPoint receiver)
        {
            byte[] buffer = message.ToBytes();

            // Send datagram to the receiver
            serverSocket.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, receiver,
                new AsyncCallback(OnSend), receiver);
        }

        // Broadcast the message
        private void SendMessage(Data message)
        {
            byte[] buffer = message.ToBytes();

            // Send datagram to each user of the chat room
            foreach (Client user in clientList)
            {
                serverSocket.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, user.endPoint,
                    new AsyncCallback(OnSend), user);
            }

            // Print message to the console of the server
            PrintMessage(message);
        }

        private void PrintMessage(Data message)
        {
            Console.WriteLine(message.Message);
        }
    }
}
