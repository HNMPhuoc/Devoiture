using System.ComponentModel.DataAnnotations;

namespace Devoiture.ViewModel
{
    public class DangkyKH_VM
    {
        [Required(ErrorMessage = "Email là trường bắt buộc.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Username là trường bắt buộc.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Họ tên là trường bắt buộc.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Họ tên chỉ chứa ký tự, không có số.")]
        public string HoTen { get; set; } = null!;
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự hoa, 1 ký tự thường, 1 ký tự đặc biệt, 1 số, và ít nhất 8 ký tự.")]
        public string Matkhau { get; set; } = null!;
        [Required(ErrorMessage = "Số điện thoại là trường bắt buộc.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải có từ 9 đến 10 ký tự số.")]
        public string Sdt { get; set; } = null!;
        [Required(ErrorMessage = "Ngày sinh phải trên 18 tuổi.")]
        public DateTime Ngsinh { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        public bool Gioitinh { get; set; }

        public bool? Online { get; set; }

        public bool? Lock { get; set; }
    }
}
