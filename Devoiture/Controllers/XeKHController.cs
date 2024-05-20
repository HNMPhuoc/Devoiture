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
        public IActionResult ThemXe()
        {
            var viewModel = new ThemxeVM();
            LoadDropdownLists(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemXe(ThemxeVM model, IFormFile Hinh, List<IFormFile> HinhAnhXe)
        {
            if (ModelState.IsValid)
            {
                var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY)?.Value;
                if (string.IsNullOrEmpty(kh))
                {
                    return RedirectToAction("Login", "Account"); // Chuyển hướng nếu không có thông tin người dùng
                }
                var xe = _imapper.Map<Xe>(model);
                xe.Email = kh;
                xe.Hide = false;
                xe.TrangthaiDuyet = false;
                xe.Trangthaibaotri = false;
                if (Hinh != null)
                {
                    xe.Hinhanh = MyUtil.UploadHinh("Xe", Hinh);
                }
                _context.Xes.Add(xe);
                _context.SaveChanges();
                foreach (var hinhAnh in HinhAnhXe)
                {
                    var hinhAnhXe = new HinhAnhXe
                    {
                        Hinh = MyUtil.UploadHinh("Xe", hinhAnh),
                        Biensoxe = xe.Biensoxe
                    };
                    _context.HinhAnhXes.Add(hinhAnhXe);
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            LoadDropdownLists(model);
            return View(model);
        }
        private void LoadDropdownLists(ThemxeVM model)
        {
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
            model.MauXeList = new List<SelectListItem>();
        }
        [HttpGet]
        public IActionResult GetMauXeList(int maHx)
        {
            var mauXeList = _context.MauXes
                                    .Where(x => x.MaHx == maHx)
                                    .Select(x => new SelectListItem
                                    {
                                        Value = x.MaMx.ToString(),
                                        Text = x.TenMx
                                    })
                                    .ToList();
            return Json(mauXeList);
        }
        [HttpGet]
        [HttpGet]
        public IActionResult SuaXe(string biensx)
        {
            var suaxe = _context.Xes.FirstOrDefault(p => p.Biensoxe == biensx);
            var model = _imapper.Map<SuaxecuaKH_VM>(suaxe);
            LoadDropdownLists1(model); // Nếu cần load dropdown lists
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaXe(SuaxecuaKH_VM model, IFormFile Hinh)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdownLists1(model); // Nếu cần load dropdown lists
                return View(model);
            }

            var suaxe = _imapper.Map<Xe>(model);
            if (Hinh != null)
            {
                suaxe.Hinhanh = MyUtil.UploadHinh("Xe", Hinh);
            }
            _context.Update(suaxe);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private void LoadDropdownLists1(SuaxecuaKH_VM model)
        {
            model.LoaiXeList = _context.LoaiXes
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaLoai.ToString(),
                                            Text = x.TenLoai
                                        })
                                        .ToList();
            model.KhuVucList = _context.Khuvucs
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaKv.ToString(),
                                            Text = x.TenKv
                                        })
                                        .ToList();
            model.MauXeList = _context.MauXes
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.MaMx.ToString(),
                                            Text = x.TenMx
                                        })
                                        .ToList();
        }
    }
}
