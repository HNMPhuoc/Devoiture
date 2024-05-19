namespace Devoiture.ViewModel
{
    public class DanhsachNV_VM
    {
        public string Email { get; set; }
        public int IdQuyen { get; set; }
        public string Username { get; set; }
        public string HoTen { get; set; }
        public string Sdt { get; set; }
        public DateTime Ngsinh { get; set; }
        public string? HinhDaiDien { get; set; }
        public bool? Gioitinh { get; set; }
        public bool? Online { get; set; }
        public bool? Lock { get; set; }
        public string TenQuyen { get; set; }
    }
}
