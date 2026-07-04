using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;

namespace BaiTapLTM
{
    public class SanPham
    {
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public int Gia { get; set; }
        public string TenFileAnh { get; set; }

        public SanPham(string ten, string moTa, int gia, string tenFileAnh)
        {
            Ten = ten;
            MoTa = moTa;
            Gia = gia;
            TenFileAnh = tenFileAnh;
        }
    }

    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private List<SanPham> danhSachSp;
        private int indexHienTai = 0;
        private int soLuotDoan = 5;
        private int diemSo = 0;
        private Random rand = new Random();

        
        private System.Windows.Forms.Timer gameTimer;
        private int thoiGianConLai = 15;

        
        private Label lblTitle;
        private GroupBox groupBoxSp;
        private Label lblTenSp;
        private Label lblMoTaSp;
        private PictureBox picSanPham;
        private Label lblYeuCau;
        private TextBox txtGiaDoan;
        private Button btnDoan;
        private Label lblGoiY;
        private Label lblStatus;
        private Label lblTimerHienThi;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Ứng dụng Game Hãy Chọn Giá Đúng - Bản Nâng Cấp";
            this.Size = new Size(650, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;

            KhoiTaoDuLieu();
            KhoiTaoGiaoDien();
            HienThiSanPham();
            KetNoiServer();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                Color.FromArgb(255, 81, 47),
                Color.FromArgb(240, 152, 25),
                45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void KhoiTaoDuLieu()
        {
            danhSachSp = new List<SanPham>()
            {
                new SanPham("Nồi chiên không dầu Philips", "Dung tích 4.1L, Công nghệ Rapid Air", 2200000, "noichien.png"),
                new SanPham("Bình giữ nhiệt Lock&Lock", "Thép không gỉ, dung tích 500ml", 350000, "binhgiunhiet.png"),
                new SanPham("Bàn phím cơ Logitech G213", "Đèn nền RGB, phím giả cơ chống nước", 1100000, "banphim.png"),
                new SanPham("Tai nghe không dây Sony WH-CH520", "Thời lượng pin lên đến 50 giờ", 1200000, "tainghe.png"),
                new SanPham("Chuột máy tính Logitech MX Master 3S", "Độ phân giải 8000 DPI, siêu mượt", 2500000, "chuot.png")
            };

            int n = danhSachSp.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                SanPham value = danhSachSp[k];
                danhSachSp[k] = danhSachSp[n];
                danhSachSp[n] = value;
            }
        }

