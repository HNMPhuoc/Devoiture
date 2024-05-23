using Devoiture.Models;

namespace Devoiture.ViewModel
{
    public class ProfileKH_VM
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string HoTen { get; set; }
        public string Sdt { get; set; }
        public DateTime Ngsinh { get; set; }
        public string? HinhDaiDien { get; set; }

        public bool? Gioitinh { get; set; }

        public string? SoGplxB2 { get; set; }

        public string? HinhGplx { get; set; }

        public string? SoCccd { get; set; }

        public string? HinhCccd { get; set; }
    }
}
