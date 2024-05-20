using AutoMapper;
using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Areas.Admin.Controllers
{
    [Authorize]
    public class QLNhanvienController : Controller
    {
        private readonly Devoiture1Context _context;
        private readonly IMapper _mapper;
        public QLNhanvienController(Devoiture1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }       
        public IActionResult DanhsachNV()
        {
            var dsnv = _context.Taikhoans
                .Where(tk => tk.IdQuyen == 2)
                .Include(tk => tk.IdQuyenNavigation)
                .Select(tk => new DanhsachNV_VM
                {
                    Email = tk.Email,
                    IdQuyen = tk.IdQuyen,
                    Username = tk.Username,
                    HoTen = tk.HoTen,
                    Sdt = tk.Sdt,
                    Ngsinh = tk.Ngsinh,
                    HinhDaiDien = tk.HinhDaiDien,
                    Gioitinh = tk.Gioitinh,
                    Online = tk.Online,
                    Lock = tk.Lock,
                    TenQuyen = tk.IdQuyenNavigation.TenQuyen
                })
                .OrderBy(tk => tk.HoTen)
                .ToList();
            return View("~/Areas/Admin/Views/QLNhanvien/DanhsachNV.cshtml", dsnv);
        }
        [HttpPost]
        public IActionResult LockAccount(string email, bool isLocked)
        {
            var nhanVien = _context.Taikhoans.SingleOrDefault(t => t.Email == email);
            if (nhanVien != null)
            {
                nhanVien.Lock = isLocked;
                _context.SaveChanges();
            }
            return RedirectToAction("DanhsachNV");
        }
        [HttpPost]
        public IActionResult XoaNhanvien(string email)
        {
            var nhanVien = _context.Taikhoans.SingleOrDefault(t => t.Email == email);
            if (nhanVien != null)
            {
                _context.Taikhoans.Remove(nhanVien);
                _context.SaveChanges();
            }
            return RedirectToAction("DanhsachNV");
        }
        [HttpGet]
        public IActionResult ThemNhanVien()
        {
            var model = new DangkyKH_VM();
            return View("~/Areas/Admin/Views/QLNhanvien/ThemNhanVien.cshtml",model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemNhanVien(DangkyKH_VM model)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = _mapper.Map<Taikhoan>(model);
                nhanVien.Email = model.Email;
                nhanVien.Matkhau = BCrypt.Net.BCrypt.HashPassword(model.Matkhau);
                nhanVien.Username = model.Username;
                nhanVien.HoTen = model.HoTen;
                nhanVien.Sdt = model.Sdt;
                nhanVien.Ngsinh = model.Ngsinh;
                nhanVien.Gioitinh = model.Gioitinh;
                nhanVien.Online = false;
                nhanVien.Lock = false;
                nhanVien.IdQuyen = 2;
                var quyenNhanVien = _context.Quyens.FirstOrDefault(q => q.MaQuyen == nhanVien.IdQuyen);
                if (quyenNhanVien != null)
                {
                    nhanVien.IdQuyenNavigation = quyenNhanVien;
                }
                _context.Taikhoans.Add(nhanVien);
                _context.SaveChanges();
                return RedirectToAction("DanhsachNV");
            }
            return View("~/Areas/Admin/Views/QLNhanvien/ThemNhanVien.cshtml", model);
        }
        [HttpGet]
        public IActionResult ChiTietNhanVien(string email)
        {
            var nhanVien = _context.Taikhoans
                .Include(tk => tk.IdQuyenNavigation)
                .FirstOrDefault(tk => tk.Email == email);
            if (nhanVien == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ChitietNV_VM>(nhanVien);
            return View("~/Areas/Admin/Views/QLNhanvien/ChiTietNhanVien.cshtml", model);
        }

    }
}
