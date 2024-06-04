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
                TempData["success"] = "Đăng ký cho thuê xe thành công. Vui lòng chờ xác nhận kiểm duyệt xe";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Đăng ký cho thuê xe thất bại";
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
        public IActionResult SuaXe(string biensx)
        {
            var suaxe = _context.Xes
                    .Include(x => x.HinhAnhXes) // Include related images
                    .FirstOrDefault(p => p.Biensoxe == biensx);
            if (suaxe == null)
            {
                return NotFound(); // Handle the case where the car is not found
            }
            var model = new SuaxecuaKH_VM
            {
                Biensoxe = suaxe.Biensoxe,
                MaMx = suaxe.MaMx,
                MaLoai = suaxe.MaLoai,
                Giathue = suaxe.Giathue,
                NamSx = suaxe.NamSx,
                Soghe = suaxe.Soghe,
                Muctieuthunhienlieu = suaxe.Muctieuthunhienlieu,
                Diachixe = suaxe.Diachixe,
                Dieukhoanthuexe = suaxe.Dieukhoanthuexe,
                MotaDacDiemChucNang = suaxe.MotaDacDiemChucNang,
                Loainhienlieu = suaxe.Loainhienlieu,
                Makv = suaxe.Makv,
                Hinhanh = suaxe.Hinhanh,
                HinhAnhXe = suaxe.HinhAnhXes.Select(h => h.Hinh).ToList() // Load images from database
            };
            LoadDropdownLists1(model); // Load dropdown lists
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaXe(SuaxecuaKH_VM model, IFormFile Hinh,List<IFormFile> HinhAnhMoi)
        {
            if (model == null)
            {
                var suaxe1 = _context.Xes
                            .Include(x => x.HinhAnhXes) // Include related images
                            .FirstOrDefault(p => p.Biensoxe == model.Biensoxe);
                if (suaxe1 == null)
                {
                    return NotFound(); // Handle the case where the car is not found
                }
                var viewModel = new SuaxecuaKH_VM
                {
                    Biensoxe = suaxe1.Biensoxe,
                    MaMx = suaxe1.MaMx,
                    MaLoai = suaxe1.MaLoai,
                    Giathue = suaxe1.Giathue,
                    NamSx = suaxe1.NamSx,
                    Soghe = suaxe1.Soghe,
                    Muctieuthunhienlieu = suaxe1.Muctieuthunhienlieu,
                    Diachixe = suaxe1.Diachixe,
                    Dieukhoanthuexe = suaxe1.Dieukhoanthuexe,
                    MotaDacDiemChucNang = suaxe1.MotaDacDiemChucNang,
                    Loainhienlieu = suaxe1.Loainhienlieu,
                    Makv = suaxe1.Makv,
                    Hinhanh = suaxe1.Hinhanh,
                    HinhAnhXe = suaxe1.HinhAnhXes.Select(h => h.Hinh).ToList()
                };
                LoadDropdownLists1(viewModel); // Nếu cần load dropdown lists
                TempData["error"] = "Cập nhật thông tin xe thất bại";
                return View(viewModel);
            }
            var suaxe = _context.Xes.Include(x => x.HinhAnhXes).FirstOrDefault(p => p.Biensoxe == model.Biensoxe);
            if (suaxe == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy xe
            }

            // Cập nhật thông tin của xe
            suaxe.MaLoai = model.MaLoai;
            suaxe.MaMx = model.MaMx;
            suaxe.Giathue = model.Giathue;
            suaxe.NamSx = model.NamSx;
            suaxe.Soghe = model.Soghe;
            suaxe.Muctieuthunhienlieu = model.Muctieuthunhienlieu;
            suaxe.Diachixe = model.Diachixe;
            suaxe.Dieukhoanthuexe = model.Dieukhoanthuexe;
            suaxe.MotaDacDiemChucNang = model.MotaDacDiemChucNang;
            suaxe.Loainhienlieu = model.Loainhienlieu;
            suaxe.Makv = model.Makv;
            if(Hinh != null)
            {
                model.Hinhanh = MyUtil.UploadHinh("Xe", Hinh);
                suaxe.Hinhanh = model.Hinhanh;
            }
            _context.Xes.Update(suaxe);
            // Xóa các hình ảnh cũ không còn được sử dụng nữa
            _context.HinhAnhXes.RemoveRange(suaxe.HinhAnhXes);

            // Lưu các hình ảnh mới vào cơ sở dữ liệu và cập nhật liên kết với xe
            foreach (var hinhAnh in HinhAnhMoi)
            {
                var hinhAnhXe = new HinhAnhXe
                {
                    Hinh = MyUtil.UploadHinh("Xe", hinhAnh),
                    Biensoxe = suaxe.Biensoxe
                };
                _context.HinhAnhXes.Add(hinhAnhXe);
            }
            _context.SaveChanges();
            TempData["success"] = "Cập nhật thông tin xe thành công";
            return RedirectToAction("DanhsachxecuaKH");
        }
        private void LoadDropdownLists1(SuaxecuaKH_VM model)
        {
            model.KhuVucList = _context.Khuvucs.Select(kv => new SelectListItem
            {
                Value = kv.MaKv.ToString(),
                Text = kv.TenKv
            }).ToList();
            model.MauXeList = _context.MauXes.Select(mx => new SelectListItem
            {
                Value = mx.MaMx.ToString(),
                Text = mx.TenMx
            }).ToList();
            model.LoaiXeList = _context.LoaiXes.Select(lx => new SelectListItem
            {
                Value = lx.MaLoai.ToString(),
                Text = lx.TenLoai
            }).ToList();
        }
        [HttpPost]
        public IActionResult CapnhatBaotri(string biensoxe)
        {
            var xe = _context.Xes.SingleOrDefault(x => x.Biensoxe == biensoxe);
            if (xe == null)
            {
                return NotFound();
            }
            xe.Trangthaibaotri = !xe.Trangthaibaotri;
            _context.SaveChanges();

            TempData["successMessage"] = xe.Trangthaibaotri ? "Đã bật trạng thái bảo trì." : "Đã tắt trạng thái bảo trì.";
            return RedirectToAction("DanhsachxecuaKH");
        }
        [HttpPost]
        public IActionResult CapnhatAnXe(string biensoxe)
        {
            var xe = _context.Xes.SingleOrDefault(x => x.Biensoxe == biensoxe);
            if (xe == null)
            {
                return NotFound();
            }

            xe.Hide = !xe.Hide;
            _context.SaveChanges();

            TempData["successMessage"] = xe.Hide ? "Đã ẩn xe." : "Đã hiện xe.";
            return RedirectToAction("DanhsachxecuaKH");
        }

    }
}
