using System;
using System.Collections.Generic;
using System.Text;

namespace LAB4_WindowsApplication
{
    public class SinhVien
    {
        public string MSSV { get; set; }
        public string Ten { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool Phai { get; set; }
        public string Lop { get; set; }
        public string SDT { get; set; }
        public string Hinh { get; set; }

        public SinhVien()
        {

        }

        public SinhVien(string mSSV, string ten, string email, string diaChi, DateTime ngaySinh, bool phai, string lop, string sDT, string hinh)
        {
            MSSV = mSSV;
            Ten = ten;
            Email = email;
            DiaChi = diaChi;
            NgaySinh = ngaySinh;
            Phai = phai;
            Lop = lop;
            SDT = sDT;
            Hinh = hinh;
        }
       
    }
    
}
