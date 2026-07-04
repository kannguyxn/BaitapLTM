using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BaiTapLTM_Server
{
    internal class Server
    {
        private TcpListener listener;

        public void Start()
        {
            listener = new TcpListener(IPAddress.Any, 8888);

            listener.Start();

            Console.WriteLine("=================================");
            Console.WriteLine("SERVER DANG CHAY");
            Console.WriteLine("=================================");

            Console.WriteLine("Dang cho ket noi...");

            TcpClient client = listener.AcceptTcpClient();

            Console.WriteLine("Da ket noi");

            NetworkStream stream = client.GetStream();

            string message = "CONNECTED";

            byte[] data = Encoding.UTF8.GetBytes(message);

            stream.Write(data, 0, data.Length);

            Console.WriteLine("Da gui du lieu.");
        }
    }
}