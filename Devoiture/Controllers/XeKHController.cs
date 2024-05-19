using Devoiture.Models;
using Devoiture.ViewModel;
using Devoiture.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Devoiture.Controllers
{
    [CustomerAuthorize]
    [Authorize]
    public class XeKHController : Controller
    {
        private readonly Devoiture1Context _context;
        private readonly IMapper _imapper;

        public XeKHController(Devoiture1Context context, IMapper mapper)
        {
            _context = context;
            _imapper = mapper;
        }
        public IActionResult DanhsachxecuaKH()
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
            var dsxe = _context.Xes
            .Where(xm => xm.Email == kh)
            .Select(x => new DanhsachxeKH_VM
            {
                Biensoxe = x.Biensoxe,
                TenXe = x.MaMxNavigation.TenMx,
                NamSx = x.NamSx,
                Tenloai = x.MaLoaiNavigation.TenLoai,
                Muctieuthunhienlieu = x.Muctieuthunhienlieu,
                Hinhanh = x.Hinhanh,
                Trangthaibaotri = x.Trangthaibaotri,
                TrangthaiDuyet = x.TrangthaiDuyet,
                Hide = x.Hide
            })
            .OrderBy(p => p.TenXe)
            .ToList();
            return View(dsxe);
        }
        [HttpGet]
        public IActionResult GetModelsByBrand(int brandId)
        {
            var models = _context.MauXes.Where(x => x.MaHx == brandId).ToList();
            return Json(models);
        }
        [HttpGet]
        public IActionResult ThemXe()
        {
            var viewModel = new ThemxeVM();
            viewModel.LoaiXeList = _context.LoaiXes
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaLoai.ToString(),
                                            Text = x.TenLoai
                                        })
                                        .ToList();
            viewModel.HangXeList = _context.HangXes
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaHx.ToString(),
                                            Text = x.TenHx
                                        })
                                        .ToList();
            viewModel.KhuVucList = _context.Khuvucs
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaKv.ToString(),
                                            Text = x.TenKv
                                        })
                                        .ToList();
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemXe(ThemxeVM model, IFormFile Hinh)
        {
            var xe = _imapper.Map<Xe>(model);
            if (ModelState.IsValid)
            {
                var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;
                var khuvuc = _context.Khuvucs.FirstOrDefault(p => p.MaKv == xe.Makv);
                if (khuvuc != null)
                {
                    // Gán đối tượng Khuvuc cho thuộc tính navigation MakvNavigation của đối tượng Xe
                    xe.MakvNavigation = khuvuc;
                }
                var email = _context.Taikhoans.FirstOrDefault(p => p.Email == xe.Email);
                if (email != null)
                {
                    // Gán đối tượng Khuvuc cho thuộc tính navigation MakvNavigation của đối tượng Xe
                    xe.EmailNavigation = email;
                }
                var loai = _context.LoaiXes.FirstOrDefault(p => p.MaLoai == xe.MaLoai);
                if (loai != null)
                {
                    // Gán đối tượng Khuvuc cho thuộc tính navigation MakvNavigation của đối tượng Xe
                    xe.MaLoaiNavigation = loai;
                }
                var mauxe = _context.MauXes.FirstOrDefault(p => p.MaMx == xe.MaMx);
                if (mauxe != null)
                {
                    // Gán đối tượng Khuvuc cho thuộc tính navigation MakvNavigation của đối tượng Xe
                    xe.MaMxNavigation = mauxe;
                }
                xe.Biensoxe = model.Biensoxe;
                xe.Email = kh.ToString();
                xe.Trangthaibaotri = model.Trangthaibaotri;
                xe.TrangthaiDuyet = false;
                xe.Hide = true;
                xe.Muctieuthunhienlieu = model.Muctieuthunhienlieu;
                xe.Giathue = model.Giathue;
                xe.Giaoxetannoi = model.Giaoxetannoi;
                xe.MaMx = model.MaMx;
                xe.MaLoai = model.MaLoai;
                xe.Dieukhoanthuexe = model.Dieukhoanthuexe;
                xe.Diachixe = model.Diachixe;
                xe.Loainhienlieu = model.Loainhienlieu;
                xe.Makv = model.Makv;
                xe.MaMx = model.MaMx;
                xe.MotaDacDiemChucNang = model.MotaDacDiemChucNang;
                if (Hinh != null)
                {
                    xe.Hinhanh = MyUtil.UploadHinh("Xe", Hinh);
                }
                _context.Xes.Add(xe);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            model.LoaiXeList = _context.LoaiXes
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaLoai.ToString(),
                                            Text = x.TenLoai
                                        })
                                        .ToList();
            model.HangXeList = _context.HangXes
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaHx.ToString(),
                                            Text = x.TenHx
                                        })
                                        .ToList();
            model.KhuVucList = _context.Khuvucs
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaKv.ToString(),
                                            Text = x.TenKv
                                        })
                                        .ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult SuaXe(string biensx)
        {
            var suaxe = _context.Xes.FirstOrDefault(p => p.Biensoxe == biensx);
            var model = new SuaxecuaKH_VM
            {
                MaMx = suaxe.MaMx,
                Biensoxe = suaxe.Biensoxe,
                Giaoxetannoi = suaxe.Giaoxetannoi,
                Trangthaibaotri = suaxe.Trangthaibaotri,
                Giathue = suaxe.Giathue,
                Dieukhoanthuexe = suaxe.Dieukhoanthuexe,
                Diachixe = suaxe.Diachixe,
                MotaDacDiemChucNang = suaxe.MotaDacDiemChucNang,
                Makv = suaxe.Makv,
                MaLoai = suaxe.MaLoai,
                Hinhanh = suaxe.Hinhanh,
                Soghe = suaxe.Soghe,
                NamSx = suaxe.NamSx,
                Loainhienlieu = suaxe.Loainhienlieu,
                Muctieuthunhienlieu = suaxe.Muctieuthunhienlieu,
                LoaiXeList = _context.LoaiXes
                        .Select(x => new SelectListItem
                        {
                            Value = x.MaLoai.ToString(),
                            Text = x.TenLoai
                        })
                        .ToList(),
                MauXeList = _context.MauXes
                        .Select(x => new SelectListItem
                        {
                            Value = x.MaMx.ToString(),
                            Text = x.TenMx
                        })
                        .ToList(),
                KhuVucList = _context.Khuvucs
                        .Select(x => new SelectListItem
                        {
                            Value = x.MaKv.ToString(),
                            Text = x.TenKv
                        })
                        .ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaXe(SuaxecuaKH_VM model, IFormFile Hinh)
        {
            var suaxe = _context.Xes.FirstOrDefault(p => p.Biensoxe == model.Biensoxe);
            suaxe.Biensoxe = model.Biensoxe;
            suaxe.MaMx = model.MaMx;
            suaxe.Makv = model.Makv;
            suaxe.NamSx = model.NamSx;
            suaxe.Diachixe = model.Diachixe;
            suaxe.Dieukhoanthuexe = model.Dieukhoanthuexe;
            if (Hinh != null)
            {
                suaxe.Hinhanh = MyUtil.UploadHinh("Xe", Hinh);
            }
            suaxe.Giaoxetannoi = model.Giaoxetannoi;
            suaxe.Muctieuthunhienlieu = model.Muctieuthunhienlieu;
            suaxe.Giathue = model.Giathue;
            suaxe.Soghe = model.Soghe;
            suaxe.Loainhienlieu = model.Loainhienlieu;
            suaxe.Trangthaibaotri = model.Trangthaibaotri;
            suaxe.MaLoai = model.MaLoai;
            suaxe.MotaDacDiemChucNang = model.MotaDacDiemChucNang;
            _context.Update(suaxe);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult XoaXe(string biensx)
        {
            var xoaxe = _context.Xes.FirstOrDefault(p => p.Biensoxe == biensx);
            var model = new XoaxeVM
            {
                Biensoxe = xoaxe.Biensoxe,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaXe(XoaxeVM model)
        {
            var xoaxe = _context.Xes.FirstOrDefault(p => p.Biensoxe == model.Biensoxe);
            if (xoaxe != null)
            {
                _context.Xes.Remove(xoaxe);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Trả về một View với thông báo lỗi nếu không tìm thấy xe để xóa
                ModelState.AddModelError(string.Empty, "Không tìm thấy xe để xóa.");
                return View(model);
            }
        }
    }
}
