using System.Windows;
using System.Windows.Media;

using Microsoft.Win32;

namespace DBHandler {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        BaseHandler myBase = new BaseHandler();

        public MainWindow() {
            InitializeComponent();
            myBase.Connected += MyBase_Connected;
            myBase.Disconected += MyBase_Disconected;
        }

        private void MyBase_Disconected() {
            Dispatcher.Invoke(() => {
                ConnectBT.IsEnabled = true;
                ConnectBT.Content = "Подключить";
                StateIndcator.Fill = new SolidColorBrush(new Color() { R = 200, G = 70, B = 70, A = 255 });
            });
        }

        private void MyBase_Connected() {
            Dispatcher.Invoke(() => {
                ConnectBT.IsEnabled = true;
                ConnectBT.Content = "Отключить";
                StateIndcator.Fill = new SolidColorBrush(new Color() { R = 70, G = 200, B = 70, A = 255 });
            });
        }

        private void ConnectBT_Click(object sender, RoutedEventArgs e) {
            ConnectBT.IsEnabled = false;
            if ( myBase.isConnected )
                myBase.Disconnect();
            else
                myBase.Connect();
        }

        private void UpdateBT_Click(object sender, RoutedEventArgs e) {

            dataCB.Items.Clear();
            foreach ( var item in myBase.GetFields() ) {
                dataCB.Items.Add(item);
            }
           
        }
    }
}
