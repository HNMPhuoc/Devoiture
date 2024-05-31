using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Taikhoan
{
    public string Email { get; set; } = null!;

    public int IdQuyen { get; set; }

    public string Username { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public DateTime Ngsinh { get; set; }

    public string? HinhDaiDien { get; set; }

    public bool? Gioitinh { get; set; }

    public string? SoGplxB2 { get; set; }

    public string? HinhGplxb2 { get; set; }

    public string? SoCccd { get; set; }

    public string? HinhCccd { get; set; }

    public bool? Online { get; set; }

    public bool? Lock { get; set; }

    public virtual Quyen IdQuyenNavigation { get; set; } = null!;

    public virtual ICollection<Xe> Xes { get; set; } = new List<Xe>();

    public virtual ICollection<Yeucauthuexe> Yeucauthuexes { get; set; } = new List<Yeucauthuexe>();
}