        private void KhoiTaoGiaoDien()
        {
            lblTitle = new Label();
            lblTitle.Text = "HÃY CHỌN GIÁ ĐÚNG";
            lblTitle.Font = new Font("Arial", 26, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Size = new Size(630, 45);
            lblTitle.Location = new Point(0, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            lblTimerHienThi = new Label();
            lblTimerHienThi.Text = "Thời gian: 15s";
            lblTimerHienThi.Font = new Font("Arial", 13, FontStyle.Bold);
            lblTimerHienThi.ForeColor = Color.Yellow;
            lblTimerHienThi.BackColor = Color.FromArgb(100, 0, 0, 0);
            lblTimerHienThi.Size = new Size(160, 30);
            lblTimerHienThi.Location = new Point(245, 65);
            lblTimerHienThi.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTimerHienThi);

            groupBoxSp = new GroupBox();
            groupBoxSp.Text = " Sản phẩm cần đoán giá ";
            groupBoxSp.Font = new Font("Arial", 11, FontStyle.Bold);
            groupBoxSp.ForeColor = Color.FromArgb(60, 60, 60);
            groupBoxSp.Size = new Size(580, 275);
            groupBoxSp.Location = new Point(25, 105);
            groupBoxSp.BackColor = Color.FromArgb(240, 255, 255, 255);
            this.Controls.Add(groupBoxSp);

            lblTenSp = new Label();
            lblTenSp.Font = new Font("Arial", 15, FontStyle.Bold);
            lblTenSp.ForeColor = Color.FromArgb(211, 47, 47);
            lblTenSp.BackColor = Color.Transparent;
            lblTenSp.Size = new Size(560, 30);
            lblTenSp.Location = new Point(10, 25);
            lblTenSp.TextAlign = ContentAlignment.MiddleCenter;
            groupBoxSp.Controls.Add(lblTenSp);

            picSanPham = new PictureBox();
            picSanPham.Size = new Size(180, 130);
            picSanPham.Location = new Point(200, 60);
            picSanPham.BorderStyle = BorderStyle.None;
            picSanPham.BackColor = Color.White;
            picSanPham.SizeMode = PictureBoxSizeMode.Zoom;
            groupBoxSp.Controls.Add(picSanPham);

            lblMoTaSp = new Label();
            lblMoTaSp.Font = new Font("Arial", 11, FontStyle.Regular);
            lblMoTaSp.ForeColor = Color.FromArgb(80, 80, 80);
            lblMoTaSp.BackColor = Color.Transparent;
            lblMoTaSp.Size = new Size(560, 40);
            lblMoTaSp.Location = new Point(10, 215);
            lblMoTaSp.TextAlign = ContentAlignment.MiddleCenter;
            groupBoxSp.Controls.Add(lblMoTaSp);

            lblYeuCau = new Label();
            lblYeuCau.Text = "Đoán giá của bạn (VNĐ):";
            lblYeuCau.Font = new Font("Arial", 12, FontStyle.Bold);
            lblYeuCau.ForeColor = Color.White;
            lblYeuCau.BackColor = Color.Transparent;
            lblYeuCau.Size = new Size(200, 30);
            lblYeuCau.Location = new Point(50, 410);
            lblYeuCau.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblYeuCau);

            txtGiaDoan = new TextBox();
            txtGiaDoan.Font = new Font("Arial", 13, FontStyle.Bold);
            txtGiaDoan.Size = new Size(160, 30);
            txtGiaDoan.Location = new Point(260, 410);
            txtGiaDoan.TextAlign = HorizontalAlignment.Center;
            txtGiaDoan.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnDoan_Click(null, null); };
            this.Controls.Add(txtGiaDoan);

            btnDoan = new Button();
            btnDoan.Text = "ĐOÁN!";
            btnDoan.Font = new Font("Arial", 11, FontStyle.Bold);
            btnDoan.BackColor = Color.FromArgb(255, 193, 7);
            btnDoan.ForeColor = Color.FromArgb(33, 33, 33);
            btnDoan.Size = new Size(90, 32);
            btnDoan.Location = new Point(435, 409);
            btnDoan.Cursor = Cursors.Hand;
            btnDoan.FlatStyle = FlatStyle.Flat;
            btnDoan.FlatAppearance.BorderSize = 0;
            btnDoan.Click += BtnDoan_Click;
            this.Controls.Add(btnDoan);

