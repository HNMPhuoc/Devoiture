using System.ComponentModel.DataAnnotations;

namespace Devoiture.ViewModel
{
    public class Quenmatkhau_VM
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        public string Matkhau { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Compare("Matkhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string Matkhauxacnhan { get; set; }
    }
}
