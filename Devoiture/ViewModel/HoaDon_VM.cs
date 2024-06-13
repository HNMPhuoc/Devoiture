namespace Devoiture.ViewModel
{
    public class HoaDon_VM
    {
        public List<HoaDonThue> Hoadonthuexes { get; set; }
        public List<HoaDonChoThue> HoadonchoThuexes { get; set; }
    }
    public class HoaDonThue
    {
        public string email { get; set; }
        public string MaHd { get; set; }
        public DateTime NgaylapHd { get; set; }
        public string TenKhachHang { get; set; }
        public double TongTien { get; set; }
    }
    public class HoaDonChoThue
    {
        public string email { get; set; }
        public string MaHd { get; set; }
        public DateTime NgaylapHd { get; set; }
        public string TenKhachHang { get; set; }
        public double TongTien { get; set; }
    }
}