            lblGoiY = new Label();
            lblGoiY.Text = "Chúc bạn may mắn!";
            lblGoiY.Font = new Font("Arial", 14, FontStyle.Bold);
            lblGoiY.ForeColor = Color.Yellow;
            lblGoiY.BackColor = Color.Transparent;
            lblGoiY.Size = new Size(630, 30);
            lblGoiY.Location = new Point(0, 465);
            lblGoiY.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblGoiY);

            lblStatus = new Label();
            lblStatus.Font = new Font("Arial", 11, FontStyle.Bold);
            lblStatus.ForeColor = Color.White;
            lblStatus.BackColor = Color.FromArgb(60, 0, 0, 0);
            lblStatus.Size = new Size(650, 35);
            lblStatus.Location = new Point(0, 528);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblStatus);
        }

        private void HienThiSanPham()
        {
            if (indexHienTai < danhSachSp.Count)
            {
                SanPham sp = danhSachSp[indexHienTai];
                lblTenSp.Text = sp.Ten;
                lblMoTaSp.Text = sp.MoTa;

                string duongDanAnh = Path.Combine(Application.StartupPath, "Images", sp.TenFileAnh);
                if (File.Exists(duongDanAnh))
                {
                    picSanPham.Image = Image.FromFile(duongDanAnh);
                }
                else
                {
                    picSanPham.Image = null;
                }

                soLuotDoan = 5;
                thoiGianConLai = 15;
                lblTimerHienThi.Text = "Thời gian: 15s";
                lblTimerHienThi.ForeColor = Color.Yellow;
                gameTimer.Start();

                txtGiaDoan.Clear();
                lblGoiY.Text = "Nhanh lên, đồng hồ đang chạy!";
                lblGoiY.ForeColor = Color.Yellow;
                CapNhatThanhTrangThai();
                txtGiaDoan.Focus();
            }
            else
            {
                KetThucGame();
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            thoiGianConLai--;
            lblTimerHienThi.Text = $"Thời gian: {thoiGianConLai}s";

            if (thoiGianConLai <= 5)
            {
                lblTimerHienThi.ForeColor = Color.Red;
                Console.Beep(800, 150);
            }

            if (thoiGianConLai <= 0)
            {
                gameTimer.Stop();
                Console.Beep(400, 500);
                MessageBox.Show("Bạn đã hết thời gian suy nghĩ cho sản phẩm này!", "Hết giờ!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                indexHienTai++;
                HienThiSanPham();
            }
        }

        private void CapNhatThanhTrangThai()
        {
            lblStatus.Text = $"Sản phẩm: {indexHienTai + 1}/{danhSachSp.Count}   |   Số lượt còn lại: {soLuotDoan}   |   Điểm tích lũy: {diemSo} điểm";
        }

        private void BtnDoan_Click(object sender, EventArgs e)
        {
            string chuoiNhap = txtGiaDoan.Text.Trim().Replace(".", "").Replace(",", "");
            int giaDoan;

            if (!int.TryParse(chuoiNhap, out giaDoan) || giaDoan <= 0)
            {
                MessageBox.Show("Vui lòng chỉ nhập số nguyên dương hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SanPham spHienTai = danhSachSp[indexHienTai];
            int giaDung = spHienTai.Gia;
            soLuotDoan--;

            if (giaDoan == giaDung)
            {
                gameTimer.Stop();
                Console.Beep(1200, 300);
                diemSo += 10;
                MessageBox.Show($"Tuyệt vời! Giá đúng của {spHienTai.Ten} chính xác là {giaDung:N0} VNĐ.", "Chính xác!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                indexHienTai++;
                HienThiSanPham();
            }
            else if (giaDoan < giaDung)
            {
                Console.Beep(500, 200);
                lblGoiY.Text = "GIÁ THẬT CAO HƠN THẾ! ↑";
                lblGoiY.ForeColor = Color.Cyan;
            }
            else
            {
                Console.Beep(500, 200);
                lblGoiY.Text = "GIÁ THẬT THẤP HƠN THẾ! ↓";
                lblGoiY.ForeColor = Color.Black;
            }

            if (giaDoan != giaDung)
            {
                if (soLuotDoan <= 0)
                {
                    gameTimer.Stop();
                    MessageBox.Show($"Bạn đã dùng hết 5 lượt đoán. Giá đúng của sản phẩm này là: {giaDung:N0} VNĐ.", "Hết lượt đoán!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    indexHienTai++;
                    HienThiSanPham();
                }
                else
                {
                    CapNhatThanhTrangThai();
                    txtGiaDoan.Clear();
                    txtGiaDoan.Focus();
                }
            }
        }

        private void KetThucGame()
        {
            gameTimer.Stop();
            int tongDiemToiDa = danhSachSp.Count * 10;
            string danhHieu = "";

            if (diemSo == tongDiemToiDa) danhHieu = "Dân chơi phố cổ - Đoán bách phát bách trúng! 👑";
            else if (diemSo >= 30) danhHieu = "Tay hòm chìa khóa - Đi chợ siêu chuẩn! 💰";
            else if (diemSo >= 10) danhHieu = "Người tiêu dùng thông thái! 👍";
            else danhHieu = "Kẻ khờ khạo - Mua cái gì cũng bị hớ giá! 🤡";

            MessageBox.Show($"Bạn đã hoàn thành trò chơi!\n\nTổng điểm đạt được: {diemSo} / {tongDiemToiDa}\nDanh hiệu: {danhHieu}", "Tổng kết cuộc chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void KetNoiServer()
        {
            try
            {
                client = new TcpClient();

                client.Connect("127.0.0.1", 8888);

                stream = client.GetStream();

                byte[] buffer = new byte[1024];

                int len = stream.Read(buffer, 0, buffer.Length);

                string message = Encoding.UTF8.GetString(buffer, 0, len);

                lblStatus.Text = message;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được Server!\n" + ex.Message);
            }
        }
    }
}