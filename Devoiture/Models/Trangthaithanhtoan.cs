using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Trangthaithanhtoan
{
    public int MaTrangthaiHd { get; set; }

    public string NoiDung { get; set; } = null!;

    public virtual ICollection<HoadonThuexe> HoadonThuexes { get; set; } = new List<HoadonThuexe>();
}
