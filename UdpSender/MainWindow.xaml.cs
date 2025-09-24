using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UdpSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ip = IpTextBox.Text;
                var port = int.Parse(PortTextBox.Text);
                var message = MessageTextBox.Text;

                using (var udpClient = new UdpClient())
                {
                    var sendBytes = Encoding.UTF8.GetBytes(message);
                    udpClient.Send(sendBytes, sendBytes.Length, ip, port);
                }

                StatusTextBlock.Text = "メッセージを送信しました。";
                StatusTextBlock.Foreground = Brushes.Green;
            }
            catch(Exception ex)
            {
                StatusTextBlock.Text = "エラーが発生しました: " + ex.Message;
                StatusTextBlock.Foreground = Brushes.Red;
            }
        }
    }
}