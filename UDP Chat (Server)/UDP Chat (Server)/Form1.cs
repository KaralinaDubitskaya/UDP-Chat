using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP_Chat__Server_
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            SetMessage setMessage = PrintMessage;
            SetUsers setUsers = PrintUsers;

            Server server = new Server();

            server.ExceptionReport += (exceptionSender, args) =>
            {
                PrintMessage("Error: " + args.Exception.Message);
            };

            server.Listen(setMessage, setUsers);
        }

        public delegate void SetMessage(string message);
        public delegate void SetUsers(List<Client> users);

        private void PrintMessage(string message)
        {
            tbChat.Text += message;
        }

        private void PrintUsers(List<Client> clientList)
        {
            tbUsers.Clear();
            tbUsers.Text += "Users: \r\n\r\n";

            byte i = 1;
            foreach (Client client in clientList)
            {
                tbUsers.Text += $" {i}. {client.username}\r\n";
                i++;
            }
        }
    }
}
