using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UDP_Chat__Client_
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            btnSend.IsEnabled = false;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            // Create and login the user
            Client user = new Client();

            user.ExceptionReport += (exceptionSender, args) =>
            {
                MessageBox.Show("Error: " + args.Exception.Message, "UDP Chat",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            };
            
            // Connect to the server
            user.Login(tbUsername.Text, tbIP.Text);

            if (user.IsConnected == true)
            {
                // Show main window of the chat
                Window MainWindow = new MainWindow();
                MainWindow.Show();

                // Close the registration form
                Close();
            }
            else
            {
                tbIP.Text = "";
                btnSend.IsEnabled = false;
                Show();
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            if ((tbUsername.Text.Length > 0) && (tbIP.Text.Length > 0))
            {
                btnSend.IsEnabled = true;
            }
            else
            {
                btnSend.IsEnabled = false;
            }
        }
    }
}
