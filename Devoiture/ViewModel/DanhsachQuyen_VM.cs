namespace Devoiture.ViewModel
{
    public class DanhsachQuyen_VM
    {
        public bool? Create { get; set; }
        public bool? Read { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public string TenQuyen { get; set; }
        public int MaQuyen { get; set; }
        public List<Website_VM> Websites { get; set; }
    }
    public class Website_VM
    {
        public string TenWebsite { get; set; }
        public bool? Quyentruycap { get; set; }
        public bool? Create { get; set; }
        public bool? Read { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
    }
}
