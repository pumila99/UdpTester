using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        UdpClient udpClient;
        bool isListening = false;
        readonly int port = 54321;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void ListenForUdpMessages()
        {
            try
            {
                udpClient = new UdpClient(port);

                var remoteEP = new IPEndPoint(IPAddress.Any, 0);

                while (isListening)
                {
                    //データを非同期で受信
                    var recvBytes = udpClient.Receive(ref remoteEP);
                    var hexString = BitConverter.ToString(recvBytes).Replace("-", " ");

                    //UIスレッドでテキストボックスに表示
                    Dispatcher.Invoke(() =>
                    {
                        ReceivedTextBox.AppendText($"送信元: {remoteEP.Address}:{remoteEP.Port}\n");
                        ReceivedTextBox.AppendText($"メッセージ(バイナリ): {hexString}\n\n");
                        ReceivedTextBox.ScrollToEnd(); //常に最新のメッセージが見えるようにする
                    });
                }
            }
            catch (SocketException ex)
            {
                // エラー処理
                this.Dispatcher.Invoke(() => MessageBox.Show($"ソケットエラー：{ex.Message}"));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(() => MessageBox.Show($"エラー：{ex.Message}"));
            }
            throw new NotImplementedException();
        }


        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isListening) return;

            isListening = true;
            ReceivedTextBox.Text = $"ポート {port} で受信待機中...\n";

            await Task.Run(() => ListenForUdpMessages());

        }
    }
}
