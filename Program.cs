using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpProof {
    class Program {
        static void Main(string[] args) {
            var listener = new UdpListener();
            listener.OnCommandRecieved += CommandReceieved;
            Console.ReadLine();
        }
        static void CommandReceieved(string command) {
            Console.WriteLine(command);
        }
    }

    class UdpListener {

        public delegate void CommandRecieved(string command);
        public CommandRecieved OnCommandRecieved;
        public int port = 52138;

        Thread receiveThread;

        public UdpListener() {
            OnCommandRecieved += (data) => { return; };
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        private void ReceiveData() {
            while (receiveThread.IsAlive) {
                UdpClient receivingUdpClient = new UdpClient(port);
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                OnCommandRecieved(Encoding.ASCII.GetString(receiveBytes));
                receivingUdpClient.Close();
            }
        }
    }
}
