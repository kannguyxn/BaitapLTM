using System;

namespace BaiTapLTM_Server
{
    public class Sanpham
    {
        
        public string Ten { get; set; }

        
        public string MoTa { get; set; }

        
        public int Gia { get; set; }

        
        public string HinhAnh { get; set; }

        
        public Sanpham(string ten, string mota, int gia, string hinhAnh)
        {
            Ten = ten;
            MoTa = mota;
            Gia = gia;
            HinhAnh = hinhAnh;
        }
    }
}