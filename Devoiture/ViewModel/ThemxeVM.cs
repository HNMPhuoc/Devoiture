using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Devoiture.Controllers;
using Devoiture.Helpers;

namespace Devoiture.ViewModel
{
    public class ThemxeVM
    {
        [Required(ErrorMessage = "Vui lòng cung cấp biển số xe")]
        public string Biensoxe { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại xe")]
        public int MaLoai { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn mẫu xe")]
        public int MaMx { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp giá thuê xe")]
        public double Giathue { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp năm sản xuất")]
        public string NamSx { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp số ghế")]
        public int Soghe { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp mức tiêu thụ nhiên liệu xe")]
        public double Muctieuthunhienlieu { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp địa chỉ xe")]
        public string Diachixe { get; set; }

        public bool Giaoxetannoi { get; set; }

        public string? Dieukhoanthuexe { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp thông tin đặc điểm, chức năng của xe")]
        public string MotaDacDiemChucNang { get; set; }


        [Required(ErrorMessage = "Vui lòng cung cấp thông tin loại nhiên liệu xe đang dùng")]
        public string Loainhienlieu { get; set; }


        public bool Trangthaibaotri { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khu vực xe đang được cất giữ")]
        public int Makv { get; set; }

        public int? MaHx { get; set; }

        public List<SelectListItem>? HangXeList { get; set; }
        public List<SelectListItem>? KhuVucList { get; set; }
        public List<SelectListItem>? MauXeList { get; set; }
        public List<SelectListItem>? LoaiXeList { get; set; }
    }
}
