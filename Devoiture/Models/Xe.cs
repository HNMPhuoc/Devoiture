using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Xe
{
    public string Biensoxe { get; set; } = null!;

    public int MaLoai { get; set; }

    public int MaMx { get; set; }

    public int Makv { get; set; }

    public double Giathue { get; set; }

    public string NamSx { get; set; } = null!;

    public int Soghe { get; set; }

    public double Muctieuthunhienlieu { get; set; }

    public string Hinhanh { get; set; } = null!;

    public string Diachixe { get; set; } = null!;

    public string? Dieukhoanthuexe { get; set; }

    public string MotaDacDiemChucNang { get; set; } = null!;

    public bool TrangthaiDuyet { get; set; }

    public string Loainhienlieu { get; set; } = null!;

    public bool Hide { get; set; }

    public string Email { get; set; } = null!;

    public bool Trangthaibaotri { get; set; }

    public virtual Taikhoan EmailNavigation { get; set; } = null!;

    public virtual ICollection<HinhAnhXe> HinhAnhXes { get; set; } = new List<HinhAnhXe>();

    public virtual LoaiXe MaLoaiNavigation { get; set; } = null!;

    public virtual MauXe MaMxNavigation { get; set; } = null!;

    public virtual Khuvuc MakvNavigation { get; set; } = null!;

    public virtual ICollection<Yeucauthuexe> Yeucauthuexes { get; set; } = new List<Yeucauthuexe>();
}
