using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BaiTapLTM_Server
{
    public class Server
    {
        private TcpListener? listener;

        private List<ClientHandler> players = new List<ClientHandler>();

        private GameManager game = new GameManager();

        public void Start()
        {
            listener = new TcpListener(IPAddress.Any, 8888);

            listener.Start();

            Console.WriteLine("=================================");
            Console.WriteLine("PRICE GUESS GAME SERVER");
            Console.WriteLine("Port: 8888");
            Console.WriteLine("Waiting for players...");
            Console.WriteLine("=================================");

            while (players.Count < 2)
            {
                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine($"Player {players.Count + 1} connected.");

                ClientHandler handler = new ClientHandler(
    client,
    game,
    this,
    players.Count + 1
);

                players.Add(handler);
            }

            Console.WriteLine();
            Console.WriteLine("=================================");
            Console.WriteLine("Both players connected.");
            Console.WriteLine("Game Started!");
            Console.WriteLine("=================================");

            foreach (ClientHandler player in players)
            {
                Thread thread = new Thread(player.XuLyClient);
                thread.IsBackground = true;
                thread.Start();
            }

            GuiTatCaSanPham();

        }
        public void KetThucGame()
        {
            string msg = game.KetQuaCuoi();

            foreach (ClientHandler player in players)
            {
                player.Gui(msg);
            }
        }
        public void GuiTatCa(string message)
        {
            foreach (ClientHandler player in players)
            {
                player.Gui(message);
            }
        }

        public void GuiTatCaSanPham()
        {
            ClientHandler.ResetTimeout();

            Sanpham? sp = game.LaySanPham();

            if (sp == null)
            {
                KetThucGame();
                return;
            }

            string msg =
                $"PRODUCT|{sp.Ten}|{sp.MoTa}|{sp.HinhAnh}";

            GuiTatCa(msg);
        }   

        public GameManager LayGame()
        {
            return game;
        }
    }
}