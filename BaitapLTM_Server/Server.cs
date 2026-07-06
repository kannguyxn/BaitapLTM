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
        private int soNguoiChoi = 0;

        private GameManager game = new GameManager();

        public void Start()
        {
            listener = new TcpListener(IPAddress.Any, 8888);

            listener.Start();

            Console.WriteLine("=================================");
            Console.WriteLine("SERVER DANG CHAY");
            Console.WriteLine("Port: 8888");
            Console.WriteLine("=================================");

            while (soNguoiChoi < 2)
            {
                Console.WriteLine($"Waiting Player {soNguoiChoi + 1}...");

                TcpClient client = listener.AcceptTcpClient();

                soNguoiChoi++;

                Console.WriteLine($"Player {soNguoiChoi} connected.");

                ClientHandler handler = new ClientHandler(client, game);

                Thread thread = new Thread(handler.XuLyClient);

                thread.Start();
            }

            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.WriteLine("Both players connected!");
            Console.WriteLine("Game is starting...");
            Console.WriteLine("==================================");
        }
    }
}
     