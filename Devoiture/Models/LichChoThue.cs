using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class LichChoThue
{
    public int MaLich { get; set; }

    public string Biensx { get; set; } = null!;

    public DateTime Ngaynhanxe { get; set; }

    public DateTime Ngaytraxe { get; set; }

    public int Idyc { get; set; }

    public virtual Xe BiensxNavigation { get; set; } = null!;

    public virtual Yeucauthuexe IdycNavigation { get; set; } = null!;
}
