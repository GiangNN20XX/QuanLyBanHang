using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;     // Sử dụng đối tượng MessageBox
using System.Globalization;

namespace QuanLyBanHang.Class
{
    class ChucNang
    {
        public static SqlConnection con;
        // Tạo phương thức KetNoi()

        public static void KetNoi()
        {
            con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.QLBanHangConnectionString;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                MessageBox.Show("Kết nối thành công!");
            }
            else
            {
                MessageBox.Show("Kết nối không thành công!");
            }
        }

        // Tạo phương thức DongKetNoi()
        public static void DongKetNoi()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close(); // Đóng kết nối
                con.Dispose(); // Giải phóng tài nguyên
                con = null;
            }
        }

        // Phương thức thực thi câu lệnh Select lấy dữ liệu
        public static DataTable LayDuLieuVaoBang(string sql)
        {
            DataTable table = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            dap.Fill(table);
            return table;
        }

        // Phương thức thực thi câu lệnh Insert, Update, Delete
        public static void ChaySQL(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();  // Mở kết nối nếu đang đóng

                cmd.ExecuteNonQuery();  // Thực thi SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                cmd.Dispose();  // Giải phóng tài nguyên
                cmd = null;

                if (con.State == ConnectionState.Open)
                    con.Close();  // Đóng kết nối sau khi xong
            }
        }


        //Hàm kiểm tra khoá trùng
        public static bool KiemTraKhoa(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }

        public static object LayGiaTri(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, con); // con là SqlConnection
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            object result = cmd.ExecuteScalar(); // Trả về giá trị đơn
            cmd.Connection.Close();
            return result;
        }

        public static string LayGiaTriTruong(string sql)
        {
            string ma = "";
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    ma = reader.GetValue(0).ToString();

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return ma;
        }


        public static void DienKetHop(string sql, ComboBox cbo, string ma, string ten)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma; //Trường giá trị
            cbo.DisplayMember = ten; //Trường hiển thị
        }

        public static bool KiemTraNgay(string date)
        {
            DateTime dt;
            return DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
        }

        public static string ChuyenDoiNgayGio(string date)
        {
            try
            {
                // Ép kiểu theo định dạng dd/MM/yyyy (kiểu Việt Nam)
                DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Trả về dạng yyyy-MM-dd là chuẩn cho SQL Server
                return dt.ToString("yyyy-MM-dd");
            }
            catch
            {
                return "1900-01-01"; // Tránh lỗi SQL nếu sai format
            }
        }

        //123 => một trăm hai ba đồng
        //1,123,000=>một triệu một trăm hai ba nghìn đồng
        //1,123,345,000 => một tỉ một trăm hai ba triệu ba trăm bốn lăm ngàn đồng
        static string[] mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');
        //Viết hàm chuyển số hàng chục, giá trị truyền vào là số cần chuyển và một biến đọc phần lẻ hay không ví dụ 101 => một trăm lẻ một
        private static string DocHangChuc(double so, bool daydu)
        {
            string chuoi = "";
            //Hàm để lấy số hàng chục ví dụ 21/10 = 2
            Int64 chuc = Convert.ToInt64(Math.Floor((double)(so / 10)));
            //Lấy số hàng đơn vị bằng phép chia 21 % 10 = 1
            Int64 donvi = (Int64)so % 10;
            //Nếu số hàng chục tồn tại tức >=20
            if (chuc > 1)
            {
                chuoi = " " + mNumText[chuc] + " mươi";
                if (donvi == 1)
                {
                    chuoi += " mốt";
                }
            }
            else if (chuc == 1)
            {//Số hàng chục từ 10-19
                chuoi = " mười";
                if (donvi == 1)
                {
                    chuoi += " một";
                }
            }
            else if (daydu && donvi > 0)
            {//Nếu hàng đơn vị khác 0 và có các số hàng trăm ví dụ 101 => thì biến daydu = true => và sẽ đọc một trăm lẻ một
                chuoi = " lẻ";
            }
            if (donvi == 5 && chuc >= 1)
            {//Nếu đơn vị là số 5 và có hàng chục thì chuỗi sẽ là " lăm" chứ không phải là " năm"
                chuoi += " lăm";
            }
            else if (donvi > 1 || (donvi == 1 && chuc == 0))
            {
                chuoi += " " + mNumText[donvi];
            }
            return chuoi;
        }
        private static string DocHangTram(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng trăm ví du 434 / 100 = 4 (hàm Floor sẽ làm tròn số nguyên bé nhất)
            Int64 tram = Convert.ToInt64(Math.Floor((double)so / 100));
            //Lấy phần còn lại của hàng trăm 434 % 100 = 34 (dư 34)
            so = so % 100;
            if (daydu || tram > 0)
            {
                chuoi = " " + mNumText[tram] + " trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }
            return chuoi;
        }
        private static string DocHangTrieu(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng triệu
            Int64 trieu = Convert.ToInt64(Math.Floor((double)so / 1000000));
            //Lấy phần dư sau số hàng triệu ví dụ 2,123,000 => so = 123,000
            so = so % 1000000;
            if (trieu > 0)
            {
                chuoi = DocHangTram(trieu, daydu) + " triệu";
                daydu = true;
            }
            //Lấy số hàng nghìn
            Int64 nghin = Convert.ToInt64(Math.Floor((double)so / 1000));
            //Lấy phần dư sau số hàng nghin 
            so = so % 1000;
            if (nghin > 0)
            {
                chuoi += DocHangTram(nghin, daydu) + " nghìn";
                daydu = true;
            }
            if (so > 0)
            {
                chuoi += DocHangTram(so, daydu);
            }
            return chuoi;
        }
        public static string ChuyenSoSangChuoi(double so)
        {
            if (so == 0)
                return mNumText[0];
            string chuoi = "", hauto = "";
            Int64 ty;
            do
            {
                //Lấy số hàng tỷ
                ty = Convert.ToInt64(Math.Floor((double)so / 1000000000));
                //Lấy phần dư sau số hàng tỷ
                so = so % 1000000000;
                if (ty > 0)
                {
                    chuoi = DocHangTrieu(so, true) + hauto + chuoi;
                }
                else
                {
                    chuoi = DocHangTrieu(so, false) + hauto + chuoi;
                }
                hauto = " tỷ";
            } while (ty > 0);
            return chuoi + " đồng";
        }

        // Tạo mã hóa đơn tự động
        public static string TaoKhoa(string tiento)
        {
            string key = tiento;

            // Dùng định dạng ddMMyyyy luôn đúng kiểu Việt Nam
            string ngay = DateTime.Now.ToString("ddMMyyyy");
            key += ngay;

            // Tạo chuỗi giờ phút giây dạng 24h
            string gio = DateTime.Now.ToString("HH_mm_ss");
            key += "_" + gio;

            return key;
        }

        public static string TaoChuoiHienThi()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
