using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace RSA
{
    public partial class Form1 : Form
    {
        private Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
        }
        // 1. Hàm kiểm tra số nguyên tố
        private bool KiemTraNguyenTo(long so)
        {
            if (so <= 1) return false;
            if (so == 2) return true;
            if (so % 2 == 0) return false;
            for (long i = 3; i * i <= so; i += 2)
            {
                if (so % i == 0) return false;
            }
            return true;
        }

        // 2. Thuật toán Euclid tìm GCD
        private long TimGCD(long a, long b)
        {
            while (b != 0)
            {
                long tam = a % b;
                a = b;
                b = tam;
            }
            return a;
        }

        // 3. Thuật toán Euclid mở rộng tìm số nghịch đảo d = e^-1 mod Phi(n)
        private long NghichDaoModulo(long eKey, long phiN)
        {
            long t = 0; long newt = 1;
            long r = phiN; long newr = eKey;

            while (newr != 0)
            {
                long thuong = r / newr;

                long tamT = t;
                t = newt;
                newt = tamT - thuong * newt;

                long tamR = r;
                r = newr;
                newr = tamR - thuong * newr;
            }

            if (r > 1) 
                return -1; // Không tồn tại nghịch đảo
            if (t < 0) 
                t = t + phiN;

            return t;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                long p = long.Parse(textBox1.Text);
                long q = long.Parse(textBox2.Text);

                if (!KiemTraNguyenTo(p) || !KiemTraNguyenTo(q))
                {
                    MessageBox.Show("Số p hoặc q nhập vào không phải số nguyên tố!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                long n = p * q; // n = p * q
                long phiN = (p - 1) * (q - 1); // phi(n) = (p-1)*(q-1)

                textBox3.Text = n.ToString();
                textBox4.Text = phiN.ToString();
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số nguyên cho p và q!", "Thông báo");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                long phiN = long.Parse(textBox4.Text);
                long eKey = long.Parse(textBox5.Text);

                if (TimGCD(eKey, phiN) != 1) // Điều kiện nguyên tố cùng nhau
                {
                    MessageBox.Show("Số e không hợp lệ! GCD(e, Phi(n)) phải bằng 1.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                long d = NghichDaoModulo(eKey, phiN); // Tính d
                if (d == -1)
                {
                    MessageBox.Show("Không tìm thấy số nghịch đảo modulo hợp lệ cho e!", "Lỗi");
                }
                else
                {
                    textBox6.Text = d.ToString();
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng tính toán n và Phi(n) trước khi tính d!", "Thông báo");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Vui lòng cấu hình đầy đủ Khóa công khai trước!", "Thông báo");
                return;
            }

            long eKey = long.Parse(textBox5.Text);
            long nKey = long.Parse(textBox3.Text);

            // Gọi Form2 và truyền e, n qua hàm khởi tạo
            Form2 formMaHoa = new Form2(eKey, nKey);
            formMaHoa.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox6.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Vui lòng cấu hình đầy đủ Khóa bí mật trước!", "Thông báo");
                return;
            }

            long dKey = long.Parse(textBox6.Text);
            long nKey = long.Parse(textBox3.Text);

            // Gọi Form3 và truyền d, n qua hàm khởi tạo
            Form3 formGiaiMa = new Form3(dKey, nKey);
            formGiaiMa.ShowDialog();
        }

        // Random p,q
        private void button5_Click(object sender, EventArgs e)
        {
            long p = 0;
            long q = 0;

            // Vòng lặp random p cho đến khi tìm được số nguyên tố
            // Giới hạn từ 50 đến 500 để tích n = p * q đủ lớn bao trọn bảng ASCII mà không lo tràn kiểu long
            while (true)
            {
                long tamP = rand.Next(50, 500);
                if (KiemTraNguyenTo(tamP))
                {
                    p = tamP;
                    break;
                }
            }

            // Vòng lặp random q cho đến khi tìm được số nguyên tố và phải khác p
            while (true)
            {
                long tamQ = rand.Next(50, 500);
                if (KiemTraNguyenTo(tamQ) && tamQ != p)
                {
                    q = tamQ;
                    break;
                }
            }

            // Hiển thị số vừa random được lên ô textBox1 và textBox2
            textBox1.Text = p.ToString();
            textBox2.Text = q.ToString();

            // Tự động kích hoạt luôn hàm tính n và Phi(n) để người dùng đỡ phải bấm nút button1
            button1_Click(sender, e);
        }

        //random e
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem đã có Phi(n) chưa
                if (string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Vui lòng tính hoặc ngẫu nhiên P, Q để có Phi(n) trước!", "Thông báo");
                    return;
                }

                long phiN = long.Parse(textBox4.Text);
                long eKey = 0;

                // Vòng lặp tìm số e nguyên tố cùng nhau với Phi(n)
                // Thông thường trong thực tế người ta hay chọn các số lẻ nhỏ hoặc quét ngẫu nhiên
                while (true)
                {
                    // Chọn ngẫu nhiên e trong khoảng từ 3 đến phiN
                    long tamE = rand.Next(3, (int)Math.Min(phiN, 5000));

                    // Điều kiện bắt buộc lý thuyết RSA: GCD(e, Phi(n)) phải bằng 1
                    if (TimGCD(tamE, phiN) == 1)
                    {
                        eKey = tamE;
                        break;
                    }
                }

                // Hiển thị số e tìm được lên textBox5
                textBox5.Text = eKey.ToString();

                // Tự động kích hoạt luôn hàm tính d (button2) cho tiện luôn bro nhé
                button2_Click(sender, e);
            }
            catch
            {
                MessageBox.Show("Đã xảy ra lỗi khi tính toán số e!", "Lỗi");
            }
        }

        //Reset các số được nhập vào
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; // Xóa p
            textBox2.Text = ""; // Xóa q
            textBox5.Text = ""; // Xóa e
            textBox3.Text = ""; // Xóa n
            textBox4.Text = ""; // Xóa Phi(n)
            textBox6.Text = ""; // Xóa d
        }
    }
}
