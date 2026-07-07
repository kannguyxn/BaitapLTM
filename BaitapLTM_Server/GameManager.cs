using System.Collections.Generic;

namespace BaiTapLTM_Server
{
    public class GameManager
    {
        public List<Sanpham> DanhSachSanPham { get; private set; }

        public int ViTriHienTai { get; private set; }

        public int DiemPlayer1 { get; private set; }
        public int DiemPlayer2 { get; private set; }

        public int SoLuotConLai { get; private set; }

        private int soLanDoanConLai = 3;
        private bool daChuyenSanPham = false;
        public int SoLanDoanConLai
        {
            get { return soLanDoanConLai; }
        }

        public GameManager()
        {
            DanhSachSanPham = new List<Sanpham>();

            DanhSachSanPham.Add(new Sanpham(
                "Nồi chiên không dầu Philips",
                "Dung tích 4.1L - Công nghệ Rapid Air",
                2200000,
                "noichien.png"));

            DanhSachSanPham.Add(new Sanpham(
                "Tai nghe Sony WH-CH520",
                "Pin 50 giờ",
                1200000,
                "tainghe.png"));

            DanhSachSanPham.Add(new Sanpham(
                "Chuột Logitech MX Master 3S",
                "8000 DPI",
                2500000,
                "chuot.png"));

            DanhSachSanPham.Add(new Sanpham(
                "Bàn phím Logitech G213",
                "RGB Gaming",
                1100000,
                "banphim.png"));

            DanhSachSanPham.Add(new Sanpham(
                "Bình giữ nhiệt Lock&Lock",
                "500ml",
                350000,
                "binhgiunhiet.png"));

            ViTriHienTai = 0;
            DiemPlayer1 = 0;
            DiemPlayer2 = 0;
            SoLuotConLai = 5;
        }

        public Sanpham LaySanPham()
        {
            if (ViTriHienTai >= DanhSachSanPham.Count)
                return null;

            return DanhSachSanPham[ViTriHienTai];
        }

        public void SanPhamTiepTheo()
        {
            ViTriHienTai++;
            SoLuotConLai = 5;
            soLanDoanConLai = 3;
            ResetTrangThai();
        }

        public void CongDiem(int playerID)
        {
            if (playerID == 1)
                DiemPlayer1 += 10;
            else
                DiemPlayer2 += 10;
        }

        public string KiemTraGia(int giaDoan)
        {
            Sanpham? sp = LaySanPham();

            if (sp == null)
                return "END";

            if (giaDoan == sp.Gia)
            {
                return "CORRECT";
            }

            SoLuotConLai--;

            if (SoLuotConLai <= 0)
            {
                return "NEXT";
            }

            if (giaDoan < sp.Gia)
                return "HIGHER";

            return "LOWER";
        }
        public bool DaChuyenSanPham()
        {
            if (daChuyenSanPham)
                return true;

            daChuyenSanPham = true;
            return false;
        }

        public void ResetTrangThai()
        {
            daChuyenSanPham = false;
        }
        public bool KetThucGame()
        {
            return ViTriHienTai >= DanhSachSanPham.Count;
        }

        public string KetQuaCuoi()
        {
            return $"GAMEOVER|{DiemPlayer1}|{DiemPlayer2}";
        }
    }
}