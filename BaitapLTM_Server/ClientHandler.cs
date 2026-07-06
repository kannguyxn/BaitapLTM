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

        public ClientHandler(TcpClient tcpClient)
        {
            client = tcpClient;
            stream = client.GetStream();
            game = new GameManager();
        }

        public void XuLyClient()
        {
            try
            {
                GuiSanPham();

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
            catch (Exception ex)
            {
                Console.WriteLine("Client ngắt kết nối: " + ex.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        private void XuLyLenh(string message)
        {
            Console.WriteLine("Nhan: " + message);
            string[] data = message.Split('|');

            if (data[0] == "GUESS")
            {
                int giaDoan = int.Parse(data[1]);

                string ketQua = game.KiemTraGia(giaDoan);

                switch (ketQua)
                {
                    case "CORRECT":
                        Gui("WIN");

                        game.SanPhamTiepTheo();

                        if (game.KetThucGame())
                        {
                            Gui("END");
                        }
                        else
                        {
                            GuiSanPham();
                        }
                        break;

                    case "HIGHER":
                        Gui("HIGHER");
                        break;

                    case "LOWER":
                        Gui("LOWER");
                        break;

                    case "NEXT":
                        Gui("LOSE");
                        GuiSanPham();
                        break;

                    case "END":
                        Gui("END");
                        break;
                }
            }
            else if (data[0] == "TIMEOUT")
            {
                if (game.KetThucGame())
                {
                    Gui("END");
                }
                else
                {
                    GuiSanPham();
                }
            }
        }

        private void GuiSanPham()
        {
            Sanpham? sp = game.LaySanPham();

            if (sp == null)
            {
                Gui("END"); // hoặc log lỗi rõ ràng
                return;
            }

            string msg =
                $"PRODUCT|{sp.Ten}|{sp.MoTa}|{sp.HinhAnh}";

            Gui(msg);
        }

        private void Gui(string message)
        {
            Console.WriteLine("Gui: " + message);

            byte[] data = Encoding.UTF8.GetBytes(message);

            stream.Write(data, 0, data.Length);
        }
    }
}