using System;

namespace BaiTapLTM_Server
{
    public class Sanpham
    {
        // Tên sản phẩm
        public string Ten { get; set; }

        // Mô tả sản phẩm
        public string MoTa { get; set; }

        // Giá thật của sản phẩm
        public int Gia { get; set; }

        // Tên file ảnh
        public string HinhAnh { get; set; }

        // Hàm khởi tạo
        public Sanpham(string ten, string mota, int gia, string hinhAnh)
        {
            Ten = ten;
            MoTa = mota;
            Gia = gia;
            HinhAnh = hinhAnh;
        }
    }
}