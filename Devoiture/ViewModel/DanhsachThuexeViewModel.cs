namespace Devoiture.ViewModel
{
    public class DanhsachThuexeViewModel
    {
        public List<ThuexeThanhToanTienCoc_VM> Thuexes { get; set; }
        public List<Chothuexe_VM> ChoThuexes { get; set; }
    }
    public class ThuexeThanhToanTienCoc_VM
    {
        public int MaYc { get; set; }
        public string MauXe { get; set; }
        public string HinhXe { get; set; }
        public string HoTenChuXe { get; set; }
        public DateTime NgayNhanXe { get; set; }
        public DateTime NgayTraXe { get; set; }
        public string TrangThaiThue { get; set; }
        public int Maht { get; set; }

        public string ThoiGianThue => $"{NgayNhanXe:dd/MM/yyyy} - {NgayTraXe:dd/MM/yyyy}";
    }

    public class Chothuexe_VM
    {
        public int MaYc { get; set; }
        public string MauXe { get; set; }
        public string HinhXe { get; set; }
        public string HoTenNguoiThue { get; set; }
        public DateTime NgayNhanXe { get; set; }
        public DateTime NgayTraXe { get; set; }
        public string TrangThaiThue { get; set; }
        public int Maht { get; set; }

        public string ThoiGianThue => $"{NgayNhanXe:dd/MM/yyyy} - {NgayTraXe:dd/MM/yyyy}";
    }
}
