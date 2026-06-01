using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RSA
{
    public partial class Form3 : Form
    {
        private long dKey;
        private long nKey;
        public Form3(long d, long n)
        {
            InitializeComponent();
            this.dKey = d;
            this.nKey = n;
        }

        private void Form3_Load_1(object sender, EventArgs e)
        {
            // HIỂN THỊ PRIVATE KEY TRONG TEXTBOX 1
            textBox1.Text = $"d = {dKey}; n = {nKey}";
        }

        private long BinhPhuongVaNhan(long coSo, long soMu, long modulo)
        {
            if (modulo == 1) return 0;
            long ketQua = 1;
            coSo = coSo % modulo;

            while (soMu > 0)
            {
                if (soMu % 2 == 1)
                {
                    ketQua = (ketQua * coSo) % modulo;
                }
                soMu = soMu >> 1;
                coSo = (coSo * coSo) % modulo;
            }
            return ketQua;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // LẤY CHUỖI MẬT MÃ TỪ TEXTBOX 2
            string chuoiMatMa = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(chuoiMatMa)) return;

            try
            {
                // Tách các cụm số phân tách bởi dấu khoảng trắng
                string[] cacCumSo = chuoiMatMa.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string vanBanGoc = "";

                foreach (string cumSo in cacCumSo)
                {
                    long c = long.Parse(cumSo);

                    // Thực hiện giải mã lý thuyết: M = C^d mod n
                    long m = BinhPhuongVaNhan(c, dKey, nKey);

                    vanBanGoc += (char)m; // Ép số nguyên ngược lại về ký tự chữ
                }

                // XUẤT VĂN BẢN GỐC THU ĐƯỢC RA TEXTBOX 3
                textBox3.Text = vanBanGoc;
            }
            catch
            {
                MessageBox.Show("Định dạng chuỗi mật mã nhập vào không đúng (phải là các số cách nhau bởi khoảng trắng)!", "Lỗi định dạng");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Khởi tạo hộp thoại chọn file của Windows
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Cấu hình bộ lọc: Chỉ cho phép chọn file text (.txt)
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.Title = "Chọn file chứa chuỗi mật mã (Ciphertext)";

            // Nếu người dùng chọn file và bấm OK
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Đọc toàn bộ nội dung chuỗi số trong file text (.txt)
                    string noiDungFile = File.ReadAllText(openFileDialog1.FileName).Trim();

                    // Đổ dữ liệu chuỗi số đó vào ô ở giữa (textBox2 của bro)
                    textBox2.Text = noiDungFile;

                    MessageBox.Show("Đọc chuỗi mật mã từ file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Báo lỗi nếu hệ thống không đọc được file
                    MessageBox.Show("Không thể đọc file! Lỗi: " + ex.Message, "Lỗi đọc file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
