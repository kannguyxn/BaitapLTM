using System;
using System.Net.Sockets;
using System.Text;

namespace BaiTapLTM_Server
{
    public class ClientHandler
    {
        private TcpClient client;
        private NetworkStream stream;
        private GameManager game;
        private Server server;

        public ClientHandler(TcpClient tcpClient, GameManager gameManager, Server gameServer)
        {
            client = tcpClient;
            stream = client.GetStream();

            game = gameManager;
            server = gameServer;
        }

        public void XuLyClient()
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    XuLyLenh(message);
                }
            }
            catch
            {
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        private void XuLyLenh(string message)
        {
            string[] data = message.Split('|');

            if (data[0] != "GUESS")
                return;

            int giaDoan = int.Parse(data[1]);

            string ketQua = game.KiemTraGia(giaDoan);

            switch (ketQua)
            {
                case "CORRECT":

                    Gui("WIN");

                    if (game.KetThucGame())
                    {
                        server.GuiTatCa("END");
                    }
                    else
                    {
                        game.SanPhamTiepTheo();

                        server.GuiTatCa("NEXT");

                        server.GuiTatCaSanPham();
                    }

                    break;

                case "HIGHER":

                    Gui("HIGHER");

                    break;

                case "LOWER":

                    Gui("LOWER");

                    break;

                case "NEXT":

                    server.GuiTatCa("NEXT");

                    server.GuiTatCaSanPham();

                    break;

                case "END":

                    server.GuiTatCa("END");

                    break;
            }
        }

        public void Gui(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            stream.Write(data, 0, data.Length);
        }
    }
}