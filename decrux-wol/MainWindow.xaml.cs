using System;
using System.Globalization;
using System.Net.Sockets;
using System.Windows;

namespace decrux_wol
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const int Port = 9;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WakePc(object sender, RoutedEventArgs e)
        {

            String ipAddress = IpAddressTextBox.Text;
            String macAddress = MacAddressTextBox.Text;

            byte[] macAddressBytes = TransformMacAddress2Bytes(macAddress);
            byte[] bytes = new byte[6 + 16 * macAddressBytes.Length];

            for (int i = 0; i < 6; i++)
            {
                bytes[i] = 0xff;
            }

            for (int i = 6; i < bytes.Length; i += macAddressBytes.Length)
            {
                Array.Copy(macAddressBytes, 0, bytes, i, macAddressBytes.Length);
            }
            
            UdpClient client = new UdpClient();

            // TODO make sure to implement functionality for resolving hostnames e.g. www.decrux.io/decrux.io to IP Addresses.
            //Dns.GetHostAddresses();
            
            client.Send(bytes, bytes.Length, ipAddress, Port);
            client.Close();
        }

        private byte[] TransformMacAddress2Bytes(String macAddress)
        {
            byte[] bytes = new byte[6];
            
            // TODO use regex to split a MAC address by ":" or "-"
            // Right now this returns as elements the split character too and breaks stuff.
            //String[] hex = Regex.Split(macAddress, "(\\:|\\-)");
            
            String[] hex = macAddress.Split("-");
            for (int i = 0; i < 6; i++)
            {
                bytes[i] = (byte)int.Parse(hex[i], NumberStyles.HexNumber);
            }

            return bytes;
        }

    }
}