using System.Collections.Generic;

namespace BaiTapLTM_Server
{
    public class GameManager
    {
        // Danh sách sản phẩm
        public List<Sanpham> DanhSachSanPham { get; private set; }

        // Vị trí sản phẩm hiện tại
        public int ViTriHienTai { get; private set; }

        // Điểm của người chơi
        public int Diem { get; private set; }
        public int SoLuotConLai { get; private set; }

        public GameManager()
        {
            DanhSachSanPham = new List<Sanpham>();

            // Thêm sản phẩm
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
                "binh.png"));

            ViTriHienTai = 0;
            Diem = 0;
            SoLuotConLai = 5;
        }

        // Lấy sản phẩm hiện tại
        public Sanpham LaySanPham()
        {
            if (ViTriHienTai >= DanhSachSanPham.Count)
                return null;

            return DanhSachSanPham[ViTriHienTai];
        }
        private int soLanDoanConLai = 3;

        public int SoLanDoanConLai
        {
            get { return soLanDoanConLai; }
        }
        public void SanPhamTiepTheo()
        {
            ViTriHienTai++;
            soLanDoanConLai = 3;
        }

        // Kiểm tra giá đoán
        public string KiemTraGia(int giaDoan)
        {
            Sanpham? sp = LaySanPham();

            if (sp == null)
                return "END";

            if (giaDoan == sp.Gia)
            {
                Diem += 10;
                ViTriHienTai++;
                SoLuotConLai = 5;
                return "CORRECT";
            }

            SoLuotConLai--;

            if (SoLuotConLai <= 0)
            {
                ViTriHienTai++;
                SoLuotConLai = 5;

                if (KetThucGame())
                    return "END";

                return "NEXT";
            }

            if (giaDoan < sp.Gia)
                return "HIGHER";

            return "LOWER";
        }

        // Chuyển sang sản phẩm tiếp theo
        
        
            public void SanPhamTiepTheo()
        {
            ViTriHienTai++;
            Console.WriteLine("ViTriHienTai = " + ViTriHienTai);
        }
        

        // Kiểm tra kết thúc game
        public bool KetThucGame()
        {
            return ViTriHienTai >= DanhSachSanPham.Count;
        }
    }
}