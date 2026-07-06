using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
            Console.WriteLine("Port: 8888");
            Console.WriteLine("=================================");

            while (true)
            {
                Console.WriteLine("Dang cho ket noi...");

                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Da co Client ket noi.");

                ClientHandler handler = new ClientHandler(client);

                Thread thread = new Thread(handler.XuLyClient);

                thread.Start();
            }
        }
    }
}
     