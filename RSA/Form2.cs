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
    public partial class Form2 : Form
    {
        private long eKey;
        private long nKey;
        public Form2(long e, long n)
        {
            InitializeComponent();
            this.eKey = e;
            this.nKey = n;
        }
        private void Form2_Load_1(object sender, EventArgs e)
        {
            // HIỂN THỊ PUBLIC KEY TRONG TEXTBOX 1
            textBox1.Text = $"e = {eKey}; n = {nKey}";
        }

        // Thuật toán bình phương và nhân tính lũy thừa modulo nhanh (M^e mod n)
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
            // LẤY VAN BAN RO TU TEXTBOX 2
            string vanBanRo = textBox2.Text;
            if (string.IsNullOrEmpty(vanBanRo)) return;

            List<string> danhSachMa = new List<string>();

            foreach (char c in vanBanRo)
            {
                long m = (long)c; // Đổi ký tự chữ sang số nguyên ASCII

                // Thực hiện mã hóa lý thuyết: C = M^e mod n
                long cChar = BinhPhuongVaNhan(m, eKey, nKey);
                danhSachMa.Add(cChar.ToString());
            }

            // XUẤT KẾT QUẢ CIPHERTEXT RA TEXTBOX 3 (Các số cách nhau bằng khoảng trắng)
            textBox3.Text = string.Join(" ", danhSachMa);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //Chỉ cho phép người dùng nhìn thấy và chọn file text (.txt)
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.Title = "Chọn file Plaintext để mã hóa";

            // Nếu người dùng chọn file và bấm OK
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Đọc tất cả các ký tự, dòng chữ có trong file
                    string noiDungFile = File.ReadAllText(openFileDialog1.FileName);

                    // Đổ nội dung vừa đọc được vào ô nhập Plaintext (textBox3 của bro)
                    textBox3.Text = noiDungFile;

                    MessageBox.Show("Đọc dữ liệu từ file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Báo lỗi nếu file đang bị lỗi hoặc ứng dụng không có quyền đọc
                    MessageBox.Show("Không thể đọc file! Lỗi: " + ex.Message, "Lỗi đọc file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
