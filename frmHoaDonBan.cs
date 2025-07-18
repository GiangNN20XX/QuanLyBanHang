using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_QuanLyBanHang
{
    public partial class frmHoaDonBan: Form
    {
        DataTable tblCTHDB;
        public frmHoaDonBan()
        {
            InitializeComponent();
        }

        private void frmHoaDonBan_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            txtMaHoaDon.ReadOnly = true;
            txtTenNhanVien.ReadOnly = true;
            txtTenKhachHang.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            mtbDienThoai.ReadOnly = true;
            txtTenHang.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txtGiamGia.Text = "0";
            txtTongTien.Text = "0";
            QuanLyBanHang.Class.ChucNang.DienKetHop("SELECT MaKhach, TenKhach FROM tblKhach", cboMaKhach, "MaKhach", "MaKhach");
            cboMaKhach.SelectedIndex = -1;
            QuanLyBanHang.Class.ChucNang.DienKetHop("SELECT MaNhanVien, TenNhanVien FROM tblNhanVien", cboMaNhanVien, "MaNhanVien", "MaNhanVien");
            cboMaNhanVien.SelectedIndex = -1;
            QuanLyBanHang.Class.ChucNang.DienKetHop("SELECT MaHang, TenHang FROM tblHang", cboMaHang, "MaHang", "MaHang");
            cboMaHang.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHoaDon.Text != "")
            {
                TaiThongTinHoaDon();
                btnHuy.Enabled = true;
            }
            dtpNgayBan.Format = DateTimePickerFormat.Custom;
            dtpNgayBan.CustomFormat = "dd/MM/yyyy";
            TaiDuLieuDangLuoi();
        }

        private void TaiDuLieuDangLuoi()
        {
            string sql;
            sql = "SELECT a.MaHang, b.TenHang, a.SoLuong, b.DonGiaBan, a.GiamGia,a.ThanhTien FROM tblChiTietHDBan AS a, tblHang AS b WHERE a.MaHDBan = N'" + txtMaHoaDon.Text + "' AND a.MaHang=b.MaHang";
            tblCTHDB = QuanLyBanHang.Class.ChucNang.LayDuLieuVaoBang(sql);
            dgvHDBanHang.DataSource = tblCTHDB;
            dgvHDBanHang.Columns[0].HeaderText = "Mã hàng";
            dgvHDBanHang.Columns[1].HeaderText = "Tên hàng";
            dgvHDBanHang.Columns[2].HeaderText = "Số lượng";
            dgvHDBanHang.Columns[3].HeaderText = "Đơn giá";
            dgvHDBanHang.Columns[4].HeaderText = "Giảm giá %";
            dgvHDBanHang.Columns[5].HeaderText = "Thành tiền";
            dgvHDBanHang.Columns[0].Width = 80;
            dgvHDBanHang.Columns[1].Width = 130;
            dgvHDBanHang.Columns[2].Width = 80;
            dgvHDBanHang.Columns[3].Width = 90;
            dgvHDBanHang.Columns[4].Width = 90;
            dgvHDBanHang.Columns[5].Width = 90;
            dgvHDBanHang.AllowUserToAddRows = false;
            dgvHDBanHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        // Nạp chi tiết hóa đơn
        private void TaiThongTinHoaDon()
        {
            string str;
            str = "SELECT NgayBan FROM tblHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'";
            dtpNgayBan.Text = QuanLyBanHang.Class.ChucNang.ChuyenDoiNgayGio(QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str));
            str = "SELECT MaNhanVien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'";
            cboMaNhanVien.SelectedValue = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
            str = "SELECT MaKhach FROM tblHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'";
            cboMaKhach.SelectedValue = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
            str = "SELECT TongTien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'";
            string ngay = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong("SELECT NgayBan FROM tblHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'");
            dtpNgayBan.Value = Convert.ToDateTime(ngay);
            txtTongTien.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
            lblBangChu.Text = "Bằng chữ: " + QuanLyBanHang.Class.ChucNang.ChuyenSoSangChuoi(Double.Parse(txtTongTien.Text));
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnHuy.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ThietLapLaiGiaTri();
            txtMaHoaDon.Text = QuanLyBanHang.Class.ChucNang.TaoKhoa("HDB");
            TaiDuLieuDangLuoi();
        }

        //Nạp các giá trị Control về mặc định
        private void ThietLapLaiGiaTri()
        {
            txtMaHoaDon.Text = "";
            dtpNgayBan.Value = DateTime.Now;
            dtpNgayBan.Format = DateTimePickerFormat.Custom;
            dtpNgayBan.CustomFormat = "dd/MM/yyyy";
            cboMaNhanVien.Text = "";
            cboMaKhach.Text = "";
            txtTongTien.Text = "0";
            lblBangChu.Text = "Bằng chữ: ";
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHDBan FROM tblHDBan WHERE MaHDBan=N'" + txtMaHoaDon.Text + "'";
            if (!QuanLyBanHang.Class.ChucNang.KiemTraKhoa(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
                /*if (dtpNgayBan.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpNgayBan.Focus();
                    return;
                }*/
                if (cboMaNhanVien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                if (cboMaKhach.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKhach.Focus();
                    return;
                }
                sql = "INSERT INTO tblHDBan(MaHDBan, NgayBan, MaNhanVien, MaKhach, TongTien) VALUES (N'" + txtMaHoaDon.Text.Trim() + "','" +
                        QuanLyBanHang.Class.ChucNang.ChuyenDoiNgayGio(dtpNgayBan.Text.Trim()) + "',N'" + cboMaNhanVien.SelectedValue + "',N'" +
                        cboMaKhach.SelectedValue + "'," + txtTongTien.Text + ")";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cboMaHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHang.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            if (txtGiamGia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamGia.Focus();
                return;
            }
            sql = "SELECT MaHang FROM tblChiTietHDBan WHERE MaHang=N'" + cboMaHang.SelectedValue + "' AND MaHDBan = N'" + txtMaHoaDon.Text.Trim() + "'";
            if (QuanLyBanHang.Class.ChucNang.KiemTraKhoa(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DatLaiGiaTriHang();
                cboMaHang.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(QuanLyBanHang.Class.ChucNang.LayGiaTriTruong("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + cboMaHang.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            sql = "INSERT INTO tblChiTietHDBan(MaHDBan,MaHang,SoLuong,DonGia, GiamGia,ThanhTien) VALUES(N'" + txtMaHoaDon.Text.Trim() + "',N'" + cboMaHang.SelectedValue + "'," + txtSoLuong.Text + "," + txtDonGia.Text + "," + txtGiamGia.Text + "," + txtThanhTien.Text + ")";
            QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
            TaiDuLieuDangLuoi();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE tblHang SET SoLuong =" + SLcon + " WHERE MaHang= N'" + cboMaHang.SelectedValue + "'";
            QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(QuanLyBanHang.Class.ChucNang.LayGiaTriTruong("SELECT TongTien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);
            sql = "UPDATE tblHDBan SET TongTien =" + Tongmoi + " WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'";
            QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
            txtTongTien.Text = Tongmoi.ToString();
            lblBangChu.Text = "Bằng chữ: " + QuanLyBanHang.Class.ChucNang.ChuyenSoSangChuoi(Double.Parse(Tongmoi.ToString()));
            DatLaiGiaTriHang();
            btnHuy.Enabled = true;
            btnThem.Enabled = true;
        }

        // Bổ sung reset phần hàng
        private void DatLaiGiaTriHang()
        {
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void cboMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaHang.Text == "")
            {
                txtTenHang.Text = "";
                txtDonGia.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT TenHang FROM tblHang WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtTenHang.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
            str = "SELECT DonGiaBan FROM tblHang WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtDonGia.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
        }

        private void cboMaKhach_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaKhach.Text == "")
            {
                txtTenKhachHang.Text = "";
                txtDiaChi.Text = "";
                mtbDienThoai.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select TenKhach from tblKhach where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
            txtTenKhachHang.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
            str = "Select DiaChi from tblKhach where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
            txtDiaChi.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
            str = "Select DienThoai from tblKhach where MaKhach= N'" + cboMaKhach.SelectedValue + "'";
            mtbDienThoai.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi giảm giá thì tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void cboMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaNhanVien.Text == "")
                txtTenNhanVien.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select TenNhanVien from tblNhanVien where MaNhanVien =N'" + cboMaNhanVien.SelectedValue + "'";
            txtTenNhanVien.Text = QuanLyBanHang.Class.ChucNang.LayGiaTriTruong(str);
        }

        private void cboMaHD_DropDown(object sender, EventArgs e)
        {
            QuanLyBanHang.Class.ChucNang.DienKetHop("SELECT MaHDBan FROM tblHDBan", cboMaHD, "MaHDBan", "MaHDBan");
            cboMaHD.SelectedIndex = -1;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboMaHD.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHD.Focus();
                return;
            }
            txtMaHoaDon.Text = cboMaHD.Text;
            TaiThongTinHoaDon();
            TaiDuLieuDangLuoi();
            btnHuy.Enabled = true;
            btnLuu.Enabled = true;
            cboMaHD.SelectedIndex = -1;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MaHang,SoLuong FROM tblChiTietHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'";
                DataTable tblHang = QuanLyBanHang.Class.ChucNang.LayDuLieuVaoBang(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(QuanLyBanHang.Class.ChucNang.LayGiaTriTruong("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE tblHang SET SoLuong =" + slcon + " WHERE MaHang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE tblChiTietHDBan WHERE MaHDBan=N'" + txtMaHoaDon.Text + "'";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);

                //Xóa hóa đơn
                sql = "DELETE tblHDBan WHERE MaHDBan=N'" + txtMaHoaDon.Text + "'";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
                ThietLapLaiGiaTri();
                TaiDuLieuDangLuoi();
                btnHuy.Enabled = false;
            }
        }

        private void dgvHDBanHang_DoubleClick(object sender, EventArgs e)
        {
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if (tblCTHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                MaHangxoa = dgvHDBanHang.CurrentRow.Cells["MaHang"].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["SoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["ThanhTien"].Value.ToString());
                sql = "DELETE tblChiTietHDBan WHERE MaHDBan=N'" + txtMaHoaDon.Text + "' AND MaHang = N'" + MaHangxoa + "'";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(QuanLyBanHang.Class.ChucNang.LayGiaTriTruong("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + MaHangxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE tblHang SET SoLuong =" + slcon + " WHERE MaHang= N'" + MaHangxoa + "'";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(QuanLyBanHang.Class.ChucNang.LayGiaTriTruong("SELECT TongTien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE tblHDBan SET TongTien =" + tongmoi + " WHERE MaHDBan = N'" + txtMaHoaDon.Text + "'";
                QuanLyBanHang.Class.ChucNang.ChaySQL(sql);
                txtTongTien.Text = tongmoi.ToString();
                lblBangChu.Text = "Bằng chữ: " + QuanLyBanHang.Class.ChucNang.ChuyenSoSangChuoi(Double.Parse(tongmoi.ToString()));
                TaiDuLieuDangLuoi();
            }
        }
    }
}
