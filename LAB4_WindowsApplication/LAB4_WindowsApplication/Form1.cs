using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB4_WindowsApplication
{
    public partial class Form1 : Form
    {
        QuanLySinhVien qlsv;        
        public Form1()
        {
            InitializeComponent();
        }
        //Lấy thông tin từ controls thông tin SV
        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();
            bool gt = true;
            List<string> cn = new List<string>();
            sv.MSSV = this.mktMSSV.Text;
            sv.Ten = this.txtHoTen.Text;
            sv.Email = this.txtEmail.Text;
            sv.DiaChi = this.txtDiaChi.Text;
            sv.NgaySinh = this.dtpNgaySinh.Value;
            if (rbNu.Checked)
                gt = false;
            sv.Phai = gt;
            sv.Lop = this.CbbLop.Text;
            sv.Hinh = this.txtHinh.Text;                 
            return sv;
        }

        //Lấy thông tin sinh viên từ dòng item của ListView
        private SinhVien GetSinhVienLV(ListViewItem lvitem)

        {
            SinhVien sv = new SinhVien();
            sv.MSSV = lvitem.SubItems[0].Text;
            sv.Ten = lvitem.SubItems[1].Text;
            sv.Phai = false;
            if (lvitem.SubItems[2].Text == "Nam")
                sv.Phai = true;
            sv.NgaySinh = DateTime.Parse(lvitem.SubItems[3].Text);
            sv.Lop = lvitem.SubItems[4].Text;
            sv.SDT = lvitem.SubItems[5].Text;
            sv.Email = lvitem.SubItems[6].Text;
            sv.DiaChi = lvitem.SubItems[7].Text;
            sv.Hinh = lvitem.SubItems[8].Text;
            return sv;
        }

        //Thiết lập các thông tin lên controls sinh viên
        private void ThietLapThongTin(SinhVien sv)
        {
            this.mktMSSV.Text = sv.MSSV;
            this.txtHoTen.Text = sv.Ten;
            if (sv.Phai)
                this.rbNam.Checked = true;
            else
                this.rbNu.Checked = true;
            this.dtpNgaySinh.Value = sv.NgaySinh;
            this.CbbLop.Text = sv.Lop;
            this.mktSDT.Text = sv.SDT;
            this.txtEmail.Text = sv.Email;
            this.txtDiaChi.Text = sv.DiaChi;
            this.txtHinh.Text = sv.Hinh;          
        }

        //Thêm sinh viên vào ListView

        private void ThemSV(SinhVien sv)
        {
            ListViewItem lvitem = new ListViewItem(sv.MSSV);
            lvitem.SubItems.Add(sv.Ten);
            string gt = "Nữ";
            if (sv.Phai)
                gt = "Nam";
            lvitem.SubItems.Add(gt);
            lvitem.SubItems.Add(sv.NgaySinh.ToShortDateString());
            lvitem.SubItems.Add(sv.Lop);
            lvitem.SubItems.Add(sv.SDT);
            lvitem.SubItems.Add(sv.Email);
            lvitem.SubItems.Add(sv.DiaChi);
            lvitem.SubItems.Add(sv.Hinh);
            this.lvDSSV.Items.Add(lvitem);
        }
        //Hiển thị các sinh viên trong qlsv lên ListView
        private void LoadListView()
        {
            this.lvDSSV.Items.Clear();
            foreach (SinhVien sv in qlsv.DanhSach)
            {
                ThemSV(sv);
            }
        }

       

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            this.mktMSSV.Text = "";
            this.txtHoTen.Text = "";
            this.rbNam.Checked = true;
            this.CbbLop.Text = this.CbbLop.Items[0].ToString();
            this.mktSDT.Text = "";
            this.txtEmail.Text="";
            this.txtDiaChi.Text = "";
            this.txtHinh.Text = "";
            this.pbHinh.ImageLocation = "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            SinhVien kq = qlsv.Tim(sv.MSSV, delegate (object obj1, object obj2) {
                return (obj2 as SinhVien).MSSV.CompareTo(obj1.ToString());
            });
            if (kq != null)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi thêm dữliệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.qlsv.Them(sv);
                this.LoadListView();
            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHinh_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "Open File Image";
            file.Filter = "Image Files (BMP, JPEG, PNG)|"
                + "*.bmp;*.jpg;*.jpeg;*.png|"
                + "BMP files (*.bmp)|*.bmp|"
                + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
                + "PNG files (*.png)|*.png|"
                + "All files (*.*)|*.*";
            file.InitialDirectory = Environment.CurrentDirectory;
            if (file.ShowDialog() == DialogResult.OK)
            {
                var fileName = file.FileName;
                txtHinh.Text = fileName;
                pbHinh.Load(fileName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DocTuFile();
            LoadListView();
        }

       
        private int SoSanhTheoMa(object obj1, object obj2)
        {
            SinhVien sv = obj2 as SinhVien;
            return sv.MSSV.CompareTo(obj1);
        }

        private void lvDSSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = lvDSSV.SelectedItems.Count;
            if (count > 0)
            {
                ListViewItem item = lvDSSV.SelectedItems[0];
                ThietLapThongTin(GetSinhVienLV(item));
            }
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count, i;
            ListViewItem item;
            count = lvDSSV.Items.Count - 1;
            for (i = count; i >= 0; i--)
            {
                item = this.lvDSSV.Items[i];
                if (item.Selected)
                    qlsv.Xoa(item.SubItems[0].Text, SoSanhTheoMa);
            }
            LoadListView();
            this.btnMacDinh.PerformClick();
        }

        private void tảiLạiDanhSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Bạn có chắc không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dlg == DialogResult.Yes)
            {
                qlsv.DanhSach.Clear();
                qlsv.DocTuFile();
                LoadListView();
            }
            return;
        }
    }
}
