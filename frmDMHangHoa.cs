using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_QuanLyBanHang
{
    public partial class frmDMHangHoa: Form
    {
        DataTable tblHH;
        public frmDMHangHoa()
        {
            InitializeComponent();
        }

        private void frmDMHangHoa_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblPhanLoai";
            txtMaHang.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            TaiDuLieuDangLuoi();
            QuanLyBanHang.Class.ChucNang.DienKetHop(sql, cbxMaPhanLoai, "MaPhanLoai", "TenLoaiHangHoa");
            cbxMaPhanLoai.SelectedIndex = -1;
            ThietLapLaiGiaTri();
        }

        private void ThietLapLaiGiaTri()
        {
            txtMaHang.Text = "";
            txtTenHang.Text = "";
            cbxMaPhanLoai.Text = "";
            txtSoLuong.Text = "0";
            txtDonGiaNhap.Text = "0";
            txtDonGiaBan.Text = "0";
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = false;
            txtDonGiaBan.Enabled = false;
            txtGhiChu.Text = "";
        }


        private void TaiDuLieuDangLuoi()
        {
            string sql;
            sql = "SELECT MaHang, TenHang, MaPhanLoai, SoLuong, DonGiaNhap, DonGiaBan, GhiChu FROM tblHang";
            tblHH = QuanLyBanHang.Class.ChucNang.LayDuLieuVaoBang(sql); // Đọc dữ liệu từ bảng
            dgvHangHoa.DataSource = tblHH; // Nguồn dữ liệu
            dgvHangHoa.Columns[0].HeaderText = "Mã hàng";
            dgvHangHoa.Columns[1].HeaderText = "Tên hàng";
            dgvHangHoa.Columns[2].HeaderText = "Mã phân loại";
            dgvHangHoa.Columns[3].HeaderText = "Số lượng";
            dgvHangHoa.Columns[4].HeaderText = "Đơn giá nhập";
            dgvHangHoa.Columns[5].HeaderText = "Đơn giá bán";
            dgvHangHoa.Columns[6].HeaderText = "Ghi chú";
            dgvHangHoa.Columns[0].Width = 100;
            dgvHangHoa.Columns[1].Width = 150;
            dgvHangHoa.Columns[2].Width = 100;
            dgvHangHoa.Columns[3].Width = 80;
            dgvHangHoa.Columns[4].Width = 100;
            dgvHangHoa.Columns[5].Width = 100;
            dgvHangHoa.Columns[6].Width = 200;
            dgvHangHoa.AllowUserToAddRows = false; // Không cho người dùng thêm dữ liệu trực tiếp
            dgvHangHoa.EditMode = DataGridViewEditMode.EditProgrammatically; // Không cho sửa dữ liệu trực tiếp
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ThietLapLaiGiaTri();
            txtMaHang.Enabled = true;
            txtMaHang.Focus();
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            txtDonGiaBan.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblHH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaHang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }
            if (txtTenHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenHang.Focus();
                return;
            }
            if (cbxMaPhanLoai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập phân loại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbxMaPhanLoai.Focus();
                return;
            }
            sql = "UPDATE tblHang SET TenHang=N'" + txtTenHang.Text.Trim().ToString() +
            "',MaPhanLoai=N'" + cbxMaPhanLoai.SelectedValue.ToString() +
            "',SoLuong=" + txtSoLuong.Text +
            ",GhiChu=N'" + txtGhiChu.Text + "' WHERE MaHang=N'" + txtMaHang.Text + "'";
            QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
            TaiDuLieuDangLuoi();
            ThietLapLaiGiaTri();
            btnBoQua.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }
            if (txtTenHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenHang.Focus();
                return;
            }
            if (cbxMaPhanLoai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập phân loại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbxMaPhanLoai.Focus();
                return;
            }
            sql = "SELECT MaHang FROM tblHang WHERE MaHang=N'" + txtMaHang.Text.Trim() + "'";
            if (QuanLyBanHang.Class.ChucNang.KiemTraKhoa(sql))
            {
                MessageBox.Show("Mã hàng này đã tồn tại, bạn phải chọn mã hàng khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }
            sql = "INSERT INTO tblHang(MaHang,TenHang,MaPhanLoai,SoLuong,DonGiaNhap, DonGiaBan,GhiChu) VALUES(N'"
            + txtMaHang.Text.Trim() + "',N'" + txtTenHang.Text.Trim() +
            "',N'" + cbxMaPhanLoai.SelectedValue.ToString() +
            "'," + txtSoLuong.Text.Trim() + "," + txtDonGiaNhap.Text +
            "," + txtDonGiaBan.Text + ",N'" + txtGhiChu.Text.Trim() + "')";

            QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
            TaiDuLieuDangLuoi();
            //ThietLapLaiGiaTri();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblHH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaHang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE tblHang WHERE MaHang=N'" + txtMaHang.Text + "'";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
                TaiDuLieuDangLuoi();
                ThietLapLaiGiaTri();
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ThietLapLaiGiaTri();
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
        }

        private void btnHienThiDS_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT MaHang,TenHang,MaPhanLoai,SoLuong,DonGiaNhap,DonGiaBan,GhiChu FROM tblHang";
            tblHH = QuanLyBanHang.Class.ChucNang.LayDuLieuVaoBang(sql);
            dgvHangHoa.DataSource = tblHH;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvHangHoa_Click(object sender, EventArgs e)
        {
            string MaPhanLoai;
            string sql;
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }
            if (tblHH.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMaHang.Text = dgvHangHoa.CurrentRow.Cells["MaHang"].Value.ToString();
            txtTenHang.Text = dgvHangHoa.CurrentRow.Cells["TenHang"].Value.ToString();
            MaPhanLoai = dgvHangHoa.CurrentRow.Cells["MaPhanLoai"].Value.ToString();
            sql = "SELECT TenLoaiHangHoa FROM tblPhanLoai WHERE MaPhanLoai=N'" + MaPhanLoai + "'";
            cbxMaPhanLoai.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(sql);
            txtSoLuong.Text = dgvHangHoa.CurrentRow.Cells["SoLuong"].Value.ToString();
            txtDonGiaNhap.Text = dgvHangHoa.CurrentRow.Cells["DonGiaNhap"].Value.ToString();
            txtDonGiaBan.Text = dgvHangHoa.CurrentRow.Cells["DonGiaBan"].Value.ToString();
            sql = "SELECT GhiChu FROM tblHang WHERE MaHang = N'" + txtMaHang.Text + "'";
            txtGhiChu.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(sql);
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }
    }
}
