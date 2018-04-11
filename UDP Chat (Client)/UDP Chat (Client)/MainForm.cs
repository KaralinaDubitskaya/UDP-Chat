using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP_Chat__Client_
{
    public partial class MainForm : Form
    {
        private Client user;

        public MainForm(Client client)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            user = client;

            // Window's caption
            this.Text = "UDP Chat | " + user.Username;

            // Request the server to send the names of all users of the chat room
            user.RequestUsers();

            user.RemoveUserDelegate = RemoveUser;
            user.AddUserDelegate = AddUser;
            user.SetUsersDelegate = SetUsers;
            user.AddMessageDelegate = AddMessage;
        }

        // Send the message to the server
        private void btnSend_Click(object sender, EventArgs e)
        {
            user.SendMessage(tbMessage.Text);
            tbMessage.Clear();
        }

        public delegate void AddUserDelegate(string name);
        public delegate void AddMessageDelegate(string message);
        public delegate void RemoveUserDelegate(string name);
        public delegate void SetUsersDelegate(string[] users);

        private void AddUser(string name)
        {
            tbUsers.Items.Add(name);
        }

        private void AddMessage(string message)
        {
            tbChat.Text += message + "\r\n";
        }

        private void RemoveUser(string name)
        {
            tbUsers.Items.Remove(name);
        }

        private void SetUsers(string[] users)
        {
            tbUsers.Items.Clear();
            tbUsers.Items.AddRange(users);
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            if (tbMessage.Text.Length == 0)
                btnSend.Enabled = false;
            else
                btnSend.Enabled = true;
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(sender, null);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to leave the chat room?", "UDP Chat | " + user.Username,
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            user.LogOut();
        }
    }
}
