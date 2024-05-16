using Microsoft.AspNetCore.Mvc.Rendering;

namespace Devoiture.ViewModel
{
    public class SuaxecuaKH_VM
    {
        public string Biensoxe { get; set; }

        public int MaLoai { get; set; }

        public int MaMx { get; set; }

        public double Giathue { get; set; }

        public string NamSx { get; set; }

        public int Soghe { get; set; }

        public double Muctieuthunhienlieu { get; set; }

        public string Diachixe { get; set; }

        public bool Giaoxetannoi { get; set; }

        public string? Dieukhoanthuexe { get; set; }

        public string MotaDacDiemChucNang { get; set; }

        public string Loainhienlieu { get; set; }

        public string Hinhanh {  get; set; }

        public bool Trangthaibaotri { get; set; }

        public int Makv { get; set; }

        public List<SelectListItem>? KhuVucList { get; set; }
        public List<SelectListItem>? MauXeList { get; set; }
        public List<SelectListItem>? LoaiXeList { get; set; }
    }
}
