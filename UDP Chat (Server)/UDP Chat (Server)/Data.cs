using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Chat__Server_
{
    // The comands for interaction between the server and the client
    public enum Command
    {
        Null,       // No command
        LogIn,      // Log into the chat
        LogOut,     // Log out of the chat
        SendMsg,    // Send message to all the chat users
        GetUsers    // Get list of the chat users
    }

    // The data structure by which the server and the client interact with each other
    public class Data
    {
        public string Username { get; set; }   // Login of the client
        public Command Command { get; set; }   // Command: LogIn, LogOut, SendMsg, GetUsers or Null
        public string Message { get; set; }    // Message text

        // Default constructor
        public Data()
        {
            Username = "";
            Command = Command.Null;
            Message = "";
        }

        // Default constructor
        public Data(string username, Command command, string message)
        {
            Username = username;
            Command = command;
            Message = message;
        }

        // Creates an object of type Data from the received array of bytes
        public Data(byte[] data)
        {
            // The first byte contains the command index
            byte commandIndex = data[0];

            // 1..4 bytes contain the length of the Username
            int usernameLen = BitConverter.ToInt32(data, 1);

            // 5..8 bytes contain the length of the Message
            int messageLen = BitConverter.ToInt32(data, 5);

            Command = Enumerable.Range(0, Enum.GetNames(typeof(Command)).Length).Contains(commandIndex) ?
                (Command)commandIndex : Command.Null;
            Username = (usernameLen > 0) ? Encoding.UTF8.GetString(data, 9, usernameLen) : "";
            Message = (messageLen > 0) ? Encoding.UTF8.GetString(data, 9 + usernameLen, messageLen) : "";
        }

        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            // The first byte contains the command index
            result.Add((byte)Command);

            // 1..4 bytes contain the length of the Username
            if (Username != null)
                result.AddRange(BitConverter.GetBytes(Username.Length));
            else
                result.AddRange(BitConverter.GetBytes(0));

            // 5..8 bytes contain the length of the Message
            if (Message != null)
                result.AddRange(BitConverter.GetBytes(Message.Length));
            else
                result.AddRange(BitConverter.GetBytes(0));

            // Add the username
            if (Username != null)
                result.AddRange(Encoding.UTF8.GetBytes(Username));

            // Add the message text
            if (Message != null)
                result.AddRange(Encoding.UTF8.GetBytes(Message));

            return result.ToArray();
        }
    }
}
