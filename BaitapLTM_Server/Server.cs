using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace BaiTapLTM_Server
{
    internal class Server
    {
        private TcpListener listener;
        private int soNguoiChoi = 0;

        private GameManager game = new GameManager();
        private List<ClientHandler> players = new List<ClientHandler>();

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

                players.Add(handler);
            }

            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.WriteLine("Both players connected!");
            Console.WriteLine("Game is starting...");
            Console.WriteLine("==================================");

            foreach (ClientHandler player in players)
            {
                Thread thread = new Thread(player.XuLyClient);
                thread.Start();
            }

            GuiTatCaSanPham();
        }

        private void GuiTatCaSanPham()
        {
            Sanpham? sp = game.LaySanPham();

            if (sp == null)
                return;

            string msg = $"PRODUCT|{sp.Ten}|{sp.MoTa}|{sp.HinhAnh}";

            foreach (ClientHandler player in players)
            {
                player.Gui(msg);
            }
        }
    }
}