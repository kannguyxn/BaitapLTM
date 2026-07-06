using System;
using System.Net.Sockets;
using System.Text;

namespace BaiTapLTM
{
    public class SocketClient
    {
        private TcpClient? client;
        private NetworkStream? stream;
        
        public bool Connect(string ip, int port)
        {
            try
            {
                client = new TcpClient();
                client.Connect(ip, port);
                stream = client.GetStream();
                return true;
            }
            catch
            {
                return false;
            }
        }

        
        public void Send(string message)
        {
            if (stream == null) return;

            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        
        public string Receive()
        {
            if (stream == null)
                return "";

            byte[] buffer = new byte[1024];

            int length = stream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer, 0, length);
        }

        
        public void Close()
        {
            stream?.Close();
            client?.Close();
        }
    }
}