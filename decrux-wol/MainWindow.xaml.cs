using System;
using System.Globalization;
using System.Net.NetworkInformation;
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

            String broadcastAddress = BroadcastAddressTextBox.Text.Trim();
            String macAddress = MacAddressTextBox.Text.Trim();

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
            
            client.Send(bytes, bytes.Length, broadcastAddress, Port);
            client.Close();
        }

        // TODO this is strictly a prototype method for pinging a host after
        // a magic packet has been sent using the broadcast address.
        // At least for now this is my way of "checking" if the host is awake.
        // This will be refactored ASAP.
        private void PingPc(object sender, RoutedEventArgs e)
        {
            String ipAddress = IpAddressTextBox.Text.Trim();
            Ping ping = new Ping();
            PingReply pingReply = ping.Send(ipAddress, 1000);
            MessageBox.Show(pingReply.Status.ToString());
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