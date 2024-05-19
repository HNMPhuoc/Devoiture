namespace Devoiture.ViewModel
{
    public class ChangeNameRole_VM
    {
        public List<QuyenViewModel> DanhSachQuyen { get; set; }
    }
    public class QuyenViewModel
    {
        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; }
    }
}
