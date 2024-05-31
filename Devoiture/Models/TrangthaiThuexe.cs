using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class TrangthaiThuexe
{
    public int Matt { get; set; }

    public string Tentrangthai { get; set; } = null!;

    public virtual ICollection<Yeucauthuexe> Yeucauthuexes { get; set; } = new List<Yeucauthuexe>();
}
