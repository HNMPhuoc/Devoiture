using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class ChitietQuyen
{
    public int MaCtq { get; set; }

    public string ActionName { get; set; } = null!;

    public string ActionCode { get; set; } = null!;

    public bool CheckAction { get; set; }

    public int MaQuyen { get; set; }

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;
}
