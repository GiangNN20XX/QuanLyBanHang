using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //Sử dụng thư viện để làm việc SQL server
using QuanLyBanHang.Class; //Sử dụng class ChucNang.cs



namespace WindowsFormsApp_QuanLyBanHang
{
    public partial class frmPhanLoai: Form
    {
        DataTable tblPL;
        public frmPhanLoai()
        {
            InitializeComponent();
        }

        private void frmPhanLoai_Load(object sender, EventArgs e)
        {
            txtMaPhanLoai.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            TaiDuLieuDangLuoi();
        }

        private void TaiDuLieuDangLuoi()
        {
            string sql;
            sql = "SELECT MaPhanLoai, TenLoaiHangHoa FROM tblPhanLoai";
            tblPL = QuanLyBanHang.Class.ChucNang.LayDuLieuVaoBang(sql); //Đọc dữ liệu từ bảng
            dgvPhanLoai.DataSource = tblPL; //Nguồn dữ liệu            
            dgvPhanLoai.Columns[0].HeaderText = "Mã phân loại";
            dgvPhanLoai.Columns[1].HeaderText = "Tên loại hàng hóa";
            dgvPhanLoai.Columns[0].Width = 100;
            dgvPhanLoai.Columns[1].Width = 300;
            dgvPhanLoai.AllowUserToAddRows = false; //Không cho người dùng thêm dữ liệu trực tiếp
            dgvPhanLoai.EditMode = DataGridViewEditMode.EditProgrammatically; //Không cho sửa dữ liệu trực tiếp
        }

        private void dgvPhanLoai_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaPhanLoai.Focus();
                return;
            }
            if (tblPL.Rows.Count == 0) //Nếu không có dữ liệu
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMaPhanLoai.Text = dgvPhanLoai.CurrentRow.Cells["MaPhanLoai"].Value.ToString();
            txtLoaiHangHoa.Text = dgvPhanLoai.CurrentRow.Cells["TenLoaiHangHoa"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ThietLapLaiGiaTri(); //Xoá trắng các textbox
            txtMaPhanLoai.Enabled = true; //cho phép nhập mới
            txtMaPhanLoai.Focus();
        }

        private void ThietLapLaiGiaTri()
        {
            txtMaPhanLoai.Text = "";
            txtLoaiHangHoa.Text = "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql; //Lưu lệnh sql
            if (txtMaPhanLoai.Text.Trim().Length == 0) //Nếu chưa nhập mã chất liệu
            {
                MessageBox.Show("Bạn phải nhập mã phân loại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaPhanLoai.Focus();
                return;
            }
            if (txtLoaiHangHoa.Text.Trim().Length == 0) //Nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn phải nhập tên loại hàng hóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLoaiHangHoa.Focus();
                return;
            }
            sql = "Select MaPhanLoai From tblPhanLoai where MaPhanLoai=N'" + txtMaPhanLoai.Text.Trim() + "'";
            if (QuanLyBanHang.Class.ChucNang.KiemTraKhoa(sql))
            {
                MessageBox.Show("Mã phân loại này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaPhanLoai.Focus();
                return;
            }

            sql = "INSERT INTO tblPhanLoai VALUES(N'" +
                txtMaPhanLoai.Text + "',N'" + txtLoaiHangHoa.Text + "')";
            QuanLyBanHang.Class.ChucNang.ChaySQL(sql); //Thực hiện câu lệnh sql
            TaiDuLieuDangLuoi(); //Nạp lại DataGridView
            ThietLapLaiGiaTri();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaPhanLoai.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql; //Lưu câu lệnh sql
            if (tblPL.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaPhanLoai.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtLoaiHangHoa.Text.Trim().Length == 0) //nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn chưa nhập tên phân loại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sql = "UPDATE tblPhanLoai SET TenLoaiHangHoa=N'" +
                txtLoaiHangHoa.Text.ToString() +
                "' WHERE MaPhanLoai=N'" + txtMaPhanLoai.Text + "'";
            QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
            TaiDuLieuDangLuoi();
            ThietLapLaiGiaTri();

            btnBoQua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblPL.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaPhanLoai.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Kiểm tra xem phân loại có đang được sử dụng trong tblHang không
            string maPL = txtMaPhanLoai.Text.Trim();
            sql = "SELECT COUNT(*) FROM tblHang WHERE MaPhanLoai = N'" + maPL + "'";
            int count = (int)QuanLyBanHang.Class.ChucNang.LayGiaTri(sql);

            if (count > 0)
            {
                MessageBox.Show("Không thể xoá vì đang có hàng thuộc phân loại này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE FROM tblPhanLoai WHERE MaPhanLoai = N'" + maPL + "'";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
                TaiDuLieuDangLuoi();
                ThietLapLaiGiaTri();
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ThietLapLaiGiaTri();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaPhanLoai.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaPhanLoai_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
    }
}
