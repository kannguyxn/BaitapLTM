using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BaiTapLTM
{


    public partial class Form1 : Form
    {
        private TcpClient? client;
        private NetworkStream? stream;

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


            KhoiTaoGiaoDien();

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
                lblGoiY.Text = "Hết thời gian!";
                lblGoiY.ForeColor = Color.Red;

                
                if (stream != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes("TIMEOUT");
                    stream.Write(data, 0, data.Length);
                }
            }
        }



        private void BtnDoan_Click(object sender, EventArgs e)
        {
            if (txtGiaDoan.Text == "")
                return;

            if (stream == null)
            {
                MessageBox.Show("Chưa kết nối tới Server!");
                return;
            }

            string message = "GUESS|" + txtGiaDoan.Text;

            byte[] data = Encoding.UTF8.GetBytes(message);

            stream.Write(data, 0, data.Length);
        }

        private void KetNoiServer()
        {
            try
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 8888);

                stream = client.GetStream();

                Thread thread = new Thread(NhanDuLieu);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NhanDuLieu()
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int len = stream!.Read(buffer, 0, buffer.Length);

                    if (len == 0)
                        break;

                    string message =
                        Encoding.UTF8.GetString(buffer, 0, len);

                    this.Invoke(new Action(() =>
                    {
                        XuLyDuLieu(message);
                    }));
                }
                catch
                {
                    break;
                }
            }
        }


        private void XuLyDuLieu(string message)
        {
            string[] data = message.Split('|');

            switch (data[0])
            {
                case "PRODUCT":

                    lblTenSp.Text = data[1];
                    lblMoTaSp.Text = data[2];

                    string duongDan = Path.Combine(
                        Application.StartupPath,
                        "Images",
                        data[3]);

                    if (File.Exists(duongDan))
                    {
                        if (picSanPham.Image != null)
                        {
                            picSanPham.Image.Dispose();
                            picSanPham.Image = null;
                        }

                        picSanPham.Image = Image.FromFile(duongDan);
                    }
                    else
                    {
                        picSanPham.Image = null;
                    }

                    txtGiaDoan.Clear();
                    lblGoiY.Text = "Nhập giá rồi nhấn ĐOÁN!";

                    gameTimer.Stop();
                    thoiGianConLai = 15;
                    lblTimerHienThi.Text = "Thời gian: 15s";
                    lblTimerHienThi.ForeColor = Color.Yellow;
                    gameTimer.Start();

                    break;

                case "HIGHER":
                    lblGoiY.Text = "⬆ Giá thật CAO HƠN!";
                    break;

                case "LOWER":
                    lblGoiY.Text = "⬇ Giá thật THẤP HƠN!";
                    break;

                case "WIN":
                    gameTimer.Stop();
                    lblStatus.Text = "🎉 Bạn đoán đúng!";
                    break;

                case "LOSE":
                    gameTimer.Stop();
                    lblStatus.Text = "❌ Bạn hết lượt!";
                    break;

                case "GAMEOVER":

                    gameTimer.Stop();

                    int p1 = int.Parse(data[1]);
                    int p2 = int.Parse(data[2]);

                    string ketQua;

                    if (p1 > p2)
                    {
                        ketQua = $"🏆 Player 1 thắng!\n\nPlayer 1: {p1} điểm\nPlayer 2: {p2} điểm";
                    }
                    else if (p2 > p1)
                    {
                        ketQua = $"🏆 Player 2 thắng!\n\nPlayer 1: {p1} điểm\nPlayer 2: {p2} điểm";
                    }
                    else
                    {
                        ketQua = $"🤝 Hòa!\n\nPlayer 1: {p1} điểm\nPlayer 2: {p2} điểm";
                    }

                    MessageBox.Show(ketQua, "Kết quả");

                    Close();

                    break;
            }
        }
    }
}
   