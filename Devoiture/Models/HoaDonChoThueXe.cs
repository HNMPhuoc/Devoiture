using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class HoaDonChoThueXe
{
    public int MaHdct { get; set; }

    public int MaYc { get; set; }

    public string Email { get; set; } = null!;

    public string Biensx { get; set; } = null!;

    public DateTime NglapHd { get; set; }

    public string Hoten { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public double Chietkhau { get; set; }

    public double Tongtiennhanduoc { get; set; }

    public virtual Yeucauthuexe MaYcNavigation { get; set; } = null!;
}
