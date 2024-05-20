using System.ComponentModel.DataAnnotations;

namespace Devoiture.ViewModel
{
    public class DangnhapKH_VM
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Mật khẩu không hợp lệ")]
        public string Matkhau { get; set; }
    }
}
