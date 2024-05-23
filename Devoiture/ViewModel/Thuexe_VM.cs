using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Devoiture.ViewModel
{
    public class Thuexe_VM
    {
        public string CarModel { get; set; }
        public string Hinhanh { get; set; }
        public List<string> HinhAnhList { get; set; }
        public string NamSx { get; set; }
        public string MotaDacDiemChucNang { get; set; }
        public string Loainhienlieu { get; set; }
        public double Muctieuthunhienlieu { get; set; }
        public string Diachixe { get; set; }
        public double Giathue { get; set; }
        public int Soghe { get; set; }
        public string? Dieukhoanthuexe { get; set; }
        public double Baohiemthuexe 
        { 
            get
            {
                return Dongiathue * 0.09843205574912892;
            }
        }

        public DateTime Ngaynhanxe { get; set; } = DateTime.Now;

        public DateTime Ngaytraxe { get; set; } = DateTime.Now.AddDays(1);

        public int Songaythue
        {
            get
            {
                return (Ngaytraxe - Ngaynhanxe).Days;
            }
        }

        public double Dongiathue
        {
            get
            {
                return Math.Round(Giathue * Songaythue,2);
            }
        }
        public double Tongtienthue 
        {
            get
            {
                return Dongiathue + Baohiemthuexe;
            }
        }
    }
}